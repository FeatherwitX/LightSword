using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using SettingsLib;

namespace Launcher
{
	public partial class Launcher : Form
	{
		Settings settings;
		string savePath = Global.SavePath;

		decimal currentSoundV, currentMusicV;

		AboutBox1 aboutBox;

		public Launcher() {
			InitializeComponent();
			aboutBox = new AboutBox1();
			if (!Directory.Exists(savePath))
				Directory.CreateDirectory(savePath);			
			settings = Settings.GetSettings();
			InitSettings();
		}

		#region Start Button

		private void startButton_Click(object sender, EventArgs e) {
			settings.Save();
			this.Close();
			Process.Start("Light Sword.exe");
		}

		#endregion

		#region Ground Combobox

		private void groundComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			settings.Ground = groundComboBox.Text;
		}

		#endregion

		#region Resolution Combobox

		private void resolutionComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			string value = resolutionComboBox.Text;
			settings.ResolutionWidth = int.Parse(value.Substring(0, value.IndexOf('x')));
			settings.ResolutionHeight = int.Parse(value.Substring(value.IndexOf('x') + 1, value.IndexOf(' ') - value.IndexOf('x')));
		}

		#endregion

		#region Scroll Sensitive NumericUpDown

		private void scrollSensitiveNumericUpDown_ValueChanged(object sender, EventArgs e) {
			settings.ScrollSensitive = (int)scrollSensitiveNumericUpDown.Value;
		}

		#endregion

		#region Sound Volume NumericUpDown

		private void soundVolumeNumericUpDown_ValueChanged(object sender, EventArgs e) {
			soundMuteCheckBox.Checked = (soundVolumeNumericUpDown.Value == 0) ? false : true;
			settings.SoundVolume = soundVolumeNumericUpDown.Value;
		}

		#endregion

		#region Music Molume NumericUpDown

		private void musicVolumeNumericUpDown_ValueChanged(object sender, EventArgs e) {
			musicMuteCheckBox.Checked = (musicVolumeNumericUpDown.Value == 0) ? false : true;
			settings.MusicVolume = musicVolumeNumericUpDown.Value;
		}

		#endregion

		#region Sound Mute Checkbox

		private void soundMuteCheckBox_CheckedChanged(object sender, EventArgs e) {
			soundMuteCheckBox.Text = (soundMuteCheckBox.Checked) ? "Sound on" : "Sound off";
			if (soundMuteCheckBox.Checked) {
				soundVolumeNumericUpDown.Enabled = true;
				soundVolumeNumericUpDown.Value = currentSoundV;
				if (soundVolumeNumericUpDown.Value == 0)
					soundVolumeNumericUpDown.Value = 100;
			} else {
				soundVolumeNumericUpDown.Enabled = false;
				currentSoundV = soundVolumeNumericUpDown.Value;
				soundVolumeNumericUpDown.Value = 0;
			}
			settings.SoundIsMuted = !soundMuteCheckBox.Checked;
		}

		#endregion

		#region Music Mute Checkbox

		private void musicMuteCheckBox_CheckedChanged(object sender, EventArgs e) {
			musicMuteCheckBox.Text = (musicMuteCheckBox.Checked) ? "Music on" : "Music off";
			if (musicMuteCheckBox.Checked) {
				musicVolumeNumericUpDown.Enabled = true;
				musicVolumeNumericUpDown.Value = currentMusicV;
				if (musicVolumeNumericUpDown.Value == 0)
					musicVolumeNumericUpDown.Value = 100;
			} else {
				musicVolumeNumericUpDown.Enabled = false;
				currentMusicV = musicVolumeNumericUpDown.Value;
				musicVolumeNumericUpDown.Value = 0;
			}
			settings.MusicIsMuted = !musicMuteCheckBox.Checked;
		}

		#endregion

		#region InitSettings

		private void InitSettings() {
			if (settings.SoundVolume < 0 || settings.SoundVolume > 100)
				settings.SoundVolume = 100;
			soundVolumeNumericUpDown.Value = settings.SoundVolume;

			if (settings.MusicVolume < 0 || settings.MusicVolume > 100)
				settings.MusicVolume = 100;
			musicVolumeNumericUpDown.Value = settings.MusicVolume;

			soundMuteCheckBox.Checked = !settings.SoundIsMuted;
			musicMuteCheckBox.Checked = !settings.MusicIsMuted;

			currentMusicV = musicVolumeNumericUpDown.Value;
			currentSoundV = soundVolumeNumericUpDown.Value;
			soundVolumeNumericUpDown.Enabled = soundMuteCheckBox.Checked;
			musicVolumeNumericUpDown.Enabled = musicMuteCheckBox.Checked;

			string resolution = String.Format("{0}x{1}", settings.ResolutionWidth, settings.ResolutionHeight);
			switch (resolution) {
				case "1920x1080":
					resolutionComboBox.SelectedIndex = 0;
					break;
				case "1280x1024":
					resolutionComboBox.SelectedIndex = 1;
					break;
				case "1280x960":
					resolutionComboBox.SelectedIndex = 2;
					break;
				case "1280x800":
					resolutionComboBox.SelectedIndex = 3;
					break;
				case "1280x768":
					resolutionComboBox.SelectedIndex = 4;
					break;
				case "1280x720":
					resolutionComboBox.SelectedIndex = 5;
					break;
				case "1152x864":
					resolutionComboBox.SelectedIndex = 6;
					break;
				case "1024x768":
					resolutionComboBox.SelectedIndex = 7;
					break;
				case "800x600":
					resolutionComboBox.SelectedIndex = 8;
					break;
				default:
					resolutionComboBox.SelectedIndex = 1;
					break;
			}
			switch (settings.Ground) {
				case "Snow":
					groundComboBox.SelectedIndex = 2;
					break;
				case "Dust":
					groundComboBox.SelectedIndex = 0;
					break;
				case "Grass":
					groundComboBox.SelectedIndex = 1;
					break;
				default:
					groundComboBox.SelectedIndex = 2;
					break;
			}
			if (settings.ScrollSensitive < 1 || settings.ScrollSensitive > 100)
				settings.ScrollSensitive = 15;
			scrollSensitiveNumericUpDown.Value = settings.ScrollSensitive;
		}

		#endregion

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			aboutBox.ShowDialog(this);
		}
	}
}
