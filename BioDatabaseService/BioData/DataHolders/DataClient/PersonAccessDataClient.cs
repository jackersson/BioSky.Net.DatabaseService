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
     
        if (item.AccessType == BioService.Location.Types.AccessType.Custom)
        {
          List<PersonAccess> listToAdd     = new List<PersonAccess>();
          HashSet<long> existingItems = new HashSet<long>(existingRecords.Select(x => x.Person_Id));
          HashSet<long> itemsToRemove = new HashSet<long>(item.Persons.Where(x => 
                                                                             x.EntityState == BioService.EntityState.Deleted)
                                                                             .Select( x => x.Id) );
          HashSet<long> itemsToAdd    = new HashSet<long>(item.Persons.Where(x => 
                                                                             x.EntityState == BioService.EntityState.Added  )
                                                                             .Select(x => x.Id ) );

          itemsToRemove.IntersectWith(existingItems);
          itemsToAdd   .ExceptWith   (existingItems);

          foreach (long id in itemsToAdd)
          {          
            PersonAccess pa = new PersonAccess() { Location_Id = existingLocation.Id, Person_Id = id };
            listToAdd.Add(pa);            
          }      

          dataContext.PersonAccess.AddRange(listToAdd);

          IQueryable<PersonAccess> toRemove = dataContext.PersonAccess.Where(x => x.Location_Id == existingLocation.Id 
                                                                               && itemsToRemove.Contains(x.Person_Id) );
          dataContext.PersonAccess.RemoveRange(toRemove);
        }
        else
          dataContext.PersonAccess.RemoveRange(existingRecords);

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
