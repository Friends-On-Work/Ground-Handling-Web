using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational.Bulletin;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    public class BulletinController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Bulletin _bulletin;
        private readonly IHostingEnvironment hostingEnvironment;
        public BulletinController(ApplicationDbContext context, IHostingEnvironment _hostingEnvironment)
        {
            _context = context;
            _bulletin = new Bulletin(_context);
            hostingEnvironment = _hostingEnvironment;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_bulletin.GetList().Result.Cast<Bulletin>());
        }

        // GET: Operational/Bulletin/Create
        public PartialViewResult Create()
        {
            ViewBag.BulletinFrom = new SelectList(_context.BulletinFrom, "Name", "Name");
            ViewBag.BuilletinTo = new SelectList(_context.BulletinTo, "Name", "Name");
            ViewBag.BulletinNoticeType = new SelectList(_context.BulletinNoticeType, "TypeName", "TypeName");
            ViewBag.BulletinType = new SelectList(_context.BulletinTypes, "TypeName", "TypeName");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(Bulletin model, IFormCollection formCollection)
        {
            IFormFile file;
            if (Request.Form.Files["url"] != null)
                file = Request.Form.Files["url"];
            else
                file = null;
            CancellationToken cancellationToken;
            if (ModelState.IsValid)
            {
                if (file != null)
                    _bulletin.BulletinUrl = await UploadFile(model, file, cancellationToken);
                _bulletin.NoticeOrBulletin = model.NoticeOrBulletin;
                _bulletin.NoticeNumber = model.NoticeNumber;
                _bulletin.CreatedBy = User.Identity.Name;
                _bulletin.NoticeTo = model.NoticeTo;
                _bulletin.SentBy = model.SentBy;
                _bulletin.Subject = model.Subject;
                _bulletin.Type = model.Type;
                _bulletin.Date = DateTime.Now;
                var result = await _bulletin.Save() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }

        public async Task<string> UploadFile(Bulletin manual, IFormFile file, CancellationToken cancellationToken)
        {
            _bulletin.NoticeOrBulletin = manual.NoticeOrBulletin;
            _bulletin.NoticeNumber = manual.NoticeNumber;
            _bulletin.CreatedBy = User.Identity.Name;
            _bulletin.NoticeTo = manual.NoticeTo;
            if (file != null)
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    await file.CopyToAsync(mStream, cancellationToken);
                    if (!Directory.Exists(hostingEnvironment.WebRootPath + @"\Bulletins\"))
                        Directory.CreateDirectory(hostingEnvironment.WebRootPath + @"\Bulletins\");
                    if (!Directory.Exists(hostingEnvironment.WebRootPath + @"\Bulletins\" + @"\Bulletin\"))
                        Directory.CreateDirectory(hostingEnvironment.WebRootPath + @"\Bulletins\" + @"\Bulletin\");
                    string accountname = "b2cappdevstorageaccount";
                    string accesskey = "wizef3HEea4togALA8D/6MbCmT8z3qutN/BD2/chMNi2uI4y5uMd1/aBC1X4cj0kZ5m9Vy8S1mW2xIJeakFKJw==";
                    try
                    {
                        StorageCredentials credintial = new StorageCredentials(accountname, accesskey);
                        CloudStorageAccount acc = new CloudStorageAccount(credintial, useHttps: true);
                        CloudBlobClient client = acc.CreateCloudBlobClient();
                        CloudBlobContainer cont = client.GetContainerReference("groundservicemanuals");
                        CloudBlobDirectory manualDir = cont.GetDirectoryReference("manuals");
                        CloudBlobDirectory booksDir = manualDir.GetDirectoryReference("book");
                        await cont.CreateIfNotExistsAsync();

                        await cont.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                        if (!Directory.Exists(hostingEnvironment.WebRootPath + $"\\Bulletins\\Bulletin\\{_bulletin.NoticeOrBulletin.Replace(" ", "_").Trim()}\\{_bulletin.NoticeNumber.Replace(" ", "_").Trim()}\\"))
                            Directory.CreateDirectory(hostingEnvironment.WebRootPath + $"\\Bulletins\\Bulletin\\{_bulletin.NoticeOrBulletin.Replace(" ", "_").Trim()}\\{_bulletin.NoticeNumber.Replace(" ", "_").Trim()}\\");
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, $"Bulletins\\Bulletin\\{_bulletin.NoticeOrBulletin.Replace(" ", "_").Trim()}\\{_bulletin.NoticeNumber.Replace(" ", "_").Trim()}\\");
                        var firlname = file.FileName.ToLower().Trim().Replace("&", "and");
                        var fileName = firlname.ToLower().Trim().Replace(" ", "_");
                        var uploadFileName = "bulletins/bulletin/" + $"{_bulletin.NoticeOrBulletin.ToLower().Replace(" ", "_").Trim()}/{_bulletin.NoticeNumber.ToLower().Replace(" ", "_").Trim()}/" + firlname.ToLower().Trim().Replace(" ", "_");
                        try
                        {
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            CloudBlockBlob cblob = cont.GetBlockBlobReference(uploadFileName);
                            using (Stream fileUpload = System.IO.File.OpenRead(Path.Combine(uploads, fileName)))
                            {
                                await cblob.UploadFromStreamAsync(fileUpload);
                                _bulletin.BulletinUrl = cblob.Uri.AbsoluteUri;
                                if (_bulletin.BulletinUrl.Contains("&"))
                                {
                                    _bulletin.BulletinUrl = Regex.Replace(_bulletin.BulletinUrl, @"&", "%26");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return _bulletin.BulletinUrl;
        }

        // GET: Operational/Bulletin/Edit
        public PartialViewResult Edit(long id)
        {
            if (id != 0)
            {
                _bulletin.Id = id;
                var bulletin = _bulletin.Refresh().Result as Bulletin;
                ViewBag.BulletinFrom = new SelectList(_context.BulletinFrom, "Id", "Name", bulletin.SentBy);
                ViewBag.BuilletinTo = new SelectList(_context.BulletinTo, "Id", "Name", bulletin.NoticeTo);
                ViewBag.BulletinNoticeType = new SelectList(_context.BulletinNoticeType, "Id", "TypeName",bulletin.NoticeOrBulletin);
                ViewBag.BulletinType = new SelectList(_context.BulletinTypes, "Id", "TypeName",bulletin.Type);
                return PartialView(bulletin);
            }
                
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, Bulletin model, IFormCollection formCollection)
        {
            if(ModelState.IsValid)
            {
                _bulletin.Id = model.Id;
                _bulletin.NoticeOrBulletin = model.NoticeOrBulletin;
                _bulletin.NoticeNumber = model.NoticeNumber;
                _bulletin.CreatedBy = model.CreatedBy;
                _bulletin.CreationDate = model.CreationDate;
                _bulletin.RevisedBy = User.Identity.Name;
                _bulletin.NoticeTo = model.NoticeTo;
                _bulletin.SentBy = model.SentBy;
                _bulletin.Subject = model.Subject;
                _bulletin.Type = model.Type;
                _bulletin.Date = model.Date;
                _bulletin.BulletinUrl = model.BulletinUrl;
                var result = await _bulletin.Update() as DatabaseOperationResponse;
                return new JsonResult(result);

            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }

        public PartialViewResult Detail(long id)
        {
            if (id != 0)
            {
                _bulletin.Id = id;
                var bulletin = _bulletin.Refresh().Result as Bulletin;
                return PartialView(bulletin);
            }
            return PartialView();
        }
        // GET: Operational/Bulletin/Delete/5
        public async Task<JsonResult> Delete(long id)
        {
            if (id != 0)
            {
                var bulletin = await _context.Bulletin.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _bulletin.Id = bulletin.Id;
                _bulletin.NoticeOrBulletin = bulletin.NoticeOrBulletin;
                _bulletin.NoticeNumber = bulletin.NoticeNumber;
                _bulletin.CreatedBy = User.Identity.Name;
                _bulletin.NoticeTo = bulletin.NoticeTo;
                _bulletin.SentBy = bulletin.SentBy;
                _bulletin.Subject = bulletin.Subject;
                _bulletin.Type = bulletin.Type;
                _bulletin.Date = bulletin.Date;
                _bulletin.BulletinUrl = bulletin.BulletinUrl;
                var result = await _bulletin.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }
    }
}