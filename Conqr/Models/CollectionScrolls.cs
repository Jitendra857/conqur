using System;
using System.Collections.Generic;

namespace Conqr.Models
{
    public partial class CollectionScrolls
    {
        public long Id { get; set; }
        public long? ScrollId { get; set; }
        public int? ChapterNo { get; set; }
        public long? CollectioId { get; set; }

        public virtual Collection Collectio { get; set; }
        public virtual Scrolls Scroll { get; set; }
    }
}
