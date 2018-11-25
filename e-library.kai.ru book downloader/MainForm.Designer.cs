namespace e_library.kai.ru_book_downloader
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.BookURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SavePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BookName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DelEn = new System.Windows.Forms.CheckBox();
            this.PdfEn = new System.Windows.Forms.CheckBox();
            this.ConvertEn = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL книги:";
            // 
            // BookURL
            // 
            this.BookURL.Location = new System.Drawing.Point(93, 7);
            this.BookURL.Name = "BookURL";
            this.BookURL.Size = new System.Drawing.Size(479, 20);
            this.BookURL.TabIndex = 1;
            this.BookURL.Text = "http://e-library.kai.ru/reader/hu/flipping/Resource-512/810080.pdf/index.html";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Куда качать:";
            // 
            // SavePath
            // 
            this.SavePath.Location = new System.Drawing.Point(93, 32);
            this.SavePath.Name = "SavePath";
            this.SavePath.Size = new System.Drawing.Size(381, 20);
            this.SavePath.TabIndex = 3;
            this.SavePath.Text = "C:\\";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Название:";
            // 
            // BookName
            // 
            this.BookName.Location = new System.Drawing.Point(93, 57);
            this.BookName.Name = "BookName";
            this.BookName.Size = new System.Drawing.Size(479, 20);
            this.BookName.TabIndex = 5;
            this.BookName.Text = "Тестовая загрузка";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(480, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Обзор...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StatusLabel);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Location = new System.Drawing.Point(15, 182);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(557, 70);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Статус";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(6, 16);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(68, 13);
            this.StatusLabel.TabIndex = 2;
            this.StatusLabel.Text = "Ожидание...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(545, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DelEn);
            this.groupBox2.Controls.Add(this.PdfEn);
            this.groupBox2.Controls.Add(this.ConvertEn);
            this.groupBox2.Location = new System.Drawing.Point(15, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(557, 93);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки";
            // 
            // DelEn
            // 
            this.DelEn.AutoSize = true;
            this.DelEn.Location = new System.Drawing.Point(6, 65);
            this.DelEn.Name = "DelEn";
            this.DelEn.Size = new System.Drawing.Size(416, 17);
            this.DelEn.TabIndex = 2;
            this.DelEn.Text = "Удалить все ненужные файлы (например, все картинки после создания pdf)";
            this.DelEn.UseVisualStyleBackColor = true;
            // 
            // PdfEn
            // 
            this.PdfEn.AutoSize = true;
            this.PdfEn.Location = new System.Drawing.Point(6, 42);
            this.PdfEn.Name = "PdfEn";
            this.PdfEn.Size = new System.Drawing.Size(182, 17);
            this.PdfEn.TabIndex = 1;
            this.PdfEn.Text = "Создавать на выходе файл pdf";
            this.PdfEn.UseVisualStyleBackColor = true;
            // 
            // ConvertEn
            // 
            this.ConvertEn.AutoSize = true;
            this.ConvertEn.Location = new System.Drawing.Point(6, 19);
            this.ConvertEn.Name = "ConvertEn";
            this.ConvertEn.Size = new System.Drawing.Size(451, 17);
            this.ConvertEn.TabIndex = 0;
            this.ConvertEn.Text = "Ковертировать страницы в Ч/Б tiff (очень сильное уменьшение итогового размера)";
            this.ConvertEn.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(14, 258);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(558, 42);
            this.button2.TabIndex = 10;
            this.button2.Text = "Старт!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 312);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BookName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SavePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BookURL);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "e-library.kai.ru book downloader";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox BookURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SavePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BookName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox DelEn;
        private System.Windows.Forms.CheckBox PdfEn;
        private System.Windows.Forms.CheckBox ConvertEn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button2;
    }
}

