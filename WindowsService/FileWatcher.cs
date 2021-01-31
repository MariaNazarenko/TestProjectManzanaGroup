using log4net;
using System;
using System.IO;

namespace WindowsService
{
    /// <summary>
    /// Класс для реализации наблючения за файлами
    /// </summary>
    public class FileWatcher
    {
        public static readonly ILog log = LogManager.GetLogger("LOGGER");

        public delegate void GetDataHandler(string pathFile, string data);
        public event GetDataHandler OnGetData;
        private FileSystemWatcher fileWatcher;
        private event FileSystemEventHandler EventCreateFile
        {
            add { fileWatcher.Created += value; }
            remove { fileWatcher.Created -= value; }
        }

        public FileWatcher(string watcherDir)
        {
            fileWatcher = new FileSystemWatcher(watcherDir);
            fileWatcher.EnableRaisingEvents = true;
            fileWatcher.IncludeSubdirectories = false;
            EventCreateFile += FileCreate;
            log.Info("Начинается наблюдение за папкой " + watcherDir);
        }
        /// <summary>
        /// Метод возникающий при появлении файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileCreate(object sender, FileSystemEventArgs e)
        {
            var pathFile = e.FullPath;
            log.Info("В папке обнаружен файл " + pathFile);
            if (Path.GetExtension(pathFile) == ".txt")
            {
                string data = null;
                using (var sreamReader = new StreamReader(pathFile))
                {
                    data = sreamReader.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(data))
                {
                    OnGetData?.Invoke(pathFile, data);
                    return;
                }
                else
                    log.Error("Не удалось считать данные или файл пуст");
            }
            else
                log.Error("Расширение файла не соответствует txt");

            MoveFileinGarbage(pathFile);
        }
        /// <summary>
        /// Перемещение файла в папку необработанных файлов
        /// </summary>
        /// <param name="pathFile">Путь к исходному файлу</param>
        public void MoveFileinGarbage(string pathFile)
        {
            string pathGarbageDir = Properties.Settings.Default.PathGarbageDir ;
            try
            {
                Directory.CreateDirectory(pathGarbageDir);
                File.Move(pathFile, pathGarbageDir + "\\" + Path.GetFileName(pathFile));
            }
            catch (Exception ex)
            {
                log.Error("Не удалось переместить файл в папку необработанных файлов." + ex.Message);
            }
        }
        /// <summary>
        /// Перемещение файла в папку обработанных файлов
        /// </summary>
        /// <param name="pathFile">Путь к исходному файлу</param>
        public void MoveFileinComplete(string pathFile)
        {
            string pathCompleteDir = Properties.Settings.Default.PathCompleteDir;
            try
            {
                Directory.CreateDirectory(pathCompleteDir);
                File.Move(pathFile, pathCompleteDir + "\\" + Path.GetFileName(pathFile));
            }
            catch (Exception ex)
            {
                log.Error("Не удалось переместить файл  в папку обработанных файлов." + ex.Message);
            }
        }
    }
}
