using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Notes.Tests.Common
{
    public class QueryTestFixture : IDisposable
    {
        public NotesDbContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public QueryTestFixture() 
        { 
            Context = NotesContextFactory.Create();

            var configBuilder = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(INotesDbContext).Assembly));
            });
            Mapper = configBuilder.CreateMapper();  
        }
        public void Dispose()
        {
            NotesContextFactory.Destroy(Context);
        }

    }
    [CollectionDefinition("QueryCollection")]
    public class QueryCollection :ICollectionFixture<QueryTestFixture> { }
}
