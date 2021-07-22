using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational.Forms.IndemnityCertificate;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    public class IndemnityCertificateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IndemnityCertificate _indemnityCertificate;
        public IndemnityCertificateController(ApplicationDbContext context)
        {
            _context = context;
            _indemnityCertificate = new IndemnityCertificate(_context);
        }
        [DisplayName("Operational")]
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_indemnityCertificate.GetList().Result.Cast<IndemnityCertificate>());
        }

        public async Task<PartialViewResult> Detail(long id)
        {
            if (id != 0)
            {
                _indemnityCertificate.Id = id;
                var indemnity = _indemnityCertificate.Refresh().Result as IndemnityCertificate;
                return PartialView(indemnity);
            }
            return PartialView();
        }
        public PartialViewResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(IndemnityCertificate model, IFormCollection formCollection)
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
                _indemnityCertificate.Id = id;
                var indemnity = _indemnityCertificate.Refresh().Result as IndemnityCertificate;
                return PartialView(indemnity);
            }

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, IndemnityCertificate model, IFormCollection formCollection)
        {
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
        [HttpGet]
        public PartialViewResult Seed()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Seed(IndemnityCertificate model, IFormCollection formCollection)
        {
            _context.IndemnityCertificate.Add(new IndemnityCertificate
            {
                FlightNumber ="ET536",
                From = "DIR",
                To ="ADD",
                FullAddress = "Bole, Addis Ababa, Ethiopia",
                Guardian ="Birhane Mola",
                IdentityCard ="K05/282/02",
                Date = DateTime.Now,
                PassangerSign = "Tenagne Molla",
                Status = RecordStatus.Active
            });
           if(_context.SaveChanges() > 0)
                return new JsonResult(new DatabaseOperationResponse
                {
                    Status = OperationStatus.SUCCESS,
                    Message = "Success"
                });

            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.SUCCESS,
                Message = "Success"
            });
        }
    }
}
