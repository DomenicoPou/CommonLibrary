using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonLibrary.Handlers
{
    public static class FileHandler
    {
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
