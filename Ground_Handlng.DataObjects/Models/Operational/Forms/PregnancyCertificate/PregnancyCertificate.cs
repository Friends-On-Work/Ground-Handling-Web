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

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PregnancyCertificate
{
    public class PregnancyCertificate : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Passanger Name")]
        public string FullName { get; set; }
        [Display(Name = "*Originatng Point")]
        public string From { get; set; }
        [Display(Name = "*Destined Point")]
        public string To { get; set; }
        [Display(Name = "*Date Of Examination")]
        public DateTime DateOfExamination { get; set; }
        [Display(Name = "*Date Of Travel")]
        public DateTime DateOfTravel { get; set; }
        [Display(Name = "Date Of Birth Estimated")]
        public DateTime DateOfBirthEstimated { get; set; }
        [Display(Name = "Date Certificate Issued")]
        public DateTime DateCertificateIssued { get; set; }
        [Display(Name = "*Signature Of Relative")]
        public string SignatureOfRelative { get; set; }
        [Display(Name = "Signature Of Physician")]
        public string SignatureOfPhysician { get; set; }
        [Display(Name = "Signature Of Passenger")]
        public string SignatureOfPassenger { get; set; }
        [Display(Name = "Certificate Type")]
        public PregnancyCertificateType PregnancyCertificateType { get; set; }
        [Display(Name = "Passenger Email")]
        public string EmailOfPassenger { get; set; }
        [Display(Name = "Captain Email")]
        public string EmailOfCaptain { get; set; }

        [NotMapped]
        ApplicationDbContext _context;

        public PregnancyCertificate(ApplicationDbContext context)
        {
            _context = context;
        }

        public PregnancyCertificate()
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
                _context.PregnancyCertificate.Add(this);
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
                _context.PregnancyCertificate.Attach(this);
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
                _context.PregnancyCertificate.Attach(this);
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
                var fdrMMR = await _context.PregnancyCertificate.FirstOrDefaultAsync(con => con.FullName == FullName && con.DateCertificateIssued == DateCertificateIssued && con.DateOfExamination == DateOfExamination && con.Status == RecordStatus.Active);
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
                var pregnancys = await Task.Run(() => _context.PregnancyCertificate.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
                return pregnancys;
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
                return await _context.PregnancyCertificate.Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
