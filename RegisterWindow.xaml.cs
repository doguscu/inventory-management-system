using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using InventoryApp.Services;
using InventoryApp.Data;

namespace InventoryApp
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void RegisterSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Form validasyonu
            string email = EmailTextBox.Text.Trim();
            string fullName = FullNameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

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

            // Ad soyad validasyonu
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Lütfen adınızı ve soyadınızı girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                FullNameTextBox.Focus();
                return;
            }

            if (fullName.Length < 3)
            {
                MessageBox.Show("Ad soyad en az 3 karakter olmalıdır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                FullNameTextBox.Focus();
                return;
            }

            // Şifre validasyonu
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen şifrenizi girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBox.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Şifre en az 6 karakter olmalıdır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBox.Focus();
                return;
            }

            // Şifre onayı validasyonu
            if (password != confirmPassword)
            {
                MessageBox.Show("Şifreler eşleşmiyor. Lütfen tekrar kontrol edin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                ConfirmPasswordBox.Focus();
                return;
            }

            try
            {
                // Loading göstergesi
                RegisterSubmitButton.Content = "⏳ Kayıt oluşturuluyor...";
                RegisterSubmitButton.IsEnabled = false;

                // Database registration
                using (var context = new InventoryDbContext())
                {
                    var userService = new UserService(context);
                    
                    // Check if email already exists
                    if (await userService.EmailExistsAsync(email))
                    {
                        MessageBox.Show("Bu e-mail adresi zaten kayıtlı!\nLütfen farklı bir e-mail adresi kullanın.", 
                                      "E-mail Zaten Mevcut", 
                                      MessageBoxButton.OK, 
                                      MessageBoxImage.Warning);
                        EmailTextBox.Focus();
                        return;
                    }

                    // Register user
                    bool success = await userService.RegisterUserAsync(email, fullName, password);
                    
                    if (success)
                    {
                        // Başarı mesajı
                        MessageBox.Show("Kayıt işleminiz başarıyla tamamlandı!\nArtık giriş yapabilirsiniz.", 
                                      "Kayıt Başarılı", 
                                      MessageBoxButton.OK, 
                                      MessageBoxImage.Information);

                        // Giriş sayfasına geri dön
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.EmailTextBox.Text = email; // E-mail'i önceden doldur
                        loginWindow.PasswordBox.Focus(); // Password alanına odaklan
                        loginWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Kayıt işlemi sırasında bir hata oluştu.\nLütfen tekrar deneyin.", 
                                      "Kayıt Başarısız", 
                                      MessageBoxButton.OK, 
                                      MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Butonu eski haline getir
                RegisterSubmitButton.Content = "👤 KAYIT OL";
                RegisterSubmitButton.IsEnabled = true;
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
            // Giriş sayfasına geri dön
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void BackToLogin_Click(object sender, MouseButtonEventArgs e)
        {
            // Giriş sayfasına geri dön
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegisterSubmitButton_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                CloseButton_Click(sender, e);
            }
        }
    }
}