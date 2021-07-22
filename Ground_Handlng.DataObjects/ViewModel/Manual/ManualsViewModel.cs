using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational.Manual;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ground_Handlng.DataObjects.ViewModel.Manual
{
    public class ManualsViewModel : AuditLog
    {
        ApplicationDbContext dbContext;
        public ManualsViewModel()
        {

        }
        public ManualsViewModel(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext;
        }
        //Book
        public long BookId { get; set; }
        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }
        [Display(Name = "Sequence No.")]
        public int SequenceNo { get; set; }
        //Book Chapter
        public long BookChapterId { get; set; }
        [Display(Name = "Chapter Number")]
        public string BookChapterNumber { get; set; }

        [Display(Name = "Chapter Title")]
        public string BookChapterTitle { get; set; }
        //Section
        public long BookChapterSectionId { get; set; }
        [Display(Name = "Section Number")]
        public string BookSectionNumber { get; set; }
        [Display(Name = "Section Title")]
        public string BookSectionTitle { get; set; }

        public long? GroupId { get; set; }

        [Display(Name = "URL")]
        public string sectionUrl { get; set; }
        public bool HasUpdate { get; set; }
        public int VersionNumber { get; set; }
        public Book book { get; set; }
        public BookChapter bookChapter { get; set; }
        public BookChapterSection bookChapterSection { get; set; }
        public IEnumerable<Book> books { get; set; }
        public IEnumerable<BookChapter> bookChapters { get; set; }
        public IEnumerable<BookChapterSection> bookChapterSections { get; set; }

        public async Task<string> UploadSection(IFormFile BookUrl, BookChapterSection BookSection)
        {
            string filePath = string.Empty;
            string relativPath = string.Empty;
            //var request = HttpContextAccessor.HttpContext.Request;
            try
            {
                if (BookUrl != null)
                {
                    BookChapter capters = dbContext.BookChapter.Find(BookSection.BookChapterID);
                    Book books = dbContext.Book.Find(capters.BookId);

                    string bookName = books.BookTitle.Replace(" ", "_");
                    string chapName = capters.BookChapterTitle.Replace(" ", "_");
                    string path = $"~/Books/{bookName.Trim()}/{chapName.Trim()}";
                    relativPath = "/Books/" + bookName.Trim() + "/" + chapName.Trim();

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string DomainName = UriBuild.GetUri().ToString();
                    relativPath = DomainName + relativPath + "/" + BookUrl.FileName.Trim().Replace(" ", "_");
                    filePath = path + "/" + Path.GetFileName(BookUrl.FileName.Replace(" ", "_"));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await BookUrl.CopyToAsync(stream);
                    }
                    //BookUrl.CopyToAsync(filePath);
                }

                return relativPath;
            }
            catch (Exception ex)
            {
                return filePath;
            }
        }
    }

    public static class UriBuild
    {
        private static IHttpContextAccessor HttpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public static Uri GetUri()
        {
            var request = HttpContextAccessor.HttpContext.Request;
            var builder = new UriBuilder();
            builder.Scheme = request.Scheme;
            builder.Host = request.Host.Value;
            builder.Path = request.Path;
            builder.Query = request.QueryString.ToUriComponent();
            return builder.Uri;
        }
    }
}
