using BioContracts;
using BioData.DataHolders;
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
    private BioSkyNetDataModel _bioSkyNetContext;
    private readonly IProcessorLocator _locator;

    public BioSkyNetRepository( IProcessorLocator locator   )
    {
      _locator = locator;
      //if (contextFactory == null)
      // throw new ArgumentNullException("entityFrameworkContextFactory");

      // _contextFactory = contextFactory;

      InitializeContext();     

      _photos        = new PhotoDataHolder(locator);

      _fullPersons   = new FullPersonHolder(locator);
      _fullVisitors  = new FullVisitorHolder(locator);
      _fullLocations = new FullLocationHolder(locator);
    }
   

    private bool InitializeContext()
    {
      try
      {
        IContextFactory contextFactory = _locator.GetProcessor<IContextFactory>();
        if (contextFactory == null)
           throw new ArgumentNullException("entityFrameworkContextFactory");

        _bioSkyNetContext = contextFactory.Create<BioSkyNetDataModel>();

        _bioSkyNetContext.Configuration.AutoDetectChangesEnabled = true;
      
        return true;
      }
      catch { }

      return false;
    }

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
    private readonly FullLocationHolder _fullLocations;
  }
}
