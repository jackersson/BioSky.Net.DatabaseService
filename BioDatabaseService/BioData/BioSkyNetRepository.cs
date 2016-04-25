using BioContracts;
using BioData.DataClients;

namespace BioData
{
  //test
  public class BioSkyNetRepository
  {
    public BioSkyNetRepository( IProcessorLocator locator )
    {
      _locator = locator;

      PhotosDataClient = new PhotoDataClient(locator);

      BiometricLocationDataClient  = new BiometricLocationDataClient(locator);
      EyesCharacteristicDataClient = new EyesCharacteristicDataClient(locator, BiometricLocationDataClient);
      FaceCharacteristicDataClient = new FaceCharacteristicDataClient(locator, EyesCharacteristicDataClient, BiometricLocationDataClient);
      FacialDataClient             = new FacialDataClient(locator, PhotosDataClient, FaceCharacteristicDataClient);

      CardsDataClient     = new CardDataClient     (locator);
      BiometricDataClient = new BiometricDataClient(locator, FacialDataClient);

      AccessDeviceDataClient      accessDeviceDataClient      = new AccessDeviceDataClient     (locator);
      CaptureDeviceDataClient     captureDeviceDataClient     = new CaptureDeviceDataClient    (locator);
      FingerprintDeviceDataClient fingerprintDeviceDataClient = new FingerprintDeviceDataClient(locator);
      PersonAccessDataClient      personAccessDataClient      = new PersonAccessDataClient     (locator);

      VisitorsDataClient  = new VisitorDataClient(locator, BiometricDataClient);
      LocationsDataClient = new LocationDataClient( locator
                                                  , accessDeviceDataClient
                                                  , captureDeviceDataClient
                                                  , fingerprintDeviceDataClient
                                                  , personAccessDataClient
                                                  , VisitorsDataClient          );

      PersonDataClient = new PersonDataClient(locator, CardsDataClient, BiometricDataClient);
   
    }  
    

    public PersonDataClient              PersonDataClient   ;
    public CardDataClient                CardsDataClient    ;
    public PhotoDataClient               PhotosDataClient   ;
    public LocationDataClient            LocationsDataClient;
    public VisitorDataClient             VisitorsDataClient ;
    public BiometricDataClient           BiometricDataClient;
    public FacialDataClient              FacialDataClient   ;
    public FaceCharacteristicDataClient  FaceCharacteristicDataClient;
    public EyesCharacteristicDataClient  EyesCharacteristicDataClient;
    public BiometricLocationDataClient   BiometricLocationDataClient ;

    private readonly IProcessorLocator _locator;
  }
}
