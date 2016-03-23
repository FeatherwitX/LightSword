using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Graph;
using Sounds;

namespace Light_Sword.UI
{
	/// <summary>
	/// Класс чекбокс
	/// </summary>
	public class Checkbox : Element
	{
		#region Variables

		Sound soundClick;
		bool isChecked;

		#endregion

		/// <summary>
		/// Значение чекбокса true\false
		/// </summary>
		public bool Checked {
			get { return isChecked; }
			set {
				isChecked = value;
				Image.FrameCurrent = (isChecked) ? new Vector2(1, 0) : new Vector2(0, 0);
			}
		}

		/// <summary>
		/// Создает чекбокс с указанным изображением
		/// </summary>
		public Checkbox(MultiSprite image, Sound soundClick)
			: base(image) {
			this.soundClick = soundClick;
			Checked = true;
			this.Click += new EventHandler<MouseElementEventArgs>(Checkbox_Click);
		}

		#region Private Methods		

		private void Checkbox_Click(object sender, MouseElementEventArgs e) {
			Checked = !Checked;
			if (soundClick != null)
				SoundPlayer.PlaySound(soundClick);
		}

		#endregion
	}
}
