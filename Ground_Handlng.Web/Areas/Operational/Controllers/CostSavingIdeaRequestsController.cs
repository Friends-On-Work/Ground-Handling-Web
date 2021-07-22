using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Ground_Handlng.DataObjects.Models.Others;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    public class CostSavingIdeaRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CostSavingIdeaRequest _CostSavingIdeaRequest;


        public CostSavingIdeaRequestsController(ApplicationDbContext context)
        {
            _context = context;
            _CostSavingIdeaRequest = new CostSavingIdeaRequest(_context);

        }


        // GET: Operational/CostSavingIdeaRequests
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_CostSavingIdeaRequest.GetList().Result.Cast<CostSavingIdeaRequest>());
        }


        // GET: Operational/CostSavingIdeaRequests/Create
        public PartialViewResult Create()
        {

            return PartialView();
        }

        // POST: Operational/CostSavingIdeaRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CostSavingIdeaRequest model, IFormCollection formCollection)
        {


            if (ModelState.IsValid)
            {
             
                _CostSavingIdeaRequest.EmployeeId = model.EmployeeId;
                _CostSavingIdeaRequest.Idea = model.Idea;
                _CostSavingIdeaRequest.Date = model.Date;

                var result = await _CostSavingIdeaRequest.Save() as DatabaseOperationResponse;
                return RedirectToAction("Index");
            }
            //return new JsonResult(new DatabaseOperationResponse
            //{
            //    Status = OperationStatus.NOT_OK,
            //    ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            //}); 
            return RedirectToAction("Index");
        }

        // GET: Operational/CostSavingIdeaRequests/Edit/5
        public PartialViewResult Edit(long id)
        {
            if (id != 0)
            {
                _CostSavingIdeaRequest.Id = id;
                var CostSaving = _CostSavingIdeaRequest.Refresh().Result as CostSavingIdeaRequest;
                ViewBag.EmployeeId = new SelectList(_context.CostSavingIdeaRequests, "Id", "EmployeeId", CostSaving.EmployeeId);
                ViewBag.Idea = new SelectList(_context.CostSavingIdeaRequests, "Id", "Idea", CostSaving.Idea);
                ViewBag.Date = new SelectList(_context.CostSavingIdeaRequests, "Id", "Date", CostSaving.Date);
                return PartialView(CostSaving);
            }
            return PartialView();
        }

        // POST: Operational/CostSavingIdeaRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, CostSavingIdeaRequest model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _CostSavingIdeaRequest.Id = model.Id;
                _CostSavingIdeaRequest.EmployeeId = model.EmployeeId;
                _CostSavingIdeaRequest.Idea = model.Idea;
                _CostSavingIdeaRequest.Date = model.Date;

                var result = await _CostSavingIdeaRequest.Update() as DatabaseOperationResponse;
                return RedirectToAction("Index");
                // return new JsonResult(result);
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
                var Cost = await _context.CostSavingIdeaRequests.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _CostSavingIdeaRequest.Id = Cost.Id;
                _CostSavingIdeaRequest.EmployeeId = Cost.EmployeeId;
                _CostSavingIdeaRequest.Idea = Cost.Idea;
                _CostSavingIdeaRequest.Date = Cost.Date;

                var result = await _CostSavingIdeaRequest.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }

        private bool CostSavingIdeaRequestExists(double id)
        {
            return _context.CostSavingIdeaRequests.Any(e => e.Id == id);
        }
    }
}
