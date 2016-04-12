using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataHolders.DataClient
{
  public class PersonDataClient
  {
    public PersonDataClient( IProcessorLocator locator, PhotoDataClient photoDataClient
                           , CardDataClient cardDataClient)
    {
      _locator    = locator;
      _convertor  = new ProtoMessageConvertor();
      _rawIndexes = new BioService.RawIndexes();

      _photoDataClient = photoDataClient;
      _cardDataClient  = cardDataClient;
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

        if(person.Dateofbirth != 0)
        {
          DateTime dateofbirth = (person.Dateofbirth != -1)? new DateTime(person.Dateofbirth): new DateTime();
          if (existingPerson.Date_Of_Birth != dateofbirth)
            existingPerson.Date_Of_Birth = dateofbirth;
        }

        if (person.Country != "")
          existingPerson.Country = (person.Country != "(Deleted)") ? person.Country : "";

        if (person.City != "")
          existingPerson.City = (person.City != "(Deleted)") ? person.City : "";

        if (person.Email != "")
          existingPerson.Email = (person.Email != "(Deleted)") ? person.Email : "";

        if (person.Comments != "")
          existingPerson.Comments = (person.Comments != "(Deleted)") ? person.Comments : "";

        byte gender = (byte)person.Gender;
        if (existingPerson.Gender != gender)
          existingPerson.Gender = gender;

        byte rights = (byte)person.Rights;
        if (existingPerson.Rights != rights)
          existingPerson.Rights = (byte)person.Rights;

        BioService.Photo currentPhoto = person.Thumbnail;

        if (currentPhoto != null)
        {
          Photo existingPhoto = existingPerson.PhotoCollection.Where(x => x.Id == currentPhoto.Id).FirstOrDefault();          

          if (existingPhoto == null)
          {
            BioService.Photo insertedPhoto = _photoDataClient.Add(currentPhoto, dataContext);

            if (insertedPhoto.Dbresult != BioService.Result.Success)
            {
              updatedProtoPerson.Thumbnail = new BioService.Photo()
              {   Id       = insertedPhoto.Id
                , PhotoUrl = insertedPhoto.PhotoUrl
                , Dbresult = insertedPhoto.Dbresult
              }; ;
              return updatedProtoPerson;
            }                

            existingPhoto = existingPerson.PhotoCollection.Where(x => x.Id == insertedPhoto.Id).FirstOrDefault();
          }

          existingPerson.Photo_Id      = existingPhoto.Id;
          updatedProtoPerson.Photoid   = existingPhoto.Id;
          updatedProtoPerson.Thumbnail = new BioService.Photo()
          { Id = existingPhoto.Id
          , PhotoUrl = existingPhoto.Photo_Url
          , Dbresult = BioService.Result.Success };

          BioService.Photo thumbnailPhoto = new BioService.Photo() { Id = existingPhoto.Id, Personid = existingPhoto.Person_Id.Value };

          BioService.Response response = SetThumbnail(thumbnailPhoto, dataContext);

          if(response.Good != BioService.Result.Success)
          {
            updatedProtoPerson.Thumbnail.Dbresult = response.Good;
            return updatedProtoPerson;
          }          
        }
        else
        {
          int affectedRows  = dataContext.SaveChanges();
          if (affectedRows <= 0)
            return updatedProtoPerson;
        }

        if (person.Firstname != "")
          updatedProtoPerson.Firstname = person.Firstname;

        if (person.Lastname != "")
          updatedProtoPerson.Lastname = person.Lastname;

        if (person.Dateofbirth != 0)
          updatedProtoPerson.Dateofbirth = (person.Dateofbirth == -1) ? 0 : person.Dateofbirth;

        if (person.Country != "")
          existingPerson.Country = (person.Country != "(Deleted)") ? person.Country : "(Deleted)";

        if (person.City != "")
          updatedProtoPerson.City = (person.City != "(Deleted)") ? person.City : "(Deleted)";

        if (person.Email != "")
          updatedProtoPerson.Email = (person.Email != "(Deleted)") ? person.Email : "(Deleted)";

        if (person.Comments != "")
          updatedProtoPerson.Comments = (person.Comments != "(Deleted)") ? person.Comments : "(Deleted)";

        if (existingPerson.Gender != gender)
          updatedProtoPerson.Gender = person.Gender;

        if (existingPerson.Rights != rights)
          updatedProtoPerson.Rights = person.Rights;

        updatedProtoPerson.Dbresult = BioService.Result.Success;
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

        //Photo
        _rawIndexes.Indexes.Clear();
        
        foreach (Photo photo in deletePerson.PhotoCollection)
          _rawIndexes.Indexes.Add(photo.Id);

        deletePerson.Photo_Id = null;
        deletePerson.PhotoCollection.Clear();
        dataContext.SaveChanges();
        
        BioService.RawIndexes photoIndexes = _photoDataClient.Remove(_rawIndexes);

        //Cards
        _rawIndexes.Indexes.Clear();

        foreach (Card card in deletePerson.Card)
          _rawIndexes.Indexes.Add(card.Id);

        deletePerson.Card.Clear();
        dataContext.SaveChanges();

        BioService.RawIndexes cardIndexes = _cardDataClient.Remove(_rawIndexes);

        dataContext.Person.Remove(deletePerson);
        //

        int affectedRows = dataContext.SaveChanges();

        if (affectedRows > 0)
        {
          deletedProtoPerson.Id = deletePerson.Id;
          deletedProtoPerson.Dbresult = BioService.Result.Success;
          foreach (long cardId in cardIndexes.Indexes)
          {
            BioService.Card card = new BioService.Card() { Id = cardId };
            deletedProtoPerson.Cards.Add(card);
          }
          foreach (long photoID in photoIndexes.Indexes)
          {
            BioService.Photo photo = new BioService.Photo() { Id = photoID };
            deletedProtoPerson.Photos.Add(photo);
          }
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

    private IProcessorLocator        _locator        ;
    private ProtoMessageConvertor    _convertor      ;
    private readonly PhotoDataClient _photoDataClient;
    private BioService.RawIndexes    _rawIndexes     ;
    private readonly CardDataClient  _cardDataClient ;
  }
}
