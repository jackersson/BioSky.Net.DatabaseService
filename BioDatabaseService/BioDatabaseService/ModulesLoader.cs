﻿using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
      catch (Exception)
      {
        //TODO: good exception handling 
        return false;
      }
    }
  }
}
