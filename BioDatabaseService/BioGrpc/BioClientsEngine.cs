using BioService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioGrpc
{
  public class BioClientsEngine
  {

    public BioClientsEngine()
    {
      _clients = new ConcurrentDictionary<string, ServiceClient>();
    }


    public Response Add( BioClient client )
    {
      Response response = new Response() { Good = Result.Failed };
      string macAddress = client.MacAddress;

      if (client == null || string.IsNullOrEmpty(client.MacAddress) || string.IsNullOrEmpty(client.IpAddress))
        return response;

      if (_clients.ContainsKey(macAddress))
        return response;

      ServiceClient newClient = new ServiceClient(client);
      newClient.Start();
      _clients.TryAdd(macAddress, newClient);

      response.Good = Result.Success;
      return response;

    }


    public Response Remove (BioClient client )
    {
      Response response = new Response() { Good = Result.Failed };
      string macAddress = client.MacAddress;

      if (client == null || string.IsNullOrEmpty(client.MacAddress) || string.IsNullOrEmpty(client.IpAddress))
        return response;

      if (_clients.ContainsKey(macAddress))
        return response;

      ServiceClient newClient = null;     
      _clients.TryRemove(macAddress, out newClient);
      if (newClient != null)
        newClient.Stop();
      response.Good = Result.Success;
      return response;
    }

    private readonly ConcurrentDictionary<string, ServiceClient> _clients;
  }
}
