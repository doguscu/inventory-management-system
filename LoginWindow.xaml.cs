using System.Windows;
using System.Threading.Tasks;
using InventoryApp.Services;
using InventoryApp.Data;
using System.Text.RegularExpressions;

namespace InventoryApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
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