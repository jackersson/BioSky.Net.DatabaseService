
using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataClients
{
  public class FingerprintDeviceDataClient
  {
    public FingerprintDeviceDataClient(IProcessorLocator locator)
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();
    }

    public BioService.FingerprintDevice Add(Location existingLocation, BioService.FingerprintDevice item)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(existingLocation, item, dataContext);
      }
    }

    public BioService.FingerprintDevice Add( Location existingLocation
                                           , BioService.FingerprintDevice request
                                           , BioSkyNetDataModel dataContext)
    {
      BioService.FingerprintDevice response = new BioService.FingerprintDevice() { Dbresult = BioService.Result.Failed
                                                                                       , EntityState = BioService.EntityState.Added };
      if (request == null || existingLocation == null)
        return response;

      try
      {
        FingerprintDevice existingDevice = dataContext.FingerprintDevice.Where(x => x.SerialNumber == request.SerialNumber
                                                                                 && x.DeviceName   == request.Devicename   )
                                                                                 .FirstOrDefault();

        if (existingDevice == null)
          existingDevice = _convertor.GetFingerprintDeviceEntity(request);
        else
          existingDevice.Location.Clear();

        existingLocation.FingerprintDevice = existingDevice;

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)
          response.Dbresult = BioService.Result.Success;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return response;
    }

    public void Update(Location entity, BioService.FingerprintDevice request, BioService.Location response, BioSkyNetDataModel dataContext)
    {
      if (request == null)
        return;

      if (request.EntityState == BioService.EntityState.Added)
        response.FingerprintDevice = Add(entity, request, dataContext);
      else if (request.EntityState == BioService.EntityState.Deleted)
        Remove(entity, response, dataContext);
    }

    public void Remove(Location entity, BioService.Location response, BioSkyNetDataModel dataContext)
    {
      bool hasResponse = response != null;
      if (entity == null)
        return;

      if (hasResponse)
        response.FingerprintDevice = new BioService.FingerprintDevice() { EntityState = BioService.EntityState.Deleted, Dbresult = BioService.Result.Failed };
      try
      {
        FingerprintDevice existingEntity = entity.FingerprintDevice;

        if (existingEntity == null)
        {
          if (hasResponse)
            response.FingerprintDevice.Dbresult = BioService.Result.Success;
          return;
        }

        dataContext.FingerprintDevice.Remove(existingEntity);
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return;

        if (hasResponse)
          response.FingerprintDevice.Dbresult = BioService.Result.Success;
      }
      catch (Exception ex){
        Console.WriteLine(ex.Message);
      }
    }

    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;
  }
}
