using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace e_library.kai.ru_book_downloader
{
    public partial class MainForm : Form
    {
        private Thread t;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            SavePath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            t = new Thread(WorkThread);
            if (!BookURL.Text.EndsWith(".pdf")) BookURL.Text = BookURL.Text.Remove(BookURL.Text.Length - 11);
            if (BookURL.Text.EndsWith(".pdf"))
            {
                t.Start();
                button2.Enabled = false;
            }
            else
            {
                MessageBox.Show("Неправильный URL");
            }
        }

        private void ProgressHandler(int i, int mode)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Status(SetProgessBar), i, mode);
            }
            else
            {
                SetProgessBar(i, mode);
            }
        }

        private void SetProgessBar(int i, int mode)
        {
            switch (mode)
            {
                case 0:
                    progressBar1.Value = i;
                    break;
                case 1:
                    progressBar1.Maximum = i;
                    break;
            }
        }

        private void TextStatusHandler(string str)
        {
            if (StatusLabel.InvokeRequired)
            {
                StatusLabel.Invoke(new TextStatus(SetStatusLabel), str);
            }
            else
            {
                SetStatusLabel(str);
            }
        }

        private void SetStatusLabel(string str)
        {
            StatusLabel.Text = str;
        }

        private void WorkThread()
        {
            var wc = new WorkClass(BookURL.Text, SavePath.Text + "\\" + BookName.Text);
            wc.status += ProgressHandler;
            wc.tstatus += TextStatusHandler;
            wc.Download();
            progressBar1.Maximum = wc.pagesCount;
            if (ConvertEn.Checked) wc.Convert(DelEn.Checked);
            if (PdfEn.Checked) wc.CreatePDF(DelEn.Checked);
            MessageBox.Show("Скачивание книги завершено!", "Все готово!", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            if (button2.InvokeRequired)
            {
                button2.Invoke(new TextStatus(BtEnable), "");
            }
        }

        private void BtEnable(string str)
        {
            button2.Enabled = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (t != null) t.Abort();
        }
    }
}