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
    [Table("ManualAccessLog")]
    public class ManualAccessLog : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public int UpdateVersionNumber { get; set; }
        public bool Downloaded { get; set; }
        public DateTime DownloadTime { get; set; }
        public DateTime SeenTime { get; set; }
        public DateTime SyncTime { get; set; }
        public bool Seen { get; set; }
        [ForeignKey("Books")]
        public long BookId { get; set; }
        [ForeignKey("BookChapters")]
        public long BookChapterId { get; set; }
        [ForeignKey("BookSections")]
        public long BookSectionId { get; set; }
        public virtual Book Books { get; set; }
        public virtual BookChapter BookChapters { get; set; }
        public virtual BookChapterSection BookSections { get; set; }

        ApplicationDbContext _context;

        public ManualAccessLog(ApplicationDbContext context)
        {
            _context = context;
        }

        public ManualAccessLog()
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
                _context.ManualAccessLog.Add(this);
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
                _context.ManualAccessLog.Attach(this);
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
                _context.ManualAccessLog.Attach(this);
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
                var fdrMMRMasters = await Task.Run(() => _context.ManualAccessLog.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
                return fdrMMRMasters;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<object>> GetList(AccessLogSearchFileds accessLogSearchFileds)
        {
            string query = "SELECT * FROM ManualAccessLog WHERE ";
            var manualAccessLog = new List<ManualAccessLog>();
            var mancualAcess = new List<object>();
            manualAccessLog = null;
            try
            {
                if (accessLogSearchFileds.EmployeeId != null)
                {
                    manualAccessLog = await Task.Run(() => _context.ManualAccessLog.Where(con => con.Status == RecordStatus.Active && con.EmployeeId == accessLogSearchFileds.EmployeeId).Cast<ManualAccessLog>().ToList());
                    mancualAcess = manualAccessLog.Cast<object>().ToList();
                }
                if(accessLogSearchFileds.BookId != null && accessLogSearchFileds.BookId != 0)
                {
                    var chapters =await _context.BookChapter.Where(con => con.BookId == accessLogSearchFileds.BookId).Select(c=>c.Id).ToListAsync();
                    if(manualAccessLog != null)
                    {
                        manualAccessLog = manualAccessLog.Where(con => chapters.Contains(con.BookChapterId)).ToList();
                        mancualAcess = manualAccessLog.Cast<object>().ToList();
                    }
                    else
                        mancualAcess = await Task.Run(() => _context.ManualAccessLog.Where(con => con.Status == RecordStatus.Active && chapters.Contains(con.BookChapterId)).Cast<object>().ToList());
                    //var section = await _context.BookChapterSection.Where(con => chapters.Contains(con.BookChapterID)).ToListAsync();
                }
                if(accessLogSearchFileds.EmployeeId == null && accessLogSearchFileds.BookId == null)
                    mancualAcess = await Task.Run(() => _context.ManualAccessLog.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());

                return mancualAcess;
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
                return await _context.ManualAccessLog.Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
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
                var fdrMMR = await _context.ManualAccessLog.FirstOrDefaultAsync(con => con.EmployeeId == EmployeeId && con.Status == RecordStatus.Active);
                return fdrMMR == null ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ManualAccessLog> GetBook()
        {
            return await _context.ManualAccessLog.FirstOrDefaultAsync(con => con.EmployeeId == EmployeeId && con.Status == RecordStatus.Active);
        }
    }
}
