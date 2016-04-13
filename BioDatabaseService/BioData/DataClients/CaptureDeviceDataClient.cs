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

    public BioService.CaptureDevice Add(Location existingLocation, BioService.CaptureDevice item, BioSkyNetDataModel dataContext)
    {
      BioService.CaptureDevice newProtoCaptureDevice = new BioService.CaptureDevice() { };
      if (item == null)
        return newProtoCaptureDevice;

      try
      {
        CaptureDevice existingCaptureDevice = dataContext.CaptureDevice.Where(x => x.Device_Name == item.Devicename).FirstOrDefault();

        if (existingCaptureDevice == null)        
          existingCaptureDevice = _convertor.GetCaptureDeviceEntity(item);

        if (existingLocation.CaptureDevice.Count > 0)
        {
          BioService.RawIndexes items = new BioService.RawIndexes();
          foreach (CaptureDevice cp in existingLocation.CaptureDevice)          
            items.Indexes.Add(cp.Id);

          Remove(items, dataContext);
        }
        
        existingLocation.CaptureDevice.Add(existingCaptureDevice);    
        
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)        
          newProtoCaptureDevice.Id = existingCaptureDevice.Id;              
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return newProtoCaptureDevice;
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
        var existingItems = dataContext.CaptureDevice.Where(x => items.Indexes.Contains(x.Id));

        if (existingItems == null)
          return removedItems;

        foreach(CaptureDevice captureDevice in existingItems)        
          captureDevice.Location_Id = null;

        var deletedItems = dataContext.CaptureDevice.RemoveRange(existingItems);
        int affectedRows = dataContext.SaveChanges();
        if (deletedItems.Count() == affectedRows)
          return items;
        else
        {
          foreach (long id in items.Indexes)
          {
            if (dataContext.CaptureDevice.Find(id) == null)
              removedItems.Indexes.Add(id);
          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return removedItems;
    }

    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;
  }
}
