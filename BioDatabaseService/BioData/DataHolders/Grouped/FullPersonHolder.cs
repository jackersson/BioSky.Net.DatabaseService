using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.DataHolders.Grouped
{
  public class FullPersonHolder
  {
    public FullPersonHolder( IProcessorLocator locator )
    {
      _locator = locator;

      _persons = locator.GetProcessor<PersonDataHolder>();
      _photos  = locator.GetProcessor<PhotoDataHolder>();
      _cards   = locator.GetProcessor<CardDataHolder>();

      _convertor = new ProtoMessageConvertor();
      _utils     = new IOUtils();
    }


    public BioService.PersonList Update(BioService.PersonList proto)
    {
      BioService.PersonList persons = new BioService.PersonList();
      foreach (BioService.Person curProto in proto.Persons)
      {
        BioService.Person updatedProto = Update(curProto);
        if (updatedProto != null)
          persons.Persons.Add(updatedProto);
      }

      return persons;
    }
    public BioService.Person Update(BioService.Person proto)
    {
      /*
      BioService.Person updatedPerson = _persons.UpdateFromProto(proto);

      if (updatedPerson == null)
        return null;

      byte targetPhotoState = (byte)BioService.PhotoOriginType.Loaded;
      if (proto.EntityState == BioService.EntityState.Deleted)
      {
          IQueryable<Photo> allphotos = _photos.Select().Where( x => x.Person_Id == updatedPerson.Id
                                                             && x.Origin_Type.HasValue
                                                             && x.Origin_Type.Value == targetPhotoState);
          foreach (Photo cp in allphotos)
              _photos.Remove(cp);

          IQueryable<Card> allcards = _cards.Select().Where(x => x.Person_Id == updatedPerson.Id );
          foreach (Card cp in allcards)
              _cards.Remove(cp);

          return updatedPerson;
      }

      if (proto.Thumbnail != null)
      {
        proto.Thumbnail.Personid = updatedPerson.Id;
        proto.Thumbnail.FileLocation = _utils.GetPersonImagePath(updatedPerson.Id);
        
        updatedPerson.Thumbnail = _photos.UpdateFromProto(proto.Thumbnail);
        if (updatedPerson.Thumbnail != null)
        {
          updatedPerson.Thumbnailid = updatedPerson.Thumbnail.Id;
          updatedPerson.Dbresult    = BioService.ResultStatus.Failed;
          updatedPerson.EntityState = BioService.EntityState.Modified;
          if ( _persons.UpdateFromProto(updatedPerson) != null )
            updatedPerson.Dbresult = BioService.ResultStatus.Success;
        }
      }

      long targetID = updatedPerson.Id;
      foreach (BioService.Photo ph in proto.Photos)
      {
        if (ph.Personid != targetID)
          ph.Personid = targetID;


        ph.FileLocation = _utils.GetPersonImagePath(updatedPerson.Id);
        if (ph.Fir != null && ph.Fir.Length > 0)          
          ph.FirLocation = _utils.GetPersonFirPath(updatedPerson.Id);          

        BioService.Photo updatedPhProto = _photos.UpdateFromProto(ph);
        if (updatedPhProto != null)
          updatedPerson.Photos.Add(updatedPhProto);
      }

      foreach (BioService.Card card in proto.Cards)
      {
        if (card.Personid != targetID)
          card.Personid = targetID;

        BioService.Card updatedCardProto = _cards.UpdateFromProto(card);
        if (updatedCardProto != null)
          updatedPerson.Cards.Add(updatedCardProto);
      }    
      */
      return null;
    }



    public BioService.PersonList Select(BioService.QueryPersons command)
    {
      BioService.PersonList result = new BioService.PersonList();

      DbSet<Person> persons  = _persons.Select();
      DbSet<Photo>  photos   = _photos.Select ();
      DbSet<Card>   cards    = _cards.Select()  ;

      foreach (Person person in persons)
      {
        BioService.Person protoPerson = _convertor.GetPersonProto(person);
        
        if (protoPerson == null)
          continue;

        long personid = person.Id;
  

        IQueryable<Card> personCards = cards.Where(x => x.Person_Id == personid);
        foreach (Card card in personCards)
        {
          BioService.Card currentCardProto = _convertor.GetCardProto(card);
          if (currentCardProto != null)
            protoPerson.Cards.Add(currentCardProto);
        }

        result.Persons.Add(protoPerson);
      }   

      return result;
    }

    private readonly IOUtils _utils;
    private readonly IProcessorLocator _locator;
    private readonly ProtoMessageConvertor _convertor;
    private readonly PersonDataHolder  _persons;
    private readonly PhotoDataHolder   _photos ;
    private readonly CardDataHolder    _cards  ;
  }
}
