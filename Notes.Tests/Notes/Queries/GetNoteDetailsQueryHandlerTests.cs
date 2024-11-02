using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Persistence;
using Notes.Tests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Notes.Queries
{
    [Collection("QueryCollection")]
    public class GetNoteDetailsQueryHandlerTests
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;
        public GetNoteDetailsQueryHandlerTests(QueryTestFixture queryTestFixture)
        {
            Context = queryTestFixture.Context;
            Mapper = queryTestFixture.Mapper;
        }
        [Fact]
        public async Task GetNoteDetailsQueryHandlerTests_Succeess()
        {
            var handler = new GetNoteDetailsQueryHandler(Context, Mapper);

            var result = await handler.Handle(new GetNoteDetailsQuery
            {
                Id = Guid.Parse("F03F6A7A-0FFE-4A1E-8F1D-F2E8D43A45C6"),
                UserId = NotesContextFactory.UserBId
            }, CancellationToken.None);

            result.ShouldBeOfType<NoteDetailsVm>();
            result.Title.ShouldBe("Title2");
            result.CreationDate.ShouldBe(DateTime.Today);
        }
    }
}
