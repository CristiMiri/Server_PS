using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class FileManager
    {
        private string _imagesDirectory;
        private string _documentsDirectory;

        public FileManager()
        {
            InitializeDirectories();
        }


        private void InitializeDirectories()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(appDirectory, @"..\..\.."));
            _imagesDirectory = Path.Combine(projectRoot, "Images");
            _documentsDirectory = Path.Combine(projectRoot, "Documents");
        }

        public void CopyFile(string sourcePath, string destinationDirectory)
        {
            string fileName = Path.GetFileName(sourcePath);
            string destinationPath = Path.Combine(destinationDirectory, fileName);
            File.Copy(sourcePath, destinationPath, true);
        }

        public string GetImageFilePath(string fileName)
        {
            return Path.Combine(_imagesDirectory, fileName);
        }
        public string GetDocumentFilePath(string fileName)
        {
            return Path.Combine(_documentsDirectory, fileName);
        }


    }
}
