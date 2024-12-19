namespace ileriWebVeriTabaniSoa.Models
{
    public class Category
    {
        public int CategoryID { get; set; } // Primary Key
        public string Name { get; set; } // Kategori adı

        // İlişkiler
        public ICollection<Post> Posts { get; set; } // Kategoriye ait gönderiler
    }
}
