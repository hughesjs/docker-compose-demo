using DemoWebApp.Common;
using DemoWebApp.Notes.Models;
using MongoDB.Driver;

namespace DemoWebApp.Notes.Repositories;

internal class NoteRepository : IRepository<Note>
{
   private readonly IMongoCollection<DecoratedEntity<Note>> _collection; 
   private readonly ILogger<NoteRepository> _logger;
   public NoteRepository(IMongoDatabase database, ILogger<NoteRepository> logger)
   {
       _logger = logger;
       _collection = database.GetCollection<DecoratedEntity<Note>>("Notes");
   }

   public async Task<Option<DecoratedEntity<Note>>> Get(string id)
    {
        Guid guid = Guid.Parse(id);
        try 
        {
            _logger.LogDebug("Fetching note with id {id}", id);
            DecoratedEntity<Note>? res = (await _collection.FindAsync(n => n.Id == guid)).FirstOrDefault();
            return new(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get note with id {id}", id);
            return new();
        }
    }

    public async Task<Option<DecoratedEntity<Note>>> TrySet(Note note)
    {
        _logger.LogInformation("Decorating note with title: {Title} ", note.Title);

        DecoratedEntity<Note> decoratedNote = DecoratedEntity<Note>.Create(note);

        try
        {
            await _collection.InsertOneAsync(decoratedNote); 
            _logger.LogInformation("Successfully inserted note with id {id}", decoratedNote.Id);
        }
        catch (MongoWriteException e)
        {
            _logger.LogError(e, "Failed to insert note with id {id}", decoratedNote.Id);
            return new();
        }
        
        return new(decoratedNote);
    }
}