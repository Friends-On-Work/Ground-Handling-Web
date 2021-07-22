using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm
{
    public class OtherGroundArrangment : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Remark")]
        public string Remark { get; set; }
        [Display(Name = "*Depature Airport")]
        public string DepatureAirport { get; set; }
        [Display(Name = "*Transit Airport")]
        public string TransitAirport { get; set; }
        [Display(Name = "*Arrival Airport")]
        public string ArrivalAirport { get; set; }
       
    }
}
