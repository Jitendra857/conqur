using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conqr.Requestmodel
{
    public class ConqurModel
    {

       
    }
    public class ScrollRequestmodel
    {
        public int collectionid { get; set; }
        public bool iscollection { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
    }

    public class RequestModel
    {
        public int page { get; set; }
        public int pagesize { get; set; }
    }
}
