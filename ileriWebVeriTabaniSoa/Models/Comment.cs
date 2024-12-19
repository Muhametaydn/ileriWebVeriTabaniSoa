namespace ileriWebVeriTabaniSoa.Models
{
    public class Comment
    {
        public int CommentID { get; set; } // Primary Key
        public string Content { get; set; } // Yorum içeriği
        public int PostID { get; set; } // Foreign Key - Hangi gönderiye ait
        public int UserID { get; set; } // Foreign Key - Yorumu yapan kullanıcı
        public DateTime CreatedDate { get; set; } // Yorumun oluşturulma tarihi

        // İlişkiler
        public Post Post { get; set; } // Yorumun ait olduğu gönderi
        public User User { get; set; } // Yorumu yapan kullanıcı
    }
}
