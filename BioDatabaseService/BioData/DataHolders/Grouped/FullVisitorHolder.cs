using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System.Data.Entity;

namespace BioData.DataHolders.Grouped
{

  public class FullVisitorHolder
  {
    public FullVisitorHolder(IProcessorLocator locator)
    {
      _locator = locator;

      _visitors = locator.GetProcessor<VisitorDataHolder>();
      _photos   = locator.GetProcessor<PhotoDataHolder>();

      _convertor = new ProtoMessageConvertor();
      _utils     = new IOUtils();
    }

    public BioService.VisitorList Update(BioService.VisitorList proto)
    {
      BioService.VisitorList visitors = new BioService.VisitorList();
      foreach (BioService.Visitor curProto in proto.Visitors)
      {
        BioService.Visitor updatedProto = Update(curProto);
        if (updatedProto != null)
          visitors.Visitors.Add(updatedProto);
      }

      return visitors;
    }

    public BioService.Visitor Update(BioService.Visitor proto)
    {
      /*
      if (proto.Photo != null)
      {
          proto.Photo.FileLocation = _utils.GetLocationImagePath(proto.Locationid);
          if (proto.Photo.Fir != null && proto.Photo.Fir.Length > 0)
              proto.Photo.FirLocation = _utils.GetLocationFirPath(proto.Locationid);
      }

      BioService.Photo updatedPhoto = _photos.UpdateFromProto(proto.Photo);

      if (proto.EntityState != BioService.EntityState.Deleted)
        proto.Photoid = updatedPhoto == null ? 0 : updatedPhoto.Id;    

      BioService.Visitor updatedVisitor = _visitors.UpdateFromProto(proto);

      if (updatedVisitor == null)
        return null;

      updatedVisitor.Photo = updatedPhoto;

      if (proto.EntityState == BioService.EntityState.Deleted)
      {
        IQueryable<Photo> allphotos = _photos.Select().Where(x => x.Id == proto.Photoid);
        foreach (Photo cp in allphotos)
        {
          _photos.Remove(cp);
          updatedVisitor.Photoid = cp.Id;
        }
      }   
      */
      return null;
    }

    

    public BioService.VisitorList Select(BioService.QueryVisitors command)
    {
      BioService.VisitorList result = new BioService.VisitorList();

      DbSet<Visitor> visitors = _visitors.Select();
      //DbSet<Photo> photos    = _photos.Select();

      if (visitors == null)
          return result;

    


          foreach (Visitor visitor in visitors)
          {
              BioService.Visitor protoVisitor = _convertor.GetVisitorProto(visitor);

              if (protoVisitor == null)
                  continue;

              //long visitorid = visitor.Id;
              /*
              Photo visitorPhoto = photos.Where(x => x.Id == visitor.Photo_ID).FirstOrDefault();
              BioService.Photo currentPhotoProto = _convertor.GetPhotoProto(visitorPhoto);
              protoVisitor.Photo = currentPhotoProto;
              */
              result.Visitors.Add(protoVisitor);
          }
    

      return result;
    }

    private readonly IOUtils               _utils;
    private readonly ProtoMessageConvertor _convertor;
    private readonly IProcessorLocator _locator ;
    private readonly VisitorDataHolder _visitors;
    private readonly PhotoDataHolder   _photos  ;
   
  }
}
