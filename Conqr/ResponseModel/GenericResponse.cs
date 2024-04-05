using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conqr.ResponseModel
{
    public class GenericResponse
    {
        public GenericResponse()
        {
            this.IsSuccess = false;
            this.Message = string.Empty;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

    }

    public class CollectionModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string ReferenceUrl { get; set; }
        public int? CategoryIconType { get; set; }
       public int ScrollsCount { get; set; }
    }

    public class ScrollsModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReferenceUrl { get; set; }
        public int? CategoryIconType { get; set; }
        public int ScrollsCount { get; set; }
        public bool? IsQuote { get; set; }
    }

    public class MasterScrollModel
    {
        public int? nexPageCount { get; set; }
        public List<ScrollsModel> lstScrollsModel { get; set; } = new List<ScrollsModel>();
    }
}
