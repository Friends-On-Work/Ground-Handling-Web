using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.HandoverForm
{
    public class HandOverStaff : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Name")]
        public string Name { get; set; }
        [Display(Name = "*Signature")]
        public string Signature { get; set; }
        [Display(Name = "*ID Number")]
        public string IDNumber { get; set; }
    }
}
