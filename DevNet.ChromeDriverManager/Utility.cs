using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet
{
    public class Utility
    {
        public static string GetGoogleChromeInstalledVersion()
        {
            bool is64bit = !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
            if (is64bit)
            {
                try
                {
                    return System.Diagnostics.FileVersionInfo.GetVersionInfo(@"C:\Program Files\Google\Chrome\Application\chrome.exe").FileVersion;
                }
                catch { }
            }
            else
            {
                try
                {
                    return System.Diagnostics.FileVersionInfo.GetVersionInfo(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe").FileVersion;
                }
                catch { }
            }
            return null;
        }
        public async static void DestroyAllChromeDrivers()
        {
            try
            {
                System.Diagnostics.ProcessStartInfo cmdsi = new System.Diagnostics.ProcessStartInfo("taskkill.exe");
                cmdsi.Arguments = "/F /IM chromedriver.exe /T";
                System.Diagnostics.Process cmd = System.Diagnostics.Process.Start(cmdsi);
                cmd.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.WaitForExit();
            }
            catch { }
            await System.Threading.Tasks.Task.Run(async delegate
            {
                await System.Threading.Tasks.Task.Delay(500);
            });
        }

        public static string ExecuteCommandSync(object command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                return proc.StandardOutput.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }
        public static bool IsFileReady(string filename)
        {
            try
            {
                using (System.IO.FileStream inputStream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                    return inputStream.Length > 0;
            }
            catch
            {
                return false;
            }
        }
        public static void WaitForFileToBeReady(string filename, System.TimeSpan timeout)
        {
            System.DateTime startedOn = System.DateTime.Now;
            while (!IsFileReady(filename))
            {
                if (startedOn.AddMilliseconds(timeout.Milliseconds) >= System.DateTime.Now)
                    break;
            }
        }

    }
}
