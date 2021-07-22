using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm
{
    [Table("IntendedEscorts")]
    public class IntendedEscorts : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "*Title")]
        public string Title { get; set; }
        [Display(Name = "*Name")]
        public string Name { get; set; }
        [Display(Name = "*PNR")]
        public string PNR { get; set; }
        [Display(Name = "*Age")]
        public int Age { get; set; }
        [Display(Name = "*Language Spoken")]
        public string LanguageSpoken { get; set; }
        [Display(Name = "*Medical Qualification")]
        public bool MedicalQualification { get; set; }
    }
}
