using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace BillingMAUIDesktopApp
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = MauiProgram.CreateMauiApp();
            app.Run(args); // Throws error if MAUI SDK not resolved
        }
    }
}
