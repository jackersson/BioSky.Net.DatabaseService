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
  public class LocationDataHolder : DataHolderBase<Location>
  {

    public LocationDataHolder( IProcessorLocator locator)
                             : base(locator)
    {

    }

    public BioService.Location UpdateFromProto(BioService.Location proto)
    {
      /*
      Location entity = _convertor.GetLocationEntity(proto);

      if (entity == null)
        return null;

      BioService.EntityState entityState = proto.EntityState;

      switch (entityState)
      {
        case BioService.EntityState.Added:
        {
            Location newEntity = Add(entity);
          if (newEntity != null)
          {
            BioService.Location newProto = _convertor.GetLocationProto(newEntity);
            newProto.EntityState = entityState;
            newProto.Dbresult    = BioService.ResultStatus.Success;
            return newProto;
          }
          else
          {
            return new BioService.Location() { Dbresult = BioService.ResultStatus.Failed
                                                 , EntityState = entityState };
          }            
        }

        case BioService.EntityState.Modified:
        {
           Location updatedEntity = Update(entity);
           if (updatedEntity != null)
           {
               BioService.Location updatedlocation = _convertor.GetLocationProto(updatedEntity);
               updatedlocation.Dbresult    = BioService.ResultStatus.Success;
               updatedlocation.EntityState = entityState;
               return updatedlocation;
           }
          return new BioService.Location()
                 { Id = entity.Id
                 , Dbresult = updatedEntity == null ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };           
        }

        case BioService.EntityState.Deleted:
        {
          bool state = Remove(entity);
          return new BioService.Location()
                 { Id = entity.Id
                 , Dbresult = state ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };                
        }                
      }
      */
      return new BioService.Location() { Id = proto.Id };     
    }

    public override Location Add(Location item)
    {
      _dataContext.Location.Add(item);
      bool success = Save();
      return (success) ? item : null;
    }

    public override bool Remove(Location item)
    {
      bool success = false;
      var original = _dataContext.Location.Find(item.Id);
      if (original != null)
      {
          _dataContext.Location.Remove(original);
          success = Save();
      }     
     
      return success;
    }

    public override Location Update(Location item)
    {
      bool success = false;
      try
      {
        var original = _dataContext.Location.Find(item.Id);

        if (original != null)
        {
            if (item.Location_Name != "")
                original.Location_Name = item.Location_Name;

            if (item.Description != "")
                original.Description = item.Description;
            
            success = Save();
        }
      }
      catch (Exception ex)
      {
          Console.WriteLine(ex.Message);
      }

      return (success) ? item : null;   
     
    }

    public DbSet<Location> Select()
    {
      return _dataContext.Location;
    }
  }
}
