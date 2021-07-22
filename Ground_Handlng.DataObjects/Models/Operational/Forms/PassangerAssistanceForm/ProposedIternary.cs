using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm
{
    public class ProposedIternary : AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Display(Name = "*Airlines")]
        public string Airlines { get; set; }

        [Display(Name = "*Flight Number")]
        public string FlightNumber { get; set; }
        [Display(Name = "*Class")]
        public string Class { get; set; }
        [Display(Name = "*Flight Date")]
        public DateTime FlightDate { get; set; }
        [Display(Name = "*Orgin")]
        public string Orgin { get; set; }
        [Display(Name = "*Destination")]
        public string Destination { get; set; }

    }
}
