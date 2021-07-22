using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm
{
    [Table("PassangerAssistanceForm")]
    public class PassangerAssistanceForm : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Title")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "*Full Name")]
        public string FullName { get; set; }

        [Display(Name = "*PNR")]
        public string PNR { get; set; }

        [Display(Name = "*Nature Of Disablity")]
        public string NatureOfDisablity { get; set; }

        [Display(Name = "*Streacher Needed")]
        public bool StreacherNeeded { get; set; }

        //[Display(Name = "*Wheel Chair Needed")]
        //public bool WheelChairNeeded { get; set; }

        [DisplayName("Special In flight Need")]
        [ForeignKey("SpecialInflightNeed")]
        public long? SpecialInflightNeedId { get; set; }

        [DisplayName("Proposed Iternary")]
        [ForeignKey("ProposedIternary")]
        public long? ProposedIternaryId { get; set; }

        [DisplayName("Ambulance")]
        [ForeignKey("Ambulance")]
        public long? AmbulanceId { get; set; }

        [DisplayName("Meet And Assist")]
        [ForeignKey("MeetAndAssist")]
        public long? MeetAndAssistId { get; set; }

        [DisplayName("Other Ground Arrangment")]
        [ForeignKey("OtherGroundArrangment")]
        public long? OtherGroundArrangmentId { get; set; }

        [DisplayName("Wheel Chair Needed")]
        [ForeignKey("WheelChairNeededs")]
        public long? WheelChairNeededsId { get; set; }
        [DisplayName("Intended Escorts")]
        [ForeignKey("IntendedEscorts")]
        public long? IntendedEscortsId { get; set; }
        [DisplayName("Frequent Traveler Medical Card")]
        [ForeignKey("FrequentTravelerMedicalCard")]
        public long? FrequentTravelerMedicalCardId { get; set; }

        public virtual SpecialInflightNeed SpecialInflightNeed { get; set; }
        public virtual ProposedIternary ProposedIternary { get; set; }
        public virtual Ambulance Ambulance { get; set; }
        public virtual MeetAndAssist MeetAndAssist { get; set; }
        public virtual OtherGroundArrangment OtherGroundArrangment { get; set; }
        public virtual WheelChairNeeded WheelChairNeededs { get; set; }
        public virtual IntendedEscorts IntendedEscorts { get; set; }
        public virtual FrequentTravelerMedicalCard FrequentTravelerMedicalCard { get; set; }


        [NotMapped]
        ApplicationDbContext _context;

        public PassangerAssistanceForm(ApplicationDbContext context)
        {
            _context = context;
        }

        public PassangerAssistanceForm()
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
                _context.PassangerAssistanceForm.Add(this);
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
                _context.PassangerAssistanceForm.Attach(this);
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
                _context.PassangerAssistanceForm.Attach(this);
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
                var fdrMMRMasters = await Task.Run(() => _context.PassangerAssistanceForm.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
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
                return await _context.PassangerAssistanceForm.Include(con=>con.ProposedIternary).Include(con => con.WheelChairNeededs).Include(con => con.IntendedEscorts).Include(con => con.Ambulance).Include(con => con.MeetAndAssist).Include(con => con.OtherGroundArrangment).Include(con => con.SpecialInflightNeed).Include(con => con.FrequentTravelerMedicalCard).Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
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
                var fdrMMR = await _context.PassangerAssistanceForm.FirstOrDefaultAsync(con => con.FullName == FullName && con.PNR == PNR && con.Status == RecordStatus.Active);
                return fdrMMR == null ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
