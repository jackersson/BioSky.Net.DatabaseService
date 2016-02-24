using BioContracts;
using BioData.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.DataHolders
{
  public class CaptureDeviceDataHolder : DataHolderBase<CaptureDevice>
  {
    public CaptureDeviceDataHolder( IProcessorLocator locator)
                                  : base(locator)
    {

    }

    public BioService.CaptureDevice UpdateFromProto(BioService.CaptureDevice proto)
    {
      CaptureDevice entity = _convertor.GetCaptureDeviceEntity(proto);

      if (entity == null)
        return null;

      BioService.EntityState entityState = proto.EntityState;

      switch (entityState)
      {
        case BioService.EntityState.Added:
        {
          CaptureDevice newEntity = Add(entity);
          if (newEntity != null)
          {
            BioService.CaptureDevice newProto = _convertor.GetCaptureDeviceProto(newEntity);
            newProto.EntityState = entityState;
            newProto.Dbresult    = BioService.ResultStatus.Success;
            return newProto;
          }
          else
          {
            return new BioService.CaptureDevice() { Dbresult = BioService.ResultStatus.Failed
                                                 , EntityState = entityState };
          }            
        }

        case BioService.EntityState.Modified:
        {
          CaptureDevice updatedEntity = Update(entity);
          return new BioService.CaptureDevice()
                 { Id = entity.Id
                 , Dbresult = updatedEntity == null ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };           
        }

        case BioService.EntityState.Deleted:
        {
          bool state = Remove(entity);
          return new BioService.CaptureDevice()
                 { Id = entity.Id
                 , Dbresult = state ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };                
        }                
      }

      return proto;      
    }

    public override CaptureDevice Add(CaptureDevice item)
    {
      _dataContext.CaptureDevice.Add(item);
      bool success = Save();
      return (success) ? item : null;
    }

    public override bool Remove(CaptureDevice item)
    {
        bool success = false;
        using (var nDataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
        {
            var original = nDataContext.CaptureDevice.Find(item.Id);
            if (original != null)
            {
                nDataContext.CaptureDevice.Remove(original);
                try
                {
                    nDataContext.SaveChanges();
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        return success;
    }

    public override CaptureDevice Update(CaptureDevice item)
    {
      bool success = false;
      try
      {
         var original = _dataContext.CaptureDevice.Find(item.Id);

         if (original != null)
         {
             if (item.Location_Id > 0)
               original.Location_Id = item.Location_Id;
            
             success = Save();
         }
      }
      catch (Exception ex)
      {
          Console.WriteLine(ex.Message);
      }

      return (success) ? item : null;      
    }

    public DbSet<CaptureDevice> Select()
    {
      return _dataContext.CaptureDevice;
    }
  }
}
