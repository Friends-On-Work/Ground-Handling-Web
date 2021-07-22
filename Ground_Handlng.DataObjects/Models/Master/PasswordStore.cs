using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ground_Handlng.DataObjects.Models.Master
{
    public class PasswordStore
    {
        public PasswordStore()
        {
           
        }
        [Key]
        public long Id { get; set; }
        public string Password { get; set; }
    }
}
