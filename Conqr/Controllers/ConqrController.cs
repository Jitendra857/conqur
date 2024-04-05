using Conqr.Helper;
using Conqr.Requestmodel;
using Conqr.ResponseModel;
using Conqr.ServiceModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conqr.Controllers
{
    [Route("api/conqr")]
    [ApiController]
    public class ConqrController : ControllerBase
    {
        private readonly ILogger<ConqrController> _logger;
        private readonly IConqrRepository _conqrRepository;

        public ConqrController(ILogger<ConqrController> logger,
            IConqrRepository conqrRepository)
        {
            this._logger = logger;
            this._conqrRepository = conqrRepository;
        }

        [Route("ImportFile")]
        [HttpPost]
        public async Task<IActionResult> ImportFile()
        {
            var retval = new GenericResponse();

            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    retval = await _conqrRepository.ImportExcelFileAsync(file).ConfigureAwait(false);
                }
                else
                {
                    retval.IsSuccess = false;
                    retval.Message = "Sorry! File is not found.";
                }
            }
            catch (Exception ex)
            {
                retval.IsSuccess = false;
                retval.Message = ex.Message;
            }

            return Ok(retval);
        }
        [Route("getcollections")]
        [HttpPost]
        public async Task<IActionResult> GetCollections(RequestModel model)
        {
            try
            {
                var result = _conqrRepository.GetCollection(model).Result;

                return Ok(new ApiResponse(true, "Success",0, result));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, new ApiResponse(false, "Internal Server Error",0, null));
            }
        }

        [Route("getscrolls")]
        [HttpPost]
        public async Task<IActionResult> GetScrolls(ScrollRequestmodel model)
        {
            var data = _conqrRepository.GetScrolls(model).Result;
            int nextPageCount = Convert.ToInt32(data.nexPageCount);
            List<ScrollsModel> lstscrolllist = data.lstScrollsModel;


            return Ok(new ApiResponse(true, "Success", nextPageCount, lstscrolllist));

        }
    }

}
