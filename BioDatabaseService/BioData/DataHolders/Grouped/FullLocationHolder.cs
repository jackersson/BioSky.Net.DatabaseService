using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.DataHolders.Grouped
{
    public class FullLocationHolder
  {
    public FullLocationHolder(IProcessorLocator locator)
    {
      _locator = locator;
       
      _locations      = locator.GetProcessor<LocationDataHolder>();
      _accessDevices  = locator.GetProcessor<AccessDeviceDataHolder>();
      _captureDevices = locator.GetProcessor<CaptureDeviceDataHolder>();

      _convertor = new ProtoMessageConvertor();
    }

    public BioService.LocationList Update(BioService.LocationList proto)
    {
      BioService.LocationList locations = new BioService.LocationList();
      foreach (BioService.Location curProto in proto.Locations)
      {
        BioService.Location updatedProto = Update(curProto);
        if (updatedProto != null)
          locations.Locations.Add(updatedProto);
      }

      return locations;
    }

    public BioService.Location Update( BioService.Location proto )
    {  
      BioService.Location updated = _locations.UpdateFromProto(proto);

  
      if (updated == null)
        return null;

      if (updated.Id == 0)
        return updated;

      IQueryable<AccessDevice> accessDevicesToRemove = _accessDevices.Select().Where(x => x.Location_Id == updated.Id);
      foreach (AccessDevice ac in accessDevicesToRemove)      
          _accessDevices.Remove(ac);
      

      IQueryable<CaptureDevice> captureDevicesToRemove = _captureDevices.Select().Where(x => x.Location_Id == updated.Id);
      foreach (CaptureDevice cp in captureDevicesToRemove)
          _captureDevices.Remove(cp);         
      
      
      long targetID = updated.Id;
      foreach (BioService.AccessDevice ac in proto.AccessDevices)
      {
        if (ac.Locationid != targetID)
          ac.Locationid = targetID;
        BioService.AccessDevice updatedADProto = _accessDevices.UpdateFromProto(ac);
        if (updatedADProto != null)
          updated.AccessDevices.Add(updatedADProto);
      }


      foreach (BioService.CaptureDevice ac in proto.CaptureDevices)
      {
        if (ac.Locationid != targetID)
          ac.Locationid = targetID;
        BioService.CaptureDevice updatedCDProto = _captureDevices.UpdateFromProto(ac);
        if (updatedCDProto != null)
          updated.CaptureDevices.Add(updatedCDProto);
      }

      return updated;
    }

    public BioService.LocationList Select( BioService.CommandLocations command)
    {
      BioService.LocationList result = new BioService.LocationList();

      DbSet<Location> locations           = _locations.Select();
      DbSet<AccessDevice> accessDevices   = _accessDevices.Select();
      DbSet<CaptureDevice> captureDevices = _captureDevices.Select();

        
      foreach (Location location in locations)
      {
        BioService.Location protoLocation = _convertor.GetLocationProto(location);

        if (protoLocation == null)
          continue;

        long locationid = location.Id;
       
        IQueryable<AccessDevice> locationAccessDevices = accessDevices.Where(x => x.Location_Id == locationid);
        foreach (AccessDevice ac in locationAccessDevices)
        {
          BioService.AccessDevice currentAccessDeviceProto = _convertor.GetAccessDeviceProto(ac);
          if (currentAccessDeviceProto != null)
            protoLocation.AccessDevices.Add(currentAccessDeviceProto);
        }

        IQueryable<CaptureDevice> locationCaptureDevices = captureDevices.Where(x => x.Location_Id == locationid);
        foreach (CaptureDevice ac in locationCaptureDevices)
        {
          BioService.CaptureDevice currentCaptureDeviceProto = _convertor.GetCaptureDeviceProto(ac);
          if (currentCaptureDeviceProto != null)
            protoLocation.CaptureDevices.Add(currentCaptureDeviceProto);
        }

        result.Locations.Add(protoLocation);
      }
    
      return result;
    }

    
    private readonly IProcessorLocator _locator;

    private readonly ProtoMessageConvertor   _convertor     ;
    private readonly LocationDataHolder      _locations     ;
    private readonly AccessDeviceDataHolder  _accessDevices ;
    private readonly CaptureDeviceDataHolder _captureDevices;
  }
}
