using VideoLibrary;
using Instadown_NetCore;
using System.Net;
using Newtonsoft.Json;
using System.Drawing.Imaging;

namespace YoutubeVideoDownloader
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog fbd;
        MediaClass m;
        string path;
        public Form1()
        {
            InitializeComponent();
            m = new MediaClass();
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            fbd = new FolderBrowserDialog() { Description = "Faylın yerini seçin !"};
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Media yüklənir ...", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (txtUrl.Text.ToString().ToLower().Contains("youtube"))
                {
                    YouTube yt = new YouTube();
                    Video video = yt.GetVideo(txtUrl.Text);
                    File.WriteAllBytes(fbd.SelectedPath + @"\" + video.FullName, video.GetBytes());
                }
                else
                {
                    m.inputUrl = txtUrl.Text;
                    m.DownloadImage(path);
                }
               
                MessageBox.Show("Media yükləndi ...", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Faylın yeri seçilmədi !", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        bool youtube = false;
        bool instagram = true;
        Video video;

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            youtube = false;
            instagram = true;
            try
            {
                if (txtUrl.Text.ToLower().Contains("youtube"))
                {
                    YouTube yt = new YouTube();
                    video = yt.GetVideo(txtUrl.Text);
                    youtube = true;
                }
                else
                {
                    m.inputUrl = txtUrl.Text;
                    WebClient webClient = new WebClient();
                    dynamic val = JsonConvert.DeserializeObject<object>(webClient.DownloadString(m.inputUrl + "?__a=1"));
                }
            }
            catch (Exception)
            {
                try
                {
                    m.inputUrl = txtUrl.Text.ToString();
                    m.DownloadImage(path);
                    instagram = true;
                }
                catch (Exception)
                {
                    instagram = false;
                }
            }
            finally
            {
                if (youtube)
                {
                    lblTittle.Text = "YouTube : " + video.Title;
                }
                else if (instagram)
                {
                    lblTittle.Text = "Instagram Media";
                }
                else
                {
                    lblTittle.Text = "Media tapılmadı...";
                }
            }
        }
    }
}