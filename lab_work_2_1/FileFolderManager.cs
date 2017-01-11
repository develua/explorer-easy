using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.InteropServices;

namespace lab_work_2_1
{
    class FileFolderManager
    {
        string path;

        public FileFolderManager()
        {
            path = "";
        }

        public void LoadDrives(ItemCollection colection)
        {
            colection.Clear();

            foreach (DriveInfo drive in DriveInfo.GetDrives())
                colection.Add(drive.ToString());
        }

        public void LoadFileFolder(ItemCollection colection, int index = -1)
        {
            if (index != -1)
            {
                if (colection[index].ToString() == "...")
                {
                    if (path.Length > 3)
                        path = Directory.GetParent(path).FullName;
                    else
                    {
                        path = "";
                        LoadDrives(colection);
                        return;
                    }
                }
                else
                {
                    if (Directory.Exists(Path.Combine(path, colection[index].ToString())))
                        path = Path.Combine(path, colection[index].ToString());
                    else
                        return;
                }
            }
            else
            {
                if (path == "")
                    return;
            }

            colection.Clear();

            DirectoryInfo dir = new DirectoryInfo(path);

            colection.Add("...");

            try
            {
                foreach (var item in dir.GetDirectories())
                    colection.Add(item.Name);

                foreach (var item in dir.GetFiles())
                    colection.Add(item.Name);
            }
            catch { }
        }

        public Task CopyFileFolder(string nameFileFolder, string selectedPath)
        {
            return Task.Factory.StartNew(() =>
            {
                if (nameFileFolder == "..." || path == "")
                    return;

                Thread.Sleep(300);

                string pathCombine = Path.Combine(path, nameFileFolder);

                try
                {
                    if (Directory.Exists(pathCombine))
                        DirectoryCopy(pathCombine, Path.Combine(selectedPath, nameFileFolder));
                    else
                        File.Copy(pathCombine, Path.Combine(selectedPath, Path.Combine(selectedPath, nameFileFolder)));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        public Task MoveFileFolder(string nameFileFolder, string selectedPath)
        {
            return Task.Factory.StartNew(() =>
            {
                if (nameFileFolder == "..." || path == "")
                    return;

                Thread.Sleep(300);

                string pathCombine = Path.Combine(path, nameFileFolder);

                try
                {
                    if (Directory.Exists(pathCombine))
                    {
                        DirectoryCopy(pathCombine, Path.Combine(selectedPath, nameFileFolder));
                        Directory.Delete(pathCombine, true);
                    }
                    else
                        File.Move(pathCombine, Path.Combine(selectedPath, nameFileFolder));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        public Task DelFileFolder(string nameFileFolder)
        {
            return Task.Factory.StartNew(() =>
            {
                if (nameFileFolder == "..." || path == "")
                    return;

                Thread.Sleep(300);

                string pathCombine = Path.Combine(path, nameFileFolder);

                try
                {
                    if (Directory.Exists(pathCombine))
                        Directory.Delete(pathCombine);
                    else
                        File.Delete(pathCombine);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void DirectoryCopy(string SourcePath, string DestinationPath)
        {
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
            SearchOption.AllDirectories))
            Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
            SearchOption.AllDirectories))
            File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }
    }
}
