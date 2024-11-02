using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Commands
{
    public class DeleteNoteCommandHandlerTests: TestCommandBase
    {
        [Fact]
        public async Task DeleteNoteCommandHandlerTests_FailOnWrongId() 
        {
            //arrange
            var handler = new DeleteNoteCommandHandler(Context);
            //act
            //assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserAId
                    }, CancellationToken.None);

            });
        }
        [Fact]
        public async Task DeleteNoteCommandHandlerTests_FailOnWrongUserId()
        {
            var deleteHandler = new DeleteNoteCommandHandler(Context);
            var createHandler = new CreateNoteCommandHandler(Context);
            var noteId = await createHandler.Handle(new CreateNoteCommand()
            {
                Title = "title",
                Details = "detailsssss",
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await deleteHandler.Handle(new DeleteNoteCommand()
                {
                    Id = noteId,
                    UserId = NotesContextFactory.UserBId
                },
                CancellationToken.None);
            });

        }
        [Fact]
        public async Task DeleteNoteCommandHandlerTests_Success()
        {
            var deleteHandler = new DeleteNoteCommandHandler(Context);

            await deleteHandler.Handle(
                new DeleteNoteCommand()
                {
                    Id = NotesContextFactory.NoteIdForDelete,
                    UserId = NotesContextFactory.UserAId
                },CancellationToken.None);

           Assert.Null(Context.Notes.SingleOrDefault(x=>x.Id == NotesContextFactory.NoteIdForDelete));

        }
    }
}
