namespace BioContracts
{
  public interface IServiceConfiguration
  {
    string IpAddress { get; }

    int Port { get; }
  }
}
