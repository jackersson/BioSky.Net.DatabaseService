using BioContracts;

using BioData.DataModels;
using BioData.Utils;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BioData
{
  public class BioDataInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {      
      container.Register(Component.For<IConnectionBuilder>()
               .ImplementedBy<BioConnectionBuilder>()
               .DependsOn(new
               {
                 dbConnectionstring = @"F:\Biometric Software\BioSky.Net.DatabaseService\BioDatabaseService\database\BioSkyNet.mdf"         

                 // Sasha @"D:\Spark\DataBase\BioDatabaseService\database\BioSkyNet.mdf"
                 // Taras @"F:\Biometric Software\BioSky.Net.DatabaseService\BioDatabaseService\database\BioSkyNet.mdf"
               })
                 .LifestyleSingleton()
               );

      container.AddFacility<TypedFactoryFacility>()
               .Register(Component.For<IContextFactory>().AsFactory());

      container.Register(Component.For<BioSkyNetDataModel>().LifestyleTransient());
            
      container.Register(Component.For<BioSkyNetRepository>());

      

    }
  }
}
