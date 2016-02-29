using BioContracts;
using BioData.DataHolders;
using BioData.DataHolders.Grouped;
using BioData.DataModels;
using BioData.Utils;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                   //dbConnectionstring = @"D:\BioDatabaseService\database\BioSkyNet.mdf"
               })
                 .LifestyleSingleton()
               );

      container.AddFacility<TypedFactoryFacility>()
               .Register(Component.For<IContextFactory>().AsFactory());

      container.Register(Component.For<BioSkyNetDataModel>().LifestyleTransient());

      container.Register(Component.For<PersonDataHolder>());
      container.Register(Component.For<PhotoDataHolder>());

      container.Register(Component.For<VisitorDataHolder>());
      container.Register(Component.For<AccessDeviceDataHolder>());
      container.Register(Component.For<CaptureDeviceDataHolder>());

      container.Register(Component.For<LocationDataHolder>());
      container.Register(Component.For<CardDataHolder>());

      container.Register(Component.For<FullPersonHolder> ());
      container.Register(Component.For<FullVisitorHolder>());

      container.Register(Component.For<BioSkyNetRepository>());

      

    }
  }
}
