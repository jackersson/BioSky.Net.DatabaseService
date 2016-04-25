using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioContracts.Common
{
  public class ServiceConfiguration : IServiceConfiguration
  {
    public ServiceConfiguration(string ipAddress)
    {
      IpAddress = ipAddress;
    }
    public ServiceConfiguration() { }

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
}
