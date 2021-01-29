using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using WindowsService.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace WindowsService
{
    public partial class Service : ServiceBase
    {
        public static readonly ILog log = LogManager.GetLogger("LOGGER");
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
                    log.Error("Не удалось содать папку, возникло исключение:" + ex);
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
                JsonConvert.DeserializeObject<Cheque>(data);
                using (var client = new HttpClient())
                {
                    string uri = string.Empty;
                    var request = new HttpRequestMessage(HttpMethod.Post, uri);
                    request.Headers.Add("Accept", "application/json");
                    request.Content = new StringContent(data);
                    client.SendAsync(request);
                    fileWatcher.MoveFileinComplete(pathFile);
                }
            }
            catch
            {
                fileWatcher.MoveFileinGarbage(pathFile);
                log.Error("Не удалось корректно преобразовать данные в json");
            }
        }
  
        protected override void OnStop()
        {
            log.Info("Остановка сервиса");
        }
    }
}
