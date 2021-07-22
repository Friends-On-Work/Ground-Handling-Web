using System;
using System.Collections.Generic;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.RequestObject
{
    public class ResetPasswordRequestObject
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
