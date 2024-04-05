using Conqr.Models;
using Conqr.Requestmodel;
using Conqr.ResponseModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conqr.ServiceModel
{
    public interface IConqrRepository
    {
        Task<GenericResponse> ImportExcelFileAsync(IFormFile file);
        Task<List<CollectionModel>> GetCollection(RequestModel model);
        Task<MasterScrollModel> GetScrolls(ScrollRequestmodel model);
    }
}
