using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects;
using Ground_Handlng.DataObjects.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    public class GroundHandlerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GroundHandler _groundHandler;
        private readonly IHostingEnvironment hostingEnvironment;
        public GroundHandlerController(ApplicationDbContext context, IHostingEnvironment _hostingEnvironment)
        {
            _context = context;
            _groundHandler = new GroundHandler(_context);
            hostingEnvironment = _hostingEnvironment;
        }
        //[HttpGet]
        //public async Task<ActionResult> Index()
        //{
        //    if (TempData["SuccessMessage"] != null)
        //    {
        //        ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
        //    }
        //    return View(_groundHandler.GetList().Result.Cast<GroundHandler>());
        //}
    }
}