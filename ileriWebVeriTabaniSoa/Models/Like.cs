namespace ileriWebVeriTabaniSoa.Models
{
    public class Like
    {
        public int LikeID { get; set; } // Primary Key
        public int PostID { get; set; } // Foreign Key - Hangi gönderi beğenildi
        public int UserID { get; set; } // Foreign Key - Kim beğendi
        public DateTime LikedDate { get; set; } // Beğenilme tarihi

        // İlişkiler
        public Post Post { get; set; } // Beğenilen gönderi
        public User User { get; set; } // Beğeniyi yapan kullanıcı
    }
}
