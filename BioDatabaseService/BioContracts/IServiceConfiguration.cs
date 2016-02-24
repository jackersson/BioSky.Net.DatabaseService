using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioContracts
{
  public interface IServiceConfiguration
  {
    string IpAddress { get; }

    int Port { get; }

  }
}
