using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevNet
{
    public partial class FrmInstaller : Form
    {
        private string LOCATION;
        private string VERSION;
        private static System.Net.WebClient client;

        private string ARCHIVE_PATH
        {
            get
            {
                return System.IO.Path.Combine(LOCATION ?? System.IO.Directory.GetCurrentDirectory(), "chromedriver.zip").Replace("\\", "/");
            }
        }
        private string ARCHIVE_DIRECTORY
        {
            get
            {
                return LOCATION ?? System.IO.Directory.GetCurrentDirectory();
            }
        }
        public FrmInstaller(string LOCATION, string VERSION, string TITLE, string MESSAGE)
        {
            InitializeComponent();
            this.LOCATION = LOCATION;
            this.VERSION = VERSION;
            this.Text = TITLE;
            this.lblMessage.Text = MESSAGE;
        }

        private void FrmInstaller_Load(object sender, EventArgs e)
        {
            try { System.IO.File.Delete(ARCHIVE_PATH); } catch { }
            using (client = new System.Net.WebClient())
            {
                System.Uri uri = new System.Uri("https://chromedriver.storage.googleapis.com/" + VERSION + "/chromedriver_win32.zip");
                client.DownloadProgressChanged += DownloadProgressChanged;
                client.DownloadFileAsync(uri, ARCHIVE_PATH);
            }
        }

        private void FrmInstaller_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(client != null) client.Dispose();
        }

        private void DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            this.progressPercent.Text = $"{e.ProgressPercentage}%";
        }
        private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.lblMessage.Text = "Chromedriver downoad successful,\nInstallation is in progress.";
            if (e.Cancelled)
            {
                MessageBox.Show("The chromedriver installation has been cancelled", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e.Error != null) // We have an error! Retry a few times, then abort.
            {
                MessageBox.Show("An error ocurred while trying to install chromedriver", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            Utility.WaitForFileToBeReady(ARCHIVE_PATH, System.TimeSpan.FromSeconds(30));
            using (System.IO.Compression.ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(ARCHIVE_PATH))
            {
                Utility.DestroyAllChromeDrivers();
                foreach (System.IO.Compression.ZipArchiveEntry entry in archive.Entries)
                {
                    try { System.IO.File.Delete(System.IO.Path.Combine(ARCHIVE_DIRECTORY, entry.FullName)); } catch { }
                    try { entry.ExtractToFile(System.IO.Path.Combine(ARCHIVE_DIRECTORY, entry.FullName)); } catch { }
                }
            }
            try { System.IO.File.Delete(ARCHIVE_PATH); } catch { }
            MessageBox.Show("Chromedriver installed successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void lnkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
