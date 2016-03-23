namespace Launcher
{
	partial class Launcher
	{
		/// <summary>
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label2 = new System.Windows.Forms.Label();
			this.scrollSensitiveNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.groundComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.startButton = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.resolutionComboBox = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.soundSettings = new System.Windows.Forms.TabControl();
			this.commonSettings = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.musicMuteCheckBox = new System.Windows.Forms.CheckBox();
			this.musicVolumeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.soundMuteCheckBox = new System.Windows.Forms.CheckBox();
			this.soundVolumeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scrollSensitiveNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.soundSettings.SuspendLayout();
			this.commonSettings.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.musicVolumeNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.soundVolumeNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackgroundImage = global::Launcher.Properties.Resources.background;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(618, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.aboutToolStripMenuItem.Text = "About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 11);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "Scroll Sensitive: ";
			// 
			// scrollSensitiveNumericUpDown
			// 
			this.scrollSensitiveNumericUpDown.Location = new System.Drawing.Point(97, 12);
			this.scrollSensitiveNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.scrollSensitiveNumericUpDown.Name = "scrollSensitiveNumericUpDown";
			this.scrollSensitiveNumericUpDown.Size = new System.Drawing.Size(101, 20);
			this.scrollSensitiveNumericUpDown.TabIndex = 14;
			this.scrollSensitiveNumericUpDown.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.scrollSensitiveNumericUpDown.ValueChanged += new System.EventHandler(this.scrollSensitiveNumericUpDown_ValueChanged);
			// 
			// groundComboBox
			// 
			this.groundComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.groundComboBox.FormattingEnabled = true;
			this.groundComboBox.Items.AddRange(new object[] {
            "Dust",
            "Grass",
            "Snow"});
			this.groundComboBox.Location = new System.Drawing.Point(61, 38);
			this.groundComboBox.Name = "groundComboBox";
			this.groundComboBox.Size = new System.Drawing.Size(137, 21);
			this.groundComboBox.TabIndex = 12;
			this.groundComboBox.SelectedIndexChanged += new System.EventHandler(this.groundComboBox_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 41);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Ground: ";
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(102, 538);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(413, 35);
			this.startButton.TabIndex = 15;
			this.startButton.Text = "Start Game!";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Launcher.Properties.Resources.background;
			this.pictureBox1.Location = new System.Drawing.Point(0, 27);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(618, 390);
			this.pictureBox1.TabIndex = 10;
			this.pictureBox1.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(521, 563);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "© Bolshakov Kirill";
			// 
			// resolutionComboBox
			// 
			this.resolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.resolutionComboBox.FormattingEnabled = true;
			this.resolutionComboBox.Items.AddRange(new object[] {
            "1920x1080 (16:9)",
            "1280x1024 (5:4)",
            "1280x960 (4:3)",
            "1280x800 (16:10)",
            "1280x768 (15:9)",
            "1280x720 (16:9)",
            "1152x864 (4:3)",
            "1024x768 (4:3)",
            "800x600 (4:3)"});
			this.resolutionComboBox.Location = new System.Drawing.Point(273, 11);
			this.resolutionComboBox.Name = "resolutionComboBox";
			this.resolutionComboBox.Size = new System.Drawing.Size(121, 21);
			this.resolutionComboBox.TabIndex = 17;
			this.resolutionComboBox.SelectedIndexChanged += new System.EventHandler(this.resolutionComboBox_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(204, 14);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 13);
			this.label4.TabIndex = 18;
			this.label4.Text = "Resolution: ";
			// 
			// soundSettings
			// 
			this.soundSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.soundSettings.Controls.Add(this.commonSettings);
			this.soundSettings.Controls.Add(this.tabPage2);
			this.soundSettings.Location = new System.Drawing.Point(0, 423);
			this.soundSettings.Name = "soundSettings";
			this.soundSettings.SelectedIndex = 0;
			this.soundSettings.Size = new System.Drawing.Size(618, 109);
			this.soundSettings.TabIndex = 19;
			// 
			// commonSettings
			// 
			this.commonSettings.BackColor = System.Drawing.Color.LightSteelBlue;
			this.commonSettings.Controls.Add(this.label2);
			this.commonSettings.Controls.Add(this.resolutionComboBox);
			this.commonSettings.Controls.Add(this.label4);
			this.commonSettings.Controls.Add(this.groundComboBox);
			this.commonSettings.Controls.Add(this.scrollSensitiveNumericUpDown);
			this.commonSettings.Controls.Add(this.label1);
			this.commonSettings.Location = new System.Drawing.Point(4, 22);
			this.commonSettings.Name = "commonSettings";
			this.commonSettings.Padding = new System.Windows.Forms.Padding(3);
			this.commonSettings.Size = new System.Drawing.Size(610, 83);
			this.commonSettings.TabIndex = 0;
			this.commonSettings.Text = "Common";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.LightSteelBlue;
			this.tabPage2.Controls.Add(this.musicMuteCheckBox);
			this.tabPage2.Controls.Add(this.musicVolumeNumericUpDown);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.soundMuteCheckBox);
			this.tabPage2.Controls.Add(this.soundVolumeNumericUpDown);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(610, 83);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Sound";
			// 
			// musicMuteCheckBox
			// 
			this.musicMuteCheckBox.AutoSize = true;
			this.musicMuteCheckBox.Checked = true;
			this.musicMuteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.musicMuteCheckBox.Location = new System.Drawing.Point(219, 32);
			this.musicMuteCheckBox.Name = "musicMuteCheckBox";
			this.musicMuteCheckBox.Size = new System.Drawing.Size(69, 17);
			this.musicMuteCheckBox.TabIndex = 6;
			this.musicMuteCheckBox.Text = "Music on";
			this.musicMuteCheckBox.UseVisualStyleBackColor = true;
			this.musicMuteCheckBox.CheckedChanged += new System.EventHandler(this.musicMuteCheckBox_CheckedChanged);
			// 
			// musicVolumeNumericUpDown
			// 
			this.musicVolumeNumericUpDown.Location = new System.Drawing.Point(90, 31);
			this.musicVolumeNumericUpDown.Name = "musicVolumeNumericUpDown";
			this.musicVolumeNumericUpDown.Size = new System.Drawing.Size(123, 20);
			this.musicVolumeNumericUpDown.TabIndex = 5;
			this.musicVolumeNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.musicVolumeNumericUpDown.ValueChanged += new System.EventHandler(this.musicVolumeNumericUpDown_ValueChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 33);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(78, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Music volume: ";
			// 
			// soundMuteCheckBox
			// 
			this.soundMuteCheckBox.AutoSize = true;
			this.soundMuteCheckBox.Checked = true;
			this.soundMuteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.soundMuteCheckBox.Location = new System.Drawing.Point(219, 8);
			this.soundMuteCheckBox.Name = "soundMuteCheckBox";
			this.soundMuteCheckBox.Size = new System.Drawing.Size(72, 17);
			this.soundMuteCheckBox.TabIndex = 3;
			this.soundMuteCheckBox.Text = "Sound on";
			this.soundMuteCheckBox.UseVisualStyleBackColor = true;
			this.soundMuteCheckBox.CheckedChanged += new System.EventHandler(this.soundMuteCheckBox_CheckedChanged);
			// 
			// soundVolumeNumericUpDown
			// 
			this.soundVolumeNumericUpDown.Location = new System.Drawing.Point(93, 8);
			this.soundVolumeNumericUpDown.Name = "soundVolumeNumericUpDown";
			this.soundVolumeNumericUpDown.Size = new System.Drawing.Size(120, 20);
			this.soundVolumeNumericUpDown.TabIndex = 1;
			this.soundVolumeNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.soundVolumeNumericUpDown.ValueChanged += new System.EventHandler(this.soundVolumeNumericUpDown_ValueChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(81, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Sound volume: ";
			// 
			// Launcher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightSteelBlue;
			this.ClientSize = new System.Drawing.Size(618, 585);
			this.Controls.Add(this.soundSettings);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.Name = "Launcher";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Launcher";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.scrollSensitiveNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.soundSettings.ResumeLayout(false);
			this.commonSettings.ResumeLayout(false);
			this.commonSettings.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.musicVolumeNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.soundVolumeNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown scrollSensitiveNumericUpDown;
		private System.Windows.Forms.ComboBox groundComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox resolutionComboBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TabControl soundSettings;
		private System.Windows.Forms.TabPage commonSettings;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.CheckBox musicMuteCheckBox;
		private System.Windows.Forms.NumericUpDown musicVolumeNumericUpDown;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox soundMuteCheckBox;
		private System.Windows.Forms.NumericUpDown soundVolumeNumericUpDown;
		private System.Windows.Forms.Label label5;
	}
}

