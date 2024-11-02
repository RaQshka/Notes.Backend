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
                    CreationDate = DateTime.Today,
                    Title = "Title1",
                    Details = "Details of 1",
                    EditDate = null,
                    Id = Guid.Parse("179550E4-4566-4F91-9133-38734C9260BC")
                }, new Note()
                {
                    UserId = UserBId,
                    CreationDate = DateTime.Today,
                    Title = "Title2",
                    Details = "Details of 2",
                    EditDate = null,
                    Id = Guid.Parse("F03F6A7A-0FFE-4A1E-8F1D-F2E8D43A45C6")
                }, new Note()
                {
                    UserId = UserAId,
                    CreationDate = DateTime.Today,
                    Title = "Title3",
                    Details = "Details of 3",
                    EditDate = null,
                    Id = NoteIdForDelete
                }, new Note()
                {
                    UserId = UserBId,
                    CreationDate = DateTime.Today,
                    Title = "Title4",
                    Details = "Details of 4",
                    EditDate = null,
                    Id = NoteIdForUpdate
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
