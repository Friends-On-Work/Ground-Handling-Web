using Ground_Handlng.DataObjects.Models.Others;
using System.ComponentModel.DataAnnotations;

namespace Ground_Handlng.DataObjects
{
    public class LoginResponseObject : OperationResult
    {
        public string Username { get; set; }
        public string EmployeeId { get; set; }
        [Display(Name = "*Password")]
        public string Password { get; set; }
        [Display(Name = "*Email")]
        public string Email { get; set; }
        [Display(Name = "*First Name")]
        public string FullName { get; set; }
        //[Display(Name = "*First Name")]
        //public string FirstName { get; set; }
        //[Display(Name = "*Last Name")]
        //public string LastName { get; set; }
        [Display(Name = "*Address")]
        public string Address { get; set; }
        [Display(Name = "*Position")]
        public string Position { get; set; }
    }
}
