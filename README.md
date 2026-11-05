# Şifre Kasası

Kullanıcıların kayıt olup giriş yaptıktan sonra farklı sitelere ait kullanıcı adı ve şifrelerini güvenli şekilde saklayabildiği, listeleyip güncelleyip silebildiği bir web uygulaması. Frontend jQuery + Ajax ile sayfa yenilenmeden (SPA mantığıyla) çalışır.

## Gereksinimler

- .NET 10 SDK
- SQL Server LocalDB (`(localdb)\mssqllocaldb`, Visual Studio ile birlikte gelir)

## Çalıştırma

1. Çözümü (`SifreKasasi.slnx`) Visual Studio ile açın.
2. Çözüme sağ tık → **Configure Startup Projects** → **Multiple startup projects** seçip şu iki projeyi **Start** yapın:
   - `SifreKasasi.API` → https://localhost:7112 (Swagger: https://localhost:7112/swagger)
   - `SifreKasasi.Web` → https://localhost:7111
3. F5 ile başlatın. API açılırken bekleyen migration'lar otomatik uygulanır; veritabanı (`AuthTakeSifreKasasiDb`) yoksa oluşturulur.

Komut satırından:

```bash
dotnet run --project SifreKasasi.API --launch-profile https
dotnet run --project SifreKasasi.Web --launch-profile https
```

## Mimari

| Katman | Sorumluluk |
|---|---|
| **SifreKasasi.Data** | EF Core `AppDbContext`, entity'ler (`User`, `VaultItem`), migration'lar |
| **SifreKasasi.Business** | `PasswordHasher` (BCrypt), `AesEncryptionService` (AES) |
| **SifreKasasi.API** | REST uç noktaları (`/api/Auth`, `/api/Vault`), JWT kimlik doğrulama, DTO doğrulama |
| **SifreKasasi.Web** | jQuery + Ajax ile API'yi çağıran tek sayfalık frontend |

Bağımlılık yönü: `API → Business → Data`. Web katmanı veritabanına erişmez, sadece HTTP üzerinden API ile konuşur.

## Güvenlik Notları

- **Kullanıcı şifreleri** BCrypt ile salt'lı ve geri döndürülemez şekilde hash'lenerek saklanır.
- **Kasa şifreleri** AES ile her kayıt için rastgele üretilen IV kullanılarak çift yönlü şifrelenir; listelemede çözülerek gösterilir.
- **Yetkilendirme:** Kasa uç noktaları `[Authorize]` ile korunur. `UserId` istemciden alınmaz, her zaman JWT claim'inden okunur; bu sayede bir kullanıcı başkasının kayıtlarına erişemez.
- **CORS** yalnızca Web uygulamasının adreslerine izin verir.

> ⚠️ `SifreKasasi.API/appsettings.json` içindeki `EncryptionKey` ve `JwtSettings:SecurityKey` değerleri ödev için **örnek** değerlerdir. Gerçek ortamda bu anahtarlar kaynak koda konmamalı; **user-secrets** (geliştirme), **environment variable** veya **Azure Key Vault** gibi bir gizli bilgi yöneticisi üzerinden verilmelidir.
