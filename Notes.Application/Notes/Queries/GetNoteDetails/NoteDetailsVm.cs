using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class NoteDetailsVm : IMapWith<Note>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Note, NoteDetailsVm>()
                .ForMember(x => x.Id, opt => opt.MapFrom(note => note.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(note => note.Title))
                .ForMember(x => x.Details, opt => opt.MapFrom(note => note.Details))
                .ForMember(x => x.CreationDate, opt => opt.MapFrom(note => note.CreationDate))
                .ForMember(x => x.EditDate, opt => opt.MapFrom(note => note.EditDate));

        }
    }
}
