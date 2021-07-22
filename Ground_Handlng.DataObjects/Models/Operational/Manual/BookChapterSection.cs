using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ground_Handlng.DataObjects.Models.Operational.Manual
{
    [Table("BookSections")]
    public class BookChapterSection : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Display(Name = "Section Number")]
        public string BookSectionNumber { get; set; }

        [Display(Name = "Section Title")]
        public string BookSectionTitle { get; set; }

        [Display(Name = "Book URL")]
        public string BookUrl { get; set; }
        public bool HasUpdate { get; set; }
        public int VersionNumber { get; set; }
        [ForeignKey("BookChapters")]
        [Required]
        public long BookChapterID { get; set; }

        [ForeignKey("BookChapterSections")]
        public long? GroupId { get; set; }

        public virtual BookChapter BookChapters { get; set; }

        public virtual BookChapterSection BookChapterSections { get; set; }

        ApplicationDbContext _context;

        public BookChapterSection(ApplicationDbContext context)
        {
            _context = context;
        }

        public BookChapterSection()
        {

        }

        public async Task<object> Save()
        {
            try
            {
                StartDate = DateTime.Now;
                EndDate = DateTime.MaxValue;
                CreationDate = DateTime.Now;
                RevisionDate = DateTime.MinValue;
                Status = RecordStatus.Active;
                _context.BookChapterSection.Add(this);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Created",
                    };
                }

                return new DatabaseOperationResponse
                {

                    Message = "Operation Was Not Sucessful",
                    Status = OperationStatus.Ok
                };

            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error Occured While Creating Record",
                    Status = OperationStatus.ERROR
                };
            }
        }

        public async Task<object> Update()
        {
            try
            {
                this.RevisionDate = DateTime.Now;
                this.Status = RecordStatus.Active;
                _context.BookChapterSection.Attach(this);
                _context.Entry(this).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Updated",
                    };
                }
                else
                {
                    return new DatabaseOperationResponse
                    {
                        Message = "Operation Was Not Sucessful",
                        Status = OperationStatus.Ok
                    };
                }
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error Occured While Updating Record",
                    Status = OperationStatus.ERROR
                };
            }
        }

        public async Task<object> UpdateOldSection()
        {
            try
            {
                this.RevisionDate = DateTime.Now;
                this.Status = RecordStatus.Inactive;
                _context.BookChapterSection.Attach(this);
                _context.Entry(this).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Updated",
                    };
                }
                else
                {
                    return new DatabaseOperationResponse
                    {
                        Message = "Operation Was Not Sucessful",
                        Status = OperationStatus.Ok
                    };
                }
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error Occured While Updating Record",
                    Status = OperationStatus.ERROR
                };
            }
        }

        public async Task<object> Delete()
        {
            try
            {
                this.Status = RecordStatus.Deleted;
                this.RevisionDate = DateTime.Now;
                EndDate = DateTime.Now;
                _context.BookChapterSection.Attach(this);
                _context.Entry(this).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Deleted",
                    };

                }
                else
                {
                    return new DatabaseOperationResponse
                    {
                        Message = "Operation Was Not Sucessful",
                        Status = OperationStatus.Ok
                    };
                }
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error Occured While Deleting Record",
                    Status = OperationStatus.ERROR
                };
            }
        }

        public async Task<List<object>> GetList()
        {
            try
            {
                var bookChapter = await Task.Run(() => _context.BookChapterSection.Include(con=>con.BookChapters.Book).Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
                return bookChapter;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<object> Refresh()
        {
            try
            {
                return await _context.BookChapterSection.Include(con=>con.BookChapters.Book).Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<object>> GetListOfVirsion(BookChapterSection bookChapterSection)
        {
            try
            {
                var bookChapter = await Task.Run(() => _context.BookChapterSection.Include(con => con.BookChapters.Book).Where(con => (con.Status == RecordStatus.Active || con.Status == RecordStatus.Inactive) &&  con.GroupId == bookChapterSection.GroupId ).Cast<object>().ToList());
                return bookChapter;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> Exist()
        {
            try
            {
                var bookChapter = await _context.BookChapterSection.FirstOrDefaultAsync(con => con.BookSectionTitle == BookSectionTitle && con.BookSectionNumber == BookSectionNumber && con.BookChapterID == BookChapterID && con.Status == RecordStatus.Active);
                return bookChapter == null ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
