using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;
using Google.Protobuf.Collections;
using System.Collections.Generic;

namespace BioData.DataHolders.DataClient
{
  public class PersonAccessDataClient
  {
    public PersonAccessDataClient(IProcessorLocator locator)
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();
    }
       
    public RepeatedField<BioService.Person> Update(Location existingLocation, BioService.Location item, BioSkyNetDataModel dataContext)
    {
      RepeatedField<BioService.Person > newProtoLocation = new RepeatedField<BioService.Person>();
      if (item == null )
        return newProtoLocation;

      try
      {
        IQueryable<PersonAccess> existingRecords = dataContext.PersonAccess.Where(x => x.Location_Id == existingLocation.Id);
     
        if (item.AccessType == BioService.Location.Types.AccessType.None)        
          dataContext.PersonAccess.RemoveRange(existingRecords);
        else if (item.AccessType == BioService.Location.Types.AccessType.All)
        {
          HashSet<long> personsToAdd = new HashSet<long>();
          foreach (PersonAccess pa in existingRecords)
            personsToAdd.Add(pa.Person_Id);
         
          IQueryable<Person> persons      = dataContext.Person.Where(x => !personsToAdd.Contains(x.Id));

          List<PersonAccess> personAccess = new List<PersonAccess>();
          foreach (Person pp in persons)
            personAccess.Add(new PersonAccess() { Location_Id = existingLocation.Id, Person_Id = pp.Id });

          dataContext.PersonAccess.AddRange(personAccess);
        }
        else
        {
          HashSet<long>      itemsToRemove = new HashSet<long>();          
          List<PersonAccess> itemsToAdd    = new List<PersonAccess>();

          foreach (BioService.Person pp in item.Persons)
          {
            if (pp.EntityState == BioService.EntityState.Deleted)
              itemsToRemove.Add(pp.Id);
            else if (pp.EntityState == BioService.EntityState.Added)
            {
              PersonAccess pa = new PersonAccess() { Location_Id = existingLocation.Id, Person_Id = pp.Id };
              itemsToAdd.Add(pa);
            }     
          }

          dataContext.PersonAccess.AddRange(itemsToAdd);
          IQueryable<PersonAccess> toRemove = dataContext.PersonAccess.Where(x => x.Location_Id == existingLocation.Id 
                                                                               && itemsToRemove.Contains(x.Person_Id) );
          dataContext.PersonAccess.RemoveRange(toRemove);
        }
        
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)
        {
          IQueryable<PersonAccess> newExistingRecords = dataContext.PersonAccess.Where(x => x.Location_Id == existingLocation.Id);
          foreach (PersonAccess pa in newExistingRecords)          
            newProtoLocation.Add(new BioService.Person() { Id = pa.Person_Id });       
        }
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return newProtoLocation;
    }
    
    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;
  }
}
