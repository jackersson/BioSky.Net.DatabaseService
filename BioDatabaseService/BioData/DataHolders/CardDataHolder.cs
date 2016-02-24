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
  public class CardDataHolder : DataHolderBase<Card>
  {
    public CardDataHolder( IProcessorLocator locator)
                          : base(locator)
    {

    }

    public BioService.Card UpdateFromProto(BioService.Card proto)
    {
      Card entity = _convertor.GetCardEntity(proto);

      if (entity == null)
        return null;

      BioService.EntityState entityState = proto.EntityState;

      switch (entityState)
      {
        case BioService.EntityState.Added:
        {
            Card newEntity = Add(entity);
          if (newEntity != null)
          {
            BioService.Card newProto = _convertor.GetCardProto(newEntity);
            newProto.EntityState = entityState;
            newProto.Dbresult    = BioService.ResultStatus.Success;
            return newProto;
          }
          else
          {
            return new BioService.Card() { Dbresult = BioService.ResultStatus.Failed
                                                 , EntityState = entityState };
          }            
        }

        case BioService.EntityState.Modified:
        {
            Card updatedEntity = Update(entity);
          return new BioService.Card()
                 { Id = entity.Id
                 , Dbresult = updatedEntity == null ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };           
        }

        case BioService.EntityState.Deleted:
        {
          bool state = Remove(entity);
          return new BioService.Card()
                 { Id = entity.Id
                 , Dbresult = state ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState
                 , UniqueNumber = entity.Unique_Number
                 };                
        }                
      }

      return proto;      
    }

    public override Card Add(Card item)
    {
      _dataContext.Card.Add(item);
      bool success = Save();
      return (success) ? item : null;
    }

    public override bool Remove(Card item)
    {
      bool success = false;
      var original = _dataContext.Card.Find(item.Id);
      if (original != null)
      {
          _dataContext.Card.Remove(original);
          success = Save();
      }     
     
      return success;
    }

    public override Card Update(Card item)
    {
        bool success = false;
        try
        {
            var original = _dataContext.Card.Find(item.Id);

            if (original != null)
            {              

                if (item.Person_Id > 0)
                    original.Person_Id = item.Person_Id;
                
                success = Save();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return (success) ? item : null;   
    }

    public DbSet<Card> Select()
    {
      return _dataContext.Card;
    }

  }
}
