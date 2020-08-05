using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonLibrary.Handlers
{
    /// <summary>
    /// Misc file handling functions
    /// </summary>
    public static class FileHandler
    {
        /// <summary>
        /// Creates the full file path if it doesn't exist
        /// </summary>
        /// <param name="fileName">The file path being created</param>
        public static void CreateFile(string fileName)
        {
            // Create the Directory of the file if it doesn't exist
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            // Create file if it doesn't exist
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }
    }
}
