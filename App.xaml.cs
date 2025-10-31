using System.Configuration;
using System.Data;
using System.Windows;

namespace InventoryApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            // Ana pencere yerine giriş penceresini aç
            LoginWindow loginWindow = new LoginWindow();
            MainWindow = loginWindow; // Ana pencere olarak belirle
            loginWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Uygulama başlatılırken hata oluştu: {ex.Message}", "Kritik Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }
}

