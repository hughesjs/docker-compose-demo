using DemoWebApp.Common;
using DemoWebApp.Notes.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.Notes.WebApplication;

public static class NotesWebApplicationExtensions
{
    public static void MapNotes(this Microsoft.AspNetCore.Builder.WebApplication app)
    {
        app.MapGet("/notes/{id}", async ([FromServices] IRepository<Note> notesRepo, [FromRoute] string id) =>
            {
                var option = await notesRepo.Get(id);

                return option.HasSome ? Results.Ok(option.Value) : Results.NotFound();
            });

        app.MapPost("/notes", async ([FromServices] IRepository<Note> notesRepo, [FromBody] Note note) =>
        {
            var option = await notesRepo.TrySet(note);

            return option.HasSome ? Results.Created($"/notes/{option.Value!.Id}", option.Value) : Results.Problem();
        });
    }
}