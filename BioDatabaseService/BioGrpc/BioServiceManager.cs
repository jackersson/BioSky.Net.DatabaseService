using System;
using BioContracts;
using BioService;
using Grpc.Core;

namespace BioGrpc
{

  public class ServiceClient
  {
    public ServiceClient( BioClient client )
    {
      Client = client;
    }

    public void Start()
    {
      Console.WriteLine("OnStart");
    }

    public void Stop()
    {
      Console.WriteLine("Stop");
    }



    private BioClient Client { get; set; }
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
      BioClientsEngine            _clients  = _locator.GetProcessor<BioClientsEngine>();
      _server = new Server
      {
        Services = { BiometricDatabaseSevice.BindService(new BiometricDatabaseSeviceImpl(_database, _clients)) },
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
