using Conqr.Models;
using Conqr.Requestmodel;
using Conqr.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conqr.ServiceModel
{
    public class ConqrRepository : IConqrRepository
    {
        private readonly ConqrContext _dbcontext;

        public ConqrRepository(ConqrContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public async Task<GenericResponse> ImportExcelFileAsync(IFormFile file)
        {
            var retval = new GenericResponse();

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage();
                    package.Load(stream);

                    if (package.Workbook.Worksheets.Count > 0)
                    {
                        using (ExcelWorksheet workSheet = package.Workbook.Worksheets.First())
                        {
                            for (int row = 2; row <= workSheet.Dimension.End.Row; row++)
                            {
                                var scrollobj = new Scrolls
                                {
                                    Title = workSheet.Cells[row, 2].Value != null ? workSheet.Cells[row, 2].Value.ToString() : "",
                                    Content = workSheet.Cells[row, 3].Value.ToString(),
                                    IsPubished = Convert.ToBoolean(workSheet.Cells[row, 4].Value),
                                    CreatedOn = DateTime.Now,
                                    IsDeleted = false,
                                    Views = true,
                                    IsQuote = Convert.ToBoolean(workSheet.Cells[row, 5].Value),
                                };

                                var collectionobj = new Collection
                                {
                                    Description = workSheet.Cells[row, 6].Value.ToString(),
                                    ReferenceUrl = workSheet.Cells[row, 7].Value != null ? workSheet.Cells[row, 7].Value.ToString() : "",
                                    CategoryIconType = 1,
                                    Views = true,
                                    CreatedOn = DateTime.Now,
                                    IsDeleted = false,
                                    IsPublished = Convert.ToBoolean(workSheet.Cells[row, 9].Value),
                                };

                                long collectionid = 0;
                                var checkDescription = await _dbcontext.Collection.Where(e => e.Description.Trim().ToLower() == collectionobj.Description.Trim().ToLower()).FirstOrDefaultAsync();

                                // var checkDescription = await _dbcontext.Collection.Where(e => collectionobj.Description.Contains(e.Description)).FirstOrDefaultAsync();
                                if (checkDescription == null)
                                {
                                    await _dbcontext.Collection.AddAsync(collectionobj);
                                    await _dbcontext.SaveChangesAsync();
                                    collectionid = collectionobj.Id;
                                }
                                else
                                {
                                    collectionid = checkDescription.Id;
                                }

                                var scrollDetail = await _dbcontext.Scrolls.AddAsync(scrollobj);
                                //var collectionDetail = await _dbcontext.Collection.AddAsync(collectionobj);
                                await _dbcontext.SaveChangesAsync();

                                if (scrollDetail != null)
                                {
                                    var collectionScrollObj = new CollectionScrolls
                                    {
                                        ScrollId = scrollobj.Id,
                                        ChapterNo = 1,
                                        CollectioId = collectionid,
                                    };

                                    var collectionScrollDetail = await _dbcontext.CollectionScrolls.AddAsync(collectionScrollObj);
                                    await _dbcontext.SaveChangesAsync();

                                    if (collectionScrollDetail != null)
                                    {
                                        retval.IsSuccess = true;
                                        retval.Message = "Record imported is successfully.";
                                    }
                                    else
                                    {
                                        retval.IsSuccess = false;
                                        retval.Message = "Record is not imported.";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retval.IsSuccess = false;
                retval.Message = ex.Message;
            }

            return retval;
        }

        public async Task<List<CollectionModel>> GetCollection(RequestModel model)
        {
            var skip = (model.page - 1) * model.pagesize;

            var data = _dbcontext.Collection
                .Where(r => r.IsDeleted != true && r.IsPublished == true)
                .OrderBy(e => e.Id) // Order by the appropriate property
                .Skip(skip)
                .Take(model.pagesize)
                .Include(e => e.CollectionScrolls).ThenInclude(e => e.Scroll)
                .Select(e => new CollectionModel
                {
                    Id = e.Id,
                    Description = e.Description,
                    CategoryIconType = e.CategoryIconType,
                    ScrollsCount = e.CollectionScrolls.Count(s => s.Scroll.IsPubished == true && s.Scroll.IsDeleted != true),
                    ReferenceUrl = e.ReferenceUrl
                }).OrderByDescending(e => e.Id)
                .ToList();

            return data;
        }

        public async Task<MasterScrollModel> GetScrolls(ScrollRequestmodel model)
        {
            MasterScrollModel objmodel = new MasterScrollModel();
            var skip = (model.page - 1) * model.pagesize;

            var scrolllist = new List<Scrolls>();
            if (model.iscollection == true)
            {
                var getscrollids = _dbcontext.CollectionScrolls
                    .Where(e => e.CollectioId == model.collectionid)
                    .Select(e => e.ScrollId)
                    .ToList();

                scrolllist = _dbcontext.Scrolls
                    .Include(r => r.CollectionScrolls)
                    .ThenInclude(r => r.Collectio)
                    .Where(r => getscrollids.Contains(r.Id) && r.IsDeleted != true && r.IsPubished == true)
                    .ToList();
            }
            else
            {
                scrolllist = _dbcontext.Scrolls
                    .Include(r => r.CollectionScrolls)
                    .ThenInclude(r => r.Collectio)
                    .Where(r => r.IsDeleted != true && r.IsPubished == true).OrderBy(x => Guid.NewGuid())
                    .ToList();
            }

            model.page = model.page + 1;
            int nextpage = 0;
            var pagecount = scrolllist.Skip(skip).Take(model.pagesize).Count();
            if (pagecount > 0)
            {
                nextpage = model.page;
            }
            // Select a subset of the shuffled list
            objmodel.lstScrollsModel = scrolllist
                .Select(e => new ScrollsModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Content,
                    ReferenceUrl = e.CollectionScrolls.FirstOrDefault() != null ? e.CollectionScrolls.FirstOrDefault().Collectio.ReferenceUrl : "",
                    IsQuote = e.CollectionScrolls.FirstOrDefault() != null ? e.CollectionScrolls.FirstOrDefault().Scroll.IsQuote : false,
                })
                .Skip(skip)
                .Take(model.pagesize)
                .ToList();

            objmodel.nexPageCount = nextpage;

            return objmodel;
        }
    }
}
