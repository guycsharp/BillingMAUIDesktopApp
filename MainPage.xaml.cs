using Microsoft.Maui.Controls;
using BillingMAUIDesktopApp.Services;

namespace BillingMAUIDesktopApp;

// Partial class merges with MainPage.xaml
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent(); // Binds XAML elements to this class
    }

    // Fired when the user taps the button
    private async void OnGeneratePdfClicked(object sender, EventArgs e)
    {
        var generator = new PdfGenerator();         // Create our PDF helper
        var filePath = generator.GenerateInvoice(); // Build the PDF

        // Show a popup with the saved file path
        await DisplayAlert("Invoice Created",
                           $"Saved to:\n{filePath}",
                           "OK");
    }
}
