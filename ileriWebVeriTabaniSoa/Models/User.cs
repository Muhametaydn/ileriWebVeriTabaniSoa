using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace ileriWebVeriTabaniSoa.Models
{
    public class User
    {
        // User tablosundaki sütunları temsil eden özellikler
        public int UserId { get; set; } // Benzersiz kullanıcı kimliği (Primary Key)
        public string Username { get; set; } // Kullanıcı adı
        public string Email { get; set; } // Kullanıcı e-posta adresi
        public string Password { get; set; } // Şifre
        public string Role { get; set; } // Kullanıcı rolü (Admin, User vb.)
        public DateTime CreatedDate { get; set; } // Oluşturulma tarihi

        // İlişkiler
        public ICollection<Post>? Posts { get; set; } // Kullanıcının gönderileri
        public ICollection<Comment>? Comments { get; set; } // Kullanıcının yorumları
        public ICollection<Like>? Likes { get; set; } // Kullanıcının beğenileri

    }
}
