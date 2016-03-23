using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BioData.DataHolders
{
  public class PhotoDataHolder : DataHolderBase<Photo>
  {

    public PhotoDataHolder( IProcessorLocator locator)
                          : base(locator)
    {
        _utils = new IOUtils();
    }

    public BioService.Photo UpdateFromProto(BioService.Photo proto)
    {
      /*
      Photo entity = _convertor.GetPhotoEntity(proto);

      if (entity == null)
        return null;

      BioService.EntityState entityState = proto.EntityState;
     
      switch (entityState)
      {
        case BioService.EntityState.Added:
        {
          Photo newEntity = Add(entity);
          if (newEntity != null)
          {
            BioService.Photo newProto = _convertor.GetPhotoProto(newEntity);
            newProto.EntityState = entityState;
            newProto.Dbresult    = BioService.ResultStatus.Success;
            UpdateLocalPhoto(proto);
            return newProto;
          }
          else
          {
            return new BioService.Photo() { Dbresult = BioService.ResultStatus.Failed
                                                 , EntityState = entityState };
          }            
        }

        case BioService.EntityState.Modified:
        {
          Photo updatedEntity = Update(entity);
          return new BioService.Photo()
                 { Id = entity.Id
                 , Dbresult = updatedEntity == null ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };           
        }

        case BioService.EntityState.Deleted:
        {
          bool state = Remove(entity);
          return new BioService.Photo()
                 { Id = entity.Id
                 , Dbresult = state ? BioService.ResultStatus.Success : BioService.ResultStatus.Failed
                 , EntityState = entityState };                
        }                
      }
      */
      return new BioService.Photo(proto);      
    }

    private void UpdateLocalPhoto( BioService.Photo item )
    {/*
       try
       {
           if (item.Description != null && item.Description.Length > 0)
               _utils.SaveLocalFile(item.Description.ToByteArray(), item.FileLocation);

           if (item.Fir != null && item.Fir.Length > 0)
               _utils.SaveLocalFile(item.Fir.ToByteArray(), item.FirLocation);      
       }
       catch (Exception ex)
       {
         Console.WriteLine(ex.Message);
       }
       */
    }

    public override Photo Add(Photo item)
    {
      _dataContext.Photo.Add(item);
      
      bool success = Save();
      
       

      return (success) ? item : null;
    }

    public override bool Remove(Photo item)
    {
/*
      bool success = false;
      var original = _dataContext.Photo.Find(item.Id);
      if (original != null)
      {
          _dataContext.Photo.Remove(original);
          success = Save();
      }    
      
      return success;*/

      bool success = false;
      using (var nDataContext = _locator.GetProcessor<IContextFactory>().Create<BioSkyNetDataModel>())
      {
        var original = nDataContext.Photo.Find(item.Id);
        if (original != null)
        {
          nDataContext.Photo.Remove(original);
          try
          {
            nDataContext.SaveChanges();
            success = true;
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
        }

      }
      return success;
    }

    public override Photo Update(Photo item)
    {
        bool success = false;
        try
        {
            var original = _dataContext.Photo.Find(item.Id);

            if (original != null)
            {
                if (item.Person_Id > 0)
                    original.Person_Id = item.Person_Id;

                success = Save();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return (success) ? item : null;   
    }

    public DbSet<Photo> Select(  )
    {
      return _dataContext.Photo;
    }

    public BioService.PhotoList Select(BioService.QueryPhoto command)
    {
      /*
      HashSet<long> targetPhotoIDs  = new HashSet<long>();
      HashSet<long> targetPersonIDs = new HashSet<long>();
      if (command.TargetPhoto != null && command.TargetPhoto.Count > 0)
      {
        foreach (BioService.Photo ph in command.TargetPhoto)
            targetPhotoIDs.Add(ph.Id);        
      }

      if (command.TargetPerson != null && command.TargetPerson.Count > 0)
      {
          foreach (BioService.Person ph in command.TargetPerson)
              targetPersonIDs.Add(ph.Id);
      }
      
      BioService.PhotoList photos = new BioService.PhotoList();
      List<Photo> entities = new List<Photo>();
      
      if (targetPhotoIDs.Count > 0)
          entities = _dataContext.Photo.Where(x => targetPhotoIDs.Contains(x.Id)).ToList();
    

      if (targetPersonIDs.Count > 0)
      {
          byte originType = (byte)BioService.PhotoOriginType.Loaded;
          entities.AddRange(_dataContext.Photo.Where(x => x.Person_Id.HasValue
                                               && targetPersonIDs.Contains(x.Person_Id.Value)                                              
                                               && x.Origin_Type == originType));
      }

      if (targetPersonIDs.Count == 0 && targetPhotoIDs.Count == 0)
          entities = _dataContext.Photo.ToList();
          

      foreach( Photo ph in entities)
      {
        BioService.Photo proto = _convertor.GetPhotoProto(ph);
        
        if (proto != null)
        {
            if (command.Description)
            {
                Google.Protobuf.ByteString description =_utils.GetFileDescription( _utils.LocalStorage + proto.FileLocation);
                if (description != null)
                  proto.Description = description;
            }
            
            if (command.Fir )
            {
                Google.Protobuf.ByteString description =_utils.GetFileDescription( _utils.LocalStorage + proto.FirLocation);
                if (description != null)
                  proto.Fir = description;
            }
            photos.Photos.Add(proto);
        }
      }
      */
      return null;
    }

    private readonly IOUtils _utils;
  }
}
