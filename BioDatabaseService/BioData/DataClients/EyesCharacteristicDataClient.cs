using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;

namespace BioData.DataClients
{
  public class EyesCharacteristicDataClient
  {
    public EyesCharacteristicDataClient(IProcessorLocator locator, BiometricLocationDataClient biometricLocationDataClient)
    {
      _locator = locator;

      _biometricLocationDataClient = biometricLocationDataClient;

      _convertor = new ProtoMessageConvertor();
    }
    

    public BioService.EyesCharacteristic Add( FaceCharacteristic            faceEntity
                                            , BioService.EyesCharacteristic request
                                            , BioSkyNetDataModel            dataContext)
    {
      BioService.EyesCharacteristic response = new BioService.EyesCharacteristic() { Dbresult    = BioService.Result.Failed
                                                                                   , EntityState = BioService.EntityState.Added };
      if (request == null || ( request.LeftEye == null && request.RightEye == null ) )
        return response;

      try
      {        
        faceEntity.EyesCharacteristic = _convertor.GetEyesCharacteristicEntity(request);

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;

        EyesCharacteristic entity = faceEntity.EyesCharacteristic;

        response.Id       = entity.Id;
        response.Dbresult = BioService.Result.Success;

        if (request.LeftEye != null)        
          response.LeftEye = _biometricLocationDataClient.Add(entity, request.LeftEye, dataContext, true);

        if (request.RightEye != null)
          response.RightEye = _biometricLocationDataClient.Add(entity, request.RightEye, dataContext, false);
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }

      return null;
    }   

    private IProcessorLocator _locator;
    private ProtoMessageConvertor _convertor;
    private BiometricLocationDataClient _biometricLocationDataClient;
  }
}
