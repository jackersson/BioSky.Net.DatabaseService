using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.DataClients
{
  public class FacialDataClient
  {
    public FacialDataClient( IProcessorLocator locator
                           , PhotoDataClient photoDataClient
                           , FaceCharacteristicDataClient faceCharacteristicDataClient)
    {
      _locator                      = locator                     ;
      _photoDataClient              = photoDataClient             ;
      _faceCharacteristicDataClient = faceCharacteristicDataClient;

      _convertor = new ProtoMessageConvertor();
      _utils     = new IOUtils();
    }

    public BioService.FacialImage Add(BioService.FacialImage item)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(null, item, dataContext);
      }
    }
      
    public BioService.FacialImage Add(BiometricData biometricDataEntity, BioService.FacialImage request, BioSkyNetDataModel dataContext)
    {
      BioService.FacialImage response = new BioService.FacialImage() { Dbresult    = BioService.Result.Failed
                                                                     , EntityState = BioService.EntityState.Added };
      if (request == null)
        return response;

      try
      {
        if (request.Image == null)
          return response;

        Photo image = null;// _photoDataClient.AddForEntity(request.Image, dataContext);

        if (image == null)
          return response;

        long imageId   = image.Id;
        response.Image = new BioService.Photo() { Id          = imageId
                                                , Dbresult    = BioService.Result.Success
                                                , EntityState = BioService.EntityState.Added };

        if (biometricDataEntity == null)
          biometricDataEntity = dataContext.BiometricData.Where(x => x.Id == request.OwnerBiometricDataId).FirstOrDefault();

        if (biometricDataEntity == null)
          return response;

        foreach (BioService.FaceCharacteristic fc in request.Faces)
        {
          fc.Photoid = imageId;          
          response.Faces.Add( _faceCharacteristicDataClient.Add(biometricDataEntity, fc, dataContext) );          
        }
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
        var existingPhotos = dataContext.Photo.Where(x => items.Indexes.Contains(x.Id));

        if (existingPhotos == null)
          return removedItems;

        foreach (Photo photo in existingPhotos)
        {
          //photo.Person_Id = null;
          //photo.Portrait_Characteristics_Id = null;
        }

        dataContext.SaveChanges();


        var deletedPhotos = dataContext.Photo.RemoveRange(existingPhotos);
        int affectedRows = dataContext.SaveChanges();
        if (deletedPhotos.Count() == affectedRows)
          return items;
        else
        {
          foreach (long id in items.Indexes)
          {
            if (dataContext.Photo.Find(id) == null)
              removedItems.Indexes.Add(id);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return removedItems;
    }



    public BioService.PhotoList Select(BioService.QueryPhoto query)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Select(query, dataContext);
      }
    }

    public BioService.PhotoList Select(BioService.QueryPhoto query, BioSkyNetDataModel dataContext)
    {
      BioService.PhotoList photos = new BioService.PhotoList();

      using (var DataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        try
        {
          IQueryable<Photo> photoEtities = DataContext.Photo;
          foreach (Photo p in photoEtities)
          {
            BioService.Photo protoPhoto = _convertor.GetPhotoProto(p);
            if (protoPhoto != null)
              photos.Photos.Add(protoPhoto);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }

        return photos;
      }
    }

    private IProcessorLocator            _locator;
    private ProtoMessageConvertor        _convertor;
    private PhotoDataClient              _photoDataClient;
    private FaceCharacteristicDataClient _faceCharacteristicDataClient;
    private IOUtils _utils;
  }
}

