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

    public BioService.AccessDevice Add(Location existingLocation, BioService.AccessDevice item, BioSkyNetDataModel dataContext)
    {
      BioService.AccessDevice newProtoAccessDevice = new BioService.AccessDevice() { };
      if (item == null)
        return newProtoAccessDevice;

      try
      {
        AccessDevice existingAccessDevice = dataContext.AccessDevice.Where(x => x.PortName == item.Portname).FirstOrDefault();

        if ( existingAccessDevice == null )
           existingAccessDevice = _convertor.GetAccessDeviceEntity(item);


        if (existingLocation.AccessDevice.Count > 0)
        {
          BioService.RawIndexes items = new BioService.RawIndexes();
          foreach (AccessDevice cp in existingLocation.AccessDevice)
            items.Indexes.Add(cp.Id);

          Remove(items);
        }

        existingLocation.AccessDevice.Add(existingAccessDevice);   

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)
          newProtoAccessDevice.Id = existingAccessDevice.Id;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return newProtoAccessDevice;
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
        var existingItems = dataContext.AccessDevice.Where(x => items.Indexes.Contains(x.Id));

        if (existingItems == null)
          return removedItems;

        foreach(AccessDevice accessDevice in existingItems)
          accessDevice.Location_Id = null;        

        var deletedItems = dataContext.AccessDevice.RemoveRange(existingItems);
        int affectedRows = dataContext.SaveChanges();
        if (deletedItems.Count() == affectedRows)
          return items;
        else
        {
          foreach (long id in items.Indexes)
          {
            if (dataContext.AccessDevice.Find(id) == null)
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

    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;
  }
}
