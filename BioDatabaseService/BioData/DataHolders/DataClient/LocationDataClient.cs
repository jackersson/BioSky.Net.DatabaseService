using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.DataHolders.DataClient
{
  public class LocationDataClient
  {
    public LocationDataClient( IProcessorLocator locator
                              , AccessDeviceDataClient  accessDeviceDataClient
                              , CaptureDeviceDataClient captureDeviceDataClient
                              , PersonAccessDataClient  personAccessDataClient   )
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();

      _accessDeviceDataClient  = accessDeviceDataClient ;
      _captureDeviceDataClient = captureDeviceDataClient;
      _personAccessDataClient  = personAccessDataClient ;
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

        newProtoLocation.Persons.Add(_personAccessDataClient.Add(newLocation, location, dataContext));

        if (newProtoLocation.Persons.Count == 0)
          newLocation.Access_Type = (byte)BioService.Location.Types.AccessType.None;
        else if (newProtoLocation.Persons.Count == dataContext.Person.Count())
          newLocation.Access_Type = (byte)BioService.Location.Types.AccessType.All;
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


        if (location.AccessDevice != null)
        {
          BioService.AccessDevice newAccessDevice = _accessDeviceDataClient.Add(existingLocation, location.AccessDevice, dataContext);
          if (newAccessDevice.Id > 0)
          {
            updatedProtoLocation.AccessDevice = newAccessDevice;
            updatedProtoLocation.Dbresult = BioService.Result.Success;
          }
        }

        if (location.CaptureDevice != null)
        {
          BioService.CaptureDevice newCaptureDevice = _captureDeviceDataClient.Add(existingLocation, location.CaptureDevice, dataContext);
          if (newCaptureDevice.Id > 0)
          {
            updatedProtoLocation.CaptureDevice = newCaptureDevice;
            updatedProtoLocation.Dbresult = BioService.Result.Success;
          }
        }

        RepeatedField<BioService.Person> results = _personAccessDataClient.Add(existingLocation, location, dataContext);
        if (results.Count > 0)
        {
          updatedProtoLocation.Persons.Add(results);
          updatedProtoLocation.Dbresult = BioService.Result.Success;
        }          
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

        var deletedLocations = dataContext.Location.RemoveRange(existingLocations);
        int affectedRows     = dataContext.SaveChanges();
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
      catch (Exception ex) {
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

    private readonly AccessDeviceDataClient  _accessDeviceDataClient ;
    private readonly CaptureDeviceDataClient _captureDeviceDataClient;
    private readonly PersonAccessDataClient  _personAccessDataClient ;
  }
}
