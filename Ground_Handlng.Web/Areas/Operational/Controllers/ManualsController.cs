using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Others;
using Ground_Handlng.DataObjects.Models.Operational.Manual;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ground_Handlng.DataObjects.ViewModel.Manual;
using Microsoft.AspNetCore.Mvc.Rendering;
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;
using System.IO;
using iTextSharp.text;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Hosting;
using iTextSharp.text.html.simpleparser;
using System.Net;
using System.Text.RegularExpressions;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    public class ManualsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Book _book;
        private readonly BookChapter _bookChapter;
        private readonly BookChapterSection _bookChapterSection;
        private readonly ManualAccessLog _manualAccessLog;
        private readonly IHostingEnvironment hostingEnvironment;
        public ManualsController(ApplicationDbContext context,IHostingEnvironment _hostingEnvironment)
        {
            _context = context;
            _bookChapterSection = new BookChapterSection(_context);
            _bookChapter = new BookChapter(_context);
            _book = new Book(_context);
            _manualAccessLog = new ManualAccessLog(_context);
            hostingEnvironment = _hostingEnvironment;
        }
        [HttpGet]
        [DisplayName("Operational")]
        public async Task<ActionResult> Index()
        {
            ManualsViewModel _manualsViewModel = new ManualsViewModel();
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            _manualsViewModel.books = _book.GetList().Result.Cast<Book>();
            _manualsViewModel.bookChapters = _bookChapter.GetList().Result.Cast<BookChapter>();
            _manualsViewModel.bookChapterSections = _bookChapterSection.GetList().Result.Cast<BookChapterSection>();
            return View(_manualsViewModel);
        }
        [HttpGet]
        public async Task<PartialViewResult> View(long id)
        {
            List<ManualsViewModel> ManualsViewModel = new List<ManualsViewModel>();

            if (id != 0)
            {
                _bookChapterSection.Id = id;
                var bookSection = _bookChapterSection.Refresh().Result as BookChapterSection;
                var allBookSectionVirsions = _bookChapterSection.GetListOfVirsion(bookSection).Result.Cast<BookChapterSection>();
                //foreach(var booksection in allBookSectionVirsions)
                //{
                //    ManualsViewModel manuals = new ManualsViewModel
                //    {
                //        BookChapterId = bookSection.BookChapterID,
                //        BookChapterNumber = bookSection.BookChapters.BookChapterNumber,
                //        BookChapterTitle = bookSection.BookChapters.BookChapterTitle,
                //        BookSectionTitle = bookSection.BookSectionTitle,
                //        BookId = bookSection.BookChapters.Book.Id,
                //        BookTitle = bookSection.BookChapters.Book.BookTitle,
                //        BookSectionNumber = bookSection.BookSectionNumber,
                //        BookChapterSectionId = bookSection.Id,
                //        SequenceNo = bookSection.BookChapters.Book.SequenceNo,
                //        sectionUrl = bookSection.BookUrl,
                //        HasUpdate = bookSection.HasUpdate,
                //        VersionNumber = bookSection.VersionNumber
                //    };
                //    ManualsViewModel.Add(manuals);
                //}
                
                return PartialView(allBookSectionVirsions);
            }
            else
                ViewBag.Message = "Bad Request";
            return PartialView();
        }
        // GET: Operational/Manuals/Create
        public PartialViewResult Create()
        {
            ViewBag.BookId = new SelectList(_context.Book, "Id", "BookTitle");
            ViewBag.BookChapterId = new SelectList(_context.BookChapter, "Id", "BookChapterTitle");
            return PartialView();
        }

        public bool IsValidPdf(string fileName)
        {
            try
            {
                new PdfReader(fileName);
                return true;
            }
            catch (InvalidPdfException)
            {
                return false;
            }
        }
        // POST: Operational/Manuals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(ManualsViewModel manual, IFormCollection formCollection)
        {
            var saveBookSectons = new List<BookChapterSection>();
            DatabaseOperationResponse result = new DatabaseOperationResponse();
            ManualsViewModel manualsViewModel = new ManualsViewModel();
            Book findBook = new Book();
            BookChapter findBookChapter = new BookChapter();
            string filePaths = $" https://b2cappdevstorageaccount.blob.core.windows.net/kaizala-manuals";
            string[] splitString = { "", "" };
            string[] separatedString = { };
            IFormFile file;
            if (Request.Form.Files["url[]"] != null)
                file = Request.Form.Files["url[]"];
            else
                file = null;
            CancellationToken cancellationToken;
            if (ModelState.IsValid)
            {
                _book.BookTitle = manual.BookTitle;
                _book.SequenceNo = manual.SequenceNo;
                _book.CreatedBy = User.Identity.Name;
                _bookChapter.BookChapterNumber = manual.BookChapterNumber;
                _bookChapter.BookChapterTitle = manual.BookChapterTitle;
                _bookChapter.CreatedBy = User.Identity.Name;
                _bookChapterSection.BookSectionTitle = manual.BookSectionTitle;
                _bookChapterSection.BookSectionNumber = manual.BookSectionNumber;
                var Url = manual.sectionUrl;
                IFormFile bookFile = Request.Form.Files["url"];

                var boolExist = await _book.Exist();
                if (!boolExist)
                {
                    result = await _book.Save() as DatabaseOperationResponse;
                }
                if (boolExist)
                {
                    findBook = await _book.GetBook();
                    _bookChapter.BookId = findBook.Id;
                }
                else
                    _bookChapter.BookId = _book.Id;
                var boocChapterExist = await _bookChapter.Exist();
                if (!boocChapterExist)
                {
                    result = await _bookChapter.Save() as DatabaseOperationResponse;
                }
                if (boocChapterExist)
                {
                    findBookChapter = await _bookChapter.GetBookChapter();
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        file = Request.Form.Files[i];
                        //do your save etc here
                        if (file != null)
                            _bookChapterSection.BookUrl = await UploadFile(manual, file, cancellationToken);
                        _bookChapterSection.BookChapterID = findBookChapter.Id;
                        _bookChapterSection.BookSectionNumber = file.FileName.ToString().Substring(0, 7);
                        splitString[0] = _bookChapterSection.BookSectionNumber;
                        splitString[1] = _bookChapterSection.BookSectionNumber;
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        separatedString = fileName.ToString().Split(_bookChapterSection.BookSectionNumber, 2, StringSplitOptions.RemoveEmptyEntries);
                        _bookChapterSection.BookSectionTitle = separatedString[0];
                        _bookChapterSection.BookChapterID = _bookChapter.Id;
                        _bookChapterSection.CreatedBy = User.Identity.Name;
                        _bookChapterSection.HasUpdate = false;
                        _bookChapterSection.VersionNumber = 0;
                        var bookSection = await _bookChapterSection.Exist();
                        if (!bookSection)
                        {
                            saveBookSectons.Add(new BookChapterSection(_context)
                            {
                                BookChapterID = _bookChapter.Id,
                                BookSectionNumber = file.FileName.ToString().Substring(0, 7),
                                BookSectionTitle = separatedString[0],
                                CreatedBy = User.Identity.Name,
                                HasUpdate = false,
                                VersionNumber = 0,
                                BookUrl = _bookChapterSection.BookUrl
                            });

                            //_bookChapterSection.Id = 0;
                            //result = await _bookChapterSection.Save() as DatabaseOperationResponse;
                            //_bookChapterSection.GroupId = _bookChapterSection.Id;
                            //var update = await _bookChapterSection.Update() as DatabaseOperationResponse;
                        }
                    }
                    _context.BookChapterSection.AddRange(saveBookSectons);
                    if (_context.SaveChanges() > 0)
                        foreach (var section in saveBookSectons)
                        {
                            section.GroupId = section.Id;
                            var update = await section.Update() as DatabaseOperationResponse;
                        }
                    return new JsonResult(new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Saved"
                    });
                }
                else
                {

                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        file = Request.Form.Files[i];
                        if (file != null)
                            _bookChapterSection.BookUrl = await UploadFile(manual, file, cancellationToken);

                        _bookChapterSection.BookSectionNumber = file.FileName.ToString().Substring(0, 7);
                        splitString[0] = _bookChapterSection.BookSectionNumber;
                        splitString[1] = _bookChapterSection.BookSectionNumber;
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        separatedString = fileName.ToString().Split(_bookChapterSection.BookSectionNumber, 2, StringSplitOptions.RemoveEmptyEntries);
                        _bookChapterSection.BookSectionTitle = separatedString[0];
                        _bookChapterSection.BookChapterID = _bookChapter.Id;
                        _bookChapterSection.CreatedBy = User.Identity.Name;
                        _bookChapterSection.HasUpdate = false;
                        _bookChapterSection.VersionNumber = 0;
                        var bookSection = await _bookChapterSection.Exist();
                        if (!bookSection)
                        {
                            saveBookSectons.Add(new BookChapterSection(_context)
                            {
                                BookChapterID = _bookChapter.Id,
                                BookSectionNumber = file.FileName.ToString().Substring(0, 7),
                                BookSectionTitle = separatedString[0],
                                CreatedBy = User.Identity.Name,
                                HasUpdate = false,
                                VersionNumber = 0,
                                BookUrl = _bookChapterSection.BookUrl
                            });

                            //_bookChapterSection.Id = 0;
                            //result = await _bookChapterSection.Save() as DatabaseOperationResponse;
                            //_bookChapterSection.GroupId = _bookChapterSection.Id;
                            //var update = await _bookChapterSection.Update() as DatabaseOperationResponse;
                        }
                    }
                    _context.BookChapterSection.AddRange(saveBookSectons);
                    if (_context.SaveChanges() > 0)
                        foreach (var section in saveBookSectons)
                        {
                            section.GroupId = section.Id;
                            var update = await section.Update() as DatabaseOperationResponse;
                        }
                    return new JsonResult(new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Saved"
                    });
                }
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }

        public async Task<string> UploadFile(ManualsViewModel manual, IFormFile file, CancellationToken cancellationToken)
        {
            _book.BookTitle = manual.BookTitle;
            _book.SequenceNo = manual.SequenceNo;
            _book.CreatedBy = User.Identity.Name;
            _bookChapter.BookChapterNumber = manual.BookChapterNumber;
            _bookChapter.BookChapterTitle = manual.BookChapterTitle;
            _bookChapter.CreatedBy = User.Identity.Name;
            _bookChapterSection.BookSectionTitle = manual.BookSectionTitle;
            _bookChapterSection.BookSectionNumber = manual.BookSectionNumber;
            var Url = manual.sectionUrl;
            
            if (file != null)
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    await file.CopyToAsync(mStream, cancellationToken);
                    Document myDocument = new Document();
                    PdfWriter myPDFWriter = PdfWriter.GetInstance(myDocument, mStream);
                    if (!Directory.Exists(hostingEnvironment.WebRootPath + @"\Manuals\"))
                        Directory.CreateDirectory(hostingEnvironment.WebRootPath + @"\Manuals\");
                    if (!Directory.Exists(hostingEnvironment.WebRootPath + @"\Manuals\" + @"\Book\"))
                        Directory.CreateDirectory(hostingEnvironment.WebRootPath + @"\Manuals\" + @"\Book\");
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
                        if (!Directory.Exists(hostingEnvironment.WebRootPath + $"\\Manuals\\Book\\{_book.BookTitle.Replace(" ", "_").Trim()}\\{_bookChapter.BookChapterTitle.Replace(" ", "_").Trim()}\\"))
                            Directory.CreateDirectory(hostingEnvironment.WebRootPath + $"\\Manuals\\Book\\{_book.BookTitle.Replace(" ", "_").Trim()}\\{_bookChapter.BookChapterTitle.Replace(" ", "_").Trim()}\\");
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, $"Manuals\\Book\\{_book.BookTitle.Replace(" ", "_").Trim()}\\{_bookChapter.BookChapterTitle.Replace(" ", "_").Trim()}\\");
                        var firlname = file.FileName.ToLower().Trim().Replace("&", "and");
                        var fileName = firlname.ToLower().Trim().Replace(" ", "_");
                        var uploadFileName = "manuals/books/" + $"{_book.BookTitle.ToLower().Replace(" ", "_").Trim()}/{_bookChapter.BookChapterTitle.ToLower().Replace(" ", "_").Trim()}/" + firlname.ToLower().Trim().Replace(" ", "_");
                        Document document = new Document();
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
                                _bookChapterSection.BookUrl = cblob.Uri.AbsoluteUri;
                                if (_bookChapterSection.BookUrl.Contains("&"))
                                {
                                    _bookChapterSection.BookUrl = Regex.Replace(_bookChapterSection.BookUrl, @"&", "%26");
                                }
                                if(_bookChapterSection.BookSectionTitle != null)
                                    if (_bookChapterSection.BookSectionTitle.Contains("&"))
                                    {
                                        _bookChapterSection.BookSectionTitle = Regex.Replace(_bookChapterSection.BookSectionTitle, @"&", "and");
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
            return _bookChapterSection.BookUrl;
        }

        // GET: Operational/Manuals/Edit/5
        public async Task<PartialViewResult> Edit(long id)
        {
            
            if (id != 0)
            {
                _bookChapterSection.Id = id;
                var bookSection = _bookChapterSection.Refresh().Result as BookChapterSection;
                ManualsViewModel manuals = new ManualsViewModel { 
                BookChapterId = bookSection.BookChapterID,
                BookChapterNumber = bookSection.BookChapters.BookChapterNumber,
                BookChapterTitle = bookSection.BookChapters.BookChapterTitle,
                BookSectionTitle = bookSection.BookSectionTitle,
                BookId = bookSection.BookChapters.Book.Id,
                BookTitle = bookSection.BookChapters.Book.BookTitle,
                BookSectionNumber = bookSection.BookSectionNumber,
                BookChapterSectionId = bookSection.Id,
                SequenceNo = bookSection.BookChapters.Book.SequenceNo,
                sectionUrl = bookSection.BookUrl,
                HasUpdate = bookSection.HasUpdate,
                VersionNumber = bookSection.VersionNumber,
                GroupId = bookSection.GroupId
                };
                return PartialView(manuals);
            }
            else
                ViewBag.Message = "Bad Request";
            return PartialView();
        }

        // POST: Operational/Manuals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(long id, ManualsViewModel manual, IFormCollection formCollection)
        {
            ManualsViewModel manualsViewModel = new ManualsViewModel();
            Book findBook = new Book();
            BookChapter findBookChapter = new BookChapter();
            var response = new DatabaseOperationResponse();
            string filePaths = $" https://b2cappdevstorageaccount.blob.core.windows.net/kaizala-manuals";
            IFormFile file;
            if (Request.Form.Files["url"] != null)
                file = Request.Form.Files["url"];
            else
                file = null;
            CancellationToken cancellationToken;
            if (ModelState.IsValid)
            {
                _book.Id = manual.BookId;
                _book.BookTitle = manual.BookTitle;
                _book.SequenceNo = manual.SequenceNo;
                _book.RevisedBy = User.Identity.Name;
                _bookChapter.Id = manual.BookChapterId;
                _bookChapter.BookChapterNumber = manual.BookChapterNumber;
                _bookChapter.BookChapterTitle = manual.BookChapterTitle;
                _bookChapter.RevisedBy = User.Identity.Name;
                _bookChapterSection.Id = manual.BookChapterSectionId;
                _bookChapterSection.BookSectionTitle = manual.BookSectionTitle;
                _bookChapterSection.BookSectionNumber = manual.BookSectionNumber;
                _bookChapterSection.HasUpdate = manual.HasUpdate;
                _bookChapterSection.VersionNumber = manual.VersionNumber;
                _bookChapterSection.GroupId = manual.GroupId;
                var Url = manual.sectionUrl;
                var boolExist = await _book.Exist();
                if (boolExist)
                {
                    //response = await _book.Update() as DatabaseOperationResponse;
                    _bookChapter.BookId = _book.Id;
                    var boocChapterExist = await _bookChapter.Exist();
                    if (boocChapterExist)
                    {
                        //response = await _bookChapter.Update() as DatabaseOperationResponse;
                        _bookChapterSection.BookChapterID = _bookChapter.Id;
                        if (file != null)
                            _bookChapterSection.BookUrl = await UploadFile(manual, file, cancellationToken);

                        _bookChapterSection.BookChapterID = _bookChapter.Id;
                        _bookChapterSection.Id = manual.BookChapterSectionId;
                        _bookChapterSection.BookSectionTitle = manual.BookSectionTitle;
                        _bookChapterSection.BookSectionNumber = manual.BookSectionNumber;
                        _bookChapterSection.VersionNumber = manual.VersionNumber;
                        _bookChapterSection.GroupId = manual.GroupId;
                        var newSection = new BookChapterSection(_context)
                        {
                            BookChapterID = _bookChapterSection.BookChapterID,
                            BookSectionNumber = _bookChapterSection.BookSectionNumber,
                            BookUrl = _bookChapterSection.BookUrl,
                            RevisedBy = User.Identity.Name,
                            CreatedBy = User.Identity.Name,
                            HasUpdate = true,
                            VersionNumber = _bookChapterSection.VersionNumber + 1,
                            BookSectionTitle = _bookChapterSection.BookSectionTitle,
                            GroupId = _bookChapterSection.GroupId
                        };
                        var addNewSection = await newSection.Save() as DatabaseOperationResponse;
                        //newSection.GroupId = newSection.Id;
                        response = await newSection.Update() as DatabaseOperationResponse;
                        var result = await _bookChapterSection.UpdateOldSection() as DatabaseOperationResponse;
                        return new JsonResult(result);
                    }
                }
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
        // GET: Operational/Manuals/Edit/5
        public async Task<PartialViewResult> Rename(long id)
        {

            if (id != 0)
            {
                _bookChapterSection.Id = id;
                var bookSection = _bookChapterSection.Refresh().Result as BookChapterSection;
                ManualsViewModel manuals = new ManualsViewModel
                {
                    BookChapterId = bookSection.BookChapterID,
                    BookChapterNumber = bookSection.BookChapters.BookChapterNumber,
                    BookChapterTitle = bookSection.BookChapters.BookChapterTitle,
                    BookSectionTitle = bookSection.BookSectionTitle,
                    BookId = bookSection.BookChapters.Book.Id,
                    BookTitle = bookSection.BookChapters.Book.BookTitle,
                    BookSectionNumber = bookSection.BookSectionNumber,
                    BookChapterSectionId = bookSection.Id,
                    SequenceNo = bookSection.BookChapters.Book.SequenceNo,
                    sectionUrl = bookSection.BookUrl,
                    HasUpdate = bookSection.HasUpdate,
                    VersionNumber = bookSection.VersionNumber,
                    GroupId = bookSection.GroupId
                };
                return PartialView(manuals);
            }
            else
                ViewBag.Message = "Bad Request";
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Rename(long id, ManualsViewModel manual, IFormCollection formCollection)
        {
            ManualsViewModel manualsViewModel = new ManualsViewModel();
            Book findBook = new Book();
            BookChapter findBookChapter = new BookChapter();
            var response = new DatabaseOperationResponse();
            if (ModelState.IsValid)
            {
                _book.Id = manual.BookId;
                _book.BookTitle = manual.BookTitle;
                _book.SequenceNo = manual.SequenceNo;
                //_book.CreatedBy = User.Identity.Name;
                _book.RevisedBy = User.Identity.Name;
                _bookChapter.Id = manual.BookChapterId;
                _bookChapter.BookChapterNumber = manual.BookChapterNumber;
                _bookChapter.BookChapterTitle = manual.BookChapterTitle;
                _bookChapter.BookId = manual.BookId;
                _bookChapter.RevisedBy = User.Identity.Name;
                _bookChapterSection.Id = manual.BookChapterSectionId;
                _bookChapterSection.BookSectionTitle = manual.BookSectionTitle;
                _bookChapterSection.BookSectionNumber = manual.BookSectionNumber;
                var Url = manual.sectionUrl;
                var boolExist = await _book.Exist();
                if (boolExist)
                {
                    var boocChapterExist = await _bookChapter.Exist();
                    if (boocChapterExist)
                    {
                        //response = await _bookChapter.Update() as DatabaseOperationResponse;
                        _bookChapterSection.BookChapterID = _bookChapter.Id;
                        //if (file != null)
                        //    _bookChapterSection.BookUrl = await UploadFile(manual, file, cancellationToken);
                        _bookChapterSection.BookUrl = manual.sectionUrl;
                        _bookChapterSection.BookChapterID = _bookChapter.Id;
                        _bookChapterSection.Id = manual.BookChapterSectionId;
                        _bookChapterSection.BookSectionTitle = manual.BookSectionTitle;
                        _bookChapterSection.BookSectionNumber = manual.BookSectionNumber;
                        _bookChapterSection.HasUpdate = false;
                        _bookChapterSection.VersionNumber = 0;
                        _bookChapterSection.RevisedBy = User.Identity.Name;
                        _bookChapterSection.GroupId = manual.GroupId;
                        var result = await _bookChapterSection.Update() as DatabaseOperationResponse;
                        return new JsonResult(result);
                    }
                }
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
        // GET: Operational/Manuals/Delete/5
        public async Task<JsonResult> Delete(long id)
        {
            if (id != 0)
            {
                var bookSection = await _context.BookChapterSection.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                _bookChapterSection.Id = bookSection.Id;
                _bookChapterSection.BookSectionNumber = bookSection.BookSectionNumber;
                _bookChapterSection.BookSectionTitle = bookSection.BookSectionTitle;
                _bookChapterSection.BookChapterID = bookSection.BookChapterID;
                var result = await _bookChapterSection.Delete() as DatabaseOperationResponse;
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }
        // GET: Operational/Manuals/DeleteBook/5
        public async Task<JsonResult> DeleteBook(long id)
        {
            var chapterIds = new List<long>();
            chapterIds = null;
            if (id != 0)
            {
                var book = await _context.Book.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                var bookChapters = await _context.BookChapter.Where(con => con.BookId == id).AsNoTracking().ToListAsync();
                if(bookChapters.Count > 0)
                    chapterIds = bookChapters.Select(con => con.Id).ToList();
                _book.Id = book.Id;
                _book.BookTitle = book.BookTitle;
                _book.SequenceNo = book.SequenceNo;
                var result = await _book.Delete() as DatabaseOperationResponse;
                if(chapterIds != null)
                {
                    var bookSections = await _context.BookChapterSection.Where(con => chapterIds.Contains(con.BookChapterID)).AsNoTracking().ToListAsync();
                    foreach (var chapter in bookChapters)
                    {
                        _bookChapter.Id = chapter.Id;
                        _bookChapter.BookChapterTitle = chapter.BookChapterTitle;
                        _bookChapter.BookChapterNumber = chapter.BookChapterNumber;
                        _bookChapter.BookId = chapter.BookId;
                        var resultChapter = await _bookChapter.Delete() as DatabaseOperationResponse;
                    }
                    foreach(var section in bookSections)
                    {
                        _bookChapterSection.Id = section.Id;
                        _bookChapterSection.BookSectionTitle = section.BookSectionTitle;
                        _bookChapterSection.BookSectionNumber = section.BookSectionNumber;
                        _bookChapterSection.BookChapterID = section.BookChapterID;
                        var resultChapter = await _bookChapterSection.Delete() as DatabaseOperationResponse;
                    }
                }
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }
        // GET: Operational/Manuals/DeleteChapter/5
        public async Task<JsonResult> DeleteChapter(long id)
        {
            if (id != 0)
            {
                var bookchapter = await _context.BookChapter.AsNoTracking().FirstOrDefaultAsync(con => con.Id == id);
                var bookSections = await _context.BookChapterSection.Where(con => con.BookChapterID == id).AsNoTracking().ToListAsync();
                _bookChapter.Id = bookchapter.Id;
                _bookChapter.BookChapterTitle = bookchapter.BookChapterTitle;
                _bookChapter.BookChapterNumber = bookchapter.BookChapterNumber;
                _bookChapter.BookId = bookchapter.BookId;
                var result = await _bookChapter.Delete() as DatabaseOperationResponse;
                if(bookSections.Count > 0)
                {
                    foreach (var section in bookSections)
                    {
                        _bookChapterSection.Id = section.Id;
                        _bookChapterSection.BookSectionTitle = section.BookSectionTitle;
                        _bookChapterSection.BookSectionNumber = section.BookSectionNumber;
                        _bookChapterSection.BookChapterID = section.BookChapterID;
                        var resultChapter = await _bookChapterSection.Delete() as DatabaseOperationResponse;
                    }
                }
                return new JsonResult(result);
            }
            return new JsonResult(null);
        }

        // GET: Operational/Manuals/CreateBook
        public PartialViewResult CreateBook()
        {
            ViewBag.BookId = new SelectList(_context.Book, "Id", "BookTitle");
            ViewBag.BookChapterId = new SelectList(_context.BookChapter, "Id", "BookChapterTitle");
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateBook(ManualsViewModel manual, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _book.BookTitle = manual.BookTitle;
                _book.SequenceNo = manual.SequenceNo;
                _book.CreatedBy = User.Identity.Name;
                var boolExist = await _book.Exist();
                if (!boolExist)
                {
                    var result = await _book.Save() as DatabaseOperationResponse;
                    return new JsonResult(result);
                }
                return new JsonResult(new DatabaseOperationResponse
                {
                    Status = OperationStatus.NOT_OK,
                    Message = "Record Already Exists"
                });
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }

        // GET: Operational/Manuals/CreateChapter
        public PartialViewResult CreateChapter()
        {
            ViewBag.BookId = new SelectList(_context.Book, "Id", "BookTitle");
            ViewBag.BookChapterId = new SelectList(_context.BookChapter, "Id", "BookChapterTitle");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateChapter(ManualsViewModel manual, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _bookChapter.BookId = manual.BookId;
                _bookChapter.BookChapterTitle = manual.BookChapterTitle;
                _bookChapter.BookChapterNumber = manual.BookChapterNumber;
                _bookChapter.CreatedBy = User.Identity.Name;
                var boolChapterExist = await _bookChapter.Exist();
                if (!boolChapterExist)
                {
                    var result = await _bookChapter.Save() as DatabaseOperationResponse;
                    return new JsonResult(result);
                }
                return new JsonResult(new DatabaseOperationResponse
                {
                    Status = OperationStatus.NOT_OK,
                    Message = "Record Already Exists"
                });
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
        // GET: Operational/Manuals/AddChapter
        public PartialViewResult AddChapter(long id)
        {
            ViewBag.BookId = id;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddChapter(long id, BookChapter model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                //_bookChapter.Id = model.Id;
                _bookChapter.BookId = model.BookId;
                _bookChapter.BookChapterTitle = model.BookChapterTitle;
                _bookChapter.BookChapterNumber = model.BookChapterNumber;
                _bookChapter.RevisedBy = User.Identity.Name;
                //_bookChapter.CreationDate = model.CreationDate;
                _bookChapter.CreatedBy = model.CreatedBy;
                var boocChapterExist = await _bookChapter.Exist();
                if (!boocChapterExist)
                {
                    var result = await _bookChapter.Save() as DatabaseOperationResponse;
                    return new JsonResult(result);
                }
                return new JsonResult(new DatabaseOperationResponse
                {
                    Status = OperationStatus.NOT_OK,
                    Message = "Record Already Exists"
                });
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }

        // GET: Operational/Manuals/AddSection
        public PartialViewResult AddSection(long id)
        {
            ViewBag.BookChapterId = id;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddSection(long id, BookChapterSection model, IFormCollection formCollection)
        {
            var saveBookSectons = new List<BookChapterSection>();
            var result = new DatabaseOperationResponse();
            ManualsViewModel manual = new ManualsViewModel();
            string filePaths = $" https://b2cappdevstorageaccount.blob.core.windows.net/kaizala-manuals";
            string[] splitString = { "", "" };
            string[] separatedString = { };
            IFormFile file;
            if (Request.Form.Files["url[]"] != null)
                file = Request.Form.Files["url[]"];
            else
                file = null;
            CancellationToken cancellationToken;
            if (ModelState.IsValid)
            {
                _bookChapter.Id = model.BookChapterID;
                var bookChapter = _bookChapter.Refresh().Result as BookChapter;
                _book.Id = bookChapter.BookId;
                var book = _book.Refresh().Result as Book;
                manual.BookChapterTitle = bookChapter.BookChapterTitle;
                manual.BookTitle = book.BookTitle;
                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    file = Request.Form.Files[i];
                    //do your save etc here
                    if (file != null)
                        _bookChapterSection.BookUrl = await UploadFile(manual, file, cancellationToken);
            
                    _bookChapterSection.BookChapterID = model.BookChapterID;
                    _bookChapterSection.BookSectionNumber = file.FileName.ToString().Substring(0, 7);
                    splitString[0] = _bookChapterSection.BookSectionNumber;
                    splitString[1] = _bookChapterSection.BookSectionNumber;
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    separatedString = fileName.ToString().Split(_bookChapterSection.BookSectionNumber, 2, StringSplitOptions.RemoveEmptyEntries);
                    _bookChapterSection.BookSectionTitle = separatedString[0];
                    _bookChapterSection.BookChapterID = _bookChapter.Id;
                    _bookChapterSection.CreatedBy = User.Identity.Name;
                    _bookChapterSection.HasUpdate = false;
                    _bookChapterSection.VersionNumber = 0;
                    var bookSection = await _bookChapterSection.Exist();
                    if (!bookSection)
                    {
                        saveBookSectons.Add(new BookChapterSection(_context)
                        {
                            BookChapterID = _bookChapter.Id,
                            BookSectionNumber = file.FileName.ToString().Substring(0, 7),
                            BookSectionTitle = separatedString[0],
                            CreatedBy = User.Identity.Name,
                            HasUpdate = false,
                            VersionNumber = 0,
                            BookUrl = _bookChapterSection.BookUrl
                        });


                        //result = await _bookChapterSection.Save() as DatabaseOperationResponse;
                        //_bookChapterSection.GroupId = _bookChapterSection.Id;
                        //var update = await _bookChapterSection.Update() as DatabaseOperationResponse;
                    }
                }
                _context.BookChapterSection.AddRange(saveBookSectons);
                if (_context.SaveChanges() > 0)
                    foreach (var section in saveBookSectons)
                    {
                        section.GroupId = section.Id;
                        var update = await section.Update() as DatabaseOperationResponse;
                    }
                return new JsonResult(new DatabaseOperationResponse
                {
                    Status = OperationStatus.SUCCESS,
                    Message = "Record Sucessfully Saved"
                });
                //if (file != null)
                //    _bookChapterSection.BookUrl = await UploadFile(manual, file, cancellationToken);
                //_bookChapterSection.BookChapterID = model.BookChapterID;
                //_bookChapterSection.BookSectionTitle = model.BookSectionTitle;
                //_bookChapterSection.BookSectionNumber = model.BookSectionNumber;
                //_bookChapterSection.RevisedBy = User.Identity.Name;
                //_bookChapterSection.CreationDate = model.CreationDate;
                //_bookChapterSection.CreatedBy = model.CreatedBy;
                //_bookChapterSection.VersionNumber = 0;
                //_bookChapterSection.HasUpdate = false;
                //var boocSectionExist = await _bookChapterSection.Exist();
                //if (!boocSectionExist)
                //{
                //     result = await _bookChapterSection.Save() as DatabaseOperationResponse;
                //    _bookChapterSection.GroupId = _bookChapterSection.Id;
                //    var response = await _bookChapterSection.Update() as DatabaseOperationResponse;
                //    return new JsonResult(result);
                //}
                //return new JsonResult(new DatabaseOperationResponse
                //{
                //    Status = OperationStatus.NOT_OK,
                //    Message = "Record Already Exists"
                //});
            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }

        // GET: Operational/Manuals/EditBook/5
        public async Task<PartialViewResult> EditBook(long id)
        {

            if (id != 0)
            {
                _book.Id = id;
                var book = _book.Refresh().Result as Book;
                return PartialView(book);
            }
            else
                ViewBag.Message = "Bad Request";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditBook(long id, Book model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _book.Id = model.Id;
                _book.BookTitle = model.BookTitle;
                _book.SequenceNo = model.SequenceNo;
                _book.RevisedBy = User.Identity.Name;
                _book.CreationDate = model.CreationDate;
                _book.CreatedBy = model.CreatedBy;

                var result = await _book.Update() as DatabaseOperationResponse;
                return new JsonResult(result);

            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }

        // GET: Operational/Manuals/EditChapter/5
        public async Task<PartialViewResult> EditChapter(long id)
        {
            if (id != 0)
            {
                _bookChapter.Id = id;
                var bookChapter = _bookChapter.Refresh().Result as BookChapter;
                return PartialView(bookChapter);
            }
            else
                ViewBag.Message = "Bad Request";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditChapter(long id, BookChapter model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                _bookChapter.Id = model.Id;
                _bookChapter.BookId = model.BookId;
                _bookChapter.BookChapterTitle = model.BookChapterTitle;
                _bookChapter.BookChapterNumber = model.BookChapterNumber;
                _bookChapter.RevisedBy = User.Identity.Name;
                _bookChapter.CreationDate = model.CreationDate;
                _bookChapter.CreatedBy = model.CreatedBy;


                var result = await _bookChapter.Update() as DatabaseOperationResponse;
                return new JsonResult(result);

            }
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }

        [HttpGet]
        [DisplayName("Operational")]
        public async Task<IActionResult> AccessLog()
        {
            var books= _book.GetList().Result.Cast<Book>();
            ViewBag.Manuals = new SelectList(books, "Id", "BookTitle");
            return View(_manualAccessLog.GetList().Result.Cast<ManualAccessLog>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccessLog(AccessLogSearchFileds accessLogSearchFileds)
        {
            var books = _book.GetList().Result.Cast<Book>();
            ViewBag.Manuals = new SelectList(books, "Id", "BookTitle");
            if (accessLogSearchFileds.EmployeeId != null || accessLogSearchFileds.BookId != null)
            {
                if(accessLogSearchFileds.EmployeeId != null)
                {
                    string userName = accessLogSearchFileds.EmployeeId.Trim();
                    string appendableDigit = "";
                    for (int i = 0; i < (8 - userName.Length); i++)
                        appendableDigit += "0";

                    accessLogSearchFileds.EmployeeId = appendableDigit + accessLogSearchFileds.EmployeeId.Trim();
                }
                return View(_manualAccessLog.GetList(accessLogSearchFileds).Result.Cast<ManualAccessLog>());
            }
            return View(_manualAccessLog.GetList().Result.Cast<ManualAccessLog>());
        }
    }
}