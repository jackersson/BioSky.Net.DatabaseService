using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataClients
{
  public class AccessDeviceDataClient
  {
    public AccessDeviceDataClient(IProcessorLocator locator)
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();
    }

    public BioService.AccessDevice Add(Location existingLocation, BioService.AccessDevice item)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(existingLocation, item, dataContext);
      }
    }

    public BioService.AccessDevice Add(Location existingLocation, BioService.AccessDevice request, BioSkyNetDataModel dataContext)
    {
      BioService.AccessDevice response = new BioService.AccessDevice() { EntityState = BioService.EntityState.Added, Dbresult = BioService.Result.Failed };
      if (request == null || existingLocation == null)
        return response;

      try
      {
        AccessDevice existingDevice = dataContext.AccessDevice.Where(x => x.PortName == request.Portname).FirstOrDefault();

        if (existingDevice == null)
          existingDevice = _convertor.GetAccessDeviceEntity(request);
        else        
          existingDevice.Location.Clear();

        existingLocation.AccessDevice = existingDevice;   

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)
          response.Dbresult = BioService.Result.Success;
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
    }

    public void Update(Location entity, BioService.AccessDevice request, BioService.Location response, BioSkyNetDataModel dataContext)
    {
      if (request == null)
        return;

      if (request.EntityState == BioService.EntityState.Added)      
        response.AccessDevice = Add(entity, request, dataContext);      
      else if (request.EntityState == BioService.EntityState.Deleted)
        Remove(entity, response, dataContext);
    }

    public void Remove (Location entity, BioService.Location response, BioSkyNetDataModel dataContext)
    {
      bool hasResponse = response != null;
      if (entity == null)
        return;

      if (hasResponse)
        response.AccessDevice = new BioService.AccessDevice() { EntityState = BioService.EntityState.Deleted, Dbresult = BioService.Result.Failed };
      try
      {
        AccessDevice existingEntity = entity.AccessDevice;

        if (existingEntity == null)
        {
          if (hasResponse)
            response.AccessDevice.Dbresult = BioService.Result.Success;
          return;
        }           

        dataContext.AccessDevice.Remove(existingEntity);
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return;

        if (hasResponse)
          response.AccessDevice.Dbresult = BioService.Result.Success;
      }
      catch (Exception ex)  {
        Console.WriteLine(ex.Message);
      }
    }
   
    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;
  }
}
