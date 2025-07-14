using Microsoft.Maui.Controls;

namespace BillingMAUIDesktopApp;

// This partial class combines with the XAML file at compile time.
public partial class App : Application
{
    public App()
    {
        InitializeComponent();     // Wire up App.xaml resources
        MainPage = new MainPage(); // Show MainPage on startup
    }
}
