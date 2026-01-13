using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class NoteLookupDto : IMapWith<Note>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Note, NoteLookupDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(note => note.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(note => note.Title))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(note => note.CreatedAt));

        }
    }
}
