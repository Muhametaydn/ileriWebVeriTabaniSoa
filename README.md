# İleri Web Programlama & Veritabanı - SOA Blog Uygulaması

ASP.NET Core 8 MVC ile geliştirilmiş, SOA (Service-Oriented Architecture) mimarisini benimseyen bir blog platformudur. Uygulama; kullanıcı yönetimi, gönderi paylaşımı, yorum ve beğeni sistemi, kategori yönetimi, WCF servis entegrasyonu, hava durumu ve döviz kuru gibi dış servisleri bir arada sunar.

---

## Proje Mimarisi

Çözüm iki ana projeden oluşmaktadır:

| Proje | Açıklama |
|-------|----------|
| **ileriWebVeriTabaniSoa** | ASP.NET Core 8 MVC web uygulaması (ana uygulama) |
| **WCFService** | WCF tabanlı SOAP servis katmanı (hava durumu verisi aktarımı) |

```
ileriWebVeriTabaniSoa/
├── Controllers/          # MVC Controller'lar
├── Data/                 # Entity Framework DbContext
├── Helpers/              # Yardımcı sınıflar (şifre hash, Türkçe karakter normalizasyonu)
├── Migrations/           # EF Core veritabanı migration dosyaları
├── Models/               # Veri modelleri ve DTO'lar
├── Services/             # Servis katmanı (Weather, Currency)
├── ViewComponents/       # Razor ViewComponent'ler
├── Views/                # Razor View dosyaları
└── wwwroot/              # Statik dosyalar (CSS, JS, Assets)

WCFService/
├── App_Code/             # WCF servis arayüzleri ve implementasyonlar
├── Service.svc           # WCF endpoint
└── Web.config            # WCF yapılandırması
```

---

## Teknolojiler

- **Framework:** ASP.NET Core 8 MVC
- **ORM:** Entity Framework Core 9 (Code-First)
- **Veritabanı:** PostgreSQL (Npgsql)
- **Kimlik Doğrulama:** Cookie Authentication
- **Servis Mimarisi:** WCF (SOAP), REST API
- **Ön Yüz:** Bootstrap 4, SCSS, Owl Carousel, Animate.css
- **Diğer:** Newtonsoft.Json, IMemoryCache

---

## Veri Modeli

Uygulama aşağıdaki entity'lerden oluşur:

- **User** — Kullanıcı bilgileri (username, email, şifre hash, rol, kayıt tarihi). Her kullanıcı birden fazla Post, Comment ve Like sahibi olabilir.
- **Post** — Blog gönderisi (başlık, içerik). Bir User ve bir Category ile ilişkilidir. Üzerinde Comment ve Like koleksiyonları barındırır.
- **Category** — Gönderi kategorileri. Birden fazla Post ile ilişkili olabilir.
- **Comment** — Gönderilere yapılan yorumlar. Bir Post ve bir User'a bağlıdır.
- **Like** — Gönderilere verilen beğeniler. Bir Post ve bir User'a bağlıdır.
- **WeatherModel** — Dış servisten alınan hava durumu verisi (açıklama, derece).

Cascade delete kuralları `AppDbContext.OnModelCreating` içinde tanımlanmıştır: kullanıcı silindiğinde postları, yorumları ve beğenileri de silinir; kategori silindiğinde ilgili postların `CategoryID` değeri `null` olarak güncellenir.

---

## Temel Özellikler

### Kullanıcı Yönetimi
Kayıt (Register), giriş (Login) ve çıkış (Logout) işlemleri Cookie Authentication ile sağlanır. Şifreler `PasswordHasher<User>` ile hash'lenir. Yeni kayıt olan kullanıcılara varsayılan olarak "Writer" rolü atanır. `[Authorize]` attribute'u ile rol bazlı erişim kontrolü uygulanır.

### Blog Gönderileri (CRUD)
Gönderiler kategorilere bağlı olarak oluşturulabilir, düzenlenebilir ve silinebilir. Ana sayfada tüm gönderiler kategorileriyle birlikte listelenir.

### Yorum ve Beğeni Sistemi
Kullanıcılar gönderilere yorum yapabilir ve beğeni bırakabilir. Her iki özellik de ayrı Controller ve View yapılarıyla yönetilir.

### Arama
Gönderi başlık ve içeriklerinde Türkçe karakter duyarsız arama yapılabilir. `StringHelper.NormalizeTurkishCharacters` metodu ile İ/ı, ç, ğ, ü, ş, ö gibi karakterler normalize edilerek arama gerçekleştirilir.

### Hava Durumu Servisi (SOA)
Hava durumu verisi harici bir Node.js servisi üzerinden WCF'e, oradan da ASP.NET Core'un REST API endpoint'ine (`/api/weather/receive`) iletilir. Alınan veri `IMemoryCache` ile bellekte saklanır ve `Weather` ViewComponent aracılığıyla layout üzerinde gösterilir.

### Döviz Kuru Servisi
`CurrencyService`, harici bir API'den güncel dolar kurunu çekerek ana sayfada `ViewBag` üzerinden görüntüler.

### Kategori ViewComponent
`CategoryList` ViewComponent, veritabanındaki kategorileri çekerek layout içinde dinamik olarak listeler.

---

## Kurulum

### Gereksinimler

- .NET 8 SDK
- PostgreSQL
- Node.js (hava durumu ve döviz harici servisleri için)

### Adımlar

1. **Repoyu klonlayın:**
   ```bash
   git clone https://github.com/Muhametaydn/ileriWebVeriTabaniSoa.git
   cd ileriWebVeriTabaniSoa
   ```

2. **Veritabanı bağlantı bilgilerini düzenleyin:**
   `ileriWebVeriTabaniSoa/appsettings.json` dosyasındaki `ConnectionStrings` alanını kendi PostgreSQL bilgilerinize göre güncelleyin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=BlogDB;Username=postgres;Password=root"
   }
   ```

3. **Migration'ları uygulayın:**
   ```bash
   cd ileriWebVeriTabaniSoa
   dotnet ef database update
   ```

4. **Uygulamayı çalıştırın:**
   ```bash
   dotnet run
   ```
   Uygulama varsayılan olarak `https://localhost:5240` adresinde çalışacaktır.

5. **(İsteğe bağlı) Harici servisleri başlatın:**
   Hava durumu ve döviz kuru özellikleri için `localhost:4000` üzerinde çalışan bir Node.js servisine ihtiyaç duyulur.

---

## API Endpoint'leri

| Metot | Endpoint | Açıklama |
|-------|----------|----------|
| POST | `/api/weather/receive` | WCF'den gelen hava durumu verisini alır |
| GET | `/api/weather/current` | Bellekteki güncel hava durumu verisini döndürür |
| POST | `/Home/GetWeather` | Şehir adına göre hava durumu sorgulama |

---

## SOA Akışı (Hava Durumu)

```
[Harici Servis] ──POST──▶ [WCF Service: GetWeather()]
                                   │
                                   ▼
                          POST /api/weather/receive
                                   │
                                   ▼
                         [ASP.NET Core WeatherController]
                                   │
                                   ▼
                           [IMemoryCache'e kaydet]
                                   │
                                   ▼
                       [Weather ViewComponent ile göster]
```

---

## Katkıda Bulunanlar

- [Muhametaydn](https://github.com/Muhametaydn)

---

## Lisans

Bu proje açık kaynak olarak paylaşılmıştır.
