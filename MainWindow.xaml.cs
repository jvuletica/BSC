using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tamir.SharpSsh;

namespace BSC
{
    public partial class MainWindow : Window
    {
        //private SshClient ssh;
        //private SshCommand cmd;
        private SshStream ssh;
        private int contentLength;
        public MainWindow()
        {
            InitializeComponent();
            grid.Children.Remove(console);
            this.Closed += new EventHandler((object sender, EventArgs e) =>
            {
                if(ssh != null)
                {
                    ssh.Close();
                }
            });
            connectBtn.Click += connectBtn_Click;
        }

        void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (connectBtn.Content == "Disconnect")
            {
                ssh.Close();
                grid.Children.Remove(console);
                CreateLoginUI();
                connectBtn.Content = "Connect";
            }
            else ConnectToServer();
        }

        private void DeleteLoginUI()
        {
            grid.Children.Remove(hostLabel);
            grid.Children.Remove(hostString);
            grid.Children.Remove(userLabel);
            grid.Children.Remove(userString);
            grid.Children.Remove(passwordLabel);
            grid.Children.Remove(passwordString);
        }

        private void CreateLoginUI()
        {
            grid.Children.Add(hostLabel);
            grid.Children.Add(hostString);
            grid.Children.Add(userLabel);
            grid.Children.Add(userString);
            grid.Children.Add(passwordLabel);
            grid.Children.Add(passwordString);
        }

        private async void ConnectToServer()
        {
            connectBtn.Content = "Connecting...";
            string host = hostString.Text;
            string username = userString.Text;
            string password = passwordString.Text;
            await Task.Run(() => {
                try
                {
                    ssh = new SshStream(host, username, password);
                    ssh.Prompt = "%";
                    ssh.RemoveTerminalEmulationCharacters = true;
                }
                catch {/*failed to connect*/}
            });
            if (ssh != null)
            {
                DeleteLoginUI();
                grid.Children.Add(console);
                connectBtn.Content = "Disconnect";
                console.Text = ssh.ReadResponse();
                contentLength = console.Text.Length;
                console.CaretIndex = contentLength;
            }
            else
                connectBtn.Content = "Failed to connect!\nRetry?";
        }

        private void CheckKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                RunCommand();
            else if (e.Key == Key.Back)
                PreventOldContentModification();
            else if (e.Key == Key.Up || e.Key == Key.Down)
                PreventOldContentModification();
            else if (e.Key == Key.Left || e.Key == Key.Right)
                PreventOldContentModification();
        }

        private void CheckClick(object sender, MouseButtonEventArgs e)
        {
            PreventOldContentModification();
        }

        private void PreventOldContentModification()
        {
            console.IsReadOnlyCaretVisible = true;
            if (console.CaretIndex < contentLength)
                console.IsReadOnly = true;
            else
                console.IsReadOnly = false;
        }

        private void RunCommand()
        {
            string commandString = console.Text.Substring(contentLength);
            string[] commandArray = commandString.Split();
            if (commandArray[0] == "download")
            {
                try
                {
                    DownloadFile(commandArray[1]);
                }
                catch
                {
                    var warning = MessageBox.Show("No arguments passed to download command");
                }
            }
            else
            {
                ssh.Write(commandString);
                console.Text += ssh.ReadResponse().Substring(commandString.Length);
            }
            contentLength = console.Text.Length;
            console.CaretIndex = contentLength;
        }

        private async void DownloadFile(string downloadTarget)
        {
            string host = hostString.Text;
            string username = userString.Text;
            string password = passwordString.Text;
            string target = GetCleanCurrentDir() + "/" + downloadTarget;
            //console.Text += target;
            await Task.Run(() =>
            {
                try
                {
                    Sftp sftp = new Sftp(host, username, password);
                    sftp.Connect();
                    sftp.Get(target);
                    sftp.Close();
                    var success = MessageBox.Show("Download Complete");
                }
                catch
                {
                    var fail = MessageBox.Show("Download Failed");
                }
            });
        }

        private void DropEnableHack(object sender, DragEventArgs e)
        {
            //prevent bubbling of events
            e.Handled = true;
        }

        private async void UploadFile(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string host = hostString.Text;
            string username = userString.Text;
            string password = passwordString.Text;
            string dir = GetCleanCurrentDir();
            string filename = System.IO.Path.GetFileName(files[0]);
            string target = dir + "/" + filename;
            await Task.Run(() => {
                try
                {
                    Sftp sftp = new Sftp(host, username, password);
                    sftp.Connect();
                    sftp.Put(files[0], target);
                    sftp.Close();
                    var success = MessageBox.Show("Upload Complete");
                }
                catch
                {
                    var fail = MessageBox.Show("Upload Failed");
                }
            });
        }

        private string GetCleanCurrentDir()
        {
            ssh.Write("pwd");
            string dirResponse = ssh.ReadResponse();
            ssh.Write("");
            string prompt = ssh.ReadResponse();
            prompt = System.Text.RegularExpressions.Regex.Replace(prompt, @"\r\n?|\n", "");
            dirResponse = dirResponse.Replace("pwd", "");
            dirResponse = System.Text.RegularExpressions.Regex.Replace(dirResponse, @"\r\n?|\n", "");
            dirResponse = dirResponse.Replace(prompt, "");
            return dirResponse;
        }

        private void LoginReturnKeyToConnect(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
                ConnectToServer();
        }

        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            string message = "WARNING!\n"
                + "- only single file upload/download currently available\n"
                + "- missing TAB autocomplete\n\n"
                + "- drop file into console to upload it to current directory"
                + "\n- use download command to download it, e.g. 'download relativePath.txt'"
                + "\n\n Author: Josip Vuletić Antić";
            var help = MessageBox.Show(message);
        }
    }
}
