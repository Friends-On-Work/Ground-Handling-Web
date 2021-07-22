using System;
using System.Collections.Generic;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Others
{
    public class DatabaseOperationResponse : OperationResult
    {
        public long AutoGenerateId { get; set; }
        public List<Exception> ListException { get; set; }
    }
}
