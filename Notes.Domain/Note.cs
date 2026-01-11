using System;

namespace Notes.Domain
{
    public class Note:BaseEntity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }
}
