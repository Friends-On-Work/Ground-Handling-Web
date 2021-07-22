using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm
{
    public class SpecialInflightNeed: AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Type Of Arrangment")]
        public TypeOfArrangment TypeOfArrangment { get; set; }
        [Display(Name = "*Equipment")]
        public Equipment Equipment { get; set; }
        [Display(Name = "*Arranging Company Name")]
        public string ArrangingCompanyName { get; set; }
        [Display(Name = "*Arrival Airport")]
        public string ArrivalAirport { get; set; }
    }
}
