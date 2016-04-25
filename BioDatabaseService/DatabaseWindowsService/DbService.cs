using BioContracts;
using BioContracts.Common;
using BioDatabaseService;
using BioGrpc;
using Castle.Windsor;
using System.IO;
using System.Reflection;
using System.ServiceProcess;


namespace DatabaseWindowsService
{
  public class DbService : ServiceBase
  {
    BioServiceManager serviceManager;
    public DbService()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
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

    public void OnDebug()
    {
      OnStart(null);
    }

    protected override void OnStart(string[] args)
    {
      serviceManager.Start(new ServiceConfiguration() { IpAddress = "0.0.0.0", Port = 50051 });
    }


    protected override void OnStop()
    {
      serviceManager.Stop();
    }
  }
}
