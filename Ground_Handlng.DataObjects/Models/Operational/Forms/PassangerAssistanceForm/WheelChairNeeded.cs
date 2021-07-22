using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm
{
    public class WheelChairNeeded : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Wheel Chair Type")]
        public WheelChairType WheelChairType { get; set; }
        [Display(Name = "*Wheel Chair Catagory")]
        public WheelChairCatagory WheelChairCatagory { get; set; }
        [Display(Name = "*Collapsaible WCOB")]
        public bool CollapsaibleWCOB { get; set; }
        [Display(Name = "*Own Wheel Chair")]
        public bool OwnWheelChair { get; set; }
    }
}
