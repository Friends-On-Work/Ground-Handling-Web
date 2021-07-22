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
    public class BulletinFromController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BulletinFrom _bulletinFrom;
        public BulletinFromController(ApplicationDbContext context)
        {
            _context = context;
            _bulletinFrom = new BulletinFrom(_context);
        }

        public async Task<ActionResult> Index()
        {
            return View(_bulletinFrom.GetList().Result.Cast<BulletinFrom>());
        }

        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(BulletinTo model)
        {

            if (ModelState.IsValid)
            {
                _bulletinFrom.Name = model.Name;
                _bulletinFrom.CreatedBy = User.Identity.Name;
                var result = await _bulletinFrom.Save() as DatabaseOperationResponse;
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
                _bulletinFrom.Id = id;
                var bulletin = _bulletinFrom.Refresh().Result as BulletinFrom;
                return PartialView(bulletin);
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, BulletinFrom model)
        {
            if (ModelState.IsValid)
            {
                _bulletinFrom.Id = model.Id;
                _bulletinFrom.Name = model.Name;
                _bulletinFrom.CreatedBy = User.Identity.Name;
                _bulletinFrom.CreationDate = model.CreationDate;
                _bulletinFrom.RevisedBy = User.Identity.Name;
                var result = await _bulletinFrom.Update() as DatabaseOperationResponse;
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
                var bulletin = await _context.BulletinFrom.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _bulletinFrom.Id = bulletin.Id;
                _bulletinFrom.Name = bulletin.Name;
                _bulletinFrom.CreatedBy = bulletin.CreatedBy;
                _bulletinFrom.CreationDate = bulletin.CreationDate;
                var result = await _bulletinFrom.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }
    }
}