using System.ComponentModel;
using System.Configuration.Install;

namespace SWI.SoftStock.Client.WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            this.InitializeComponent();
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
        }
    }
}