using BioData.DataModels;
using System;
using System.Linq;

namespace BioData.Utils
{
  public class ProtoMessageConvertor
  {
    public BioService.Person GetPersonProto(Person entity)
    {
      if (entity == null)
        return null;

      BioService.Person proto = new BioService.Person();

      proto.Id        = entity.Id;
      proto.Firstname = entity.First_Name_;
      proto.Lastname  = entity.Last_Name_;
      proto.Gender    = (BioService.Person.Types.Gender)entity.Gender;

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

      if (entity.Photo != null)
      {
        proto.Thumbnail = GetPhotoProto(entity.Photo);
        proto.Photoid   = entity.Photo.Id;
      }

      if ( entity.PhotoCollection != null && entity.PhotoCollection.Count > 0)
      {
        foreach (Photo photo in entity.PhotoCollection)
          proto.Photos.Add(GetPhotoProto(photo));
      }

      if (entity.Card != null && entity.Card.Count > 0)
      {
        foreach (Card card in entity.Card)
          proto.Cards.Add(GetCardProto(card));
      }

      if (entity.Criminal != null)            
        proto.Criminal  = GetCriminalProto(entity.Criminal);
      
      return proto;
    }

    public Person GetPersonEntity(BioService.Person proto)
    {
      if (proto == null)
        return null;

      Person entity  = new Person();

      entity.Id          = proto.Id;
      entity.First_Name_ = proto.Firstname;
      entity.Last_Name_  = proto.Lastname;
      entity.Gender      = (byte)proto.Gender;
      entity.Country     = proto.Country;
      entity.Email       = proto.Email;
      entity.City        = proto.City;
      entity.Comments    = proto.Comments;
      entity.Rights      = (byte)proto.Rights;

      //if (proto.Photoid > 0 && proto.Thumbnail != null)
         //entity.Photo_Id    = proto.Photoid;

      DateTime dateofbirth = new DateTime(proto.Dateofbirth);
      if (dateofbirth > DateTime.MinValue && dateofbirth < DateTime.MaxValue)
        entity.Date_Of_Birth = dateofbirth;
     // entity.Thumbnail = proto.Thumbnailid;

      return entity;
    }

    public BioService.AccessDevice GetAccessDeviceProto(AccessDevice entity)
    {
      if (entity == null)
        return null;

      BioService.AccessDevice proto = new BioService.AccessDevice();

      //proto.Id = entity.Id;
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
    
      //proto.Id = entity.Id;
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

      if (entity.Id > 0)
        entity.Id           = proto.Id;

      if (proto.UniqueNumber != "")
        entity.Unique_Number = proto.UniqueNumber;

      if (proto.Personid > 0)
        entity.Person_Id = proto.Personid;

      return entity;
    }

    public BioService.Location GetLocationProto(Location entity)
    {
      if (entity == null)
        return null;

      BioService.Location proto = new BioService.Location();

      proto.Id           = entity.Id;
      proto.LocationName = entity.Location_Name;
      proto.Description  = entity.Description;

      if (entity.Access_Type.HasValue)
        proto.AccessType = (BioService.Location.Types.AccessType)entity.Access_Type.Value;
     
      if (    proto.AccessType == BioService.Location.Types.AccessType.Custom
           && entity.PersonAccessCollection != null
           && entity.PersonAccessCollection.Count > 0  )
      {
        foreach (PersonAccess pa in entity.PersonAccessCollection)
          proto.Persons.Add(new BioService.Person() { Id = pa.Person_Id });
      }

      CaptureDevice capDev = entity.CaptureDevice.FirstOrDefault();
      if (capDev != null)
        proto.CaptureDevice = GetCaptureDeviceProto(capDev);

      AccessDevice accDev = entity.AccessDevice.FirstOrDefault();
      if (accDev != null)
        proto.AccessDevice = GetAccessDeviceProto(accDev);

      return proto;
    }

    public Location GetLocationEntity(BioService.Location proto)
    {
      if (proto == null)
        return null;

      Location entity = new Location();
      
      entity.Id = proto.Id;
      entity.Location_Name = proto.LocationName;
      entity.Description   = proto.Description;
      entity.Access_Type   = (byte)proto.AccessType;

      return entity;
    }


    public BioService.Visitor GetVisitorProto(Visitor entity)
    {
      if (entity == null)
        return null;

      BioService.Visitor proto = new BioService.Visitor();

      proto.Id         = entity.Id;
      proto.Locationid = entity.Location_Id;
      proto.Time       = entity.Detection_Time.Ticks;
      proto.CardNumber = entity.Card_Number;
      proto.Status     = (BioService.Result)entity.Status;

      if (entity.FullPhoto != null)
        proto.Fullphoto = GetPhotoProto(entity.FullPhoto);

      if (entity.CropedPhoto != null)
        proto.Cropedphoto = GetPhotoProto(entity.CropedPhoto);
  
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
      //entity.Photo_ID  = proto.Photoid;
      entity.Person_ID = proto.Personid;
      entity.Status    = (byte)proto.Status;
      return entity;
    }

    public BioService.PortraitCharacteristic GetPortraitCharacteristicProto(PortraitCharacteristic entity)
    {
      if (entity == null)
        return null;

      BioService.PortraitCharacteristic proto = new BioService.PortraitCharacteristic();

      if ( entity.Age.HasValue )
        proto.Age = entity.Age.Value;

      if (entity.Face_Count.HasValue)
        proto.FacesCount = entity.Face_Count.Value;

      if ( entity.FaceCharacteristic != null && entity.FaceCharacteristic.Count > 0)
      {
        foreach (FaceCharacteristic fc in entity.FaceCharacteristic)
          proto.Faces.Add(GetFaceCharacteristicProto(fc));
      }
     
      return proto;
    }

    public BioService.FaceCharacteristic GetFaceCharacteristicProto(FaceCharacteristic entity)
    {
      if (entity == null)
        return null;

      BioService.FaceCharacteristic proto = new BioService.FaceCharacteristic();

      if (entity.Width.HasValue)
        proto.Width = (long)entity.Width.Value;

      if (entity.FaceBiometricLocation != null)
        proto.Location = GetBiometricLocationProto(entity.FaceBiometricLocation);

      if (entity.EyesCharacteristic != null)
        proto.Eyes = GetEyesCharacteristicProto(entity.EyesCharacteristic);
      
      return proto;
    }

    public BioService.EyesCharacteristic GetEyesCharacteristicProto(EyesCharacteristic entity)
    {
      if (entity == null)
        return null;

      BioService.EyesCharacteristic proto = new BioService.EyesCharacteristic();

      if (entity.LeftEyeBiometricLocation != null)
        proto.LeftEye = GetBiometricLocationProto(entity.LeftEyeBiometricLocation);

      if (entity.RightEyeBiometricLocation != null)
        proto.RightEye = GetBiometricLocationProto(entity.RightEyeBiometricLocation);

       return proto;
    }

    public BioService.BiometricLocation GetBiometricLocationProto(BiometricLocation entity)
    {
      if (entity == null)
        return null;

      BioService.BiometricLocation proto = new BioService.BiometricLocation();

      proto.Xpos       = (float)entity.XPos;
      proto.Ypos       = (float)entity.YPos;
      proto.Confidence = (float)entity.Confidence;

      return proto;
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
            
      proto.PhotoUrl = entity.Photo_Url;
      
      if (entity.Person_Id.HasValue)
        proto.Personid = entity.Person_Id.Value;

      if (entity.Datetime.HasValue)
        proto.Datetime = entity.Datetime.Value.Ticks;

      if (entity.Width.HasValue)
        proto.Width = entity.Width.Value;

      if (entity.Height.HasValue)
        proto.Height = entity.Height.Value;

      if (entity.PortraitCharacteristic != null)
        proto.PortraitCharacteristic = GetPortraitCharacteristicProto(entity.PortraitCharacteristic);

      return proto;
    }


    public BioService.Photo GetFullPhotoProto(Photo entity)
    {
      if (entity == null)
        return null;

      BioService.Photo proto = GetPhotoProto(entity);
      Google.Protobuf.ByteString description = _utils.GetFileDescription(_utils.LocalStorage + proto.PhotoUrl);
      if (description != null)
        proto.Bytestring = description;
      return proto;
    }

    public Photo GetPhotoEntity(BioService.Photo proto)
    {
      if (proto == null)
        return null;

      Photo entity = new Photo();

      entity.Id = proto.Id;
      entity.Size_Type   = (byte)proto.SizeType;
      entity.Origin_Type = (byte)proto.OriginType;
      entity.Width       = (int)proto.Width;
      entity.Height      = (int)proto.Height;
      entity.Datetime    = new DateTime(proto.Datetime);     
      entity.Person_Id   = proto.Personid;

      return entity;
    }

    public BioService.Criminal GetCriminalProto(Criminal entity)
    {
      if (entity == null)
        return null;

      BioService.Criminal proto = new BioService.Criminal();

      proto.Description = entity.Description;
      //TODO add alert
      return proto;
    }

    public Criminal GetCriminalEntity(BioService.Criminal proto)
    {
      if (proto == null)
        return null;

      Criminal entity = new Criminal();

      //entity.Id = proto.Id;
      //entity.Unique_Number = proto.UniqueNumber;
      //entity.Person_Id = proto.Personid;

      return entity;
    }

    private readonly IOUtils _utils = new IOUtils();

  }
}
