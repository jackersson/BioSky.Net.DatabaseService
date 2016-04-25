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

    public BioService.Card Add(BioService.Card request, BioSkyNetDataModel dataContext)
    {
      BioService.Card response = new BioService.Card() { Dbresult = BioService.Result.Failed, EntityState = BioService.EntityState.Added };
      if (request == null)
        return response;
     
      try
      {
        Card existingCard = dataContext.Card.Where(x => x.Unique_Number == request.UniqueNumber).FirstOrDefault();
        if (existingCard != null)
          return response;

        Person owner = dataContext.Person.Where(x => x.Id == request.Personid).FirstOrDefault();
        if (owner == null)
          return response;

        Card entity = _convertor.GetCardEntity(request);
        owner.Card.Add(entity);

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;

        response.Id       = entity.Id;
        response.Dbresult = BioService.Result.Success;        
      }
      catch (Exception ex){
        Console.WriteLine(ex.Message);
      }
      
      return response;
    }

    public void RemoveAll(Person entity, BioService.Person response, BioSkyNetDataModel dataContext)
    {
      try
      {
        var existingItems = entity.Card;

        if (existingItems == null)
          return;

        entity.Card.Clear();
        foreach (Card card in existingItems)
          card.Person_Id = null;

        var deletedItems = dataContext.Card.RemoveRange(existingItems);
        int affectedRows = dataContext.SaveChanges();
        if ( affectedRows <= 0)
          return;


        if (response != null)
        {
          foreach (Card card in deletedItems)
          {
            response.Cards.Add(new BioService.Card()
            {
                Id = card.Id
              , EntityState = BioService.EntityState.Deleted
              , Dbresult = BioService.Result.Success
            });
          }         
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    public BioService.CardList Remove( BioService.CardList items )
    {
      BioService.CardList response = new BioService.CardList();

      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        foreach (BioService.Card card in items.Cards)
          Remove(card, dataContext);

        return response;
      }
    }

    public BioService.Card Remove(BioService.Card request)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Remove(request, dataContext);
      }
    }

    public BioService.Card Remove(BioService.Card request, BioSkyNetDataModel dataContext)
    {
      BioService.Card response = new BioService.Card() { EntityState = BioService.EntityState.Deleted, Dbresult = BioService.Result.Failed};
      if (request == null)
        return response;

      response.Id = request.Id;

      try
      {
        var existingCard = dataContext.Card.Where(x => x.Id == request.Id).FirstOrDefault();

        if (existingCard == null)
          return response;

        dataContext.Card.Remove(existingCard);
        int affectedRows = dataContext.SaveChanges();
        if ( affectedRows <= 0)
          return request;

        response.Dbresult = BioService.Result.Success;
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);          
      }
      
      return response;
    }

    private IProcessorLocator      _locator;
    private ProtoMessageConvertor _convertor;
  }
}
