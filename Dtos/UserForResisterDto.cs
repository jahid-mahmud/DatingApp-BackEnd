using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForResisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength=6 ,ErrorMessage ="Invalid password")]
        public string Password { get; set; }
    }
}