using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBackUpService
{
    public class BackupFileService
    {
        static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB" };

        public List<Folder> getBackupFolders(string path, bool SkipSize=true)
        {
            List<Folder> folders = new List<Folder>();
            foreach (var dirPath in Directory.GetDirectories(path))
            {
                long size = 0;
                var dirInfo = new DirectoryInfo(dirPath);
                if (!SkipSize)
                {
                    foreach (FileInfo fi in dirInfo.GetFiles("*", SearchOption.AllDirectories))
                    {
                        size += fi.Length;
                    }
                }
                folders.Add(new Folder {
                    Name = dirInfo.Name,
                    DirInfo = dirInfo,
                    Size = FormatSize(size)
                });
            }
            return folders.OrderBy(x => x.DirInfo.Name).ToList();
        }
        public List<BackupFile> GetBackupFile(string path)
        {
            List<BackupFile> files = new List<BackupFile>();
            var dirInfo = new DirectoryInfo(path);
            foreach (var file in Directory.GetFiles(path))
            {
                FileInfo fileInfo = new FileInfo(file);
                files.Add(
                    new BackupFile
                    {
                        FileName = fileInfo.Name,
                        CreatedOn = fileInfo.LastAccessTime.ToString(),
                        Size = FormatSize(fileInfo.Length),
                        DirInfo = dirInfo
                    });
            }
            return files.OrderByDescending(x=>x.CreatedOn).ToList();
        }
        public static string FormatSize(long bytes)
        {
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1} {1}", number, suffixes[counter]);
        }  
    }
}
