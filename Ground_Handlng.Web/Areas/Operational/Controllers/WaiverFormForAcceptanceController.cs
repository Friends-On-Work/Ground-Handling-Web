using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational.Forms.WaiverFormForAcceptance;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    public class WaiverFormForAcceptanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly WaiverFormForAcceptance waiverAcceptance;
        public WaiverFormForAcceptanceController(ApplicationDbContext context)
        {
            _context = context;
            waiverAcceptance = new WaiverFormForAcceptance(_context);
        }
        [DisplayName("Operational")]
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            if (!_context.WaiverFormForAcceptance.Any())
            {
                _context.WaiverFormForAcceptance.Add(new WaiverFormForAcceptance
                {
                    SignatureOfPassenger = "Tenagne Mola",
                    NumberPETAVI = "2323/PCV",
                    TypePETAVI = "AVI",
                    BagTagNumber = "BHE4493492",
                    Status = DataObjects.Models.Others.RecordStatus.Active,
                    DateOfTravel = DateTime.Now.AddDays(3),
                    FullName = "Tenagne Mola"
                });
                _context.SaveChanges();
            }
            return View(waiverAcceptance.GetList().Result.Cast<WaiverFormForAcceptance>());
        }

        public async Task<PartialViewResult> Detail(long id)
        {
            if (id != 0)
            {
                waiverAcceptance.Id = id;
                var waiver = waiverAcceptance.Refresh().Result as WaiverFormForAcceptance;
                return PartialView(waiver);
            }
            return PartialView();
        }
        public PartialViewResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(WaiverFormForAcceptance model, IFormCollection formCollection)
        {
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
                waiverAcceptance.Id = id;
                var waiver = waiverAcceptance.Refresh().Result as WaiverFormForAcceptance;
                return PartialView(waiver);
            }

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, WaiverFormForAcceptance model, IFormCollection formCollection)
        {
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
    }
}
