using System;

namespace Ground_Handlng.DataObjects.Models.Others
{
    public class AuditLog
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime RevisionDate { get; set; }
        public string RevisedBy { get; set; }
        public RecordStatus Status { get; set; }
    }
 }
