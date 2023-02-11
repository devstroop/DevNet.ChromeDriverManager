using DevNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet
{
    public class ChromeDriverManager
    {
        private string Location;


        private string CHROMEDRIVER_PATH
        {
            get
            {
                return System.IO.Path.Combine(Location ?? System.IO.Directory.GetCurrentDirectory(), "chromedriver.exe").Replace("\\", "/");
            }
        }



        public ChromeDriverManager(string Location = null)
        {
            this.Location = Location ?? System.IO.Directory.GetCurrentDirectory();
        }

        public string InstalledVersion()
        {
            try
            {
                if (System.IO.File.Exists(CHROMEDRIVER_PATH))
                    System.Console.WriteLine(CHROMEDRIVER_PATH);
                return Utility.ExecuteCommandSync($"\"{CHROMEDRIVER_PATH}\" -v").Split(' ')[1];
            }
            catch { }
            return null;
        }
        public string LatestAvailable()
        {
            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    return client.DownloadString("https://chromedriver.storage.googleapis.com/LATEST_RELEASE");
                }
            }
            catch
            {
                return null;
            }
        }

        public void Install(string Version = null)
        {
            Version = Version ?? LatestAvailable();
            FrmInstaller frmInstaller = new FrmInstaller(Location, Version, "Installing driver", $"Please wait,\nChromedriver ver. {Version} installation in progress");
            frmInstaller.ShowDialog();
        }
    }
}
