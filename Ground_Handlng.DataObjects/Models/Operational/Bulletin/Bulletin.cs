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

namespace Ground_Handlng.DataObjects.Models.Operational.Bulletin
{
    [Table("Bulletin")]
    public class Bulletin : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Notice/Bulletin")]
        [Required(ErrorMessage = "Notice Or Bulletin is required")]
        public string NoticeOrBulletin { get; set; }

        [Required(ErrorMessage = "NoticeNumber is required")]
        [Display(Name = "*Notice/Bulletin Number")]
        public string NoticeNumber { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "NoticeTo to is required")]
        [Display(Name = "*To")]
        public string NoticeTo { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [Display(Name = ("*Subject"))]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Type is Required")]
        [Display(Name = ("Type"))]
        public string Type { get; set; }

        [Required(ErrorMessage = "Sent By   is required")]
        [Display(Name = "*Sent By")]
        public string SentBy { get; set; }

        [Display(Name = "URL")]
        public string BulletinUrl { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime ExpirationDate { get; set; }
        //[DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        ApplicationDbContext _context;

        public Bulletin(ApplicationDbContext context)
        {
            _context = context;
        }

        public Bulletin()
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
                _context.Bulletin.Add(this);
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
                _context.Bulletin.Attach(this);
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
                _context.Bulletin.Attach(this);
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
                var fdrMMRMasters = await Task.Run(() => _context.Bulletin.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
                return fdrMMRMasters;
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
                return await _context.Bulletin.Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
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
                var fdrMMR = await _context.Bulletin.FirstOrDefaultAsync(con => con.NoticeOrBulletin == NoticeOrBulletin && con.NoticeNumber == NoticeNumber && con.Status == RecordStatus.Active);
                return fdrMMR == null ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<Bulletin> GetBook()
        {
            return await _context.Bulletin.FirstOrDefaultAsync(con => con.NoticeOrBulletin == NoticeOrBulletin && con.NoticeNumber == NoticeNumber && con.Status == RecordStatus.Active);
        }
    }
}
