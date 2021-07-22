using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm
{
    public class MeetAndAssist : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Contact")]
        public string Contact { get; set; }
        [Display(Name = "*Name")]
        public string Name { get; set; }
    }
}
