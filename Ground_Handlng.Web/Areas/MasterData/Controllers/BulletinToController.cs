using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Master.BulletinMaster;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ground_Handlng.Web.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [DisplayName("Operational")]
    public class BulletinToController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BulletinTo _bulletinTo;
        public BulletinToController(ApplicationDbContext context)
        {
            _context = context;
            _bulletinTo = new BulletinTo(_context);
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_bulletinTo.GetList().Result.Cast<BulletinTo>());
        }

        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(BulletinTo model, IFormCollection formCollection)
        {

            if (ModelState.IsValid)
            {
                _bulletinTo.Name = model.Name;
                _bulletinTo.CreatedBy = User.Identity.Name;
                var result = await _bulletinTo.Save() as DatabaseOperationResponse;
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
                _bulletinTo.Id = id;
                var bulletin = _bulletinTo.Refresh().Result as BulletinTo;
                return PartialView(bulletin);
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, BulletinTo model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _bulletinTo.Id = model.Id;
                _bulletinTo.Name = model.Name;
                _bulletinTo.CreatedBy = User.Identity.Name;
                _bulletinTo.CreationDate = model.CreationDate;
                _bulletinTo.RevisedBy = User.Identity.Name;
                var result = await _bulletinTo.Update() as DatabaseOperationResponse;
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
                var bulletin = await _context.BulletinTo.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _bulletinTo.Id = bulletin.Id;
                _bulletinTo.Name = bulletin.Name;
                _bulletinTo.CreatedBy = bulletin.CreatedBy;
                _bulletinTo.CreationDate = bulletin.CreationDate;
                var result = await _bulletinTo.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }
    }
}