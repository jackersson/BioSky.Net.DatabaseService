using BioData.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.Utils
{
  public class ProtoMessageConvertor
  {
    public BioService.Person GetPersonProto(Person entity)
    {
      if (entity == null)
        return null;


      BioService.Person proto = new BioService.Person();

      proto.Id = entity.Id;
      proto.Firstname = entity.First_Name_;
      proto.Lastname = entity.Last_Name_;
      proto.Gender = (BioService.Person.Types.Gender)entity.Gender;

      if (entity.Country != null)
        proto.Country = entity.Country;
      
      if (entity.City != null)
        proto.City = entity.City;

      if (entity.Comments != null)
        proto.Comments = entity.Comments;

      if (entity.Email != null)
        proto.Email = entity.Email;

      proto.Rights = (BioService.Person.Types.Rights)entity.Rights;

      if (entity.Date_Of_Birth.HasValue)
        proto.Dateofbirth = entity.Date_Of_Birth.Value.Ticks;

      if (entity.Thumbnail.HasValue)
        proto.Thumbnailid = entity.Thumbnail.Value;

      return proto;
    }

    public Person GetPersonEntity(BioService.Person proto)
    {
      if (proto == null)
        return null;

      Person entity = new Person();

      entity.Id = proto.Id;
      entity.First_Name_ = proto.Firstname;
      entity.Last_Name_ = proto.Lastname;
      entity.Gender = (byte)proto.Gender;
      entity.Country = proto.Country;
      entity.Email = proto.Email;
      entity.City = proto.City;
      entity.Comments = proto.Comments;
      entity.Rights = (byte)proto.Rights;
      DateTime dateofbirth = new DateTime(proto.Dateofbirth);
      if (dateofbirth > DateTime.MinValue && dateofbirth < DateTime.MaxValue)
        entity.Date_Of_Birth = dateofbirth;
      entity.Thumbnail = proto.Thumbnailid;

      return entity;
    }

    public BioService.AccessDevice GetAccessDeviceProto(AccessDevice entity)
    {
      if (entity == null)
        return null;

      BioService.AccessDevice proto = new BioService.AccessDevice();

      proto.Id = entity.Id;
      proto.Portname = entity.PortName;
      proto.Type = (BioService.AccessDevice.Types.AccessDeviceType)entity.Location_Type;

      if (entity.Location_Id.HasValue)
        proto.Locationid = entity.Location_Id.Value;


      return proto;
    }

    public AccessDevice GetAccessDeviceEntity(BioService.AccessDevice proto)
    {
      if (proto == null)
        return null;

      AccessDevice entity = new AccessDevice();

      entity.Id = proto.Id;
      entity.PortName = proto.Portname;
      entity.Location_Type = (byte)proto.Type;
      entity.Location_Id = proto.Locationid;

      return entity;
    }


    public BioService.CaptureDevice GetCaptureDeviceProto(CaptureDevice entity)
    {
      if (entity == null)
        return null;

      BioService.CaptureDevice proto = new BioService.CaptureDevice();

      proto.Id = entity.Id;
      proto.Devicename = entity.Device_Name;

      if (entity.Location_Id.HasValue)
        proto.Locationid = entity.Location_Id.Value;

      return proto;
    }

    public CaptureDevice GetCaptureDeviceEntity(BioService.CaptureDevice proto)
    {
      if (proto == null)
        return null;

      CaptureDevice entity = new CaptureDevice();

      entity.Id = proto.Id;
      entity.Device_Name = proto.Devicename;
      entity.Location_Id = proto.Locationid;

      return entity;
    }


    public BioService.Card GetCardProto(Card entity)
    {
      if (entity == null)
        return null;

      BioService.Card proto = new BioService.Card();

      proto.Id = entity.Id;
      proto.UniqueNumber = entity.Unique_Number;

      if (entity.Person_Id.HasValue)
        proto.Personid = entity.Person_Id.Value;

      return proto;
    }

    public Card GetCardEntity(BioService.Card proto)
    {
      if (proto == null)
        return null;

      Card entity = new Card();

      entity.Id = proto.Id;
      entity.Unique_Number = proto.UniqueNumber;
      entity.Person_Id = proto.Personid;

      return entity;
    }


    public BioService.Location GetLocationProto(Location entity)
    {
      if (entity == null)
        return null;

      BioService.Location proto = new BioService.Location();

      proto.Id = entity.Id;
      proto.LocationName = entity.Location_Name;
      proto.Description = entity.Description;

      return proto;
    }

    public Location GetLocationEntity(BioService.Location proto)
    {
      if (proto == null)
        return null;

      Location entity = new Location();

      entity.Id = proto.Id;
      entity.Location_Name = proto.LocationName;
      entity.Description = proto.Description;

      return entity;
    }


    public BioService.Visitor GetVisitorProto(Visitor entity)
    {
      if (entity == null)
        return null;

      BioService.Visitor proto = new BioService.Visitor();

      proto.Id = entity.Id;
      proto.Locationid = entity.Location_Id;
      proto.Time = entity.Detection_Time.Ticks;
      proto.CardNumber = entity.Card_Number;
      proto.Status = (BioService.ResultStatus)entity.Status;
      //TODO something bad in logic
      if (entity.Photo_ID.HasValue)
        proto.Photoid = entity.Photo_ID.Value;

      if (entity.Person_ID.HasValue)
        proto.Personid = entity.Person_ID.Value;


      return proto;
    }

    public Visitor GetVisitorEntity(BioService.Visitor proto)
    {
      if (proto == null)
        return null;

      Visitor entity = new Visitor();

      entity.Id = proto.Id;
      entity.Location_Id = proto.Locationid;
      entity.Detection_Time = new DateTime(proto.Time);
      entity.Card_Number = proto.CardNumber;
      entity.Photo_ID  = proto.Photoid;
      entity.Person_ID = proto.Personid;
      entity.Status    = (byte)proto.Status;
      return entity;
    }


    public BioService.Photo GetPhotoProto(Photo entity)
    {
      if (entity == null)
        return null;

      BioService.Photo proto = new BioService.Photo();

      proto.Id = entity.Id;

      if (entity.Size_Type != null)
        proto.SizeType = (BioService.PhotoSizeType)entity.Size_Type;

      if (entity.Origin_Type != null)
        proto.OriginType = (BioService.PhotoOriginType)entity.Origin_Type;

      proto.FileLocation = entity.Image_Pathway;
      proto.FirLocation = entity.Fir_Pathway;

      if (entity.Person_Id.HasValue)
        proto.Personid = entity.Person_Id.Value;

      return proto;
    }

    public Photo GetPhotoEntity(BioService.Photo proto)
    {
      if (proto == null)
        return null;

      Photo entity = new Photo();

      entity.Id = proto.Id;
      entity.Size_Type = (byte)proto.SizeType;
      entity.Origin_Type = (byte)proto.OriginType;

      entity.Image_Pathway = proto.FileLocation;
      entity.Fir_Pathway = proto.FirLocation;
      entity.Person_Id = proto.Personid;

      return entity;
    }


  }
}
