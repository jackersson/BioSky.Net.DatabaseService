using System.Data.Entity;

namespace BioContracts
{
  public interface IContextFactory
  {
    T Create<T>() where T : DbContext;

    void Release<T>(T context) where T : DbContext;
  }
}
