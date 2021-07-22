using System.ComponentModel.DataAnnotations;

namespace Ground_Handlng.DataObjects.ViewModel.Identity
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
