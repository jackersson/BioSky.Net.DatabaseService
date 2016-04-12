using System;

namespace BioGrpc
{
  using BioContracts;
  using BioService;
  using Grpc.Core;


  public class ServiceConfiguration : IServiceConfiguration
    {
      public ServiceConfiguration(string ipAddress)
      {
        IpAddress = ipAddress;
      }
      public ServiceConfiguration() {}
      
      private string _ipAddress;
      public string IpAddress
      {
        get { return _ipAddress; }
        set
        {
          if (_ipAddress != value)
            _ipAddress = value;
        }
      }

    private int _port;
    public int Port
    {
      get { return _port; }
      set
      {
        if (_port != value)
          _port = value;
      }
    }
  }


    public class BioServiceManager 
    {
      public BioServiceManager(IProcessorLocator locator)
      {
        _locator = locator;
      }

      public void Start(IServiceConfiguration configuration)
      {
        BioData.BioSkyNetRepository _database = _locator.GetProcessor<BioData.BioSkyNetRepository>();
        _server = new Server
        {
          Services = { BiometricDatabaseSevice.BindService(new BiometricDatabaseSeviceImpl(_database)) },
          Ports = { new ServerPort(configuration.IpAddress, configuration.Port, ServerCredentials.Insecure) }
        };
        _server.Start();

        Console.WriteLine("BiometricDatabaseSevice server listening on port " + configuration.Port);
        Console.WriteLine("Press any key to stop the server...");
    }
      public void Stop()
      {
        if (_server != null)
        _server.ShutdownAsync().Wait();
      }


      private Server _server;

      private readonly IProcessorLocator _locator;

    }  

}
