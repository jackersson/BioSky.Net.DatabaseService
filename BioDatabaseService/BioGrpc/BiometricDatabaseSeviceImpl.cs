using BioService;
using System;
using System.Threading.Tasks;
using Grpc.Core;
using BioData;

namespace BioGrpc
{
  public class BiometricDatabaseSeviceImpl : BiometricDatabaseSevice.IBiometricDatabaseSevice
  {   
    private readonly BioSkyNetRepository _database;
    public BiometricDatabaseSeviceImpl(BioSkyNetRepository database)
    {
      _database = database;
    }

    public Task<Response> AddSocket(SocketConfiguration request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }    

    public Task<PersonList> PersonSelect(QueryPersons request, ServerCallContext context)
    {
      return Task.FromResult(_database.PDataClient.Select());
    }

    public Task<Person> AddPerson(Person request, ServerCallContext context)
    {
      return Task.FromResult(_database.PDataClient.Add(request));
    }

    public Task<Person> UpdatePerson(Person request, ServerCallContext context)
    {
      return Task.FromResult(_database.PDataClient.Update(request));
    }

    public Task<Person> RemovePerson(Person request, ServerCallContext context)
    {
      return Task.FromResult(_database.PDataClient.Remove(request));
    }

    public Task<Card> AddCard(Card request, ServerCallContext context)
    {
      return Task.FromResult(_database.CardsDataClient.Add(request));
    }

    public Task<RawIndexes> RemoveCards(RawIndexes request, ServerCallContext context)
    {
      return Task.FromResult(_database.CardsDataClient.Remove(request));
    }

    public Task<PhotoList> SelectPhotos(QueryPhoto request, ServerCallContext context)
    {
      return Task.FromResult(_database.PhotosDataClient.Select(request));
    }

    public Task<Photo> AddPhoto(Photo request, ServerCallContext context)
    {
      return Task.FromResult(_database.PhotosDataClient.Add(request));
    }
    
    public Task<RawIndexes> RemovePhotos(RawIndexes request, ServerCallContext context)
    {
      return Task.FromResult(_database.PhotosDataClient.Remove(request));
    }

    Task<Response> BiometricDatabaseSevice.IBiometricDatabaseSevice.SetThumbnail(Photo request, ServerCallContext context)
    {
      return Task.FromResult(_database.PDataClient.SetThumbnail(request));
    }

    public Task<VisitorList> SelectVisitors(QueryVisitors request, ServerCallContext context)
    {
      return Task.FromResult(_database.VisitorsDataClient.Select(request));
    }

    public Task<Response> AttachVisitorToPerson(Visitor request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<Visitor> AddVisitor(Visitor request, ServerCallContext context)
    {
      return Task.FromResult(_database.VisitorsDataClient.Add(request));
    }

    public Task<RawIndexes> RemoveVisitors(RawIndexes request, ServerCallContext context)
    {
      return Task.FromResult(_database.VisitorsDataClient.Remove(request));
    }

    public Task<LocationList> SelectLocations(QueryLocations request, ServerCallContext context)
    {
      return Task.FromResult(_database.LocationsDataClient.Select(request));
    }

    public Task<Location> AddLocation(Location request, ServerCallContext context)
    {
      return Task.FromResult(_database.LocationsDataClient.Add(request));
    }

    public Task<Location> UpdateLocation(Location request, ServerCallContext context)
    {
      return Task.FromResult(_database.LocationsDataClient.Update(request));
    }

    public Task<RawIndexes> RemoveLocations(RawIndexes request, ServerCallContext context)
    {
      return Task.FromResult(_database.LocationsDataClient.Remove(request));
    }

    
  }
}
