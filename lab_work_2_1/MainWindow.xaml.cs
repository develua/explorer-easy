using MahApps.Metro.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using System.Threading;
using System;

namespace lab_work_2_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        FileFolderManager manager;

        public MainWindow()
        {
            InitializeComponent();

            manager = new FileFolderManager();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            manager.LoadDrives(listFileFolder.Items);
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyMoveFileFolder(manager.CopyFileFolder);
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            CopyMoveFileFolder(manager.MoveFileFolder);
        }

        private async void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if(listFileFolder.SelectedItem == null)
                return;

            MessageBoxResult resultMB = MessageBox.Show("Вы уверены, что хотите произвести удаление?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultMB == MessageBoxResult.No)
                return;

            progressBar.Maximum = listFileFolder.SelectedItems.Count;

            for (int i = 0; i < listFileFolder.SelectedItems.Count; i++)
            {
                await manager.DelFileFolder(listFileFolder.SelectedItems[i].ToString());
                progressBar.Value = i + 1;
            }

            await Task.Factory.StartNew(() => Thread.Sleep(1000));
            progressBar.Value = 0;
            manager.LoadFileFolder(listFileFolder.Items);
        }

        private void listFileFolder_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listFileFolder.SelectedItem != null)
                manager.LoadFileFolder(listFileFolder.Items, listFileFolder.SelectedIndex);
        }

        private async void CopyMoveFileFolder(Func<string, string, Task> methodCopyMove)
        {
            if (listFileFolder.SelectedItem == null)
                return;

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult resultDialog = dialog.ShowDialog();

            if (resultDialog == System.Windows.Forms.DialogResult.OK)
            {
                progressBar.Maximum = listFileFolder.SelectedItems.Count;

                for (int i = 0; i < listFileFolder.SelectedItems.Count; i++)
                {
                    await methodCopyMove(listFileFolder.SelectedItems[i].ToString(), dialog.SelectedPath);
                    progressBar.Value = i + 1;
                }

                await Task.Factory.StartNew(() => Thread.Sleep(1000));
                progressBar.Value = 0;
                manager.LoadFileFolder(listFileFolder.Items);
            }
        }
    }
}
