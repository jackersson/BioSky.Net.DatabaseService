using System.ServiceProcess;

using BioContracts;
using BioContracts.Common;
using BioDatabaseService;
using BioGrpc;
using Castle.Windsor;
using System.IO;
using System.Reflection;

namespace DatabaseWindowsService
{
  partial class Service1 : ServiceBase
  {
    BioServiceManager serviceManager;
    public Service1()
    {
      InitializeComponent();
      
      WindsorContainer container = new WindsorContainer();
      var modulesLoader = new ModulesLoader(container);


      container
         .Register(Castle.MicroKernel.Registration.Component.For<IWindsorContainer>().Instance(container))
         .Register(Castle.MicroKernel.Registration.Component.For<IProcessorLocator>().ImplementedBy<ProcessorLocator>())
         .Register(Castle.MicroKernel.Registration.Component.For<BioDatabaseServiceInstaller>());

      var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      modulesLoader.LoadData(Assembly.LoadFile(exeDir + @"\BioData.dll"));
      modulesLoader.LoadData(Assembly.LoadFile(exeDir + @"\BioGrpc.dll"));

      container.Install(new BioDatabaseServiceInstaller());


      serviceManager = container.Resolve<BioServiceManager>();
      
      //  serviceManager.Start(new ServiceConfiguration() { IpAddress = "0.0.0.0", Port = 50051 });


      //Console.ReadKey();
      //serviceManager.Stop();
    }

  

    protected override void OnStart(string[] args)
    {
      serviceManager.Start(new ServiceConfiguration() { IpAddress = "0.0.0.0", Port = 50051 });
      // TODO: Add code here to start your service.
    }

    protected override void OnStop()
    {
      serviceManager.Stop();
      // TODO: Add code here to perform any tear-down necessary to stop your service.
    }
  }
}
