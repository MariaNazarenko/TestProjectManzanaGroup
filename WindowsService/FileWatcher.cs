using System;
using System.IO;
using log4net;

namespace WindowsService
{
    /// <summary>
    /// Класс для реализации наблючения за файлами
    /// </summary>
    public class FileWatcher
    {
        private static readonly ILog Log = LogManager.GetLogger("LOGGER");
        private FileSystemWatcher fileWatcher;

        public FileWatcher(string watcherDir)
        {
            fileWatcher = new FileSystemWatcher(watcherDir);
            fileWatcher.EnableRaisingEvents = true;
            fileWatcher.IncludeSubdirectories = false;
            EventCreateFile += FileCreate;
            Log.Info("Начинается наблюдение за папкой " + watcherDir);
        }

        public delegate void GetDataHandler(string pathFile, string data);

        private event FileSystemEventHandler EventCreateFile
        {
            add { fileWatcher.Created += value; }

            remove { fileWatcher.Created -= value; }
        }

        public event GetDataHandler OnGetData;

        /// <summary>
        /// Перемещение файла в папку необработанных файлов
        /// </summary>
        /// <param name="pathFile">Путь к исходному файлу</param>
        public void MoveFileinGarbage(string pathFile)
        {
            string pathGarbageDir = Properties.Settings.Default.PathGarbageDir;
            try
            {
                Directory.CreateDirectory(pathGarbageDir);
                File.Move(pathFile, pathGarbageDir + "\\" + Path.GetFileName(pathFile));
            }
            catch (Exception ex)
            {
                Log.Error("Не удалось переместить файл в папку необработанных файлов.", ex);
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
                Log.Error("Не удалось переместить файл  в папку обработанных файлов.", ex);
            }
        }

        /// <summary>
        /// Метод возникающий при появлении файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileCreate(object sender, FileSystemEventArgs e)
        {
            var pathFile = e.FullPath;
            Log.Info("В папке обнаружен файл " + pathFile);
            if (Path.GetExtension(pathFile) == ".txt")
            {
                string data = null;
                using (var streamReader = new StreamReader(pathFile))
                {
                    data = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                if (!string.IsNullOrEmpty(data))
                {
                    OnGetData?.Invoke(pathFile, data);
                    return;
                }
                else
                {
                    Log.Error("Не удалось считать данные или файл пуст");
                }
            }
            else
            {
                Log.Error("Расширение файла не соответствует txt");
            }

            MoveFileinGarbage(pathFile);
        }
    }
}
