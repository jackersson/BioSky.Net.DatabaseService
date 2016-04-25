using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BioData.DataClients
{
  public class PersonDataClient
  {
    public PersonDataClient( IProcessorLocator locator
                           , CardDataClient cardDataClient
                           , BiometricDataClient biometricDataClient  )
    {
      _locator    = locator;
      _convertor  = new ProtoMessageConvertor();
      _rawIndexes = new BioService.RawIndexes();

      _biometricDataClient = biometricDataClient;
      _cardDataClient      = cardDataClient;
    }

    public bool PersonExists(string firstName, string lastName, BioSkyNetDataModel dataContext)
    {
      return dataContext.Person.Where(x => x.First_Name_ == firstName && x.Last_Name_ == lastName).Count() > 0;
    }

    public BioService.Person Add(BioService.Person person)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(person, dataContext);
      }
    }

    public BioService.Person Add(BioService.Person request, BioSkyNetDataModel dataContext)
    {
      BioService.Person response = new BioService.Person { Dbresult = BioService.Result.Failed, EntityState = BioService.EntityState.Added };
      if (request == null)
        return response;

      try
      {
        if (PersonExists(request.Firstname, request.Lastname, dataContext))
          return response;
              
        Person entity = _convertor.GetPersonEntity(request);
        dataContext.Person.Add(entity);

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;

        response.Id       = entity.Id;
        response.Dbresult = BioService.Result.Success;

        BioService.BiometricData bd = _biometricDataClient.Add(entity, new BioService.BiometricData(), dataContext);
        if (bd != null)
          response.BiometricData = bd;
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
    }



    public BioService.Person Update(BioService.Person person)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Update(person, dataContext);
      }      
    }
    //TODO make update for all person
    public BioService.Person Update(BioService.Person request, BioSkyNetDataModel dataContext)
    {
      BioService.Person response = new BioService.Person { Id = request.Id
                                                         , EntityState = BioService.EntityState.Modified
                                                         , Dbresult = BioService.Result.Failed };
      if (request == null)
        return response;
     
      try
      {
        Person existingPerson = dataContext.Person.Find(request.Id);

        if (existingPerson == null)
          return response;


        #region validation
        bool hasFirstName = !string.IsNullOrEmpty(request.Firstname);
        bool hasLastName  = !string.IsNullOrEmpty(request.Lastname );

        if (hasFirstName || hasLastName)
        {
          bool hasNewFirstName = hasFirstName && !existingPerson.First_Name_.Equals(request.Firstname);
          bool hasNewLastName  = hasLastName  && !existingPerson.Last_Name_.Equals(request.Lastname);

          string targetFirstName = hasNewFirstName ? request.Firstname : existingPerson.First_Name_;
          string targetLastName  = hasNewLastName  ? request.Lastname : existingPerson.Last_Name_;

          if (!PersonExists(targetFirstName, targetLastName, dataContext))
          {
            existingPerson.First_Name_ = targetFirstName;
            existingPerson.Last_Name_  = targetLastName;
          }
          else
            return response;
        }
               
        if(request.Dateofbirth != 0)
        {
          DateTime dateofbirth = new DateTime(request.Dateofbirth);
          if (existingPerson.Date_Of_Birth != dateofbirth)
            existingPerson.Date_Of_Birth = dateofbirth;
        }

        if (!string.IsNullOrEmpty(request.Country))        
          existingPerson.Country = _convertor.IsDeleteState(request.Country) ? string.Empty : request.Country;

        if (!string.IsNullOrEmpty(request.City))
          existingPerson.City = _convertor.IsDeleteState(request.Country) ? string.Empty : request.City;

        if (!string.IsNullOrEmpty(request.Comments))
          existingPerson.Comments = _convertor.IsDeleteState(request.Country) ? string.Empty : request.Comments;

        if (!string.IsNullOrEmpty(request.Email))
          existingPerson.Email = _convertor.IsDeleteState(request.Country) ? string.Empty : request.Email;
        
        byte gender = (byte)request.Gender;
        if (existingPerson.Gender != gender)
          existingPerson.Gender = gender;

        byte rights = (byte)request.Rights;
        if (existingPerson.Rights != rights)
          existingPerson.Rights = (byte)request.Rights;
        #endregion

       
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;

        response.Dbresult = BioService.Result.Success;

        // BioService.BiometricData biometricData = person.Data;



        /*

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
        #region  send validation
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
        #endregion
        
        updatedProtoPerson.Dbresult = BioService.Result.Success; */
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }     

      return response;
    }

    
    public Photo GetPersonThumbnailOrDefault( Person person, BioSkyNetDataModel dataContext)
    {
      if (person.BiometricData != null || person.BiometricData.FaceCharacteristic == null)
        return null;

      ICollection<FaceCharacteristic> faces = person.BiometricData.FaceCharacteristic;

      byte thumbnail = (byte)BioService.PhotoOriginType.Thumbnail;
      FaceCharacteristic fc = person.BiometricData.FaceCharacteristic
                       .Where(x => x.Photo.Origin_Type == thumbnail).FirstOrDefault();
      if (fc == null)
        fc = person.BiometricData.FaceCharacteristic.FirstOrDefault();

      if (fc == null)
        return null;

      return fc.Photo;
    }

 
    public BioService.Person Remove( BioService.Person person )
    { 
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Remove(person, dataContext);
      }      
    }

    public BioService.Person Remove(BioService.Person request, BioSkyNetDataModel dataContext)
    {
      BioService.Person response = new BioService.Person { EntityState = BioService.EntityState.Deleted
                                                         , Dbresult = BioService.Result.Failed };
      if (request == null)
        return response;

      response.Id = request.Id;
      try
      {
        var entity = dataContext.Person.Find(request.Id);
        if (entity == null)
        {
          response.Dbresult = BioService.Result.Success;
          return response;
        }
               
        dataContext.Person.Remove(entity);
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)
          response.Dbresult = BioService.Result.Success;

        /*
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
        */
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }      

      return response;
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

    #region thumbnail
    public BioService.Photo SetDefaultThumbnail(Person owner, BioSkyNetDataModel dataContext)
    {
      if (owner.Thumbnail == null)
        return null;

      Photo photo = GetDefaultPhoto(owner, dataContext);

      if (photo == null)
        return null;

      return SetThumbnail(owner, photo, dataContext);
    }

    public BioService.Photo SetThumbnail(Person owner, Photo photoEntity, BioSkyNetDataModel dataContext)
    {
      BioService.Photo response = new BioService.Photo() { Dbresult    = BioService.Result.Failed
                                                         , EntityState = BioService.EntityState.Modified
                                                         , OriginType  = BioService.PhotoOriginType.Thumbnail };


      if (owner == null || photoEntity == null)
        return response;

      try
      {
        owner.Thumbnail_Id = photoEntity.Id;

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)
          response.Dbresult = BioService.Result.Success;
      }   
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return response;
    }


    public BioService.Photo SetThumbnail(long ownerId, long photoID, BioSkyNetDataModel dataContext)
    {
      Person owner         = dataContext.Person.Where(x => x.Id == ownerId).FirstOrDefault();
      Photo  existingPhoto = GetPersonPhotoById(owner, photoID, dataContext);

      return SetThumbnail(owner, existingPhoto, dataContext);      
    }

    public BioService.Photo SetThumbnail(long ownerId, long photoID)
    {    
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return SetThumbnail(ownerId, photoID, dataContext);
      }     
    }
    #endregion

    #region PhotoSearch
    public Photo GetPersonPhotoById(Person person, long photoId, BioSkyNetDataModel dataContext)
    {
      if (person.Photos != null && person.Photos.Count > 0)
      {
        Photo photo = person.Photos.Where(x => x.Id == photoId).FirstOrDefault();
        if (photo != null)
          return photo;
      }

      if (person.BiometricData != null || person.BiometricData.FaceCharacteristic == null)
        return null;
            
      ICollection<FaceCharacteristic> faces = person.BiometricData.FaceCharacteristic;
      FaceCharacteristic fc = person.BiometricData.FaceCharacteristic
                             .Where(x => x.Photo_Id == photoId).FirstOrDefault();
      if (fc == null)
        return null;

      return fc.Photo;
    }
    

    public Photo GetDefaultPhoto(Person owner, BioSkyNetDataModel dataContext)
    {     
      if (owner.Photos != null && owner.Photos.Count > 0)
      {
        Photo photo = owner.Photos.FirstOrDefault();
        if (photo != null)
          return photo;
      }

      if (owner.BiometricData != null || owner.BiometricData.FaceCharacteristic == null)
        return null;

      ICollection<FaceCharacteristic> faces = owner.BiometricData.FaceCharacteristic;
      FaceCharacteristic fc = owner.BiometricData.FaceCharacteristic.FirstOrDefault();
      if (fc == null)
        return null;

      return fc.Photo;
    }
    #endregion

    private IProcessorLocator        _locator        ;
    private ProtoMessageConvertor    _convertor      ;
    private readonly BiometricDataClient _biometricDataClient;
    private BioService.RawIndexes    _rawIndexes     ;
    private readonly CardDataClient  _cardDataClient ;
  }
}
