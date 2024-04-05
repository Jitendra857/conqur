using System;
using System.Collections.Generic;

namespace Conqr.Models
{
    public partial class Scrolls
    {
        public Scrolls()
        {
            CollectionScrolls = new HashSet<CollectionScrolls>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool? IsPubished { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Views { get; set; }
        public bool? IsQuote { get; set; }

        public virtual ICollection<CollectionScrolls> CollectionScrolls { get; set; }
    }
}
