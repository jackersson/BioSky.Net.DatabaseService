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
using BioData.DataHolders.DataClient;

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

      //IProcessorLocator locator = container.Resolve<IProcessorLocator>();
      //PersonDataClient pf = new PersonDataClient(locator);

      //BioService.PersonList personList = pf.Select();
      //BioService.Person pp = personList.Persons.FirstOrDefault();
      //Console.WriteLine(personList);
      BioServiceManager serviceManager = container.Resolve<BioServiceManager>();
      serviceManager.Start(new ServiceConfiguration() { IpAddress = "0.0.0.0", Port = 50051 });


      Console.ReadKey();
      serviceManager.Stop();
      
      //IList<Person> person = pf.Select();
      // Card card = person.FirstOrDefault().Card.FirstOrDefault();
      //Console.WriteLine();


      //var nDataContext = container.Resolve<IContextFactory>().Create<BioSkyNetDataModel>();

      //Person ph = nDataContext.Person.Where(x=> x.Id == 74).FirstOrDefault();
      //Console.ReadKey();
      /*
           Person newPerson = new Person()
           {
               First_Name_ = "Taras",
               Last_Name_ = "Lishchenko"
               ,
               Gender = 0,
               Rights = 1
           };

           nDataContext.Person.Add(newPerson);



           Photo photo = new Photo() 
           { 
              Photo_Url = "/www/sss/image.jpg"
            , Datetime = DateTime.Now
            , Size_Type = 0, Origin_Type = 1
           };



           Card card = new Card() {Unique_Number = "sdfsdfasdfsdf"};

           //newPerson.Photo = photo;
           newPerson.PhotoCollection.Add(photo);
           newPerson.Card.Add(card);

           BiometricLocation bioLocation = new BiometricLocation()
           {  XPos = 0.1
            , YPos = 0.2
            , Confidence = 1};



           EyesCharacteristic eyes = new EyesCharacteristic();
           eyes.LeftEyeBiometricLocation = bioLocation;
           eyes.RightEyeBiometricLocation = bioLocation;

           FaceCharacteristic faces = new FaceCharacteristic() {Width = 100 };
           faces.FaceBiometricLocation = bioLocation;
           faces.EyesCharacteristic = eyes;

           PortraitCharacteristic pc = new PortraitCharacteristic() { Age = 18
                                                                    , Face_Count = 1
                                                                    , Fir_Url = "/sdf/sdf/sdf/1.fir"};
           pc.FaceCharacteristic.Add(faces);

           photo.PortraitCharacteristic = pc;

           nDataContext.SaveChanges();
           */
      /*
    Location location = new Location() {Location_Name = "Main", Description = "Test main"};
    location.AccessDevice.Add(new AccessDevice(){PortName = "Com1"});
    location.CaptureDevice.Add(new CaptureDevice(){Device_Name = "WebCam"});

    nDataContext.Location.Add(location);

    PersonAccess acc = new PersonAccess();
    acc.Person   = newPerson;
    acc.Location = location;
      */
      /*
            using (var nDataContext = container.Resolve<IContextFactory>().Create<BioSkyNetDataModel>())
            {
              nDataContext.Person.Add(newPerson);
              try
              {
                  nDataContext.SaveChanges();           
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.Message);
              }
            }
      */


      //newPerson.BiometricPhoto = new BiometricPhoto() { Photo = 1, }

      /*
    DbSet<Person> pp = repo.Select();
      Person pps = pp.FirstOrDefault();
      Card card = pps.Card.Where(x => x.Person_Id == pps.Id).FirstOrDefault();
      */
      //Person item = new Person() { Id = 7, Date_Of_Birth = DateTime.Now };

      //repo.Update(item);
      //repo.Remove(item);

      //IProcessorLocator locator = container.Resolve<IProcessorLocator>();


    }
  }
}
