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

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.HandoverForm
{
    [Table("HandoverForm")]
    public class HandoverForm : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Flight Number")]
        public string FlightNumber { get; set; }
        [Display(Name = "*Passenger Name")]
        public string PassengerName { get; set; }
        [Display(Name = "*Passenger Contact Address")]
        public string PassengerContactAddress { get; set; }
        [Display(Name = "*Passenger Seat No")]
        public string PassengerSeatNo { get; set; }
        [Display(Name = "*Document Number")]
        public string DocumentNumber  { get; set; }
        [Display(Name = "*Quantity of Documents")]
        public string QuantityOfDocuments { get; set; }
        [Display(Name = "*Type of Travel Documents")]
        public string TypeofTravelDocuments  { get; set; }
        [Display(Name = "*Class")]
        public string Class { get; set; }
        [Display(Name = "*Flight Date")]
        public DateTime FlightDate { get; set; }
        [Display(Name = "*Departure Station")]
        public string DepartureStation { get; set; }
        [Display(Name = "*Destination Station")]
        public string DestinationStation { get; set; }
        [Display(Name = "*Transit Station")]
        public string TransitStation { get; set; }

        [DisplayName("Staff Who Handover The Document At Departure")]
        [ForeignKey("HandOverStaffTheDocumentAtDeparture")]
        public long? StaffWhoHandoverTheDocumentAtDepartureId { get; set; }

        [DisplayName("Cabin team leader taking over the document")]
        [ForeignKey("HandOverStaffCabinTeamLeaderTakingOverTheDocument")]
        public long? CabinTeamLeaderTakingOverTheDocumentId { get; set; }

        [DisplayName("Ground Staff at transfer station ")]
        [ForeignKey("GroundStaffAtTransferStation")]
        public long? GroundStaffAtTransferStationId { get; set; }

        [DisplayName("Cabin Team Leader of the flight on which the deportee passenger travels")]
        [ForeignKey("CabinTeamLeaderOfTheFlight")]
        public long? CabinTeamLeaderOfTheFlightId { get; set; }

        [DisplayName("Ground staff on which passenger on the final destination")]
        [ForeignKey("GroundStaffOnFinalDestination")]
        public long? GroundStaffOnFinalDestinationId { get; set; }

        public virtual HandOverStaff HandOverStaffTheDocumentAtDeparture { get; set; }
        public virtual HandOverStaff HandOverStaffCabinTeamLeaderTakingOverTheDocument { get; set; }
        public virtual HandOverStaff GroundStaffAtTransferStation { get; set; }
        public virtual HandOverStaff CabinTeamLeaderOfTheFlight { get; set; }
        public virtual HandOverStaff GroundStaffOnFinalDestination { get; set; }

        [NotMapped]
        ApplicationDbContext _context;

        public HandoverForm(ApplicationDbContext context)
        {
            _context = context;
        }

        public HandoverForm()
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
                _context.HandoverForm.Add(this);
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
                _context.HandoverForm.Attach(this);
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
                _context.HandoverForm.Attach(this);
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
                var fdrMMR = await _context.HandoverForm.FirstOrDefaultAsync(con => con.PassengerName == PassengerName && con.PassengerSeatNo == PassengerSeatNo && con.Status == RecordStatus.Active);
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
                var fdrMMRMasters = await Task.Run(() => _context.HandoverForm.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
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
                return await _context.HandoverForm.Include(con => con.HandOverStaffCabinTeamLeaderTakingOverTheDocument).Include(con => con.GroundStaffAtTransferStation).Include(con => con.GroundStaffOnFinalDestination).Include(con => con.HandOverStaffTheDocumentAtDeparture).Include(con => con.CabinTeamLeaderOfTheFlight).Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
