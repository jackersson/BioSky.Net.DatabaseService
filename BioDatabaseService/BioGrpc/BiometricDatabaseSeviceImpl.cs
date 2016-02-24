using BioService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using BioData;
//using BioGrpc.DataConverters;

namespace BioGrpc
{
  public class BiometricDatabaseSeviceImpl : BiometricDatabaseSevice.IBiometricDatabaseSevice
  {
    //private readonly GrpcResponseTranslator _translator;
    private readonly BioSkyNetRepository _database;
    public BiometricDatabaseSeviceImpl(BioSkyNetRepository database)
    {
      _database = database;
    }

    public Task<Response> AddSocket(SocketConfiguration request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<LocationList> LocationSelect(CommandLocations request, ServerCallContext context)
    {
      return Task.FromResult(_database.FullLocations().Select(request));
    }

    public Task<LocationList> LocationUpdate(LocationList request, ServerCallContext context)
    {
      return Task.FromResult(_database.FullLocations().Update(request));
    }

    public Task<PersonList> PersonSelect(CommandPersons request, ServerCallContext context)
    {
      return Task.FromResult(_database.FullPersons().Select(request));
    }

    public Task<PersonList> PersonUpdate(PersonList request, ServerCallContext context)
    {
      return Task.FromResult(_database.FullPersons().Update(request));
    }

    public Task<PhotoList> PhotoSelect(CommandPhoto request, ServerCallContext context)
    {
      return Task.FromResult(_database.Photos().Select(request));
    }

    public Task<VisitorList> VisitorSelect(CommandVisitors request, ServerCallContext context)
    {
      return Task.FromResult(_database.FullVisitors().Select(request));
    }

    public Task<VisitorList> VisitorUpdate(VisitorList request, ServerCallContext context)
    {
      return Task.FromResult(_database.FullVisitors().Update(request));
    }
  }
}
