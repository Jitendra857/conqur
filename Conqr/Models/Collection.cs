using System;
using System.Collections.Generic;

namespace Conqr.Models
{
    public partial class Collection
    {
        public Collection()
        {
            CollectionScrolls = new HashSet<CollectionScrolls>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public string ReferenceUrl { get; set; }
        public int? CategoryIconType { get; set; }
        public bool? Views { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPublished { get; set; }

        public virtual ICollection<CollectionScrolls> CollectionScrolls { get; set; }
    }
}
