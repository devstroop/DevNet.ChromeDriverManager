namespace DevNet
{
    public class Utility
    {
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
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
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
