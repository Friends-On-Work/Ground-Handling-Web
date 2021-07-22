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

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm
{
    public class FrequentTravelerMedicalCard : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*FREMEC Number")]
        public string FrequentTravelerCardNumber { get; set; }
        [Display(Name = "*Issued By")]
        public string IssuedBy { get; set; }
        [Display(Name = "*Expire Date")]
        public DateTime ExpireDate { get; set; }

        [NotMapped]
        ApplicationDbContext _context;

        public FrequentTravelerMedicalCard(ApplicationDbContext context)
        {
            _context = context;
        }

        public FrequentTravelerMedicalCard()
        {

        }

        public async Task<List<object>> GetList()
        {
            try
            {
                var fdrMMRMasters = await Task.Run(() => _context.FrequentTravelerMedicalCard.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
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
                return await _context.FrequentTravelerMedicalCard.Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
