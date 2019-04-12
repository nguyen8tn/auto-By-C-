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
using System.Diagnostics;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs evt)
        {
            Process.Start("D:\\Program Files\\Nox\\bin\\Nox.exe");
        }

        private void openAzurLane(object sender, RoutedEventArgs e)
        {
            runAdb();
        }
        private void runAdb()
        {
            string result = "";
            StringBuilder output = new StringBuilder();
            Process cmd = new Process();
            string arg1 = @"cd\";
            string arg3 = @"cd Program Files\Nox\bin";
            string arg4 = @"nox_adb.exe connect 127.0.0.1:62001";
            string arg5 = @"adb shell screencap /sdcard/screen.png";
            string arg6 = @"adb pull /sdcard/screen.png D:\gundam";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            cmd.StartInfo = startInfo;
            cmd.Start();

            cmd.StandardInput.WriteLine(arg1);
            cmd.StandardInput.WriteLine(arg3);
            cmd.StandardInput.WriteLine(arg4);
            //(loop) Occurs each time an application writes a line to its redirected StandardOutput stream
            cmd.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if(!String.IsNullOrEmpty(e.Data))
                {
                    result = e.Data;
                    output.Append("\n" + e.Data);
                    if(result.Contains("cannot connect to"))
                    {
                        cmd.CancelOutputRead();//cancel asynchronous read operations
                    } 
                }
            });
            cmd.BeginOutputReadLine(); //asynchronous read operations-use to add e.Data to result
            if (!result.Contains("cannot connect to"))
            {
                cmd.StandardInput.WriteLine(arg5);
                cmd.StandardInput.WriteLine(arg6);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                //result = cmd.StandardOutput.ReadToEnd();//->exception asyn cannot mix syn -> cannot use both BeginOutputReadLine() and ReadToEnd()
            }
            MessageBox.Show(output.ToString());
        }
    }
}
