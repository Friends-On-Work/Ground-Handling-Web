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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;
using iTextSharp.text;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text.RegularExpressions;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    public class SliderViewItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SliderViewItem _SliderViewItems;
        private readonly IHostingEnvironment hostingEnvironment;

        public SliderViewItemsController(ApplicationDbContext context, IHostingEnvironment _hostingEnvironment)
        {
            _context = context;
            _SliderViewItems = new SliderViewItem(_context);
            hostingEnvironment = _hostingEnvironment;
        }

        // GET: Operational/SliderViewItems
        public async Task<IActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_SliderViewItems.GetList().Result.Cast<SliderViewItem>());
        }

        // GET: Operational/SliderViewItems/Details/5

        // GET: Operational/SliderViewItems/Create
        public PartialViewResult Create()
        {
            return PartialView();
        }

        // POST: Operational/SliderViewItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderViewItem model, IFormCollection formCollection, IFormFile files, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                _SliderViewItems.CreatedBy = User.Identity.Name;
                _SliderViewItems.Title = model.Title;
                _SliderViewItems.Description = model.Description;
                _SliderViewItems.URL = model.URL;
                _SliderViewItems.PostedDate = DateTime.Today;
                _SliderViewItems.ExpiresDate = model.ExpiresDate;

                if (files != null)
                {
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        await files.CopyToAsync(mStream, cancellationToken);
                        Document myDocument = new Document();
                        PdfWriter myPDFWriter = PdfWriter.GetInstance(myDocument, mStream);
                        if (!Directory.Exists(hostingEnvironment.WebRootPath + @"\news\"))
                            Directory.CreateDirectory(hostingEnvironment.WebRootPath + @"\news\");
                        if (!Directory.Exists(hostingEnvironment.WebRootPath + @"\news\" + @"\image\"))
                            Directory.CreateDirectory(hostingEnvironment.WebRootPath + @"\news\" + @"\image\");
                        string accountname = "b2cappdevstorageaccount";
                        string accesskey = "wizef3HEea4togALA8D/6MbCmT8z3qutN/BD2/chMNi2uI4y5uMd1/aBC1X4cj0kZ5m9Vy8S1mW2xIJeakFKJw==";
                        try
                        {
                            StorageCredentials credintial = new StorageCredentials(accountname, accesskey);
                            CloudStorageAccount acc = new CloudStorageAccount(credintial, useHttps: true);
                            CloudBlobClient client = acc.CreateCloudBlobClient();
                            CloudBlobContainer cont = client.GetContainerReference("groundservicenews");
                            CloudBlobDirectory manualDir = cont.GetDirectoryReference("news");
                            CloudBlobDirectory booksDir = manualDir.GetDirectoryReference("image");
                            await cont.CreateIfNotExistsAsync();

                            await cont.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                            var firlname = files.FileName.ToLower().Trim().Replace("&", "and");
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, $"news\\image\\");
                 
                            var fileName = firlname.ToLower().Trim().Replace(" ", "_");
                            var uploadFileName = "news/image/" + firlname.ToLower().Trim().Replace(" ", "_");
                            Document document = new Document();
                            try
                            {
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await files.CopyToAsync(fileStream);
                                }
                                CloudBlockBlob cblob = cont.GetBlockBlobReference(uploadFileName);
                                using (Stream fileUpload = System.IO.File.OpenRead(Path.Combine(uploads, fileName)))
                                {
                                    await cblob.UploadFromStreamAsync(fileUpload);
                                    _SliderViewItems.Image = cblob.Uri.AbsoluteUri;
                                    if (_SliderViewItems.Image.Contains("&"))
                                    {
                                        _SliderViewItems.Image = Regex.Replace(_SliderViewItems.Image, @"&", "%26");
                                    }
                                    if (_SliderViewItems.Title != null)
                                        if (_SliderViewItems.Title.Contains("&"))
                                        {
                                            _SliderViewItems.Title = Regex.Replace(_SliderViewItems.Title, @"&", "and");
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
      

                var result = await _SliderViewItems.Save() as DatabaseOperationResponse;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        // GET: Operational/SliderViewItems/Edit/5
        public PartialViewResult Edit(long id)
        {
            if (id != 0)
            {
                _SliderViewItems.Id = id;
                var FAQ = _SliderViewItems.Refresh().Result as SliderViewItem;

                return PartialView(FAQ);
            }
            return PartialView();
        }

        // POST: Operational/SliderViewItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, SliderViewItem model, IFormCollection formCollection, IFormFile files, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                _SliderViewItems.Id = model.Id;
                _SliderViewItems.CreatedBy = User.Identity.Name;
                _SliderViewItems.Title = model.Title;
                _SliderViewItems.Description = model.Description;
                _SliderViewItems.URL = model.URL;
                _SliderViewItems.PostedDate = DateTime.Today;
                _SliderViewItems.ExpiresDate = model.ExpiresDate;

                if (files != null)
                {
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        await files.CopyToAsync(mStream, cancellationToken);
                        Document myDocument = new Document();
                        PdfWriter myPDFWriter = PdfWriter.GetInstance(myDocument, mStream);
                        if (!Directory.Exists(hostingEnvironment.WebRootPath + @"\news\"))
                            Directory.CreateDirectory(hostingEnvironment.WebRootPath + @"\news\");
                        if (!Directory.Exists(hostingEnvironment.WebRootPath + @"\news\" + @"\image\"))
                            Directory.CreateDirectory(hostingEnvironment.WebRootPath + @"\news\" + @"\image\");
                        string accountname = "b2cappdevstorageaccount";
                        string accesskey = "wizef3HEea4togALA8D/6MbCmT8z3qutN/BD2/chMNi2uI4y5uMd1/aBC1X4cj0kZ5m9Vy8S1mW2xIJeakFKJw==";
                        try
                        {
                            StorageCredentials credintial = new StorageCredentials(accountname, accesskey);
                            CloudStorageAccount acc = new CloudStorageAccount(credintial, useHttps: true);
                            CloudBlobClient client = acc.CreateCloudBlobClient();
                            CloudBlobContainer cont = client.GetContainerReference("groundservicenews");
                            CloudBlobDirectory manualDir = cont.GetDirectoryReference("news");
                            CloudBlobDirectory booksDir = manualDir.GetDirectoryReference("image");
                            await cont.CreateIfNotExistsAsync();

                            await cont.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                            var firlname = files.FileName.ToLower().Trim().Replace("&", "and");
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, $"news\\image\\");

                            var fileName = firlname.ToLower().Trim().Replace(" ", "_");
                            var uploadFileName = "news/image/" + firlname.ToLower().Trim().Replace(" ", "_");
                            Document document = new Document();
                            try
                            {
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await files.CopyToAsync(fileStream);
                                }
                                CloudBlockBlob cblob = cont.GetBlockBlobReference(uploadFileName);
                                using (Stream fileUpload = System.IO.File.OpenRead(Path.Combine(uploads, fileName)))
                                {
                                    await cblob.UploadFromStreamAsync(fileUpload);
                                    _SliderViewItems.Image = cblob.Uri.AbsoluteUri;
                                    if (_SliderViewItems.Image.Contains("&"))
                                    {
                                        _SliderViewItems.Image = Regex.Replace(_SliderViewItems.Image, @"&", "%26");
                                    }
                                    if (_SliderViewItems.Title != null)
                                        if (_SliderViewItems.Title.Contains("&"))
                                        {
                                            _SliderViewItems.Title = Regex.Replace(_SliderViewItems.Title, @"&", "and");
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
                var result = await _SliderViewItems.Update() as DatabaseOperationResponse;
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<PartialViewResult> Detail(long id)
        {
            List<SliderViewItem> SliderViewItem = new List<SliderViewItem>();

            if (id != 0)
            {
                _SliderViewItems.Id = id;
                var News = _SliderViewItems.Refresh().Result as SliderViewItem;

                return PartialView(News);
            }
            else
                ViewBag.Message = "Bad Request";
            return PartialView();
        }

        public async Task<JsonResult> Delete(long id)
        {
            if (id != 0)
            {
                var NewsItem = await _context.SliderViewItems.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);

                _SliderViewItems.Id = NewsItem.Id;
                _SliderViewItems.CreatedBy = NewsItem.CreatedBy;
                _SliderViewItems.Title = NewsItem.Title;
                _SliderViewItems.Description = NewsItem.Description;
                _SliderViewItems.URL = NewsItem.URL;
                _SliderViewItems.PostedDate = NewsItem.PostedDate;
                _SliderViewItems.ExpiresDate = NewsItem.ExpiresDate;

                var result = await _SliderViewItems.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }

        private bool SliderViewItemExists(long id)
        {
            return _context.SliderViewItems.Any(e => e.Id == id);
        }
    }
}
