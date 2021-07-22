using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational;
using Microsoft.AspNetCore.Http;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Hosting;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    public class ManagementContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ManagementContact _ManagementContacts;
        private IHostingEnvironment _env;
        public ManagementContactsController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _ManagementContacts = new ManagementContact(_context);
            _env = env;
        }

        // GET: Operational/ManagementContacts
        public async Task<IActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_ManagementContacts.GetList().Result.Cast<ManagementContact>());
        }



        // GET: Operational/ManagementContacts/Create
        public PartialViewResult Create()
        {
            return PartialView();
        }

        // POST: Operational/ManagementContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManagementContact model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {

                _ManagementContacts.FullName = model.FullName;
                _ManagementContacts.Position = model.Position;
                _ManagementContacts.PhoneNumber = model.PhoneNumber;
                _ManagementContacts.Email = model.Email;
                _ManagementContacts.WorkingArea = model.WorkingArea;

                var result = await _ManagementContacts.Save() as DatabaseOperationResponse;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: Operational/ManagementContacts/Edit/5
        public PartialViewResult Edit(long id)
        {
            if (id != 0)
            {
                _ManagementContacts.Id = id;
                var FAQ = _ManagementContacts.Refresh().Result as ManagementContact;

                return PartialView(FAQ);
            }
            return PartialView();
        }

        // POST: Operational/ManagementContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ManagementContact model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _ManagementContacts.Id = model.Id;
                _ManagementContacts.FullName = model.FullName;
                _ManagementContacts.Position = model.Position;
                _ManagementContacts.PhoneNumber = model.PhoneNumber;
                _ManagementContacts.Email = model.Email;
                _ManagementContacts.WorkingArea = model.WorkingArea;
                
                var result = await _ManagementContacts.Update() as DatabaseOperationResponse;
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }
        public async Task<JsonResult> Delete(long id)
        {
            if (id != 0)
            {
                var ManagementContact = await _context.ManagementContacts.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);

                _ManagementContacts.Id = ManagementContact.Id;
                _ManagementContacts.FullName = ManagementContact.FullName;
                _ManagementContacts.Position = ManagementContact.Position;
                _ManagementContacts.PhoneNumber = ManagementContact.PhoneNumber;
                _ManagementContacts.Email = ManagementContact.Email;
                _ManagementContacts.WorkingArea = ManagementContact.WorkingArea;

                var result = await _ManagementContacts.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }

        [HttpGet]
        public PartialViewResult Upload()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile files, ManagementContact ManagementContacts)
        {
            var SavedContacts = _context.ManagementContacts.ToList();
            //if (ModelState.IsValid)
            //{
            try
            {
                string filePath = string.Empty;
                try
                {
                    if (files != null)
                    {
                        string path = System.IO.Path.Combine(_env.WebRootPath,"Uploads");                        
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        filePath = Path.Combine(path,Path.GetFileName(files.FileName));
                        string extension = Path.GetExtension(files.FileName);
                        var streamReader = new StreamReader(files.OpenReadStream());                  
                        var csvcontactData = streamReader.ReadToEndAsync().Result;
                        
                            foreach (string row in csvcontactData.Split('\n'))
                            {
                                string rowString = row.ToLower();
                                rowString.Trim();
                                if (rowString.Contains("full name"))
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(row))
                                {
                                    ManagementContact managementContact = null;
                                    if (SavedContacts?.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(row.Split(',')[3]))
                                        {
                                        managementContact = SavedContacts.Where(c => c.Email.Equals(row.Split(',')[3], StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                        }
                                        else
                                            continue;
                                    }
                                    if (managementContact == null)
                                    {

                                    ManagementContact Mcontact = new ManagementContact()
                                    {

                                        FullName = row.Split(',')[1],
                                        Position = row.Split(',')[2],
                                        PhoneNumber = row.Split(',')[4],
                                        Email = await EmailNormilizer(row.Split(',')[3]),
                                        WorkingArea = row.Split(',')[0].Trim(),
                                        Status = RecordStatus.Active,
                                };
                                    _context.ManagementContacts.Add(Mcontact);
                                    _context.SaveChanges();

                                }
                                    else
                                    {
                                    managementContact.WorkingArea = row.Split(',')[0];
                                    managementContact.FullName = row.Split(',')[1];
                                    managementContact.Position = row.Split(',')[2];
                                    managementContact.Email = await EmailNormilizer(row.Split(',')[3]);
                                    managementContact.PhoneNumber = row.Split(',')[4];
                                    managementContact.Status = RecordStatus.Active;

                                    _context.Entry(managementContact).State = EntityState.Modified;
                                    _context.SaveChanges();

                                    //return RedirectToAction("Index");
                                    }
                                }
                            }
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }


                return RedirectToAction("Index");
            }
            catch (System.IndexOutOfRangeException e)  // CS0168
            {
                Console.WriteLine(e.Message);
                throw new System.ArgumentOutOfRangeException("index parameter is out of range.", e);
            }
            catch (Exception ex)
            {

            }
            //}
            return RedirectToAction("Index");
        }
        public async Task<string> EmailNormilizer(string email)
        {
            var crewmail = string.Empty;
            if (!string.IsNullOrEmpty(email))// && IsValidEmail(email))
            {
                crewmail = Regex.Replace(email.Trim(), @"\t|\n|\r", "");
            }
            return crewmail;

        }

        private bool ManagementContactExists(long id)
        {
            return _context.ManagementContacts.Any(e => e.Id == id);
        }
    }
}
