using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;
using Google.Protobuf.Collections;
using System.Collections.Generic;

namespace BioData.DataClients
{
  public class PersonAccessDataClient
  {
    public PersonAccessDataClient(IProcessorLocator locator)
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();
    }
       
    public BioService.AccessInfo Update( Location existingLocation
                                       , BioService.AccessInfo request
                                       , BioSkyNetDataModel dataContext )
    {
      if (request == null || request.EntityState == BioService.EntityState.Unchanged)
        return null;

      BioService.AccessInfo response = new BioService.AccessInfo() { EntityState = BioService.EntityState.Modified
                                                                   , Dbresult = BioService.Result.Failed };

      try
      {
        IQueryable<PersonAccess> existingRecords = dataContext.PersonAccess.Where(x => x.Location_Id == existingLocation.Id);
     
        if (request.AccessType == BioService.AccessInfo.Types.AccessType.Custom)
        {
          List<PersonAccess> listToAdd  = new List<PersonAccess>();
          HashSet<long> existingItems   = new HashSet<long>(existingRecords.Select(x => x.Person_Id));
          HashSet<long> itemsToRemove   = new HashSet<long>(request.Persons.Where(x => 
                                                                             x.EntityState == BioService.EntityState.Deleted)
                                                                             .Select( x => x.Id) );
          HashSet<long> itemsToAdd    = new HashSet<long>(request.Persons.Where(x => 
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

        existingLocation.Access_Type = (byte)request.AccessType;

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;           

        if ( request.AccessType == BioService.AccessInfo.Types.AccessType.Custom)
        {
          IQueryable<PersonAccess> newExistingRecords = dataContext.PersonAccess.Where(x => x.Location_Id == existingLocation.Id);
          foreach (PersonAccess pa in newExistingRecords)
            response.Persons.Add(new BioService.Person() { Id = pa.Person_Id });
        }        
       
        response.AccessType = request.AccessType;        
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
    }

    /*
    private BioService.AccessInfo.Types.AccessType GetAccessType(int personAccessCount, int allPersonsCount)
    {
      BioService.AccessInfo.Types.AccessType result = BioService.AccessInfo.Types.AccessType.Custom;
      if (personAccessCount == 0)
        result = BioService.AccessInfo.Types.AccessType.None;
      else if (personAccessCount == allPersonsCount)
        result = BioService.AccessInfo.Types.AccessType.All;

      return result;
    }
    */

    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;
  }
}
