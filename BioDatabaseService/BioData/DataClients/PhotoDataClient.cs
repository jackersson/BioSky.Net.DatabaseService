using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Linq;

namespace BioData.DataClients
{
  public class PhotoDataClient
  {
    public PhotoDataClient(IProcessorLocator locator)
    {
      _locator = locator;
      _convertor = new ProtoMessageConvertor();
      _utils     = new IOUtils              ();
    }

    public BioService.Photo Add(BioService.Photo item)
    {
      using (var dataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        return Add(item, dataContext);
      }     
    }

    public BioService.Photo Add( Person existingPerson    ,  BioService.Photo request
                               , BioService.Photo response, BioSkyNetDataModel dataContext)
    {
      if (response == null)
         response = new BioService.Photo() { Dbresult    = BioService.Result.Failed
                                           , EntityState = BioService.EntityState.Added };   

      if (IsPhotoValid(request))
        return null;

      try
      {        
        Photo entity = _convertor.GetPhotoEntity(request);
        entity.Photo_Url = _utils.SavePersonImage(request.Bytestring, request.OwnerId);
        
        if (entity == null)
          return response;

        existingPerson.Photos.Add(entity);

        int affectedRows = dataContext.SaveChanges();
        if (affectedRows <= 0)
          return response;

        response.Dbresult = BioService.Result.Success;
        response.Id       = entity.Id;
        response.PhotoUrl = entity.Photo_Url;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      return null;
    }


    public BioService.Photo Add(BioService.Photo request, BioSkyNetDataModel dataContext)
    {
      BioService.Photo response = new BioService.Photo() { Dbresult    = BioService.Result.Failed
                                                         , EntityState = BioService.EntityState.Added };
      if (IsPhotoValid(request))
        return response;
            
      try
      {
        Person owner = dataContext.Person.Where(x => x.Id == request.OwnerId).FirstOrDefault();
        if (owner == null)
          return response;

        Add(owner, request, response, dataContext);
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
        var existingPhotos = dataContext.Photo.Where(x => items.Indexes.Contains(x.Id));

        if (existingPhotos == null)
          return removedItems;

        foreach (Photo photo in existingPhotos)
        {
          //photo.Person_Id = null;
         // photo.Portrait_Characteristics_Id = null;
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
      catch (Exception ex) {
        Console.WriteLine(ex.Message);         
      }
      
      return removedItems;
    }

   

    public BioService.PhotoList Select( BioService.QueryPhoto query )
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
          long targetId = query.Photos.FirstOrDefault();
         
            

          Photo photoEtities = DataContext.Photo.Find(targetId);
          BioService.Photo protoPhoto = _convertor.GetPhotoProto(photoEtities);
          if (protoPhoto != null)
            photos.Photos.Add(protoPhoto);
          //foreach (Photo p in photoEtities)
          //{
          //  BioService.Photo protoPhoto = _convertor.GetPhotoProto(p);      
          //  if (protoPhoto != null)
          //    photos.Photos.Add(protoPhoto);
          //}
        }
        catch (Exception ex)   {
          Console.WriteLine(ex.Message);
        }

        return photos;
      }
    }

    private bool IsPhotoValid(BioService.Photo request)
    {
      return request == null || request.Bytestring == null || request.Bytestring.Count() <= 0;
    }


    private IProcessorLocator     _locator  ;
    private ProtoMessageConvertor _convertor;
    private IOUtils               _utils    ;
  }
}
