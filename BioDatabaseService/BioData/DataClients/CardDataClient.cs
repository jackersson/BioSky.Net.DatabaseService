using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataClients
{
  public class CardDataClient
  {
    public CardDataClient(IProcessorLocator locator)
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();
    }

    public BioService.Card Add(BioService.Card item)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(item, dataContext);
      }
    }

    public BioService.Card Add(BioService.Card item, BioSkyNetDataModel dataContext)
    {
      BioService.Card newProtoCard = new BioService.Card() { Dbresult = BioService.Result.Failed };
      if (item == null)
        return newProtoCard;
     
      try
      {
        Card existingCard = dataContext.Card.Where(x => x.Unique_Number == item.UniqueNumber).FirstOrDefault();

        if (existingCard != null)
          return newProtoCard;

        Person owner = dataContext.Person.Where(x => x.Id == item.Personid).FirstOrDefault();

        if (owner == null)
          return newProtoCard;

        Card newCard = _convertor.GetCardEntity(item);
        owner.Card.Add(newCard);
        int affectedRows = dataContext.SaveChanges();

        if (affectedRows > 0)
        {
          newProtoCard.Id = newCard.Id;
          newProtoCard.Dbresult = BioService.Result.Success;
        }
      }
      catch (Exception ex){
        Console.WriteLine(ex.Message);
      }
      
      return newProtoCard;
    }

    public BioService.RawIndexes Remove( BioService.RawIndexes items )
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Remove(items, dataContext);
      }
    }

    public BioService.RawIndexes Remove(BioService.RawIndexes items, BioSkyNetDataModel dataContext)
    {
      BioService.RawIndexes removedItems = new BioService.RawIndexes();
      if (items == null || items.Indexes.Count <= 0)
        return removedItems;
     
      try
      {
        var existingCards = dataContext.Card.Where(x => items.Indexes.Contains(x.Id));

        if (existingCards == null)
          return removedItems;

        var deletedCards = dataContext.Card.RemoveRange(existingCards);
        int affectedRows = dataContext.SaveChanges();
        if (deletedCards.Count() == affectedRows)
          return items;
        else
        {
          foreach (long id in items.Indexes)
          {
            if (dataContext.Card.Find(id) == null)
              removedItems.Indexes.Add(id);
          }
        }

      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);          
      }
      
      return removedItems;
    }

    private IProcessorLocator      _locator;
    private ProtoMessageConvertor _convertor;
  }
}
