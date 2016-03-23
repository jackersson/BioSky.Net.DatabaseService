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
  public class AccessDeviceDataHolder : DataHolderBase<AccessDevice>
  {
    public AccessDeviceDataHolder( IProcessorLocator locator)
                                  : base(locator)
    {

    }

    public BioService.AccessDevice UpdateFromProto(BioService.AccessDevice proto)
    {
      /*
      AccessDevice entity = _convertor.GetAccessDeviceEntity(proto);

      if (entity == null)
        return null;

      BioService.EntityState entityState = proto.EntityState;

      switch (entityState)
      {
        case BioService.EntityState.Added:
        {
          AccessDevice newEntity = Add(entity);
          if (newEntity != null)
          {
            BioService.AccessDevice newProto = _convertor.GetAccessDeviceProto(newEntity);
            newProto.EntityState = entityState;
            newProto.Dbresult    = BioService.ResultStatus.Success;
            return newProto;
          }
          else
          {
            return new BioService.AccessDevice() { Dbresult = BioService.ResultStatus.Failed
                                                 , EntityState = entityState };
          }            
        }

        case BioService.EntityState.Modified:
        {
          AccessDevice updatedEntity = Update(entity);
          return new BioService.AccessDevice()
                 { Id = entity.Id
                 , Dbresult = updatedEntity == null ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };           
        }

        case BioService.EntityState.Deleted:
        {
          bool state = Remove(entity);
          return new BioService.AccessDevice()
                 { Id = entity.Id
                 , Dbresult = state ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };                
        }                
      }
      */
      return proto;      
    }

    public override AccessDevice Add(AccessDevice item)
    {
      _dataContext.AccessDevice.Add(item);
      bool success = Save();
      return (success) ? item : null;
    }

    public override bool Remove(AccessDevice item)
    {
      bool success = false;
      using (var nDataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {          
          var original = nDataContext.AccessDevice.Find(item.Id);
          if (original != null)
          {
              nDataContext.AccessDevice.Remove(original);
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

    public override AccessDevice Update(AccessDevice item)
    {
      bool success = false;
      try
      {
         var original = _dataContext.AccessDevice.Find(item.Id);

         if (original != null)
         {            
             if (item.Location_Id > 0)
                 original.Location_Id = item.Location_Id;

             original.Location_Type = item.Location_Type;

             success = Save();
         }
      }
      catch (Exception ex)
      {
          Console.WriteLine(ex.Message);
      }

      return (success) ? item : null;   
    
    }

    public DbSet<AccessDevice> Select()
    {
      return _dataContext.AccessDevice;
    }
  }
}
