using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;

namespace BioGrpc
{
  public class BioGrpcInstaller : IWindsorInstaller
  {    
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      try  {
        container.Register(Component.For<BioServiceManager>());
      }
      catch (Exception ex) {
        Console.WriteLine("BioGrpc.dll" + ex.Message);
      }
    }
  }
}
