using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataClients
{
  public class CaptureDeviceDataClient
  {
    public CaptureDeviceDataClient(IProcessorLocator locator)
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();
    }

    public BioService.CaptureDevice Add(Location existingLocation, BioService.CaptureDevice item)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(existingLocation, item, dataContext);
      }
    }

    public BioService.CaptureDevice Add(Location existingLocation, BioService.CaptureDevice request, BioSkyNetDataModel dataContext)
    {
      BioService.CaptureDevice response = new BioService.CaptureDevice() { Dbresult = BioService.Result.Failed
                                                                         , EntityState = BioService.EntityState.Added };
      if (request == null || existingLocation == null)
        return response;

      try
      {
        CaptureDevice existingDevice = dataContext.CaptureDevice.Where(x => x.Device_Name == request.Devicename).FirstOrDefault();

        if (existingDevice == null)        
          existingDevice = _convertor.GetCaptureDeviceEntity(request);
        else
          existingDevice.Location.Clear();

        existingLocation.CaptureDevice = existingDevice;    
        
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)        
          response.Dbresult = BioService.Result.Success;              
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
    }

    public void Update(Location entity, BioService.CaptureDevice request, BioService.Location response, BioSkyNetDataModel dataContext)
    {
      if (request == null)
        return;

      if (request.EntityState == BioService.EntityState.Added)
        response.CaptureDevice = Add(entity, request, dataContext);
      else if (request.EntityState == BioService.EntityState.Deleted)
        Remove(entity, response, dataContext);
    }

    public void Remove (Location entity, BioService.Location response, BioSkyNetDataModel dataContext)
    {
      bool hasResponse = response != null;
      if (entity == null)
        return;
      if (hasResponse)
        response.CaptureDevice = new BioService.CaptureDevice() { EntityState = BioService.EntityState.Deleted, Dbresult = BioService.Result.Failed };
      try
      {
        CaptureDevice existingEntity = entity.CaptureDevice;

        if (existingEntity == null)
        {
          if (hasResponse)
            response.CaptureDevice.Dbresult = BioService.Result.Success;
          return;
        }

        dataContext.CaptureDevice.Remove(existingEntity);
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return;

        if (hasResponse)
          response.CaptureDevice.Dbresult = BioService.Result.Success;            
      }
      catch (Exception ex)  {
        Console.WriteLine(ex.Message);
      }
    }    

    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;
  }
}
