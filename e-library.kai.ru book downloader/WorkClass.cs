using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = System.Drawing.Image;
using Rectangle = iTextSharp.text.Rectangle;

namespace e_library.kai.ru_book_downloader
{
    internal delegate void Status(int count, int mode);

    internal delegate void TextStatus(string status);

    internal class WorkClass
    {
        private readonly String SavePath;
        private readonly String URL;
        private readonly ArrayList pages;
        private bool converted;
        private bool downloaded;
        private string nowDownloading;
        public int pagesCount = 0;
        public long size = 0;

        public WorkClass(string url, string savepath)
        {
            URL = url;
            SavePath = savepath;
            pages = new ArrayList();
        }

        public event TextStatus tstatus;
        public event Status status;

        private void SendTextStatus(string str)
        {
            if (tstatus != null) tstatus(str);
        }

        private void SendProgress(int i)
        {
            if (status != null) status(i + 1, 0);
        }

        private void GetPagesList()
        {
            SendTextStatus("Скачиваем список страниц...");
            var wc = new WebClient();
            string ret = null;
            try
            {
                ret = wc.DownloadString(URL + "/pages.xml");
            }
            catch (WebException we)
            {
                SendTextStatus(we.Message + "\n" + we.Status);
                return;
            }
            catch (NotSupportedException ne)
            {
                SendTextStatus(ne.Message);
                return;
            }

            var xmlDoc = new XmlDocument();
            ret = ret.Trim();
            xmlDoc.LoadXml(ret);

            foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
            {
                if (table.Name == "PageOrder")
                {
                    foreach (XmlNode ch in table.ChildNodes)
                    {
                        if (ch.Name == "PageData")
                        {
                            foreach (XmlAttribute attr in ch.Attributes)
                            {
                                if (attr.Name == "LargeFile")
                                {
                                    pages.Add("/" + attr.Value);
                                }
                            }
                        }
                    }
                }
            }
            pagesCount = pages.Count;
            if (status != null) status(pagesCount, 1);
            SendTextStatus("Количество страниц для загрузки - " + pagesCount);
        }

        public void Download()
        {
            GetPagesList();
            if (pagesCount == 0) return;
            if (!(Directory.Exists(SavePath)))
            {
                Directory.CreateDirectory(SavePath);
            }
            var wc = new WebClient();
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            for (int i = 0; i < pages.Count; i++)
            {
                SendProgress(i);
                downloaded = false;
                var page_uri = new Uri(URL + pages[i]);
                nowDownloading = "Загрузка страницы №" + (i + 1) + "...";
                SendTextStatus(nowDownloading);
                try
                {
                    wc.DownloadFileAsync(page_uri, SavePath + "\\" + (i + 1) + ".jpg");
                }
                catch (WebException we)
                {
                    SendTextStatus(we.Message + "\n" + we.Status);
                }
                catch (NotSupportedException ne)
                {
                    SendTextStatus(ne.Message);
                }
                while (!downloaded) Thread.Sleep(40);
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloaded = true;
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SendTextStatus(nowDownloading + " загружено " + e.BytesReceived + " байт");
        }

        public void Convert(bool delOldFiles)
        {
            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myImageCodecInfo = GetEncoderInfo("image/tiff");
            myEncoder = Encoder.Compression;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(
                myEncoder,
                (long) EncoderValue.CompressionCCITT4);
            myEncoderParameters.Param[0] = myEncoderParameter;
            for (int i = 0; i < pages.Count; i++)
            {
                SendProgress(i);
                SendTextStatus("Конвертирование страницы " + (i + 1) + " в Ч/Б tiff");
                Image tiffImg = Image.FromFile(SavePath + "\\" + (i + 1) + ".jpg");
                var bm = new Bitmap(tiffImg);
                MakeBalackWhite(bm);
                tiffImg.Dispose();
                bm.Save(SavePath + "\\" + (i + 1) + ".tiff", myImageCodecInfo, myEncoderParameters);
                bm.Dispose();
                if (delOldFiles) File.Delete(SavePath + "\\" + (i + 1) + ".jpg");
            }
            converted = true;
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        public void CreatePDF(bool delOldFiles)
        {
            var document = new Document(new Rectangle(421, 595));
            document.SetMargins(0, 0, 0, 0);
            string fileformat;
            if (converted) fileformat = ".tiff";
            else fileformat = ".jpg";
            using (var stream = new FileStream(SavePath + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                writer.SetFullCompression();
                document.Open();
                for (int i = 0; i < pages.Count; i++)
                {
                    SendProgress(i);
                    using (
                        var imageStream = new FileStream(SavePath + "\\" + (i + 1) + fileformat, FileMode.Open,
                            FileAccess.Read, FileShare.ReadWrite))
                    {
                        SendTextStatus("Добавление в pdf страницы " + (i + 1) + "...");
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);
                        image.ScaleToFit(421, 595);
                        document.Add(image);
                    }
                }
                if (delOldFiles) Directory.Delete(SavePath, true);
                document.Close();
            }
        }

        public void MakeBalackWhite(Bitmap bmp)
        {
            // Задаём формат Пикселя.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Получаем данные картинки.
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            //Блокируем набор данных изображения в памяти
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Получаем адрес первой линии.
            IntPtr ptr = bmpData.Scan0;

            int numBytes = bmpData.Width * bmp.Height * 4;
            byte[] rgbValues = new byte[numBytes];

            // Копируем значения в массив.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            // Перебираем пикселы по 4 байта на каждый и меняем значения
            for (int counter = 0; counter < rgbValues.Length; counter += 4)
            {
                var luma = (int)(rgbValues[counter + 0] * 0.3 + rgbValues[counter + 1] * 0.59 + rgbValues[counter + 2] * 0.11);
                byte color_b = 0;

                if (luma > 160) color_b = 255;

                rgbValues[counter + 0] = color_b;
                rgbValues[counter + 1] = color_b;
                rgbValues[counter + 2] = color_b;

            }
            // Копируем набор данных обратно в изображение
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Разблокируем набор данных изображения в памяти.
            bmp.UnlockBits(bmpData);
        }
    }
}