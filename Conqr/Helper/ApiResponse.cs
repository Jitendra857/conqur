using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conqr.Helper
{
    public class ApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public int pagecount { get; set; }

        public ApiResponse(bool status, string message,int pagecount, object result)
        {
            this.Status = status;
            this.Message = message;
            this.pagecount = pagecount;
            this.Result = result;

        }
    }
}
