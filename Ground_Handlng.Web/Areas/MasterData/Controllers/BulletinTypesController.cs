using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Master.BulletinMaster;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ground_Handlng.Web.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [DisplayName("Operational")]
    public class BulletinTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BulletinTypes _bulletinTypes;
        public BulletinTypesController(ApplicationDbContext context)
        {
            _context = context;
            _bulletinTypes = new BulletinTypes(_context);
        }

        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_bulletinTypes.GetList().Result.Cast<BulletinTypes>());
        }

        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(BulletinTypes model)
        {

            if (ModelState.IsValid)
            {
                _bulletinTypes.TypeName = model.TypeName;
                _bulletinTypes.CreatedBy = User.Identity.Name;
                var result = await _bulletinTypes.Save() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
        public PartialViewResult Edit(long id)
        {
            if (id != 0)
            {
                _bulletinTypes.Id = id;
                var bulletin = _bulletinTypes.Refresh().Result as BulletinTypes;
                return PartialView(bulletin);
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, BulletinTypes model)
        {
            if (ModelState.IsValid)
            {
                _bulletinTypes.Id = model.Id;
                _bulletinTypes.TypeName = model.TypeName;
                _bulletinTypes.CreatedBy = User.Identity.Name;
                _bulletinTypes.CreationDate = model.CreationDate;
                _bulletinTypes.RevisedBy = User.Identity.Name;
                var result = await _bulletinTypes.Update() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
        public async Task<JsonResult> Delete(long id)
        {
            if (id != 0)
            {
                var bulletinType = await _context.BulletinTypes.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _bulletinTypes.Id = bulletinType.Id;
                _bulletinTypes.TypeName = bulletinType.TypeName;
                _bulletinTypes.CreatedBy = bulletinType.CreatedBy;
                _bulletinTypes.CreationDate = bulletinType.CreationDate;
                var result = await _bulletinTypes.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }
    }
}