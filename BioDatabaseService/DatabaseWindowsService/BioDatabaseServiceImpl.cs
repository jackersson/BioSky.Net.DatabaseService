using BioContracts;

namespace BioDatabaseService
{
  public class BioDatabaseServiceImpl
  {
    private readonly IProcessorLocator _locator;
    public BioDatabaseServiceImpl(IProcessorLocator locator)
    {
      _locator = locator;
    }

    public void Init()
    {
   
    }
  }
}
