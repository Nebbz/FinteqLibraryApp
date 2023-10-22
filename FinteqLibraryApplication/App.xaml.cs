using System.Windows;

namespace FinteqLibrary
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create an instance of the LibraryManager and inject it into your MainWindow
            var libraryManager = new LibraryManager("bookData.json");
            var mainWindow = new MainWindow();

            // Handle the Exit event to stop the application when the main window is closed
            mainWindow.Closed += (sender, args) => { this.Shutdown(); };
        }
    }

}
