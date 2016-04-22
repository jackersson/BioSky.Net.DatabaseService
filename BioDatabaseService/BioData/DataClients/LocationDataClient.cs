using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.DataClients
{
  public class LocationDataClient
  {
    public LocationDataClient( IProcessorLocator locator
                              , AccessDeviceDataClient  accessDeviceDataClient
                              , CaptureDeviceDataClient captureDeviceDataClient
                              , PersonAccessDataClient  personAccessDataClient 
                              , VisitorDataClient       visitorDataClient
      
      )
    {
      _locator = locator;
      _convertor                = new ProtoMessageConvertor();
      _rawIndexes               = new BioService.RawIndexes();
      _accessDevicesRawIndexes  = new BioService.RawIndexes();
      _caprureDevicesRawIndexes = new BioService.RawIndexes();
      _visitorsRawIndexes       = new BioService.RawIndexes();

    _accessDeviceDataClient  = accessDeviceDataClient ;
      _captureDeviceDataClient = captureDeviceDataClient;
      _personAccessDataClient  = personAccessDataClient ;
      _visitorDataClient       = visitorDataClient;
    }

    public BioService.Location Add(BioService.Location location)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(location, dataContext);
      }
    }

    public BioService.Location Add(BioService.Location location, BioSkyNetDataModel dataContext)
    {
      BioService.Location newProtoLocation = new BioService.Location { Dbresult = BioService.Result.Failed };

      //check on access devices vs capture devices vs person accesability
      if (location == null || ( location.CaptureDevice == null && location.AccessDevice == null) )
        return newProtoLocation;

      try
      {
        Location existingLocation = dataContext.Location.Where( x => x.Location_Name == location.LocationName ).FirstOrDefault();

        if (existingLocation != null)
          return newProtoLocation;

  
        Location newLocation = _convertor.GetLocationEntity(location);
        dataContext.Location.Add(newLocation);
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return newProtoLocation;

        newProtoLocation.Dbresult = BioService.Result.Success;
        newProtoLocation.Id       = newLocation.Id;

        if (location.AccessDevice != null )
        {
          BioService.AccessDevice newAccessDevice = _accessDeviceDataClient.Add(newLocation, location.AccessDevice, dataContext);
          if (newAccessDevice.Id > 0)
            newProtoLocation.AccessDevice = newAccessDevice;
        }

        if (location.CaptureDevice != null)
        {
          BioService.CaptureDevice newCaptureDevice = _captureDeviceDataClient.Add(newLocation, location.CaptureDevice, dataContext);
          if (newCaptureDevice.Id > 0)
            newProtoLocation.CaptureDevice  = newCaptureDevice;
        }

        newProtoLocation.Persons.Add(_personAccessDataClient.Update(newLocation, location, dataContext));

        if (newProtoLocation.Persons.Count == 0)        
          newLocation.Access_Type = (byte)location.AccessType;        
        else
          newLocation.Access_Type = (byte)BioService.Location.Types.AccessType.Custom;

        newProtoLocation.AccessType = (BioService.Location.Types.AccessType) newLocation.Access_Type;

        dataContext.SaveChanges();
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return newProtoLocation;
    }

    public BioService.Location Update(BioService.Location location)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Update(location, dataContext);
      }
    }

    public BioService.Location Update(BioService.Location location, BioSkyNetDataModel dataContext)
    {
      BioService.Location updatedProtoLocation = new BioService.Location { Dbresult = BioService.Result.Failed };
      if (location == null)
        return updatedProtoLocation;

      try
      {
        Location existingLocation = dataContext.Location.Find(location.Id);

        if (existingLocation == null)
          return updatedProtoLocation;

        if (location.LocationName != "")
          existingLocation.Location_Name = location.LocationName;

        if (location.Description != "")
          existingLocation.Description = location.Description;

        byte accesstype = (byte)location.AccessType;
        if (accesstype != existingLocation.Access_Type)
          existingLocation.Access_Type = accesstype;

        int affectedRows = dataContext.SaveChanges();

        if (affectedRows > 0)
        {
          if (location.LocationName != "")
            updatedProtoLocation.LocationName = location.LocationName;

          if (location.Description != "")
            updatedProtoLocation.Description  = location.Description ;

          if (accesstype != existingLocation.Access_Type)
            updatedProtoLocation.AccessType = (BioService.Location.Types.AccessType)existingLocation.Access_Type;

          updatedProtoLocation.Dbresult = BioService.Result.Success;
        }

        //AccessDevice
        BioService.AccessDevice accessDevice = location.AccessDevice;
        if (accessDevice != null)
        {
          if (accessDevice.EntityState == BioService.EntityState.Deleted)
          {
            AccessDevice existingAccessDevice = dataContext.AccessDevice.Where(x => x.PortName == accessDevice.Portname).FirstOrDefault();
            if (existingAccessDevice != null)
            {
              _rawIndexes.Indexes.Clear();
              _rawIndexes.Indexes.Add(existingAccessDevice.Id);
              BioService.RawIndexes removedAccessDevice = _accessDeviceDataClient.Remove(_rawIndexes);
              if (removedAccessDevice.Indexes.Count > 0)
              {
                updatedProtoLocation.AccessDevice = new BioService.AccessDevice()
                {
                    Id       = existingAccessDevice.Id
                  , Dbresult = BioService.Result.Success
                };
                updatedProtoLocation.Dbresult = BioService.Result.Success;
              }
              else
              {
                updatedProtoLocation.AccessDevice = new BioService.AccessDevice()
                {
                    Id       = existingAccessDevice.Id
                  , Dbresult = BioService.Result.Failed
                };
                updatedProtoLocation.Dbresult = BioService.Result.Failed;
                return updatedProtoLocation;
              }
            }          
          }
          else
          {
            BioService.AccessDevice newAccessDevice = _accessDeviceDataClient.Add(existingLocation, accessDevice, dataContext);
            if (newAccessDevice.Id > 0)
            {
              updatedProtoLocation.AccessDevice = newAccessDevice;
              updatedProtoLocation.Dbresult     = BioService.Result.Success;
            }
          }
        }

        //CaptureDevice
        BioService.CaptureDevice captureDevice = location.CaptureDevice;
        if (captureDevice != null)
        {
          if (captureDevice.EntityState == BioService.EntityState.Deleted)
          {
            CaptureDevice existingCaptureDevice = dataContext.CaptureDevice.Where(x => x.Device_Name == captureDevice.Devicename).FirstOrDefault();
            if(existingCaptureDevice != null)
            {
              _rawIndexes.Indexes.Clear();
              _rawIndexes.Indexes.Add(existingCaptureDevice.Id);
              BioService.RawIndexes removedCaptureDevice = _captureDeviceDataClient.Remove(_rawIndexes);
              if (removedCaptureDevice.Indexes.Count > 0)
              {
                updatedProtoLocation.CaptureDevice = new BioService.CaptureDevice()
                {
                    Id       = existingCaptureDevice.Id
                  , Dbresult = BioService.Result.Success
                };
                updatedProtoLocation.Dbresult = BioService.Result.Success;
              }
              else
              {
                updatedProtoLocation.CaptureDevice = new BioService.CaptureDevice()
                {
                    Id       = existingCaptureDevice.Id
                  , Dbresult = BioService.Result.Failed
                };
                updatedProtoLocation.Dbresult = BioService.Result.Failed;
                return updatedProtoLocation;
              }
            }            
          }
          else
          {
            BioService.CaptureDevice newCaptureDevice = _captureDeviceDataClient.Add(existingLocation, location.CaptureDevice, dataContext);
            if (newCaptureDevice.Id > 0)
            {
              updatedProtoLocation.CaptureDevice = newCaptureDevice;
              updatedProtoLocation.Dbresult      = BioService.Result.Success;
            }
          }
        }

        //PersonAccess
        if(location.EntityState == BioService.EntityState.Modified)
        {
          RepeatedField<BioService.Person> results = _personAccessDataClient.Update(existingLocation, location, dataContext);
          if (results.Count > 0)
          {
            updatedProtoLocation.Persons.Add(results);
            updatedProtoLocation.Dbresult = BioService.Result.Success;
          }
          updatedProtoLocation.AccessType = location.AccessType;
        }
        updatedProtoLocation.Id          = location.Id;
        updatedProtoLocation.EntityState = location.EntityState;

      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return updatedProtoLocation;
    }

    public BioService.RawIndexes Remove(BioService.RawIndexes items)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Remove(items, dataContext);
      }
    }

    public BioService.RawIndexes Remove(BioService.RawIndexes items, BioSkyNetDataModel dataContext)
    {
      BioService.RawIndexes removedItems = new BioService.RawIndexes();
      if (items == null || items.Indexes.Count <= 0)
        return removedItems;

      try
      {
        var existingLocations = dataContext.Location.Where(x => items.Indexes.Contains(x.Id));

        if (existingLocations == null)
          return removedItems;

        _rawIndexes              .Indexes.Clear();
        _accessDevicesRawIndexes .Indexes.Clear();
        _caprureDevicesRawIndexes.Indexes.Clear();
        _visitorsRawIndexes      .Indexes.Clear();

        Location location = existingLocations.FirstOrDefault();

        if (location == null)
          return removedItems;

        //AccessDevice
        foreach (AccessDevice accessDevice in location.AccessDevice)
          _accessDevicesRawIndexes.Indexes.Add(accessDevice.Id);

        location.AccessDevice.Clear();        

        //CaptureDevice
        foreach (CaptureDevice captureDevice in location.CaptureDevice)
          _caprureDevicesRawIndexes.Indexes.Add(captureDevice.Id);

        location.CaptureDevice.Clear();       

        //PersonAccess
        location.Access_Map_Id = null;
        location.PersonAccess  = null;
        _rawIndexes.Indexes.Add(location.Id);  

        foreach (long id in _rawIndexes.Indexes)
        {
          BioService.Location locationItem = new BioService.Location()
          {
              Id = id
            , AccessType = BioService.Location.Types.AccessType.None
          };

          Location currenLocation = existingLocations.Where(x => x.Id == id).FirstOrDefault();
          if (location != null)
            _personAccessDataClient.Update(currenLocation, locationItem, dataContext);
        }

        BioService.RawIndexes accessDeviceIndexes = _accessDeviceDataClient.Remove(_accessDevicesRawIndexes);
        BioService.RawIndexes captureDeviceIndexes = _captureDeviceDataClient.Remove(_caprureDevicesRawIndexes);

        var deletedLocations = dataContext.Location.RemoveRange(existingLocations);
        int affectedRows = dataContext.SaveChanges();
        if (deletedLocations.Count() == affectedRows)
          return items;
        else
        {
          foreach (long id in items.Indexes)
          {
            if (dataContext.Location.Find(id) == null)
              removedItems.Indexes.Add(id);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return removedItems;
    }

    public BioService.LocationList Select(BioService.QueryLocations query, BioSkyNetDataModel dataContext)
    {
      BioService.LocationList locations = new BioService.LocationList();

      try
      {
        IQueryable<Location> locationEntities = dataContext.Location;
        foreach (Location p in locationEntities)
        {
          BioService.Location protoLocation = _convertor.GetLocationProto(p);
          if (protoLocation != null)
            locations.Locations.Add(protoLocation);
        }
      }
      catch (Exception ex)  {
        Console.WriteLine(ex.Message);
      }
      return locations;
    }

    public BioService.LocationList Select(BioService.QueryLocations query)
    {
      using (var DataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Select(query, DataContext);
      }
    }

    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;

    private readonly AccessDeviceDataClient  _accessDeviceDataClient  ;
    private readonly CaptureDeviceDataClient _captureDeviceDataClient ;
    private readonly PersonAccessDataClient  _personAccessDataClient  ;
    private          BioService.RawIndexes   _rawIndexes              ;
    private          VisitorDataClient       _visitorDataClient       ;
    private          BioService.RawIndexes   _accessDevicesRawIndexes ;
    private          BioService.RawIndexes   _caprureDevicesRawIndexes;
    private          BioService.RawIndexes   _visitorsRawIndexes      ;

  }
}
