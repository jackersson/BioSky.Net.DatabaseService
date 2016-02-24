using BioContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
     // _locator.GetProcessor
    }
  }
}
