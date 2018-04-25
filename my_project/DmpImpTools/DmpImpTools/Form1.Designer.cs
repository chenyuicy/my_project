namespace DmpImpTools
{
    partial class s
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OraDbnameLEdt = new System.Windows.Forms.TextBox();
            this.OraPasswordLEdt = new System.Windows.Forms.TextBox();
            this.OraUserLEdt = new System.Windows.Forms.TextBox();
            this.OraIPLEdt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FtpPasswordLEdt = new System.Windows.Forms.TextBox();
            this.FtpUserLEdt = new System.Windows.Forms.TextBox();
            this.FtpIPLEdt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.LocalPathLEdt = new System.Windows.Forms.TextBox();
            this.LocalPathBtn = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.FtpPathLEdt = new System.Windows.Forms.TextBox();
            this.textBox_destPort = new System.Windows.Forms.TextBox();
            this.textBox_destPassword = new System.Windows.Forms.TextBox();
            this.textBox_destUser = new System.Windows.Forms.TextBox();
            this.textBox_destIP = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_File = new System.Windows.Forms.TextBox();
            this.ConfigfileBtn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.PeriodLEdt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.SaveDayLab = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.ImpDate = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.FileDateBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(275, 397);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "确定";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(407, 397);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "取消";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "User / Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "IP / DataBase";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "Oracle数据库";
            // 
            // OraDbnameLEdt
            // 
            this.OraDbnameLEdt.Location = new System.Drawing.Point(160, 191);
            this.OraDbnameLEdt.Name = "OraDbnameLEdt";
            this.OraDbnameLEdt.Size = new System.Drawing.Size(57, 21);
            this.OraDbnameLEdt.TabIndex = 6;
            this.OraDbnameLEdt.Text = "SK";
            // 
            // OraPasswordLEdt
            // 
            this.OraPasswordLEdt.Location = new System.Drawing.Point(113, 234);
            this.OraPasswordLEdt.Name = "OraPasswordLEdt";
            this.OraPasswordLEdt.Size = new System.Drawing.Size(104, 21);
            this.OraPasswordLEdt.TabIndex = 7;
            this.OraPasswordLEdt.Text = "admin";
            // 
            // OraUserLEdt
            // 
            this.OraUserLEdt.Location = new System.Drawing.Point(28, 234);
            this.OraUserLEdt.Name = "OraUserLEdt";
            this.OraUserLEdt.Size = new System.Drawing.Size(79, 21);
            this.OraUserLEdt.TabIndex = 8;
            this.OraUserLEdt.Text = "pbdb_dt";
            // 
            // OraIPLEdt
            // 
            this.OraIPLEdt.Location = new System.Drawing.Point(28, 191);
            this.OraIPLEdt.Name = "OraIPLEdt";
            this.OraIPLEdt.Size = new System.Drawing.Size(126, 21);
            this.OraIPLEdt.TabIndex = 9;
            this.OraIPLEdt.Text = "127.0.0.1";
            this.OraIPLEdt.TextChanged += new System.EventHandler(this.OraIPLEdt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "User / Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "FtpIP / Port";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "Ftp服务器";
            // 
            // FtpPasswordLEdt
            // 
            this.FtpPasswordLEdt.Location = new System.Drawing.Point(114, 106);
            this.FtpPasswordLEdt.Name = "FtpPasswordLEdt";
            this.FtpPasswordLEdt.Size = new System.Drawing.Size(104, 21);
            this.FtpPasswordLEdt.TabIndex = 14;
            this.FtpPasswordLEdt.Text = "123456";
            // 
            // FtpUserLEdt
            // 
            this.FtpUserLEdt.Location = new System.Drawing.Point(29, 106);
            this.FtpUserLEdt.Name = "FtpUserLEdt";
            this.FtpUserLEdt.Size = new System.Drawing.Size(79, 21);
            this.FtpUserLEdt.TabIndex = 15;
            this.FtpUserLEdt.Text = "ftptest";
            // 
            // FtpIPLEdt
            // 
            this.FtpIPLEdt.Location = new System.Drawing.Point(29, 63);
            this.FtpIPLEdt.Name = "FtpIPLEdt";
            this.FtpIPLEdt.Size = new System.Drawing.Size(93, 21);
            this.FtpIPLEdt.TabIndex = 16;
            this.FtpIPLEdt.Text = "192.168.10.171";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(274, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 12);
            this.label9.TabIndex = 26;
            this.label9.Text = "dmp文件本地保存路径";
            // 
            // LocalPathLEdt
            // 
            this.LocalPathLEdt.Location = new System.Drawing.Point(276, 63);
            this.LocalPathLEdt.Name = "LocalPathLEdt";
            this.LocalPathLEdt.ReadOnly = true;
            this.LocalPathLEdt.Size = new System.Drawing.Size(169, 21);
            this.LocalPathLEdt.TabIndex = 28;
            this.LocalPathLEdt.Text = "D:\\OracleSql\\";
            // 
            // LocalPathBtn
            // 
            this.LocalPathBtn.Location = new System.Drawing.Point(451, 61);
            this.LocalPathBtn.Name = "LocalPathBtn";
            this.LocalPathBtn.Size = new System.Drawing.Size(39, 23);
            this.LocalPathBtn.TabIndex = 27;
            this.LocalPathBtn.Text = "选择";
            this.LocalPathBtn.UseVisualStyleBackColor = true;
            this.LocalPathBtn.Click += new System.EventHandler(this.LocalPathBtn_Click);
            // 
            // FtpPathLEdt
            // 
            this.FtpPathLEdt.Location = new System.Drawing.Point(125, 63);
            this.FtpPathLEdt.Name = "FtpPathLEdt";
            this.FtpPathLEdt.Size = new System.Drawing.Size(93, 21);
            this.FtpPathLEdt.TabIndex = 32;
            this.FtpPathLEdt.Text = "21";
            // 
            // textBox_destPort
            // 
            this.textBox_destPort.Location = new System.Drawing.Point(407, 191);
            this.textBox_destPort.Name = "textBox_destPort";
            this.textBox_destPort.Size = new System.Drawing.Size(38, 21);
            this.textBox_destPort.TabIndex = 36;
            this.textBox_destPort.Text = "9090";
            // 
            // textBox_destPassword
            // 
            this.textBox_destPassword.Location = new System.Drawing.Point(358, 234);
            this.textBox_destPassword.Name = "textBox_destPassword";
            this.textBox_destPassword.Size = new System.Drawing.Size(87, 21);
            this.textBox_destPassword.TabIndex = 37;
            this.textBox_destPassword.Text = "123456";
            // 
            // textBox_destUser
            // 
            this.textBox_destUser.Location = new System.Drawing.Point(275, 234);
            this.textBox_destUser.Name = "textBox_destUser";
            this.textBox_destUser.Size = new System.Drawing.Size(77, 21);
            this.textBox_destUser.TabIndex = 38;
            this.textBox_destUser.Text = "admin";
            // 
            // textBox_destIP
            // 
            this.textBox_destIP.Location = new System.Drawing.Point(275, 191);
            this.textBox_destIP.Name = "textBox_destIP";
            this.textBox_destIP.Size = new System.Drawing.Size(126, 21);
            this.textBox_destIP.TabIndex = 39;
            this.textBox_destIP.Text = "10.234.234.125";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(273, 149);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 35;
            this.label10.Text = "神库数据库";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(273, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 33;
            this.label11.Text = "IP / Port";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(273, 219);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 12);
            this.label12.TabIndex = 34;
            this.label12.Text = "User / Password";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(274, 98);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 42;
            this.label13.Text = "配置文件路径";
            // 
            // textBox_File
            // 
            this.textBox_File.Location = new System.Drawing.Point(276, 113);
            this.textBox_File.Name = "textBox_File";
            this.textBox_File.ReadOnly = true;
            this.textBox_File.Size = new System.Drawing.Size(169, 21);
            this.textBox_File.TabIndex = 41;
            this.textBox_File.Text = "D:\\OracleSql\\";
            this.textBox_File.TextChanged += new System.EventHandler(this.textBox_File_TextChanged);
            // 
            // ConfigfileBtn
            // 
            this.ConfigfileBtn.Location = new System.Drawing.Point(451, 111);
            this.ConfigfileBtn.Name = "ConfigfileBtn";
            this.ConfigfileBtn.Size = new System.Drawing.Size(39, 23);
            this.ConfigfileBtn.TabIndex = 40;
            this.ConfigfileBtn.Text = "选择";
            this.ConfigfileBtn.UseVisualStyleBackColor = true;
            this.ConfigfileBtn.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(275, 362);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 43;
            this.checkBox1.Text = "数据连续";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(149, 286);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 46;
            this.label8.Text = "小时";
            // 
            // PeriodLEdt
            // 
            this.PeriodLEdt.Location = new System.Drawing.Point(30, 283);
            this.PeriodLEdt.Name = "PeriodLEdt";
            this.PeriodLEdt.Size = new System.Drawing.Size(104, 21);
            this.PeriodLEdt.TabIndex = 45;
            this.PeriodLEdt.Text = "12";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 268);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 44;
            this.label7.Text = "ftp扫描周期";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(149, 338);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 49;
            this.label14.Text = "天";
            // 
            // SaveDayLab
            // 
            this.SaveDayLab.Location = new System.Drawing.Point(30, 335);
            this.SaveDayLab.Name = "SaveDayLab";
            this.SaveDayLab.Size = new System.Drawing.Size(104, 21);
            this.SaveDayLab.TabIndex = 48;
            this.SaveDayLab.Text = "180";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(29, 320);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(95, 12);
            this.label15.TabIndex = 47;
            this.label15.Text = "ftp数据保存天数";
            // 
            // ImpDate
            // 
            this.ImpDate.Location = new System.Drawing.Point(275, 283);
            this.ImpDate.Name = "ImpDate";
            this.ImpDate.Size = new System.Drawing.Size(170, 21);
            this.ImpDate.TabIndex = 51;
            this.ImpDate.Text = "20171227";
            this.ImpDate.TextChanged += new System.EventHandler(this.ImpDate_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(274, 268);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(101, 12);
            this.label16.TabIndex = 50;
            this.label16.Text = "神库数据最新时间";
            // 
            // FileDateBox
            // 
            this.FileDateBox.Location = new System.Drawing.Point(275, 335);
            this.FileDateBox.Name = "FileDateBox";
            this.FileDateBox.Size = new System.Drawing.Size(170, 21);
            this.FileDateBox.TabIndex = 53;
            this.FileDateBox.Text = "20170630";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(273, 320);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(95, 12);
            this.label17.TabIndex = 52;
            this.label17.Text = "ftp文件起始日期";
            // 
            // s
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 447);
            this.Controls.Add(this.FileDateBox);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.ImpDate);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.SaveDayLab);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.PeriodLEdt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox_File);
            this.Controls.Add(this.ConfigfileBtn);
            this.Controls.Add(this.textBox_destPort);
            this.Controls.Add(this.textBox_destPassword);
            this.Controls.Add(this.textBox_destUser);
            this.Controls.Add(this.textBox_destIP);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.FtpPathLEdt);
            this.Controls.Add(this.LocalPathLEdt);
            this.Controls.Add(this.LocalPathBtn);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FtpPasswordLEdt);
            this.Controls.Add(this.FtpUserLEdt);
            this.Controls.Add(this.FtpIPLEdt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OraDbnameLEdt);
            this.Controls.Add(this.OraPasswordLEdt);
            this.Controls.Add(this.OraUserLEdt);
            this.Controls.Add(this.OraIPLEdt);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "s";
            this.Text = "数据库DMP导入工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OraDbnameLEdt;
        private System.Windows.Forms.TextBox OraPasswordLEdt;
        private System.Windows.Forms.TextBox OraUserLEdt;
        private System.Windows.Forms.TextBox OraIPLEdt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox FtpPasswordLEdt;
        private System.Windows.Forms.TextBox FtpUserLEdt;
        private System.Windows.Forms.TextBox FtpIPLEdt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox LocalPathLEdt;
        private System.Windows.Forms.Button LocalPathBtn;
        private System.Windows.Forms.FolderBrowserDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.TextBox FtpPathLEdt;
        private System.Windows.Forms.TextBox textBox_destPort;
        private System.Windows.Forms.TextBox textBox_destPassword;
        private System.Windows.Forms.TextBox textBox_destUser;
        private System.Windows.Forms.TextBox textBox_destIP;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_File;
        private System.Windows.Forms.Button ConfigfileBtn;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox PeriodLEdt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox SaveDayLab;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox ImpDate;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox FileDateBox;
        private System.Windows.Forms.Label label17;
    }
}

