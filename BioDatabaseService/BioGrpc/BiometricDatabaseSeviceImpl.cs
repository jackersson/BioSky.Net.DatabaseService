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
    private readonly BioClientsEngine    _client  ;
    public BiometricDatabaseSeviceImpl(BioSkyNetRepository database, BioClientsEngine client )
    {
      _database = database;

      _client   = client;
    }

    public Task<Response> AddSocket(SocketConfiguration request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }
    #region person
    public Task<PersonList> PersonSelect(QueryPersons request, ServerCallContext context)
    {
      return Task.FromResult(_database.PersonDataClient.Select());
    }

    public Task<Person> AddPerson(Person request, ServerCallContext context)
    {
      return Task.FromResult(_database.PersonDataClient.Add(request));
    }

    public Task<Person> UpdatePerson(Person request, ServerCallContext context)
    {
      return Task.FromResult(_database.PersonDataClient.Update(request));
    }

    public Task<Person> RemovePerson(Person request, ServerCallContext context)
    {
      return Task.FromResult(_database.PersonDataClient.Remove(request));
    }
    #endregion

    #region cards
    public Task<Card> AddCard(Card request, ServerCallContext context)
    {
      return Task.FromResult(_database.CardsDataClient.Add(request));
    }

    public Task<CardList> RemoveCards(CardList request, ServerCallContext context)
    {
      return Task.FromResult(_database.CardsDataClient.Remove(request));
    }

    public Task<Card> RemoveCard(Card request, ServerCallContext context)
    {
      return Task.FromResult(_database.CardsDataClient.Remove(request));
    }
    #endregion

    #region photos
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

    public Task<Photo> SetThumbnail(long personId, long photoId, ServerCallContext context)
    {
      return Task.FromResult(_database.PersonDataClient.SetThumbnail(personId, photoId));
    }
    #endregion

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
      throw new NotImplementedException();
      //return Task.FromResult(_database.VisitorsDataClient.Add(request));
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
    

    public Task<Location> RemoveLocation(Location request, ServerCallContext context)
    {
      return Task.FromResult(_database.LocationsDataClient.Remove(request));
    }

    public Task<FacialImage> AddFace(FacialImage request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<FacialImage> RemoveFace(FacialImage request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<FacialImage> UpdateFace(FacialImage request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<FingerprintImage> AddFingerprint(FingerprintImage request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<FingerprintImage> RemoveFingerprint(FingerprintImage request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<FingerprintImage> UpdateFingerprint(FingerprintImage request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<Response> SetThumbnail(Photo request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<Visitor> AddVisitor(FullVisitorData request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

    public Task<Response> AddClient(BioClient request, ServerCallContext context)
    {
      return Task.FromResult(_client.Add(request));
    }

    public Task<Response> RemoveClient(BioClient request, ServerCallContext context)
    {
      throw new NotImplementedException();
    }

   
  }
}
