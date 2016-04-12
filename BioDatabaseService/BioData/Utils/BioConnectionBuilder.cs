using BioContracts;

namespace BioData.Utils
{
  public class BioConnectionBuilder : IConnectionBuilder
  {
    public string _dbConnectionstring;

    public BioConnectionBuilder(string dbConnectionstring)
    {
      _dbConnectionstring = dbConnectionstring;
    }

    public string Create()
    {
      string datasource = @"data source=(LocalDB)\MSSQLLocalDB;";

      string attachDbFileName = "attachdbfilename=" + _dbConnectionstring + ";";

      string integratedSecurity = "integrated security=True;";
      string multipleActiveResultSets = "MultipleActiveResultSets=True;";
      string app = "App=EntityFramework";

      string providerConnectionString =
                                        datasource
                                      + attachDbFileName
                                      + integratedSecurity
                                      + multipleActiveResultSets
                                      + app;

      string connection_string = providerConnectionString;

      return connection_string;
    }

  }
}
