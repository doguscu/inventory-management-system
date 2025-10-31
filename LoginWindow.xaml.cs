using System.Windows;
using System.Threading.Tasks;

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
            // Şimdilik basit doğrulama
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Boş alan kontrolü
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Lütfen kullanıcı adınızı girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                UsernameTextBox.Focus();
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

                // Şimdilik her giriş başarılı sayılır (database bağlantısı yok)
                await Task.Delay(1000); // Simulated loading

                // Ana pencereyi aç
                MainWindow mainWindow = new MainWindow();
                
                // Önce yeni pencereyi göster
                mainWindow.Show();
                
                // Sonra giriş penceresini gizle
                this.Hide();
                
                // Ana pencereyi uygulama ana penceresi olarak belirle
                Application.Current.MainWindow = mainWindow;
                
                // Giriş penceresini kapat
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giriş sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // Butonu eski haline getir
                LoginButton.Content = "🔐 GİRİŞ YAP";
                LoginButton.IsEnabled = true;
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