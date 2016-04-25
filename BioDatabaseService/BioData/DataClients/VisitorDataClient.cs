using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataClients
{
  public class VisitorDataClient
  {
    public VisitorDataClient(IProcessorLocator locator, BiometricDataClient biometricDataClient)
    {
      _locator         = locator;
      _convertor       = new ProtoMessageConvertor();
      _biometricDataClient = biometricDataClient;
    
    }

    public BioService.Visitor Add(BioService.FullVisitorData visitor)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(visitor, dataContext);
      }
    }

    public BioService.Visitor Add(BioService.FullVisitorData request, BioSkyNetDataModel dataContext)
    {
      BioService.Visitor response = new BioService.Visitor { Dbresult = BioService.Result.Failed
                                                           , EntityState = BioService.EntityState.Added };      
      if (request == null || request.Visitor == null)
        return response;

      try
      {
        BioService.Visitor visitor = request.Visitor;
        
        Visitor entity = _convertor.GetVisitorEntity(visitor);
        dataContext.Visitor.Add(entity);
        
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;
        response.Id       = entity.Id                ;
        response.Dbresult = BioService.Result.Success;

        response.BiometricData = _biometricDataClient.Add(entity, request, dataContext);          
        
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
    }

    public BioService.VisitorList Select(BioService.QueryVisitors query, BioSkyNetDataModel dataContext)
    {
      BioService.VisitorList visitors = new BioService.VisitorList();

      try
      {
        IQueryable<Visitor> visitorEntities = dataContext.Visitor;
        foreach (Visitor p in visitorEntities)
        {
          BioService.Visitor protoVisitor = _convertor.GetVisitorProto(p);
          if (protoVisitor != null)
            visitors.Visitors.Add(protoVisitor);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return visitors;
    }

    public BioService.VisitorList Select(BioService.QueryVisitors query)
    {
      using (var DataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Select(query, DataContext);
      }
    }

    public BioService.RawIndexes Remove(BioService.RawIndexes items)
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
      
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return removedItems;
    }

    private IProcessorLocator            _locator            ;
    private ProtoMessageConvertor        _convertor          ;
    private readonly BiometricDataClient _biometricDataClient;

  }
}
