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

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.WaiverFormForAcceptance
{
    public class WaiverFormForAcceptance : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Passanger Name")]
        public string FullName { get; set; }
        [Display(Name = "*Bag Tag Number")]
        public string BagTagNumber { get; set; }
        [Display(Name = "*Number Of PET/AVI")]
        public string NumberPETAVI { get; set; }
        [Display(Name = "*Type Of PET/AVI")]
        public string TypePETAVI { get; set; }
        [Display(Name = "*Date Of Travel")]
        public DateTime DateOfTravel { get; set; }
        [Display(Name = "Signature Of Passenger")]
        public string SignatureOfPassenger { get; set; }

        [NotMapped]
        ApplicationDbContext _context;

        public WaiverFormForAcceptance(ApplicationDbContext context)
        {
            _context = context;
        }

        public WaiverFormForAcceptance()
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
                _context.WaiverFormForAcceptance.Add(this);
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
                _context.WaiverFormForAcceptance.Attach(this);
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
                _context.WaiverFormForAcceptance.Attach(this);
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
        public async Task<bool> Exist()
        {
            try
            {
                var fdrMMR = await _context.WaiverFormForAcceptance.FirstOrDefaultAsync(con => con.FullName == FullName && con.BagTagNumber == BagTagNumber && con.Status == RecordStatus.Active);
                return fdrMMR == null ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<object>> GetList()
        {
            try
            {
                var waivers = await Task.Run(() => _context.WaiverFormForAcceptance.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
                return waivers;
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
                return await _context.WaiverFormForAcceptance.Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
