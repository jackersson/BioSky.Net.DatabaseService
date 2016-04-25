using BioData.DataModels;
using System;
using System.Linq;

namespace BioData.Utils
{
  public class ProtoMessageConvertor
  {

    #region person
    public BioService.Person GetPersonProto(Person entity)
    {
      if (entity == null)
        return null;

      BioService.Person proto = new BioService.Person();

      proto.Id        = entity.Id;
      proto.Firstname = entity.First_Name_;
      proto.Lastname  = entity.Last_Name_;
      proto.Gender    = (BioService.Gender)entity.Gender;

      if (entity.Country != null)
        proto.Country = entity.Country;
      
      if (entity.City != null)
        proto.City = entity.City;

      if (entity.Comments != null)
        proto.Comments = entity.Comments;

      if (entity.Email != null)
        proto.Email = entity.Email;

       proto.Rights = (BioService.Rights)entity.Rights;

      if (entity.Date_Of_Birth.HasValue)
        proto.Dateofbirth = entity.Date_Of_Birth.Value.Ticks;

      if (entity.Thumbnail != null)      
        proto.Thumbnailid = entity.Thumbnail.Id;

      if (entity.BiometricData != null)      
        proto.BiometricData = GetBiometricDataProto(entity.BiometricData);
      
      if (entity.Card != null && entity.Card.Count > 0)
      {
        foreach (Card card in entity.Card)
          proto.Cards.Add(GetCardProto(card));
      }

      if (entity.Photos != null && entity.Photos.Count > 0)
      {
        foreach (Photo photo in entity.Photos)
          proto.Photos.Add(GetPhotoProto(photo));
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
      
      entity.First_Name_ = proto.Firstname;
      entity.Last_Name_  = proto.Lastname;
      entity.Gender      = (byte)proto.Gender;
      entity.Country     = proto.Country;
      entity.Email       = proto.Email;
      entity.City        = proto.City;
      entity.Comments    = proto.Comments;
      entity.Rights      = (byte)proto.Rights;

      if (proto.Thumbnailid > 0)
        entity.Thumbnail_Id = proto.Thumbnailid;      

      DateTime dateofbirth = new DateTime(proto.Dateofbirth);
      if (dateofbirth > DateTime.MinValue && dateofbirth < DateTime.MaxValue)
        entity.Date_Of_Birth = dateofbirth;    

      return entity;
    }
    #endregion

    #region BiometricData
    public BioService.BiometricData GetBiometricDataProto(BiometricData entity)
    {
      if (entity == null)
        return null;

      BioService.BiometricData proto = new BioService.BiometricData();

      foreach (FaceCharacteristic fc in entity.FaceCharacteristic)
        proto.Faces.Add(GetFaceCharacteristicProto(fc));

      foreach (FingerprintCharacteristic fc in entity.FingerprintCharacteristic)
        proto.Fingerprints.Add(GetFingerprintCharacteristicProto(fc));      

      return proto;
    }

    public BiometricData GetBiometricDataEntity(BioService.BiometricData proto)
    {
      if (proto == null)
        return null;

      BiometricData entity = new BiometricData();
      
      foreach (BioService.FaceCharacteristic fc in proto.Faces)      
          entity.FaceCharacteristic.Add(GetFaceCharacteristicEntity(fc));             
            
      foreach (BioService.FingerprintCharacteristic fc in proto.Fingerprints)
        entity.FingerprintCharacteristic.Add(GetFingerprintCharacteristicEntity(fc));

      return entity;
    }
    #endregion

    #region fingerprintDevice
    public BioService.FingerprintDevice GetFingerprintDeviceProto(FingerprintDevice entity)
    {
      if (entity == null)
        return null;

      BioService.FingerprintDevice proto = new BioService.FingerprintDevice() { Devicename = entity.DeviceName 
                                                                              , SerialNumber = entity.SerialNumber };
     
      return proto;
    }
    public FingerprintDevice GetFingerprintDeviceEntity(BioService.FingerprintDevice proto)
    {
      if (proto == null)
        return null;

      FingerprintDevice entity = new FingerprintDevice();
      
      entity.DeviceName   = proto.Devicename;
      entity.SerialNumber = proto.SerialNumber;
    
      return entity;
    }
    #endregion

    #region accessDevice
    public BioService.AccessDevice GetAccessDeviceProto(AccessDevice entity)
    {
      if (entity == null || string.IsNullOrEmpty( entity.PortName ))
        return null;

      BioService.AccessDevice proto = new BioService.AccessDevice() { Portname = entity.PortName };
  

      return proto;
    }
    public AccessDevice GetAccessDeviceEntity(BioService.AccessDevice proto)
    {
      if (proto == null)
        return null;

      AccessDevice entity = new AccessDevice();     

      entity.PortName   = proto.Portname; 
     
      return entity;
    }
    #endregion

    #region captureDevice
    public BioService.CaptureDevice GetCaptureDeviceProto(CaptureDevice entity)
    {
      if (entity == null || string.IsNullOrEmpty( entity.Device_Name ))
        return null;

      BioService.CaptureDevice proto = new BioService.CaptureDevice() { Devicename = entity.Device_Name };
    
      return proto;
    }

    public CaptureDevice GetCaptureDeviceEntity(BioService.CaptureDevice proto)
    {
      if (proto == null || string.IsNullOrEmpty(proto.Devicename))
        return null;

      CaptureDevice entity = new CaptureDevice() { Device_Name = proto.Devicename };

      return entity;
    }
    #endregion

    #region Card
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
    #endregion

    #region location
    public BioService.Location GetLocationProto(Location entity)
    {
      if (entity == null)
        return null;

      BioService.Location proto = new BioService.Location();

      proto.Id           = entity.Id;
      proto.LocationName = entity.Location_Name;
      proto.Description  = entity.Description;
      proto.MacAddress   = entity.MacAddress;

      #region accessInfo
      BioService.AccessInfo accessInfo = new BioService.AccessInfo();

      if (entity.Access_Type.HasValue)
        accessInfo.AccessType = (BioService.AccessInfo.Types.AccessType)entity.Access_Type.Value;
     
      if (accessInfo.AccessType == BioService.AccessInfo.Types.AccessType.Custom
           && entity.PersonAccess != null
           && entity.PersonAccess.Count > 0  )
      {
        foreach (PersonAccess pa in entity.PersonAccess)
          accessInfo.Persons.Add(new BioService.Person() { Id = pa.Person_Id });
      }

      proto.AccessInfo = accessInfo;
      #endregion

      if (entity.CaptureDevice != null)
        proto.CaptureDevice = GetCaptureDeviceProto(entity.CaptureDevice);

      if (entity.AccessDevice != null)
        proto.AccessDevice = GetAccessDeviceProto(entity.AccessDevice);

      if (entity.FingerprintDevice != null)
        proto.FingerprintDevice = GetFingerprintDeviceProto(entity.FingerprintDevice);

      return proto;
    }

    public Location GetLocationEntity(BioService.Location proto)
    {
      if (proto == null)
        return null;

      Location entity = new Location();
      
      if ( proto.Id > 0)
        entity.Id = proto.Id;
      
      if ( !string.IsNullOrEmpty(proto.LocationName))
        entity.Location_Name = proto.LocationName;

      entity.Description   = proto.Description;

      if (!string.IsNullOrEmpty(proto.LocationName))
        entity.MacAddress = proto.MacAddress;

      if (proto.AccessInfo != null)
        entity.Access_Type   = (byte)proto.AccessInfo.AccessType;

      return entity;
    }
    #endregion

   

    #region Visitor
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

      if (entity.BiometricData != null)
        proto.BiometricData = GetBiometricDataProto(entity.BiometricData);
  
      if (entity.Person_ID.HasValue)
        proto.Personid = entity.Person_ID.Value;

      return proto;
    }

    public Visitor GetVisitorEntity(BioService.Visitor proto)
    {
      if (proto == null)
        return null;

      Visitor entity = new Visitor();
      
      if (proto.Id > 0 )
        entity.Id           = proto.Id;

      entity.Location_Id    = proto.Locationid;
      entity.Detection_Time = new DateTime(proto.Time);
      entity.Card_Number    = proto.CardNumber;
 
      if (proto.Personid > 0)
        entity.Person_ID = proto.Personid;

      entity.Status    = (byte)proto.Status;
      return entity;
    }
    #endregion
      
    #region fingerprintCharacteristic

    public BioService.FingerprintCharacteristic GetFingerprintCharacteristicProto(FingerprintCharacteristic entity)
    {
      if (entity == null)
        return null;

      BioService.FingerprintCharacteristic proto = new BioService.FingerprintCharacteristic();

      proto.Position = (BioService.Finger)entity.Position;
      /*
      to load with byte string
      proto.FirUrl = entity.FirUrl;
      proto.FirBytestring = null;
      */
      return proto;
    }

    public FingerprintCharacteristic GetFingerprintCharacteristicEntity( BioService.FingerprintCharacteristic proto)
    {
      if (proto == null)
        return null;

      FingerprintCharacteristic entity = new FingerprintCharacteristic();

      entity.Position = (byte)proto.Position;
      
      /*
      to load with byte string
      proto.FirUrl = entity.FirUrl;
      proto.FirBytestring = null;
      */
      return entity;
    }
    #endregion
    
    #region faceCharacteristics
    public BioService.FaceCharacteristic GetFaceCharacteristicProto(FaceCharacteristic entity)
    {
      if (entity == null)
        return null;

      BioService.FaceCharacteristic proto = new BioService.FaceCharacteristic();

      if (entity.Width.HasValue)
        proto.Width = (float)entity.Width.Value;

      if (entity.Age.HasValue)
        proto.Age = entity.Age.Value;

      if (entity.Gender.HasValue)
        proto.Gender = (BioService.Gender)entity.Gender;

      /*
      to load with byte string
      proto.FirUrl = entity.FirUrl;
      proto.FirBytestring = null;
      */
      if (entity.BiometricLocation != null)
        proto.Location = GetBiometricLocationProto(entity.BiometricLocation);

      if (entity.EyesCharacteristic != null)
        proto.Eyes = GetEyesCharacteristicProto(entity.EyesCharacteristic);
      
      return proto;
    }

    public FaceCharacteristic GetFaceCharacteristicEntity(BioService.FaceCharacteristic proto)
    {      
      if (proto == null)
        return null;

      FaceCharacteristic entity = new FaceCharacteristic();

      if (proto.Age > 0)
        entity.Age = (byte)proto.Age;

      if (proto.Width > 0)
        entity.Width = proto.Width;

      entity.Gender = (byte)proto.Gender;

      if (proto.Location != null)
        entity.BiometricLocation = GetBiometricLocationEntity(proto.Location);
  
      if (proto.Eyes != null)      
        entity.EyesCharacteristic = GetEyesCharacteristicEntity(proto.Eyes);

      if (proto.Photoid > 0)
        entity.Photo_Id = proto.Photoid;
              
      /*
      to load with byte string
      proto.FirUrl = entity.FirUrl;
      proto.FirBytestring = null;
      */         

      return entity;   
    }
    #endregion

    #region eyesCharacteristic
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

    public EyesCharacteristic GetEyesCharacteristicEntity(BioService.EyesCharacteristic proto)
    {
      if (proto == null)
        return null;

      EyesCharacteristic entity = new EyesCharacteristic();

      if (proto.LeftEye != null)
        entity.LeftEyeBiometricLocation = GetBiometricLocationEntity(proto.LeftEye);

      if (proto.RightEye != null)
        entity.RightEyeBiometricLocation = GetBiometricLocationEntity(proto.RightEye);

      return entity;
    }
    #endregion
    
    #region BiometricLocation
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

    public BiometricLocation GetBiometricLocationEntity(BioService.BiometricLocation proto)
    {
      if (proto == null)
        return null;

      BiometricLocation entity = new BiometricLocation();

      entity.XPos       = proto.Xpos      ;
      entity.YPos       = proto.Ypos      ;
      entity.Confidence = proto.Confidence;      

      return entity;
    }
    #endregion
    
    #region Photo
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
       
      if (entity.Datetime.HasValue)
        proto.Datetime = entity.Datetime.Value.Ticks;

      if (entity.Owner_Id.HasValue)
        proto.OwnerId = entity.Owner_Id.Value;
           
      proto.Width  = entity.Width ;     
      proto.Height = entity.Height;

      return proto;
    }

    public Photo GetPhotoEntity(BioService.Photo proto)
    {
      if (proto == null)
        return null;

      Photo entity = new Photo();

      if (proto.Id > 0)
        entity.Id          = proto.Id;

      entity.Size_Type   = (byte)proto.SizeType;
      entity.Origin_Type = (byte)proto.OriginType;
      entity.Width       = (int)proto.Width;
      entity.Height      = (int)proto.Height;

      if ( proto.Datetime > 0)
        entity.Datetime    = new DateTime(proto.Datetime);

      if (proto.OwnerId > 0)
        entity.Owner_Id    = proto.OwnerId;

      if (!string.IsNullOrEmpty(proto.PhotoUrl))
        entity.Photo_Url = proto.PhotoUrl;

      return entity;
    }
    #endregion
        
    #region criminal
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
    #endregion


    public bool IsDeleteState( string field )
    {
      if (string.IsNullOrEmpty(field))
        return false;

      string deleteState = "delete";
      return deleteState.Equals(field);
    }

    private readonly IOUtils _utils = new IOUtils();

  }
}
