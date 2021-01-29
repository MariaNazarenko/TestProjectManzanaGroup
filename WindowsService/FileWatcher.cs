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
            using (var sreamReader = new StreamReader(pathFile))
            {
                var data = sreamReader.ReadToEnd();
                OnGetData?.Invoke(pathFile, data);
            }
        }
        /// <summary>
        /// Перемещение файла в папку необработанных файлов
        /// </summary>
        /// <param name="pathFile">Путь к исходному файлу</param>
        public void MoveFileinGarbage(string pathFile)
        {
            string pathGarbageDir = Properties.Settings.Default.PathGarbageDir;
            try
            {
                File.Move(pathFile, pathGarbageDir);
            }
            catch (Exception ex)
            {
                log.Error("Не удалось переместить файл в папку необработанных файлов." + ex);
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
                File.Move(pathFile, pathCompleteDir);
            }
            catch (Exception ex)
            {
                log.Error("Не удалось переместить файл  в папку обработанных файлов." + ex);
            }
        }
    }
}
