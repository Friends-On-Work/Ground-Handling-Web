using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational.Forms.Illustration;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    public class IllustrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Illustration _illustration;
        public IllustrationController(ApplicationDbContext context)
        {
            _context = context;
            _illustration = new Illustration(_context);
        }
        [DisplayName("Operational")]
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            if (!_context.Illustration.Any())
            {
                _context.Illustration.Add(new Illustration
                {
                    FullName = "Tenagne Mola",
                    KindOfDocument = "certificate of vaccination",
                    RequiredForm = "Addis Ababa",
                    Date = DateTime.Now,
                    FlightNumber = "ET123",
                    Location ="Bole, Addis Ababa, Ethiopia",
                    PermanentAddress = "Dire Dawa, Ethiopia",
                    Witness = "Wende",
                    TelephoneNo = 0936118197,
                    Status = DataObjects.Models.Others.RecordStatus.Active

                });
                _context.SaveChanges();
            }
            return View(_illustration.GetList().Result.Cast<Illustration>());
        }

        public async Task<PartialViewResult> Detail(long id)
        {
            if (id != 0)
            {
                _illustration.Id = id;
                var illustration = _illustration.Refresh().Result as Illustration;
                return PartialView(illustration);
            }
            return PartialView();
        }
        public PartialViewResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(Illustration model, IFormCollection formCollection)
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
                _illustration.Id = id;
                var illustration = _illustration.Refresh().Result as Illustration;
                return PartialView(illustration);
            }

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, Illustration model, IFormCollection formCollection)
        {
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
    }
}
