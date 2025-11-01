using System.Windows;
using System.Threading.Tasks;
using InventoryApp.Services;
using InventoryApp.Data;
using System.Text.RegularExpressions;

namespace InventoryApp
{
    public partial class LoginWindow : Window
    {
        private SettingsService _settingsService;

        public LoginWindow()
        {
            InitializeComponent();
            _settingsService = new SettingsService();
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            try
            {
                var settings = _settingsService.LoadSettings();
                if (settings.RememberMe && !string.IsNullOrEmpty(settings.SavedEmail))
                {
                    EmailTextBox.Text = settings.SavedEmail;
                    if (!string.IsNullOrEmpty(settings.SavedPassword))
                    {
                        PasswordBox.Password = CryptoHelper.Decrypt(settings.SavedPassword);
                    }
                    RememberMeCheckBox.IsChecked = true;
                    
                    // Password alanına focus ver
                    if (!string.IsNullOrEmpty(PasswordBox.Password))
                    {
                        PasswordBox.Focus();
                        PasswordBox.SelectAll();
                    }
                }
            }
            catch
            {
                // Herhangi bir hata durumunda sessizce devam et
            }
        }

        private async Task SaveCredentialsIfRequested(string email, string password)
        {
            try
            {
                var settings = _settingsService.LoadSettings();
                
                if (RememberMeCheckBox.IsChecked == true)
                {
                    // "Beni Hatırla" işaretliyse bilgileri kaydet
                    settings.RememberMe = true;
                    settings.SavedEmail = email;
                    settings.SavedPassword = CryptoHelper.Encrypt(password);
                }
                else
                {
                    // İşaretli değilse kayıtlı bilgileri temizle
                    settings.RememberMe = false;
                    settings.SavedEmail = string.Empty;
                    settings.SavedPassword = string.Empty;
                }
                
                _settingsService.SaveSettings(settings);
                await Task.CompletedTask;
            }
            catch
            {
                // Herhangi bir hata durumunda sessizce devam et
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Form validasyonu
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // E-mail validasyonu
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Lütfen e-mail adresinizi girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                EmailTextBox.Focus();
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Lütfen geçerli bir e-mail adresi girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                EmailTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen şifrenizi girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBox.Focus();
                return;
            }

            try
            {
                // Loading göstergesi
                LoginButton.Content = "⏳ Giriş yapılıyor...";
                LoginButton.IsEnabled = false;

                // Database authentication
                using (var context = new InventoryDbContext())
                {
                    var userService = new UserService(context);
                    var user = await userService.AuthenticateUserAsync(email, password);

                    if (user != null)
                    {
                        // "Beni Hatırla" seçeneği kontrol edilirse ayarları kaydet
                        await SaveCredentialsIfRequested(email, password);
                        
                        // Başarılı giriş
                        await Task.Delay(500); // Smooth UX

                        // Ana pencereyi kullanıcı bilgisi ile aç
                        MainWindow mainWindow = new MainWindow(user);
                        
                        // Önce yeni pencereyi göster
                        mainWindow.Show();
                        
                        // Sonra giriş penceresini gizle
                        this.Hide();
                        
                        // Ana pencereyi uygulama ana penceresi olarak belirle
                        Application.Current.MainWindow = mainWindow;
                        
                        // Giriş penceresini kapat
                        this.Close();
                    }
                    else
                    {
                        // Hatalı giriş
                        MessageBox.Show("E-mail adresi veya şifre hatalı!\nLütfen bilgilerinizi kontrol edin.", 
                                      "Giriş Başarısız", 
                                      MessageBoxButton.OK, 
                                      MessageBoxImage.Error);
                        
                        // Şifre alanını temizle
                        PasswordBox.Password = "";
                        PasswordBox.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giriş sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Butonu eski haline getir
                LoginButton.Content = "🔐 GİRİŞ YAP";
                LoginButton.IsEnabled = true;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Uygulamayı tamamen kapat
            Application.Current.Shutdown();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Kayıt sayfasını aç
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        // Enter tuşu ile giriş yapma
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}