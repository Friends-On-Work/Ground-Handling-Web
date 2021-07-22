using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational.Manual;
using Ground_Handlng.DataObjects.Models.Others;
using Ground_Handlng.DataObjects.Models.Operational;
using Microsoft.AspNetCore.Http;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    public class FeedbackRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FeedbackRequest _FeedbackRequest;

        public FeedbackRequestsController(ApplicationDbContext context)
        {
            _context = context;
            _FeedbackRequest = new FeedbackRequest(_context);
        }

        // GET: Operational/FeedbackRequests
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_FeedbackRequest.GetList().Result.Cast<FeedbackRequest>());
        }


        // GET: Operational/FeedbackRequests/Create
        public PartialViewResult Create()
        {

            return PartialView();
        }

        // POST: Operational/FeedbackRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackRequest model, IFormCollection formCollection)
            { 


            if (ModelState.IsValid)
            {
                _FeedbackRequest.Id = model.Id;
                _FeedbackRequest.EmployeeId = model.EmployeeId;
                _FeedbackRequest.Description = model.Description;
                _FeedbackRequest.Type = model.Type;

                var result = await _FeedbackRequest.Save() as DatabaseOperationResponse;
                return RedirectToAction("Index");
                //return new JsonResult(result);
            }
            return RedirectToAction("Index");
            //return new JsonResult(new DatabaseOperationResponse
            //{
            //    Status = OperationStatus.NOT_OK,
            //    ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            //});
        }

        // GET: Operational/FeedbackRequests/Edit/5
        public PartialViewResult Edit(long id)
        {
            if (id != 0)
            {
                _FeedbackRequest.Id = id;
                var Feedback = _FeedbackRequest.Refresh().Result as FeedbackRequest;
                ViewBag.EmployeeId = new SelectList(_context.FeedbackRequests, "Id", "EmployeeId", Feedback.EmployeeId);
                ViewBag.Description = new SelectList(_context.FeedbackRequests, "Id", "Description", Feedback.Description);
                ViewBag.Type = new SelectList(_context.FeedbackRequests, "Id", "Type", Feedback.Type);
                return PartialView(Feedback);
            }

            return PartialView();
        }

        // POST: Operational/FeedbackRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, FeedbackRequest model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _FeedbackRequest.Id = model.Id;
                _FeedbackRequest.EmployeeId = model.EmployeeId;
                _FeedbackRequest.Description = model.Description;
                _FeedbackRequest.Type = model.Type;

                var result = await _FeedbackRequest.Update() as DatabaseOperationResponse;
                return RedirectToAction("Index");
                //return new JsonResult(result);
            }
            return RedirectToAction("Index");
            //return new JsonResult(new DatabaseOperationResponse
            //{
            //    Status = OperationStatus.NOT_OK,
            //    ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            //});
        }

        public async Task<JsonResult> Delete(long id)
        {
            if (id != 0)
            {
                var FeedbackRequest = await _context.FeedbackRequests.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _FeedbackRequest.Id = FeedbackRequest.Id;
                _FeedbackRequest.EmployeeId = FeedbackRequest.EmployeeId;
                _FeedbackRequest.Type = FeedbackRequest.Type;
                _FeedbackRequest.Description = FeedbackRequest.Description;

                var result = await _FeedbackRequest.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }
        private bool FeedbackRequestExists(long id)
        {
            return _context.FeedbackRequests.Any(e => e.Id == id);
        }
    }
}
