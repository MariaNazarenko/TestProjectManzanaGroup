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
        FileWatcher fileWatcher = null;

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
                    log.Error("Не удалось содать папку, возникло исключение.", ex);
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
                try
                {
                    client.SaveCheque(cheque);
                }
                catch(Exception ex)
                {
                    log.Error("Не удалось сохранить чек.", ex);
                }
                fileWatcher.MoveFileinComplete(pathFile);
                return;
            }
            catch (Exception ex)
            {
                log.Error("Не удалось корректно преобразовать данные в json.", ex);
            }
            fileWatcher.MoveFileinGarbage(pathFile);
        }
  
        protected override void OnStop()
        {
            if (fileWatcher != null)
                fileWatcher.OnGetData -= ReceiveData;
            log.Info("Остановка сервиса");
        }
    }
}
