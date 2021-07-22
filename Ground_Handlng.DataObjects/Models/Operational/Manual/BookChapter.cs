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
    [Table("BookChapters")]
    public class BookChapter : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Chapter Number")]
        public string BookChapterNumber { get; set; }

        [Required]
        [Display(Name = "Chapter Title")]
        public string BookChapterTitle { get; set; }

        [ForeignKey("Book")]
        [Required]
        public long BookId { get; set; }
        public virtual Book Book { get; set; }

        ApplicationDbContext _context;

        public BookChapter(ApplicationDbContext context)
        {
            _context = context;
        }

        public BookChapter()
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
                _context.BookChapter.Add(this);
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
                _context.BookChapter.Attach(this);
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
                this.Status = RecordStatus.Inactive;
                this.RevisionDate = DateTime.Now;
                EndDate = DateTime.Now;
                _context.BookChapter.Attach(this);
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
                var bookChapter = await Task.Run(() => _context.BookChapter.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
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
                _context.Entry(this).State = EntityState.Detached;
                return await _context.BookChapter.Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Exist()
        {
            try
            {
                var bookChapter = await _context.BookChapter.FirstOrDefaultAsync(con => con.BookChapterTitle == BookChapterTitle && con.BookId == BookId && con.Status == RecordStatus.Active);
                return bookChapter == null ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<BookChapter> GetBookChapter()
        {
            return await _context.BookChapter.FirstOrDefaultAsync(con => con.BookChapterTitle == BookChapterTitle && con.BookId == BookId && con.Status == RecordStatus.Active);
        }
    }
}
