using BioContracts;
using BioData.DataHolders;
using BioData.DataHolders.DataClient;
using BioData.DataHolders.Grouped;
using BioData.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData
{
  public class BioSkyNetRepository
  {
   // private readonly IContextFactory _contextFactory;
    //private BioSkyNetDataModel _bioSkyNetContext;
    private readonly IProcessorLocator _locator;

    public BioSkyNetRepository( IProcessorLocator locator   )
    {
      _locator = locator;
      //if (contextFactory == null)
      // throw new ArgumentNullException("entityFrameworkContextFactory");

      // _contextFactory = contextFactory;

      //InitializeContext();  
      
      CardsDataClient  = new CardDataClient(locator);
      PhotosDataClient = new PhotoDataClient(locator);

      AccessDeviceDataClient  acdt = new AccessDeviceDataClient (locator);
      CaptureDeviceDataClient cddt = new CaptureDeviceDataClient(locator);
      PersonAccessDataClient  padt = new PersonAccessDataClient (locator);

      VisitorsDataClient  = new VisitorDataClient(locator);
      LocationsDataClient = new LocationDataClient(locator, acdt, cddt, padt, VisitorsDataClient);

      PDataClient = new PersonDataClient(locator, PhotosDataClient, CardsDataClient);
      //_photos        = new PhotoDataHolder(locator);

      // _fullPersons   = new FullPersonHolder(locator);
      //_fullVisitors  = new FullVisitorHolder(locator);
      //_fullLocations = new FullLocationHolder(locator);
    }
   

  
    /*
    public PhotoDataHolder Photos()
    {
      return _photos;
    }

    public FullPersonHolder FullPersons()
    {
      return _fullPersons;
    }

    public FullVisitorHolder FullVisitors()
    {
      return _fullVisitors;
    }

    public FullLocationHolder FullLocations()
    {
      return _fullLocations;
    }
    
    private readonly PhotoDataHolder _photos;

    private readonly FullPersonHolder   _fullPersons  ;
    private readonly FullVisitorHolder  _fullVisitors ;
    private readonly FullLocationHolder _fullLocations;*/

    public PersonDataClient   PDataClient       ;
    public CardDataClient     CardsDataClient   ;
    public PhotoDataClient    PhotosDataClient  ;
    public LocationDataClient LocationsDataClient;
    public VisitorDataClient  VisitorsDataClient ;
  }
}
