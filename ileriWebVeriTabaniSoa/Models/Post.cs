

using System.ComponentModel.DataAnnotations;

namespace ileriWebVeriTabaniSoa.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İçerik alanı zorunludur.")]
        public string Content { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        
        public int CategoryID { get; set; } // Foreign Key

        public Category? Category { get; set; } // Navigation Property

        public ICollection<Comment>? Comments { get; set; } // Gönderiye yapılan yorumlar
        public ICollection<Like>? Likes { get; set; } // Gönderiye yapılan beğeniler
        

    }
}
