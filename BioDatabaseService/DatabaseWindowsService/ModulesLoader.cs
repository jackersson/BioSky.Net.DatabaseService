using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Reflection;

namespace BioDatabaseService
{
  public class ModulesLoader
  {
    private readonly IWindsorContainer _mainContainer;

    public ModulesLoader(IWindsorContainer mainContainer)
    {
      _mainContainer = mainContainer;
    }

    public bool LoadData(Assembly assembly)
    {
      try
      {
        var moduleInstaller = FromAssembly.Instance(assembly);
        _mainContainer.Install(moduleInstaller);
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        //TODO: good exception handling 
        return false;
      }
    }
  }
}
