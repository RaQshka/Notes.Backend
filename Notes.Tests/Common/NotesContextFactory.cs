using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common
{
    public class NotesContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();
        public static Guid NoteIdForDelete = Guid.NewGuid();
        public static Guid NoteIdForUpdate = Guid.NewGuid();

        public static NotesDbContext Create()
        {
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new NotesDbContext(options);
            context.Database.EnsureCreated();
            context.Notes.AddRange(
                new Note()
                {
                    UserId = UserAId,
                    CreatedAt = DateTime.Today,
                    Title = "Title1",
                    Details = "Details of 1",
                }, new Note()
                {
                    UserId = UserBId,
                    Title = "Title2",
                    Details = "Details of 2",
                }, new Note()
                {
                    UserId = UserAId,
                    Title = "Title3",
                    Details = "Details of 3",
                }, new Note()
                {
                    UserId = UserBId,
                    Title = "Title4",
                    Details = "Details of 4",
                }
            ) ;
            context.SaveChanges();
            return context;
        }

        public static void Destroy(NotesDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
