using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    public class FileWatcherHanlder
    {
        // The locally stored file watchers
        private static List<FileSystemWatcher> fileWatchers = new List<FileSystemWatcher>();


        /// <summary>
        /// Creates the output file watcher that activates when anything in the folder changes
        /// </summary>
        /// <param name="folder">The folder being watched</param>
        /// <param name="task">The task being activated</param>
        /// <param name="includeSubfolders">Does it include any subfolders?</param>
        public static void CreateFileWatcher(string folder, Action task, bool includeSubfolders = false)
        {
            try
            {
                // If the folder exists create the filewatcher
                if (Directory.Exists(folder))
                {
                    FileSystemWatcher fileWatcher = new FileSystemWatcher();
                    fileWatcher.Path = folder;
                    fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
                    fileWatcher.Filter = "*.*";
                    fileWatcher.Changed += new FileSystemEventHandler((sender, e) => task.Invoke());
                    fileWatcher.Created += new FileSystemEventHandler((sender, e) => task.Invoke());
                    fileWatcher.EnableRaisingEvents = true;
                    fileWatcher.IncludeSubdirectories = includeSubfolders;

                    Task once = new Task(() => { task.Invoke(); });
                    once.Start();

                    // Store it local;y 
                    fileWatchers.Add(fileWatcher);
                }
                else
                {
                    // If the file doesn't exist
                    Console.WriteLine($"{folder} doesn't exist!");
                }
            }
            catch (Exception ex)
            {
                // If something went wrong in the configuration
                Console.WriteLine($"Error when configuring folder {folder} \r\n {ex.Message}");
            }
        }
    }
}
