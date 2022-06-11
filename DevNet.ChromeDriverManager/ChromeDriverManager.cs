using System.IO.Compression;

namespace DevNet
{
    public class ChromeDriverManager
    {
        private static string _LOCATION = System.IO.Directory.GetCurrentDirectory();
        private static string ARCHIVE_PATH
        {
            get
            {
                return System.IO.Path.Combine(_LOCATION ?? System.IO.Directory.GetCurrentDirectory(), "chromedriver.zip").Replace("\\", "/");
            }
        }
        private static string ARCHIVE_DIRECTORY
        {
            get
            {
                return _LOCATION ?? System.IO.Directory.GetCurrentDirectory();
            }
        }
        private static string CHROMEDRIVER_PATH
        {
            get
            {
                return System.IO.Path.Combine(_LOCATION ?? System.IO.Directory.GetCurrentDirectory(), "chromedriver.exe").Replace("\\", "/");
            }
        }

        // public (set)
        public static string LOCATION
        {
            set
            {
                _LOCATION = value;
            }
        }


        private static int? _DOWNLOAD_PROGRESS_PERCENTAGE = null;
        public static int? DOWNLOAD_PROGRESS_PERCENTAGE
        {
            get { return _DOWNLOAD_PROGRESS_PERCENTAGE; }
        }

        // public (get)
        public static string LATEST_VERSION
        {
            get
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
        }

        // public (get)
        public static string INSTALLED_VERSION
        {
            get
            {
                try
                {
                    if (System.IO.File.Exists(CHROMEDRIVER_PATH))
                        return Utility.ExecuteCommandSync($"{CHROMEDRIVER_PATH} -v").Split(' ')[1];
                }
                catch { }
                return null;
            }
        }

        private static System.Net.WebClient client;


        public static void InstallOrUpdate()
        {
            try { System.IO.File.Delete(ARCHIVE_PATH); } catch { }
            using (client = new System.Net.WebClient())
            {
                System.Uri uri = new System.Uri("https://chromedriver.storage.googleapis.com/" + LATEST_VERSION + "/chromedriver_win32.zip");
                client.DownloadProgressChanged += DownloadProgressChanged;
                client.DownloadFileAsync(uri, ARCHIVE_PATH);
            }
        }
        public static void Uninstall()
        {
            Utility.DestroyAllChromeDrivers();
            try { System.IO.File.Delete(CHROMEDRIVER_PATH); } catch { }
            try { System.IO.File.Delete(ARCHIVE_PATH); } catch { }
        }
        private static void DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 100)
            {
                _DOWNLOAD_PROGRESS_PERCENTAGE = e.ProgressPercentage;
            }
            else
            {
                Utility.WaitForFileToBeReady(ARCHIVE_PATH, System.TimeSpan.FromSeconds(30));
                using (System.IO.Compression.ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(ARCHIVE_PATH))
                {
                    Utility.DestroyAllChromeDrivers();
                    foreach (System.IO.Compression.ZipArchiveEntry entry in archive.Entries)
                    {
                        try { System.IO.File.Delete(System.IO.Path.Combine(ARCHIVE_DIRECTORY, entry.FullName)); } catch { }
                        entry.ExtractToFile(System.IO.Path.Combine(ARCHIVE_DIRECTORY, entry.FullName));
                    }
                }
                System.IO.File.Delete(ARCHIVE_PATH);
                _DOWNLOAD_PROGRESS_PERCENTAGE = null;
                client.DownloadProgressChanged -= DownloadProgressChanged;
            }

        }
    }
}
