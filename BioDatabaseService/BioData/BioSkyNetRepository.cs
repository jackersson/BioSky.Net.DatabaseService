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
      
      CardsDataClient  = new CardDataClient(locator);
      PhotosDataClient = new PhotoDataClient(locator);

      AccessDeviceDataClient  accessDeviceDataClient  = new AccessDeviceDataClient (locator);
      CaptureDeviceDataClient captureDeviceDataClient = new CaptureDeviceDataClient(locator);
      PersonAccessDataClient  personAccessDataClient  = new PersonAccessDataClient (locator);

      VisitorsDataClient  = new VisitorDataClient(locator);
      LocationsDataClient = new LocationDataClient(locator, accessDeviceDataClient, captureDeviceDataClient, personAccessDataClient, VisitorsDataClient);

      PersonDataClient = new PersonDataClient(locator, PhotosDataClient, CardsDataClient);
   
    }  
    

    public PersonDataClient   PersonDataClient   ;
    public CardDataClient     CardsDataClient    ;
    public PhotoDataClient    PhotosDataClient   ;
    public LocationDataClient LocationsDataClient;
    public VisitorDataClient  VisitorsDataClient ;

    private readonly IProcessorLocator _locator;
  }
}
