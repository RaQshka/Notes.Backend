using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands
{
    public class UpdateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandlerTests_Success()
        {
            //
            var handler = new UpdateNoteCommandHandler(Context);
            var noteTitle = "YAYY";
            //
            await handler.Handle(new UpdateNoteCommand()
            {
                Id = NotesContextFactory.NoteIdForUpdate,
                UserId = NotesContextFactory.UserBId,
                Title = noteTitle
            }, CancellationToken.None);
            //
            Assert.NotNull(await Context.Notes.SingleOrDefaultAsync(x =>
                x.Id == NotesContextFactory.NoteIdForUpdate &&
                x.Title == noteTitle));

        }
        [Fact]
        public async Task UpdateNoteCommandHandlerTests_FailOnWrongId()
        {
            //
            var handler = new UpdateNoteCommandHandler(Context);
            var noteTitle = "YAYY";
            //
            //
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateNoteCommand()
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserBId,
                        Title = noteTitle
                    }, CancellationToken.None);
            }
            );
        }
        [Fact]
        public async Task UpdateNoteCommandHandlerTests_FailOnWrongUserId()
        {
            //
            var handler = new UpdateNoteCommandHandler(Context);
            var noteTitle = "YAYY";
            //
            //
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateNoteCommand()
                    {
                        Id = NotesContextFactory.NoteIdForUpdate,
                        UserId = NotesContextFactory.UserAId,
                        Title = noteTitle
                    }, CancellationToken.None);
            }
            );
        }
    }
}
