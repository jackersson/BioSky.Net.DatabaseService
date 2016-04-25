using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataClients
{
  public class LocationDataClient
  {
    public LocationDataClient(  IProcessorLocator           locator
                              , AccessDeviceDataClient      accessDeviceDataClient
                              , CaptureDeviceDataClient     captureDeviceDataClient
                              , FingerprintDeviceDataClient fingerprintDeviceClient
                              , PersonAccessDataClient      personAccessDataClient 
                              , VisitorDataClient           visitorDataClient        )
    {
      _locator = locator;
      _convertor                   = new ProtoMessageConvertor();   

      _accessDeviceDataClient      = accessDeviceDataClient ;
      _captureDeviceDataClient     = captureDeviceDataClient;
      _fingerprintDeviceDataClient = fingerprintDeviceClient;
      _personAccessDataClient      = personAccessDataClient ;
      _visitorDataClient           = visitorDataClient      ;
    }

    public BioService.Location Add(BioService.Location location)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(location, dataContext);
      }
    }

   
    public BioService.Location Add(BioService.Location request, BioSkyNetDataModel dataContext)
    {
      BioService.Location response = new BioService.Location { Dbresult = BioService.Result.Failed, EntityState = BioService.EntityState.Added };
      if (request == null || ( request.CaptureDevice == null && request.AccessDevice == null && request.FingerprintDevice == null) )
        return response;

      try
      {        
        if (LocationExists(request.LocationName, request.MacAddress, dataContext))
          return response;
  
        Location newLocation = _convertor.GetLocationEntity(request);
        dataContext.Location.Add(newLocation);
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;

        response.Dbresult = BioService.Result.Success;
        response.Id       = newLocation.Id;

        UpdateDevices(newLocation, request, response, dataContext);
        
        response.AccessInfo = _personAccessDataClient.Update(newLocation, request.AccessInfo, dataContext);      
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
    }
    
    public BioService.Location Update(BioService.Location location)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Update(location, dataContext);
      }
    }
    
    public BioService.Location Update(BioService.Location request, BioSkyNetDataModel dataContext)
    {
      BioService.Location response = new BioService.Location { Id = request.Id
                                                             , Dbresult = BioService.Result.Failed
                                                             , EntityState = BioService.EntityState.Modified };

      bool hasMacAddress = !string.IsNullOrEmpty(request.MacAddress);
      if (request == null || !hasMacAddress)
        return response;
      
      try
      {
        Location existingLocation = dataContext.Location.Find(request.Id);

        if (existingLocation == null)
          return response;

        #region validation
             
        if (!string.IsNullOrEmpty(request.LocationName) && hasMacAddress)
        {
          string targetLocationName = request.LocationName;
          string targetMacAddress   = request.MacAddress  ;

          if (!LocationExists(targetLocationName, targetMacAddress, dataContext))
          {
            existingLocation.Location_Name = targetLocationName;
            existingLocation.MacAddress    = targetMacAddress  ;
          }
          else
            return response;
        }    
               
        if (!string.IsNullOrEmpty(request.Description))
          existingLocation.Description = _convertor.IsDeleteState(request.Description) ? string.Empty : request.Description;

        bool accessTypeChanged = false;
        bool hasAccessInfo     = request.AccessInfo != null;
        if (hasAccessInfo)
        {
          byte accesstype = (byte)request.AccessInfo.AccessType;
          accessTypeChanged = accesstype != existingLocation.Access_Type;
          if (accessTypeChanged)
            existingLocation.Access_Type = accesstype;
        }       
        #endregion

        int affectedRows = dataContext.SaveChanges();

        //if (affectedRows > 0)
        response.Dbresult = BioService.Result.Success;        

        UpdateDevices(existingLocation, request, response, dataContext);

        bool needToChangePersonAcceess = accessTypeChanged || ( hasAccessInfo && request.AccessInfo.Persons.Count > 0 );
        if (needToChangePersonAcceess)
          response.AccessInfo = _personAccessDataClient.Update(existingLocation, request.AccessInfo, dataContext);        
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
    }

    public BioService.Location Remove(BioService.Location request)
    {
      using (var DataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Remove(request, DataContext);
      }
    }

    public BioService.Location Remove(BioService.Location request, BioSkyNetDataModel dataContext)
    {
      BioService.Location response = new BioService.Location() { Dbresult = BioService.Result.Failed
                                                               , EntityState = BioService.EntityState.Deleted };
      if (request == null)
        return response;

      response.Id = request.Id;

      request.AccessInfo = new BioService.AccessInfo() { AccessType = BioService.AccessInfo.Types.AccessType.None };

      try
      {
        var entity = dataContext.Location.Find(request.Id);
        if (entity == null)
        {
          response.Dbresult = BioService.Result.Success;
          return response;
        }

        _accessDeviceDataClient     .Remove(entity, null              , dataContext);
        _captureDeviceDataClient    .Remove(entity, null              , dataContext);
        _fingerprintDeviceDataClient.Remove(entity, null              , dataContext);
        _personAccessDataClient     .Update(entity, request.AccessInfo, dataContext);

        dataContext.Location.Remove(entity);
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)        
          response.Dbresult = BioService.Result.Success;        
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
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

    private void UpdateDevices(  Location entity
                               , BioService.Location requested
                               , BioService.Location response
                               , BioSkyNetDataModel dataContext)
    {

      _accessDeviceDataClient     .Update(entity, requested.AccessDevice     , response, dataContext);
      _captureDeviceDataClient    .Update(entity, requested.CaptureDevice    , response, dataContext);
      _fingerprintDeviceDataClient.Update(entity, requested.FingerprintDevice, response, dataContext);
    }

    public bool LocationExists(string locationName, string macAddress, BioSkyNetDataModel dataContext)
    {
      return dataContext.Location.Where(x => x.Location_Name == macAddress && x.MacAddress == macAddress).Count() > 0;
    }

    public BioService.LocationList Select(BioService.QueryLocations query)
    {
      using (var DataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Select(query, DataContext);
      }
    }

    private IProcessorLocator     _locator  ;
    private ProtoMessageConvertor _convertor;

    private readonly AccessDeviceDataClient      _accessDeviceDataClient     ;
    private readonly CaptureDeviceDataClient     _captureDeviceDataClient    ;
    private readonly FingerprintDeviceDataClient _fingerprintDeviceDataClient;
    private readonly PersonAccessDataClient      _personAccessDataClient     ; 
    private          VisitorDataClient           _visitorDataClient          ;
 

  }
}
