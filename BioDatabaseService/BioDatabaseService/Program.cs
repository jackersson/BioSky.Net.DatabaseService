using BioContracts;
using BioContracts.Common;
using BioData;
using BioData.DataHolders.Grouped;
using BioData.DataModels;
using BioGrpc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BioData.Utils;

using BioData.DataHolders;

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

        /*
      PersonDataHolder repo = container.Resolve<PersonDataHolder>();

      Person item = new Person() { Id = 7, Date_Of_Birth = DateTime.Now };

      repo.Update(item);
      repo.Remove(item);
         */
      //IProcessorLocator locator = container.Resolve<IProcessorLocator>();

      BioServiceManager serviceManager = container.Resolve<BioServiceManager>();
      serviceManager.Start(new ServiceConfiguration() { IpAddress = "0.0.0.0", Port = 50051 });
    
      Console.ReadKey();
      serviceManager.Stop();
    }
  }
}
