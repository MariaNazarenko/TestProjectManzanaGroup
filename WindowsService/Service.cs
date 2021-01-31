using log4net;
using System;
using System.IO;
using System.ServiceProcess;
using Newtonsoft.Json;
using WindowsService.ServiceReference;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {
        public static readonly ILog log = LogManager.GetLogger("LOGGER");
        ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
        FileWatcher fileWatcher;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.InitLogger();
            log.Info("Старт сервиса");
            string watcherDir = Properties.Settings.Default.PathDir;
            if (Directory.Exists(watcherDir))
            {
                fileWatcher = new FileWatcher(watcherDir);
                fileWatcher.OnGetData += ReceiveData;
            }
            else
            {
                log.Warn("Не найденна указаннная папка для файлов");
                try
                {
                    Directory.CreateDirectory(watcherDir);
                    log.Info("Создана директория" + watcherDir);
                    var fileWatcher = new FileWatcher(watcherDir);
                    fileWatcher.OnGetData += ReceiveData;
                }
                catch(Exception ex)
                {
                    log.Error("Не удалось содать папку, возникло исключение." + ex.Message);
                }
            }
        }

        /// <summary>
        /// Отправка данных
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveData(string pathFile, string data)
        {
            try
            {
                var cheque = JsonConvert.DeserializeObject<Cheque>(data);
                client.SaveCheque(cheque);
                fileWatcher.MoveFileinComplete(pathFile);
                return;
            }
            catch (Exception ex)
            {
                log.Error("Не удалось корректно преобразовать данные в json." + ex.Message);
            }
            fileWatcher.MoveFileinGarbage(pathFile);
        }
  
        protected override void OnStop()
        {
            log.Info("Остановка сервиса");
        }
    }
}
