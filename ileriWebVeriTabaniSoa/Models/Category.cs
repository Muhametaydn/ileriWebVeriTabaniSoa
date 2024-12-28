using System.ComponentModel.DataAnnotations;

namespace ileriWebVeriTabaniSoa.Models
{
    public class Category
    {
        public int CategoryID { get; set; } // Primary Key
        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        // İlişkiler
        public ICollection<Post>? Posts { get; set; } // Kategoriye ait gönderiler
    }
}
