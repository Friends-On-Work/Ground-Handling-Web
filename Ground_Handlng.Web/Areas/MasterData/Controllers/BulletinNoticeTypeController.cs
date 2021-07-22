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
    public class BulletinNoticeTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BulletinNoticeType _bulletinNoticeType;
        public BulletinNoticeTypeController(ApplicationDbContext context)
        {
            _context = context;
            _bulletinNoticeType = new BulletinNoticeType(_context);
        }
        public async Task<ActionResult> Index()
        {
            return View(_bulletinNoticeType.GetList().Result.Cast<BulletinNoticeType>());
        }
        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(BulletinNoticeType model)
        {

            if (ModelState.IsValid)
            {
                _bulletinNoticeType.TypeName = model.TypeName;
                _bulletinNoticeType.CreatedBy = User.Identity.Name;
                var result = await _bulletinNoticeType.Save() as DatabaseOperationResponse;
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
                _bulletinNoticeType.Id = id;
                var bulletin = _bulletinNoticeType.Refresh().Result as BulletinNoticeType;
                return PartialView(bulletin);
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, BulletinNoticeType model)
        {
            if (ModelState.IsValid)
            {
                _bulletinNoticeType.Id = model.Id;
                _bulletinNoticeType.TypeName = model.TypeName;
                _bulletinNoticeType.CreatedBy = User.Identity.Name;
                _bulletinNoticeType.CreationDate = model.CreationDate;
                _bulletinNoticeType.RevisedBy = User.Identity.Name;
                var result = await _bulletinNoticeType.Update() as DatabaseOperationResponse;
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
                var bulletin = await _context.BulletinNoticeType.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _bulletinNoticeType.Id = bulletin.Id;
                _bulletinNoticeType.TypeName = bulletin.TypeName;
                _bulletinNoticeType.CreatedBy = bulletin.CreatedBy;
                _bulletinNoticeType.CreationDate = bulletin.CreationDate;
                var result = await _bulletinNoticeType.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }
    }
}