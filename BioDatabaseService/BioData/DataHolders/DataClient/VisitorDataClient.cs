﻿using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.DataHolders.DataClient
{
  public class VisitorDataClient
  {
    public VisitorDataClient(IProcessorLocator locator)
    {
      _locator   = locator;
      _convertor = new ProtoMessageConvertor();
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
        var existingVisitors = dataContext.Visitor.Where(x => items.Indexes.Contains(x.Id));

        if (existingVisitors == null)
          return removedItems;

        var deletedLocations = dataContext.Visitor.RemoveRange(existingVisitors);
        int affectedRows = dataContext.SaveChanges();
        if (deletedLocations.Count() == affectedRows)
          return items;
        else
        {
          foreach (long id in items.Indexes)
          {
            if (dataContext.Visitor.Find(id) == null)
              removedItems.Indexes.Add(id);
          }
        }
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return removedItems;
    }

    private IProcessorLocator        _locator        ;
    private ProtoMessageConvertor    _convertor      ;
    private readonly PhotoDataClient _photoDataClient;
  }
}
