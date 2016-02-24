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
  public class VisitorDataHolder : DataHolderBase<Visitor>
  {
    public VisitorDataHolder( IProcessorLocator locator)
                            : base(locator)
    {

    }

    public BioService.Visitor UpdateFromProto(BioService.Visitor proto)
    {
      Visitor entity = _convertor.GetVisitorEntity(proto);

      if (entity == null)
        return null;

      BioService.EntityState entityState = proto.EntityState;

      switch (entityState)
      {
        case BioService.EntityState.Added:
        {
          Visitor newEntity = Add(entity);
          if (newEntity != null)
          {
            BioService.Visitor newProto = _convertor.GetVisitorProto(newEntity);
            newProto.EntityState = entityState;
            newProto.Dbresult    = BioService.ResultStatus.Success;
            return newProto;
          }
          else
          {
            return new BioService.Visitor() { Dbresult = BioService.ResultStatus.Failed
                                                 , EntityState = entityState };
          }            
        }

        case BioService.EntityState.Modified:
        {
          Visitor updatedEntity = Update(entity);
          return new BioService.Visitor()
                 { Id = entity.Id
                 , Dbresult = updatedEntity == null ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };           
        }

        case BioService.EntityState.Deleted:
        {
          bool state = Remove(entity);
          return new BioService.Visitor()
                 { Id = entity.Id
                 , Dbresult = state ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };                
        }                
      }

      return new BioService.Visitor() { Id = proto.Id };      
    }

    public override Visitor Add(Visitor item)
    {
      _dataContext.Visitor.Add(item);
      bool success = Save();
      return (success) ? item : null;
    }

    public override bool Remove(Visitor item)
    {
      bool success = false;
      var original = _dataContext.Visitor.Find(item.Id);
      if (original != null)
      {
          _dataContext.Visitor.Remove(original);
          success = Save();
      }       
      return success;
    }

    public override Visitor Update(Visitor item)
    {
        bool success = false;
        try
        {
            var original = _dataContext.Visitor.Find(item.Id);

            if (original != null)
            {               
                if (item.Person_ID > 0)
                    original.Person_ID = item.Person_ID;

                success = Save();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return (success) ? item : null;   
    }

    public DbSet<Visitor> Select()
    {
        try
        {
            return _dataContext.Visitor;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
      
    }
  }
}
