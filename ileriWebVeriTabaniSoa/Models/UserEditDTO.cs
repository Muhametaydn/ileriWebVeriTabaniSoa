using System.ComponentModel.DataAnnotations;

namespace ileriWebVeriTabaniSoa.Models
{
    public class UserEditDTO
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string Role { get; set; }
    }
}
