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
        private readonly ArrayList _pages;
        private readonly String _savePath;
        private readonly String _url;
        public int PagesCount = 0;
        public long Size = 0;
        private bool _converted;
        private bool _downloaded;
        private string _nowDownloading;

        public WorkClass(string url, string savepath)
        {
            _url = url;
            _savePath = savepath;
            _pages = new ArrayList();
        }

        public event TextStatus Tstatus;
        public event Status Status;

        private void SendTextStatus(string str)
        {
            if (Tstatus != null) Tstatus(str);
        }

        private void SendProgress(int i)
        {
            if (Status != null) Status(i + 1, 0);
        }

        private void GetPagesList()
        {
            SendTextStatus("Скачиваем список страниц...");
            var wc = new WebClient();
            string ret;
            try
            {
                ret = wc.DownloadString(_url + "/pages.xml");
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

            if (xmlDoc.DocumentElement != null)
            {
                foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (table.Name == "PageOrder")
                    {
                        foreach (XmlNode ch in table.ChildNodes)
                        {
                            if (ch.Name == "PageData")
                            {
                                if (ch.Attributes != null)
                                {
                                    foreach (XmlAttribute attr in ch.Attributes)
                                    {
                                        if (attr.Name == "LargeFile")
                                        {
                                            _pages.Add("/" + attr.Value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            PagesCount = _pages.Count;
            if (Status != null) Status(PagesCount, 1);
            SendTextStatus("Количество страниц для загрузки - " + PagesCount);
        }

        public void Download()
        {
            int count = 0;
            GetPagesList();
            if (PagesCount == 0) return;
            if (!(Directory.Exists(_savePath)))
            {
                Directory.CreateDirectory(_savePath);
            }
            var wc = new WebClient();
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += wc_DownloadFileCompleted;


            for (int i = 0; i < _pages.Count; i++)
            {
                int downCount = 0;
                bool fullDownloaded = false;
                while (!fullDownloaded)
                {
                    fullDownloaded = true;
                    SendProgress(i);
                    _downloaded = false;
                    var pageUri = new Uri(_url + _pages[i]);
                    _nowDownloading = "Загрузка страницы №" + (i + 1) + (downCount>0 ? "(попытка " +(downCount+1) + ")" : "") + "...";
                    SendTextStatus(_nowDownloading);
                    try
                    {
                        wc.DownloadFileAsync(pageUri, _savePath + "\\" + (i + 1) + ".jpg");
                    }
                    catch (WebException we)
                    {
                        SendTextStatus(we.Message + "\n" + we.Status);
                    }
                    catch (NotSupportedException ne)
                    {
                        SendTextStatus(ne.Message);
                    }
                    catch (Exception ex)
                    {
                        SendTextStatus(ex.Message);
                    }
                    while (!_downloaded)
                    {
                        Thread.Sleep(10);
                        count++;
                        if (count > 1000)
                        {
                            count = 0;
                            wc.CancelAsync();
                            fullDownloaded = false;
                            downCount++;
                        }
                    }

                }
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _downloaded = true;
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SendTextStatus(_nowDownloading + " загружено " + e.BytesReceived + " байт");
        }

        public void Convert(bool delOldFiles)
        {
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/tiff");
            Encoder myEncoder = Encoder.Compression;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(
                myEncoder,
                (long) EncoderValue.CompressionCCITT4);
            myEncoderParameters.Param[0] = myEncoderParameter;
            for (int i = 0; i < _pages.Count; i++)
            {
                SendProgress(i);
                SendTextStatus("Конвертирование страницы " + (i + 1) + " в Ч/Б tiff");
                Image tiffImg = Image.FromFile(_savePath + "\\" + (i + 1) + ".jpg");
                var bm = new Bitmap(tiffImg);
                MakeBalackWhite(bm);
                tiffImg.Dispose();
                bm.Save(_savePath + "\\" + (i + 1) + ".tiff", myImageCodecInfo, myEncoderParameters);
                bm.Dispose();
                if (delOldFiles) File.Delete(_savePath + "\\" + (i + 1) + ".jpg");
            }
            _converted = true;
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
            string fileformat = _converted ? ".tiff" : ".jpg";
            using (var stream = new FileStream(_savePath + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                writer.SetFullCompression();
                document.Open();
                for (int i = 0; i < _pages.Count; i++)
                {
                    SendProgress(i);
                    using (
                        var imageStream = new FileStream(_savePath + "\\" + (i + 1) + fileformat, FileMode.Open,
                            FileAccess.Read, FileShare.ReadWrite))
                    {
                        SendTextStatus("Добавление в pdf страницы " + (i + 1) + "...");
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);
                        image.ScaleToFit(421, 595);
                        document.Add(image);
                    }
                }
                if (delOldFiles) Directory.Delete(_savePath, true);
                document.Close();
            }
        }

        public void MakeBalackWhite(Bitmap bmp)
        {
            // Задаём формат Пикселя.
            const PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Получаем данные картинки.
            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            //Блокируем набор данных изображения в памяти
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Получаем адрес первой линии.
            IntPtr ptr = bmpData.Scan0;

            int numBytes = bmpData.Width*bmp.Height*4;
            var rgbValues = new byte[numBytes];

            // Копируем значения в массив.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            // Перебираем пикселы по 4 байта на каждый и меняем значения
            for (int counter = 0; counter < rgbValues.Length; counter += 4)
            {
                var luma =
                    (int) (rgbValues[counter + 0]*0.3 + rgbValues[counter + 1]*0.59 + rgbValues[counter + 2]*0.11);
                byte color = 0;

                if (luma > 160) color = 255;

                rgbValues[counter + 0] = color;
                rgbValues[counter + 1] = color;
                rgbValues[counter + 2] = color;
            }
            // Копируем набор данных обратно в изображение
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Разблокируем набор данных изображения в памяти.
            bmp.UnlockBits(bmpData);
        }
    }
}
