using BioContracts;
using BioData.DataModels;
using System;
using System.Data.Entity;

namespace BioData.DataHolders
{
  public class PersonDataHolder : DataHolderBase<Person>
  {

    public PersonDataHolder( IProcessorLocator locator )
                           : base(locator)
    {
     
    }

    public BioService.Person UpdateFromProto(BioService.Person proto)
    {
      Person entity = _convertor.GetPersonEntity(proto);
      /*
      if (entity == null)
        return null;

      BioService.EntityState entityState = proto.EntityState;

      switch (entityState)
      {
        case BioService.EntityState.Added:
        {
            Person newEntity = Add(entity);
          if (newEntity != null)
          {
            BioService.Person newProto = _convertor.GetPersonProto(newEntity);
            newProto.EntityState = entityState;
           // newProto.Dbresult    = BioService.ResultStatus.Success;
            return newProto;
          }
          else
          {
            //return new BioService.Person() { Dbresult = BioService.ResultStatus.Failed
                                           //      , EntityState = entityState };
          }            
        }

        case BioService.EntityState.Modified:
        {
          Person updatedEntity = Update(entity);
          if (updatedEntity != null)
          {
              BioService.Person updatedperson = _convertor.GetPersonProto(updatedEntity);
              updatedperson.Dbresult    = BioService.ResultStatus.Success;
              updatedperson.EntityState = entityState;
              return updatedperson;
          }
          return new BioService.Person()
                 { Id = entity.Id
                 , Dbresult = updatedEntity == null ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };           
        }

        case BioService.EntityState.Deleted:
        {
          bool state = Remove(entity);
          return new BioService.Person()
                 { Id = entity.Id
                 , Dbresult = state ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };                
        }                
      }*/

      return new BioService.Person() { Id = proto.Id };      
    }

    public override Person Add(Person item)
    {
      _dataContext.Person.Add(item);
      bool success = Save();
      return (success) ? item : null;
    }

    public override bool  Remove(Person item)
    {
      bool success = false;
      var original = _dataContext.Person.Find(item.Id);
      if (original != null)
      {
          _dataContext.Person.Remove(original);
          success = Save();
      }
      return (success);
    }

    public override Person Update(Person item)
    {
      bool success = false;
      try
      {
         var original = _dataContext.Person.Find(item.Id);
      
         if (original != null)
         {
             original.First_Name_   = item.First_Name_;
             original.Last_Name_    = item.Last_Name_ ;
             original.Gender        = item.Gender;
             original.Email         = item.Email;
             original.Rights        = item.Rights;
            // original.Thumbnail     = item.Thumbnail;
             original.Country       = item.Country;
             original.Comments      = item.Comments;
             original.City          = item.City;
             original.Date_Of_Birth = item.Date_Of_Birth;
            // _dataContext.Entry(original).CurrentValues.SetValues(item);
             success = Save();
         }
      }
     catch (Exception ex)
     {
         Console.WriteLine(ex.Message);
     }
      
      return (success) ? item : null;
    }
    public DbSet<Person> Select()
    {
      return _dataContext.Person;
    }

  }
}
