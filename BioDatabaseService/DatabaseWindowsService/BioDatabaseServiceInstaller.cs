using BioContracts;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioDatabaseService
{
  public class BioDatabaseServiceInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      try
      {
        // container.Register(Component.For<IServiceManager>().ImplementedBy<BioServiceManager>());

        container.Resolve<IProcessorLocator>().Init(container);

        container.Register(Component.For<BioDatabaseServiceImpl>());
      }
      catch (Exception ex)
      {
        Console.WriteLine("BioDatabaseService" + ex.Message);
      }
    }
  }
}
