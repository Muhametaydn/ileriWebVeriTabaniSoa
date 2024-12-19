namespace ileriWebVeriTabaniSoa.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }

        // Yeni eklenen Category sütunu
        public int? CategoryID { get; set; } // Foreign Key
        public Category Category { get; set; } // Navigation Property

        public ICollection<Comment> Comments { get; set; } // Gönderiye yapılan yorumlar
        public ICollection<Like> Likes { get; set; } // Gönderiye yapılan beğeniler
        

    }
}
