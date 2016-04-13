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

    public BioService.Photo Add(BioService.Photo item, BioSkyNetDataModel dataContext)
    {
      BioService.Photo newProtPhoto = new BioService.Photo() { Dbresult = BioService.Result.Failed };
      if (item == null)
        return newProtPhoto;
            
      try
      {
        Photo existingPhoto = dataContext.Photo.Where(x => x.Id == item.Id).FirstOrDefault();

        if (existingPhoto != null)
          return newProtPhoto;

        Person owner = dataContext.Person.Where(x => x.Id == item.Personid).FirstOrDefault();

        if (owner == null)
          return newProtPhoto;

        Photo newPhoto = _convertor.GetPhotoEntity(item);
        newPhoto.Photo_Url = _utils.SavePersonImage(item.Bytestring, item.Personid);
        owner.PhotoCollection.Add(newPhoto);
        if (owner.PhotoCollection.Count == 1)
          owner.Photo = newPhoto;

        int affectedRows = dataContext.SaveChanges();

        if (affectedRows > 0)
        {
          newProtPhoto.Id = newPhoto.Id;
          newProtPhoto.PhotoUrl = newPhoto.Photo_Url;
          newProtPhoto.Dbresult = BioService.Result.Success;
        }
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }
      
      return newProtPhoto;
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
          photo.Person_Id = null;
          photo.Portrait_Characteristics_Id = null;
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
          IQueryable<Photo> photoEtities = DataContext.Photo;
          foreach (Photo p in photoEtities)
          {
            BioService.Photo protoPhoto = _convertor.GetPhotoProto(p);      
            if (protoPhoto != null)
              photos.Photos.Add(protoPhoto);
          }
        }
        catch (Exception ex)   {
          Console.WriteLine(ex.Message);
        }

        return photos;
      }
    }

    private IProcessorLocator     _locator  ;
    private ProtoMessageConvertor _convertor;
    private IOUtils               _utils    ;
  }
}
