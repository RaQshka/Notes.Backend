using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteList;
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
    public class GetNoteListQueryHandlerTests
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;
        public GetNoteListQueryHandlerTests(QueryTestFixture queryTestFixture)
        {
            Context = queryTestFixture.Context;
            Mapper = queryTestFixture.Mapper;
        }
        [Fact]
        public async Task GetNoteListQueryHandlerTests_Success()
        {
            var handler = new GetNoteListQueryHandler(Context, Mapper);

            var result = await handler.Handle(new GetNoteListQuery()
            {
                UserId = NotesContextFactory.UserBId
            }, CancellationToken.None);

            result.ShouldBeOfType<NoteListVm>();
            result.Notes.Count.ShouldBe(2);
        }
    }
}
