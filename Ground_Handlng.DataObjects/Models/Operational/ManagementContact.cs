using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ground_Handlng.DataObjects.Models.Operational
{
    [Table("ManagementContact")]
    public class ManagementContact : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Working Area")]
        public string WorkingArea { get; set; }
        public string PositionandWorkingArea
        {
            get
            {
                return string.Format("{0}{1}", this.Position, this.WorkingArea);
            }
        }


        ApplicationDbContext _context;

        public ManagementContact(ApplicationDbContext context)
        {
            _context = context;
        }

        public ManagementContact()
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
                _context.ManagementContacts.Add(this);
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
                _context.ManagementContacts.Attach(this);
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
                _context.ManagementContacts.Attach(this);
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
                var fdrMMRMasters = await Task.Run(() => _context.ManagementContacts.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
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
                return await _context.ManagementContacts.Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
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
                var fdrMMR = await _context.ManagementContacts.FirstOrDefaultAsync(con => con.PhoneNumber == PhoneNumber && con.Status == RecordStatus.Active);
                return fdrMMR == null ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
