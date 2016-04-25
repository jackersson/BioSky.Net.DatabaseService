using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataClients
{
  public class BiometricDataClient
  {
    public BiometricDataClient(IProcessorLocator locator, FacialDataClient facialDataClient)
    {
      _locator          = locator         ;
      _facialDataClient = facialDataClient;

      _convertor = new ProtoMessageConvertor();
    }

    public BioService.BiometricData Add( Visitor existingVisitor, BioService.FullVisitorData request, BioSkyNetDataModel dataContext)
    {
      BioService.BiometricData response = new BioService.BiometricData() { };
      if (request == null)
        return response;

      try
      {
        BiometricData entity = new BiometricData();
        existingVisitor.BiometricData = entity;

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;

        response.Id = entity.Id;
        
        BioService.FacialImage fi = _facialDataClient.Add(request.Face);
        if (fi != null)
          response.Faces.Add( fi.Faces );

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return response;
    }

    /*
    public BioService.BiometricData Add(Person existingPerson, BioService.BiometricData item)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(existingPerson, item, dataContext);
      }
    }
    */
    public BioService.BiometricData Add( Person existingPerson, BioService.BiometricData request, BioSkyNetDataModel dataContext)
    {
      BioService.BiometricData response = new BioService.BiometricData() { };
      if (request == null)
        return response;

      try
      {
        BiometricData entity = new BiometricData();
        existingPerson.BiometricData = entity;

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;

        response.Id = existingPerson.BiometricData.Id;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return response;
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
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return removedItems;
    }

    private IProcessorLocator     _locator         ;
    private ProtoMessageConvertor _convertor       ;
    private FacialDataClient      _facialDataClient;

  }
}
