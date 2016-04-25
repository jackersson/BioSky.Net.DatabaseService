using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;

namespace BioData.DataClients
{
  public class BiometricLocationDataClient
  {
    public BiometricLocationDataClient(IProcessorLocator locator)
    {
      _locator   = locator;   
      _convertor = new ProtoMessageConvertor();     
    }
    
    public BiometricLocation Add( BioService.BiometricLocation request
                                , BioSkyNetDataModel           dataContext )
    {      
      if (request == null)
        return null;

      try
      {
        BiometricLocation entity = dataContext.BiometricLocation.Add(_convertor.GetBiometricLocationEntity(request));

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)        
          return entity;     

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return null;
    }

    public BioService.BiometricLocation Add( EyesCharacteristic eyesEntity
                                           , BioService.BiometricLocation request
                                           , BioSkyNetDataModel dataContext, bool isLeft)
    {
      BioService.BiometricLocation response = new BioService.BiometricLocation() { Dbresult = BioService.Result.Failed
                                                                                 , EntityState = BioService.EntityState.Added };
      if (request == null || eyesEntity == null)
        return response;

      try
      {
        BiometricLocation bm = _convertor.GetBiometricLocationEntity(request);
        if (isLeft)
          eyesEntity.LeftEyeBiometricLocation = bm;
        else
          eyesEntity.RightEyeBiometricLocation = bm;

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)        
          response.Dbresult = BioService.Result.Success;       

      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return response;
    }

     public BioService.BiometricLocation Add( FaceCharacteristic faceEntity
                                            , BioService.BiometricLocation request
                                            , BioSkyNetDataModel dataContext)
     {
      BioService.BiometricLocation response = new BioService.BiometricLocation() { Dbresult = BioService.Result.Failed
                                                                                 , EntityState = BioService.EntityState.Added };
      if (request == null || faceEntity == null)
        return response;

      try
      {
        faceEntity.BiometricLocation = _convertor.GetBiometricLocationEntity(request);
      
        int affectedRows = dataContext.SaveChanges();
        if (affectedRows > 0)        
          response.Dbresult = BioService.Result.Success;  
      }
      catch (Exception ex) {
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

    private IProcessorLocator     _locator  ;
    private ProtoMessageConvertor _convertor;

  }
}
