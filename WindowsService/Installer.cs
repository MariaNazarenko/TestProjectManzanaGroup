using System.ComponentModel;
using System.ServiceProcess;

namespace WindowsService
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public Installer()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "ServiceForManzanaGroup";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
