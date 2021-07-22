using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational.Forms.PregnancyCertificate;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    public class PregnancyCertificateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PregnancyCertificate _pregnancyCertificate;
        public PregnancyCertificateController(ApplicationDbContext context)
        {
            _context = context;
            _pregnancyCertificate = new PregnancyCertificate(_context);
        }
        [DisplayName("Operational")]
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            if(!_context.PregnancyCertificate.Any())
            {
                _context.PregnancyCertificate.Add(new PregnancyCertificate
                {
                    From = "DXB",
                    To= "ADD",
                    SignatureOfPassenger = "Tenagne Mola",
                    DateCertificateIssued = DateTime.Now,
                    DateOfBirthEstimated = DateTime.Now.AddMonths(4),
                    DateOfExamination = DateTime.Now.AddDays(-10),
                    Status = DataObjects.Models.Others.RecordStatus.Active,
                    DateOfTravel = DateTime.Now.AddDays(3),
                    PregnancyCertificateType = DataObjects.Models.Others.PregnancyCertificateType.Certificate1,
                    SignatureOfPhysician ="Dr. Wende",
                    FullName = "Tenagne Mola"
                });
                _context.SaveChanges();
            }
            return View(_pregnancyCertificate.GetList().Result.Cast<PregnancyCertificate>());
        }

        public async Task<PartialViewResult> Detail(long id)
        {
            if (id != 0)
            {
                _pregnancyCertificate.Id = id;
                var pregnancy = _pregnancyCertificate.Refresh().Result as PregnancyCertificate;
                return PartialView(pregnancy);
            }
            return PartialView();
        }

        public async Task<PartialViewResult> DetailPregnancy(long id)
        {
            if(id != 0)
            {
                _pregnancyCertificate.Id = id;
                var pregnancy = _pregnancyCertificate.Refresh().Result as PregnancyCertificate;
                return PartialView(pregnancy);
            }
            return PartialView();
        }
        public PartialViewResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(PregnancyCertificate model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _pregnancyCertificate.SignatureOfPassenger = model.SignatureOfPassenger;
                _pregnancyCertificate.SignatureOfPhysician = model.SignatureOfPhysician;
                _pregnancyCertificate.CreatedBy = model.CreatedBy;
                _pregnancyCertificate.CreationDate = model.CreationDate;
                _pregnancyCertificate.RevisedBy = User.Identity.Name;
                _pregnancyCertificate.SignatureOfRelative = model.SignatureOfRelative;
                _pregnancyCertificate.FullName = model.FullName;
                _pregnancyCertificate.From = model.From;
                _pregnancyCertificate.To = model.To;
                _pregnancyCertificate.DateCertificateIssued = model.DateCertificateIssued;
                _pregnancyCertificate.DateOfBirthEstimated = model.DateOfBirthEstimated;
                _pregnancyCertificate.DateOfExamination = model.DateOfExamination;
                _pregnancyCertificate.DateOfTravel = model.DateOfTravel;
                _pregnancyCertificate.EmailOfCaptain = model.EmailOfCaptain;
                _pregnancyCertificate.EmailOfPassenger = model.EmailOfPassenger;
                var result = await _pregnancyCertificate.Save() as DatabaseOperationResponse;
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
                _pregnancyCertificate.Id = id;
                var pregnancy = _pregnancyCertificate.Refresh().Result as PregnancyCertificate;
                return PartialView(pregnancy);
            }

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, PregnancyCertificate model, IFormCollection formCollection)
        {

            if (ModelState.IsValid)
            {
                _pregnancyCertificate.Id = model.Id;
                _pregnancyCertificate.SignatureOfPassenger = model.SignatureOfPassenger;
                _pregnancyCertificate.SignatureOfPhysician = model.SignatureOfPhysician;
                _pregnancyCertificate.CreatedBy = model.CreatedBy;
                _pregnancyCertificate.CreationDate = model.CreationDate;
                _pregnancyCertificate.RevisedBy = User.Identity.Name;
                _pregnancyCertificate.SignatureOfRelative = model.SignatureOfRelative;
                _pregnancyCertificate.FullName = model.FullName;
                _pregnancyCertificate.From = model.From;
                _pregnancyCertificate.To = model.To;
                _pregnancyCertificate.DateCertificateIssued = model.DateCertificateIssued;
                _pregnancyCertificate.DateOfBirthEstimated = model.DateOfBirthEstimated;
                _pregnancyCertificate.DateOfExamination = model.DateOfExamination;
                _pregnancyCertificate.DateOfTravel = model.DateOfTravel;
                _pregnancyCertificate.EmailOfCaptain = model.EmailOfCaptain;
                _pregnancyCertificate.EmailOfPassenger = model.EmailOfPassenger;
                var result = await _pregnancyCertificate.Update() as DatabaseOperationResponse;
                return new JsonResult(result);

            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
    }
}
