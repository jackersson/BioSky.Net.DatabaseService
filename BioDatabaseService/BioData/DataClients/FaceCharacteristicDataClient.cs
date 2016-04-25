using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;

namespace BioData.DataClients
{
  public class FaceCharacteristicDataClient
  {
    public FaceCharacteristicDataClient( IProcessorLocator locator
                                       , EyesCharacteristicDataClient eyesCharacteristicDataClient
                                       , BiometricLocationDataClient  biometricLocationDataClient )
    {
      _locator = locator;

      _biometricLocationDataClient  = biometricLocationDataClient ;
      _eyesCharacteristicDataClient = eyesCharacteristicDataClient;

      _convertor = new ProtoMessageConvertor();   
    }    
             
    public BioService.FaceCharacteristic Add( BiometricData biometricDataEntity
                                            , BioService.FaceCharacteristic request
                                            , BioSkyNetDataModel dataContext)
    {
      BioService.FaceCharacteristic response = new BioService.FaceCharacteristic() { Dbresult = BioService.Result.Failed
                                                                                   , EntityState = BioService.EntityState.Added};

      if (request == null || biometricDataEntity == null)
        return response;

      try
      {
        FaceCharacteristic entity = _convertor.GetFaceCharacteristicEntity(request);
        biometricDataEntity.FaceCharacteristic.Add(entity);

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows < 0)        
          return response;
        
        if (request.Eyes != null)
          response.Eyes = _eyesCharacteristicDataClient.Add(entity, request.Eyes, dataContext);

        if (request.Location != null)
          response.Location = _biometricLocationDataClient.Add(entity, request.Location, dataContext);        
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return response;
    }
       
    private IProcessorLocator             _locator                     ;
    private ProtoMessageConvertor         _convertor                   ;
    private BiometricLocationDataClient   _biometricLocationDataClient ;
    private EyesCharacteristicDataClient  _eyesCharacteristicDataClient;

  }
}
