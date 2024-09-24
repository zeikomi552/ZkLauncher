using System.Configuration;
using System.Data;
using System.Windows;

namespace ZkLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            //var icon = GetResourceStream(new Uri("icon.ico", UriKind.Relative)).Stream;
            //var notifyIcon = new System.Windows.Forms.NotifyIcon
            //{
            //    Visible = true,
            //    Icon = new System.Drawing.Icon(icon),
            //    Text = "タスクトレイ常駐アプリのテストです"
            //};
        }
    }

}
