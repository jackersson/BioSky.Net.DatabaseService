using BioContracts;
using BioContracts.Common;
using BioData;
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

      /*
      BioSkyNetRepository repo = container.Resolve<BioSkyNetRepository>();

      BioService.Person pp = new BioService.Person() { Firstname = "Taras", Lastname = "Lishchenko", Gender = BioService.Gender.Male, Rights = BioService.Rights.Manager };
      
      BioService.Person pp2 = repo.PersonDataClient.Add(pp);

      BioService.FacialImage image = new BioService.FacialImage();

      BioService.FaceCharacteristic fc = new BioService.FaceCharacteristic() { Width = 1, Age = 34 };

      fc.Location = new BioService.BiometricLocation() { Xpos = 2, Ypos = 2, Confidence = 2 };
      fc.Eyes = new BioService.EyesCharacteristic();

      fc.Eyes.LeftEye  = new BioService.BiometricLocation() { Xpos = 2, Ypos = 2, Confidence = 2 };
      fc.Eyes.RightEye = new BioService.BiometricLocation() { Xpos = 2, Ypos = 2, Confidence = 2 };

      image.OwnerBiometricDataId = pp2.BiometricData.Id;
      image.Image = new BioService.Photo() { Width = 100, Height = 100, PhotoUrl = "www/dfsd/sdf" };
      image.Faces.Add(fc);


      BioService.FacialImage pp3 = repo.FacialDataClient.Add(image);
      */
    //  Console.ReadKey();
      //repo.FaceCharacteristic.RemoveRange( repo.FaceCharacteristic.Where(x => true) );
      //FaceCharacteristic fc = repo.FaceCharacteristic.FirstOrDefault();
      //fc.Age = 25;
      //fc.EyesCharacteristic.LeftEyeLocation.Confidence = 100;

      /*
      Photo ph = new Photo() { Photo_Url = "www/www", Height = 480 };
      repo.Photo.Add(ph);

      IEnumerable<DbEntityEntry> entry31 = repo.ChangeTracker.Entries();
      Console.WriteLine(entry31);

      //repo.SaveChanges();

      IEnumerable<DbEntityEntry> entry1 = repo.ChangeTracker.Entries();
      Console.WriteLine(entry1);
      

      FaceCharacteristic fc = new FaceCharacteristic() {  };
      fc.BiometricLocation  = new BiometricLocation() { };
      fc.EyesCharacteristic = new EyesCharacteristic();
      fc.EyesCharacteristic.LeftEyeLocation  = new BiometricLocation() { XPos = 2, YPos = 2, Confidence = 2 };
      fc.EyesCharacteristic.RightEyeLocation = new BiometricLocation() { XPos = 3, YPos = 3, Confidence = 3 };

      ph.FaceCharacteristic.Add( fc );

      IEnumerable<DbEntityEntry> entry2 = repo.ChangeTracker.Entries();
      Console.WriteLine(entry2);

      repo.SaveChanges();

      IEnumerable<DbEntityEntry> entry = repo.ChangeTracker.Entries();
      Console.WriteLine(entry);
      */
      
     
      BioServiceManager serviceManager = container.Resolve<BioServiceManager>();
      serviceManager.Start(new ServiceConfiguration() { IpAddress = "0.0.0.0", Port = 50051 });


      Console.ReadKey();
      serviceManager.Stop();    
      
    }
  }
}
