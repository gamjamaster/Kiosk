using System;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace KioskApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            base.OnStartup(e);
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ShowUnhandledException(e.Exception);
            e.Handled = true;
        }

        private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                ShowUnhandledException(ex);
            }
            else
            {
                MessageBox.Show(
                    "An unknown unhandled exception occurred.",
                    "Unhandled Exception",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private static void ShowUnhandledException(Exception ex)
        {
            var message = new StringBuilder();
            message.AppendLine(ex.GetType().FullName);
            message.AppendLine(ex.Message);
            message.AppendLine();
            message.AppendLine(ex.StackTrace);

            if (ex.InnerException != null)
            {
                message.AppendLine();
                message.AppendLine("--- Inner Exception ---");
                message.AppendLine(ex.InnerException.GetType().FullName);
                message.AppendLine(ex.InnerException.Message);
                message.AppendLine();
                message.AppendLine(ex.InnerException.StackTrace);
            }

            MessageBox.Show(
                message.ToString(),
                "Unhandled Exception",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
