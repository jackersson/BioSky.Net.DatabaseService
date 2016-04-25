using System.ServiceProcess;

namespace DatabaseWindowsService
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
#if DEBUG
      DbService databaseService = new DbService();
      databaseService.OnDebug();

#else
      ServiceBase[] ServicesToRun;
      ServicesToRun = new ServiceBase[]
      {
                new Service1()
      };
      ServiceBase.Run(ServicesToRun);
#endif
    }
  }
}
