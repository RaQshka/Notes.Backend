using MediatR;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Guid>
    {
        private readonly INotesDbContext _context;
        public CreateNoteCommandHandler(INotesDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = new Note()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                CreationDate = DateTime.Now,
                UserId = request.UserId,
                Details = request.Details,
                EditDate = null
            };
            await _context.Notes.AddAsync(note, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return note.Id;
        }
    }
}
