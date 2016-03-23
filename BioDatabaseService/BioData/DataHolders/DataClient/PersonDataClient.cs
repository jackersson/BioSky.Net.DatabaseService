using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataHolders.DataClient
{
  public class PersonDataClient
  {
    public PersonDataClient(IProcessorLocator locator, PhotoDataClient photoDataClient)
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();

      _photoDataClient = photoDataClient;
    }

    public BioService.Person Update(BioService.Person person)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Update(person, dataContext);
      }      
    }
    //TODO make update for all person
    public BioService.Person Update(BioService.Person person, BioSkyNetDataModel dataContext)
    {
      BioService.Person updatedProtoPerson = new BioService.Person { Dbresult = BioService.Result.Failed };
      if (person == null)
        return updatedProtoPerson;
     
      try
      {
        Person existingPerson = dataContext.Person.Find(person.Id);

        if (existingPerson == null)
          return updatedProtoPerson;

        if (person.Firstname != "")
          existingPerson.First_Name_ = person.Firstname;

        if (person.Lastname != "")
          existingPerson.Last_Name_ = person.Lastname;

        int affectedRows = dataContext.SaveChanges();

        if (affectedRows > 0)
        {
          if (person.Firstname != "")
            updatedProtoPerson.Firstname = person.Firstname;

          if (person.Lastname != "")
            updatedProtoPerson.Lastname = person.Lastname;

          updatedProtoPerson.Dbresult = BioService.Result.Success;
        }
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }     

      return updatedProtoPerson;
    }

    public BioService.Person Add( BioService.Person person )
    {     
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(person, dataContext);
      }      
    }

    public BioService.Person Add(BioService.Person person, BioSkyNetDataModel dataContext)
    {
      BioService.Person newProtoPerson = new BioService.Person { Dbresult = BioService.Result.Failed };
      if (person == null || person.Thumbnail == null)
        return newProtoPerson;
     
      try
      {
        Person existingPerson = dataContext.Person.Where(x => x.First_Name_ == person.Firstname
                                                      && x.Last_Name_ == person.Lastname).FirstOrDefault();

        if (existingPerson != null)
          return newProtoPerson;

        Person newPerson = _convertor.GetPersonEntity(person);
        dataContext.Person.Add(newPerson);

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return newProtoPerson;

        newProtoPerson.Id = newPerson.Id;

        BioService.Photo requestedPhoto = person.Thumbnail;
        requestedPhoto.Personid = newPerson.Id;

        BioService.Photo insertedPhoto = _photoDataClient.Add(person.Thumbnail, dataContext);

        if (insertedPhoto.Dbresult != BioService.Result.Success)
        {
          Remove(newProtoPerson, dataContext);
          return newProtoPerson;
        }

        requestedPhoto.Id        = insertedPhoto.Id;        
        newProtoPerson.Photoid   = insertedPhoto.Id;
        newProtoPerson.Thumbnail = new BioService.Photo() { Id = insertedPhoto.Id, PhotoUrl = insertedPhoto.PhotoUrl };

        BioService.Response response = SetThumbnail(requestedPhoto, dataContext);

        newProtoPerson.Dbresult = response.Good;
      }
      catch (Exception ex){
        Console.WriteLine(ex.Message);
      }     

      return newProtoPerson;
    }

    public BioService.Person Remove( BioService.Person person )
    { 
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Remove(person, dataContext);
      }      
    }

    public BioService.Person Remove(BioService.Person person, BioSkyNetDataModel dataContext)
    {
      BioService.Person deletedProtoPerson = new BioService.Person { Dbresult = BioService.Result.Failed };
      if (person == null)
        return deletedProtoPerson;
     
      try
      {
        Person deletePerson = dataContext.Person.Find(person.Id);
        if (deletePerson == null)
          return deletedProtoPerson;

        dataContext.Person.Remove(deletePerson);

        int affectedRows = dataContext.SaveChanges();

        if (affectedRows > 0)
        {
          deletedProtoPerson.Id = deletePerson.Id;
          deletedProtoPerson.Dbresult = BioService.Result.Success;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }      

      return deletedProtoPerson;
    }

    public BioService.PersonList Select( BioSkyNetDataModel dataContext )
    {
      BioService.PersonList persons = new BioService.PersonList();
      
      try
      {
        IQueryable<Person> personEtities = dataContext.Person;
        foreach (Person p in personEtities)
        {
          BioService.Person protoPerson = _convertor.GetPersonProto(p);
          if (protoPerson != null)
            persons.Persons.Add(protoPerson);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return persons;
    }

    public BioService.PersonList Select()
    {      
      using (var DataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Select(DataContext);
      }
    }

    public BioService.Response SetThumbnail(BioService.Photo item, BioSkyNetDataModel dataContext)
    {
      BioService.Response response = new BioService.Response() { Good = BioService.Result.Failed };
      if (item == null)
        return response;
     
      try
      {
        Person owner = dataContext.Person.Where(x => x.Id == item.Personid).FirstOrDefault();
      
        if (owner == null)
          return response;
      
        Photo existingPhoto = owner.PhotoCollection.Where(x => x.Id == item.Id).FirstOrDefault();
      
        if (existingPhoto == null)
          return response;
      
        owner.Photo = existingPhoto;
      
        dataContext.SaveChanges();      
        response.Good = BioService.Result.Success;      
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
     
      return response;
    }

    public BioService.Response SetThumbnail(BioService.Photo item)
    {    
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return SetThumbnail(item, dataContext);
      }     
    }

    private IProcessorLocator     _locator ;
    private ProtoMessageConvertor _convertor;
    private readonly PhotoDataClient _photoDataClient;
  }
}
