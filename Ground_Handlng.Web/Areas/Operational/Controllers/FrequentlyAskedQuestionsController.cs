using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational;
using Microsoft.AspNetCore.Http;
using Ground_Handlng.DataObjects.Models.Others;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    public class FrequentlyAskedQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FrequentlyAskedQuestion _FrequentlyAskedQuestions;

        public FrequentlyAskedQuestionsController(ApplicationDbContext context)
        {
            _context = context;
            _FrequentlyAskedQuestions = new FrequentlyAskedQuestion(_context);
        }

        // GET: Operational/FrequentlyAskedQuestions
        public async Task<IActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_FrequentlyAskedQuestions.GetList().Result.Cast<FrequentlyAskedQuestion>());
        }


        // GET: Operational/FrequentlyAskedQuestions/Create
        public PartialViewResult Create()
        {
            return PartialView();
        }

        // POST: Operational/FrequentlyAskedQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FrequentlyAskedQuestion model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {

                _FrequentlyAskedQuestions.Question = model.Question;
                _FrequentlyAskedQuestions.Answer = model.Answer;
                _FrequentlyAskedQuestions.ImageUrl = model.ImageUrl;
                _FrequentlyAskedQuestions.Type = model.Type;

                var result = await _FrequentlyAskedQuestions.Save() as DatabaseOperationResponse;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: Operational/FrequentlyAskedQuestions/Edit/5
        public PartialViewResult Edit(long id)
        {
            if (id != 0)
            {
                _FrequentlyAskedQuestions.Id = id;
                var FAQ = _FrequentlyAskedQuestions.Refresh().Result as FrequentlyAskedQuestion;
                ViewBag.Question = new SelectList(_context.FrequentlyAskedQuestions, "Id", "Question", FAQ.Question);
                ViewBag.Answer = new SelectList(_context.FrequentlyAskedQuestions, "Id", "Answer", FAQ.Answer);
                ViewBag.ImageUrl = new SelectList(_context.FrequentlyAskedQuestions, "Id", "ImageUrl", FAQ.ImageUrl);
                ViewBag.Type = new SelectList(_context.FrequentlyAskedQuestions, "Id", "Type", FAQ.Type);
                return PartialView(FAQ);
            }
            return PartialView();
        }

        // POST: Operational/FrequentlyAskedQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, FrequentlyAskedQuestion model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _FrequentlyAskedQuestions.Id = model.Id;
                _FrequentlyAskedQuestions.Question = model.Question;
                _FrequentlyAskedQuestions.Answer = model.Answer;
                _FrequentlyAskedQuestions.ImageUrl = model.ImageUrl;
                _FrequentlyAskedQuestions.Type = model.Type;

                var result = await _FrequentlyAskedQuestions.Update() as DatabaseOperationResponse;
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }


        public async Task<JsonResult> Delete(long id)
        {
            if (id != 0)
            {
                var FAQDelete = await _context.FrequentlyAskedQuestions.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _FrequentlyAskedQuestions.Id = FAQDelete.Id;
                _FrequentlyAskedQuestions.Question = FAQDelete.Question;
                _FrequentlyAskedQuestions.Answer = FAQDelete.Answer;
                _FrequentlyAskedQuestions.ImageUrl = FAQDelete.ImageUrl;
                _FrequentlyAskedQuestions.Type = FAQDelete.Type;

                var result = await _FrequentlyAskedQuestions.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }

        private bool FrequentlyAskedQuestionExists(long id)
        {
            return _context.FrequentlyAskedQuestions.Any(e => e.Id == id);
        }
    }
}
