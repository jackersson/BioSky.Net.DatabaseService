using BioContracts;
using BioContracts.Common;
using BioGrpc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.IO;
using System.Reflection;

namespace BioDatabaseService
{
  class Program
  {    
    static void Main(string[] args)
    {      
      var container     = new WindsorContainer();
      var modulesLoader = new ModulesLoader(container);


      container
         .Register(Component.For<IWindsorContainer>().Instance(container))
         .Register(Component.For<IProcessorLocator>().ImplementedBy<ProcessorLocator>())
         .Register(Component.For<BioDatabaseServiceInstaller>());

      var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      modulesLoader.LoadData(Assembly.LoadFile(exeDir + @"\BioData.dll"));
      modulesLoader.LoadData(Assembly.LoadFile(exeDir + @"\BioGrpc.dll"));

      container.Install(new BioDatabaseServiceInstaller());

     
      BioServiceManager serviceManager = container.Resolve<BioServiceManager>();
      serviceManager.Start(new ServiceConfiguration() { IpAddress = "0.0.0.0", Port = 50051 });


      Console.ReadKey();
      serviceManager.Stop();    
    }
  }
}
