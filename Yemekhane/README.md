# Yemekhane Menü Sistemi

ASP.NET Core MVC ve SQL Server ile geliştirilmiş bir yemekhane menü yönetim sistemi. Yönetici, haftanın her günü için yemekleri girer. Kullanıcılar giriş yapmadan güncel menüyü görüntüler.

## Özellikler

- **Menü Listesi:** Herkes giriş yapmadan haftalık menüyü görüntüleyebilir.
- **Yönetici Girişi:** Kullanıcı adı ve şifre ile giriş yapılır (cookie tabanlı kimlik doğrulama).
- **Admin Paneli:** Pazartesi'den Cuma'ya her gün için ayrı bir panel vardır. Her satıra bir yemek yazılır ve tek bir **Kaydet** butonuyla veritabanına kaydedilir. Menü listesi sayfası hemen güncellenir.
- **Güvenli şifre saklama:** Şifreler veritabanında düz metin olarak değil, hash'lenmiş halde tutulur.
- **Korumalı sayfalar:** Admin paneli `[Authorize]` ile korunur. Giriş yapmayan kullanıcı giriş sayfasına yönlendirilir.

## Kullanılan Teknolojiler

- C# / .NET 10
- ASP.NET Core MVC (Razor View'lar)
- Entity Framework Core (Code First, Migrations)
- SQL Server
- Bootstrap

## Veritabanı Tasarımı

Gün ve yemek arasında **çoktan çoğa** (many-to-many) bir ilişki vardır: bir günde birden fazla yemek olur, bir yemek birden fazla günde servis edilebilir. Bu yüzden iki tabloyu bağlayan bir ilişki tablosu kullanılmıştır.

```
Gunler (Id, Ad)
   │ 1
   │
   │ N
GunYemekleri (Id, GunId, YemekId)   ← ilişki tablosu
   │ N
   │
   │ 1
Yemekler (Id, Ad)

Users (Id, KullaniciAdi, SifreHash)  ← yönetici hesabı (bağımsız tablo)
```

| Tablo | Açıklama |
|---|---|
| `Gunler` | Haftanın günleri (Pazartesi-Cuma). Migration ile hazır eklenir. |
| `Yemekler` | Yemek adları. Aynı yemek tek satır olarak tutulur. |
| `GunYemekleri` | Hangi gün hangi yemeğin verildiğini tutan ilişki tablosu. |
| `Users` | Yönetici hesabı. Şifre hash olarak saklanır. |

Menü sayfası, EF Core'un `Include` ve `ThenInclude` yöntemleriyle üç tabloyu birleştirerek (SQL'deki `JOIN`) veriyi çeker.

## Gereksinimler

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server (yerel kurulum, varsayılan örnek: `localhost`)
- `dotnet-ef` aracı:

```
dotnet tool install --global dotnet-ef
```

## Kurulum ve Çalıştırma

1. Projeyi indirin ve proje klasörüne girin:

```
git clone <depo-adresi>
cd Yemekhane/Yemekhane
```

2. Bağlantı dizesini kontrol edin (`appsettings.json`):

```
Server=localhost;Database=YemekhaneDB;Trusted_Connection=True;TrustServerCertificate=True;
```

SQL Server örnek adınız farklıysa (ör. `SQLEXPRESS`), `Server=localhost\SQLEXPRESS` olarak değiştirin.

3. Veritabanını oluşturun:

```
dotnet ef database update
```

Bu komut `YemekhaneDB` veritabanını, tabloları ve Pazartesi-Cuma günlerini oluşturur.

4. Uygulamayı çalıştırın:

```
dotnet run
```

5. Tarayıcıda konsolda yazan adresi açın (genellikle `http://localhost:5288`).

## Giriş Bilgileri

İlk çalıştırmada `Users` tablosu boşsa bir yönetici hesabı otomatik oluşturulur. Bu bilgiler yalnızca **geliştirme/demo amaçlıdır**:

- **Kullanıcı adı:** `admin`
- **Şifre:** `Admin123!`

> Gerçek bir ortamda şifre kod içinde tutulmamalı, ortam değişkeni veya gizli yapılandırma kullanılmalıdır.

## Proje Yapısı

| Klasör / Dosya | Görevi |
|---|---|
| `Models/` | `Gun`, `Yemek`, `GunYemek` (ilişki), `Kullanici` ve admin formu için `GunFormu` sınıfları |
| `Data/YemekhaneContext.cs` | Veritabanı bağlantısı, tablo tanımları ve ilişkiler |
| `Controllers/` | `HomeController`, `MenuController`, `AccountController`, `AdminController` |
| `Views/` | Ana sayfa, menü listesi, giriş formu ve admin paneli sayfaları |
| `Migrations/` | EF Core veritabanı planı |
| `Program.cs` | Uygulama başlangıcı, servis ayarları ve ilk yönetici hesabının oluşturulması |

## Sayfalar

| Adres | Erişim | Açıklama |
|---|---|---|
| `/` | Herkes | İki bloklu ana sayfa (Menü Listesi, Main Girişi) |
| `/Menu/Liste` | Herkes | Haftalık menü tablosu |
| `/Account/Login` | Herkes | Yönetici giriş formu |
| `/Admin` | Sadece giriş yapmış yönetici | Menü düzenleme paneli |