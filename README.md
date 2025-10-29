# 📦 Envanter Yönetim Sistemi (Inventory Management System)

Modern ve kullanıcı dostu envanter yönetim sistemi. C# WPF ve .NET 9.0 ile geliştirilmiştir.

## ✨ Özellikler

### 🎯 **Temel Fonksiyonlar**
- **Ürün Yönetimi**: Ürün ekleme, düzenleme, silme ve arama
- **Kategori Sistemi**: Ürünleri kategorilere ayırma
- **Stok Takibi**: Gerçek zamanlı stok seviyesi izleme  
- **İstatistikler**: Envanter değeri ve düşük stok uyarıları
- **Modern UI**: Sol navigasyon menüsü ile kolay erişim

### 🖥️ **Navigasyon Menüsü**
- 🏠 **Anasayfa**: Ana dashboard ve ürün listesi
- 📦 **Stok**: Detaylı stok yönetimi
- 💰 **Satışlar**: Satış işlemleri (geliştirilecek)
- 📊 **İstatistikler**: Raporlar ve analizler (geliştirilecek)
- ⚙️ **Ayarlar**: Uygulama ayarları (geliştirilecek)

### 🛠️ **Teknoloji Stack**
- **Framework**: .NET 9.0 (Windows)
- **UI**: WPF (Windows Presentation Foundation)
- **MVVM**: CommunityToolkit.Mvvm
- **Database**: SQLite + Entity Framework Core
- **Charts**: LiveCharts2 (hazır)
- **Barcode**: ZXing.Net (hazır)
- **Excel**: ClosedXML (hazır)

## 🚀 Kurulum

### **Gereksinimler**
- Windows 10/11
- .NET 9.0 SDK
- Visual Studio 2022 (önerilen)

### **Adımlar**
1. Repository'yi klonlayın:
   ```bash
   git clone https://github.com/[YOUR-USERNAME]/inventory-management-system.git
   cd inventory-management-system
   ```

2. Proje bağımlılıklarını yükleyin:
   ```bash
   dotnet restore
   ```

3. Veritabanını oluşturun:
   ```bash
   dotnet ef database update
   ```

4. Uygulamayı çalıştırın:
   ```bash
   dotnet run
   ```

## 📁 Proje Yapısı

```
InventoryApp/
├── Models/              # Veri modelleri
│   ├── Product.cs       # Ürün modeli
│   ├── Category.cs      # Kategori modeli
│   ├── Sale.cs          # Satış modeli
│   └── SaleItem.cs      # Satış kalemi modeli
├── ViewModels/          # MVVM view modelleri
│   ├── BaseViewModel.cs # Temel view model
│   └── MainViewModel.cs # Ana view model
├── Views/               # XAML görünümleri
├── Data/                # Veritabanı katmanı
│   └── InventoryDbContext.cs
├── Converters/          # UI dönüştürücüler
├── Services/            # İş mantığı servisleri
└── Migrations/          # EF Core migrasyonlar
```

## 🎨 Ekran Görüntüleri

### Ana Ekran
- Modern sol navigasyon menüsü
- Üst istatistik paneli (Toplam Değer, Düşük Stok)
- Ürün listesi ve yönetim araçları
- Arama ve filtreleme sistemi

### Özellikler
- **Responsive Design**: Farklı ekran boyutlarına uyum
- **Dark Theme**: Profesyonel koyu tema
- **Hover Effects**: İnteraktif kullanıcı deneyimi
- **Real-time Updates**: Anlık veri güncellemeleri

## 🔄 Geliştirilecek Özellikler

### 📊 **Yakında**
- [ ] Dinamik grafikler ve raporlar
- [ ] Barkod okuma sistemi
- [ ] Excel import/export
- [ ] Satış formu ve işlemleri
- [ ] Gelişmiş filtreleme

### 🎯 **Gelecek Sürümler**
- [ ] Çoklu dil desteği
- [ ] Tema özelleştirme
- [ ] Kullanıcı yönetimi
- [ ] Cloud synchronization
- [ ] Mobile responsive

## 💻 Geliştirme

### **Test için Hızlı Başlatma**
Masaüstünde bulunan `Envanter_Test.bat` dosyasını çalıştırarak otomatik build + run yapabilirsiniz.

### **Katkıda Bulunma**
1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/yeni-ozellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik: açıklama'`)
4. Branch'inizi push edin (`git push origin feature/yeni-ozellik`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 🤝 Destek

Herhangi bir sorun yaşarsanız veya öneriniz varsa lütfen [Issues](https://github.com/[YOUR-USERNAME]/inventory-management-system/issues) kısmında bildirin.

## 👨‍💻 Geliştirici

- **Ozan** - Ana Geliştirici

---

⭐ Bu projeyi beğendiyseniz yıldızlamayı unutmayın!