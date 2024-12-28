using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ileriWebVeriTabaniSoa.Models
{
    public class User
    {
        // User tablosundaki sütunları temsil eden özellikler
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Password alanını yalnızca Create işlemi için zorunlu kılalım
        [Required(ErrorMessage = "Password is required on Create only")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public DateTime CreatedDate { get; set; }

        // İlişkiler
        public ICollection<Post>? Posts { get; set; } // Kullanıcının gönderileri
        public ICollection<Comment>? Comments { get; set; } // Kullanıcının yorumları
        public ICollection<Like>? Likes { get; set; } // Kullanıcının beğenileri

    }
}
