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
using UnitsLib.Surround;
using SettingsLib;

namespace Light_Sword.UI
{
	/// <summary>
	/// Класс дополнительная панель
	/// </summary>
	public abstract class Subpanel : IDrawableComponent
	{
		#region Variables

		protected ContentManager content;

		Sprite sprite;
		bool vis = false;
		List<Button> buttons;
		List<IComponent> components;
		Screen owner;

		protected MouseState MS, OMS;
		protected SpriteBatch spriteBatch;

		protected static Texture2D mesh = World.Content.Load<Texture2D>(Fnames.Mesh);

		#endregion

		/// <summary>
		/// Список кнопок для панели
		/// </summary>
		public List<Button> Buttons {
			get { return buttons; }
			protected set { buttons = value; }
		}

		/// <summary>
		/// Список элементов IComponent, которые обрабатываются панелью
		/// </summary>
		public List<IComponent> Components {
			get { return components; }
			protected set { components = value; }
		}

        /// <summary>
        /// Экран, к которому привязана дополнительная панель
        /// </summary>
		public Screen Owner { get { return owner; } }

		/// <summary>
		/// Границы панели
		/// </summary>
		public Rectangle Bounds { get { return sprite.Bounds; } }

		/// <summary>
		/// Цвет, применяемы к панели
		/// </summary>
		public Color Color {
			get { return sprite.Color; }
			set { sprite.Color = value; }
		}

		/// <summary>
		/// Видна ли панель
		/// </summary>
		public virtual bool Visible {
			get { return vis; }
			set {
				bool prevVis = vis;
				vis = value;
				sprite.Visible = vis;
				foreach (var button in buttons) {
					button.ToNormal();
					button.Visible = value;
				}
				if (prevVis != vis) {
					if (vis)
						OnEnabled(new EventArgs());
					else
						OnDisabled(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Событие проиходит, когда панель активируется
		/// </summary>
		public event EventHandler<EventArgs> Enabled;
		/// <summary>
		/// Событие возникает, когда панель деактивируется
		/// </summary>
		public event EventHandler<EventArgs> Disabled;

		/// <summary>
		/// Создает новую дополнительну панель с указанными параметрами
		/// </summary>
		/// <param name="content">Управляющий контентом для этой панели</param>
		/// <param name="sprite">Спрайт, изображение панели</param>
		public Subpanel(Screen owner, ContentManager content, Sprite sprite) {
			this.sprite = sprite;
			this.content = content;
			this.owner = owner;
			buttons = new List<Button>();
			components = new List<IComponent>();
		}

		/// <summary>
		/// Обновление панели в текущем кадре
		/// </summary>
		public virtual void Update(GameTime gameTime) {
			if (Visible) {
				OMS = MS;
				MS = Mouse.GetState();
				MouseHandler(MS, OMS);
				for (int i = components.Count - 1; i >= 0; i--) {
					components[i].Update(gameTime);
				}				
			}
		}

		/// <summary>
		/// Прорисовка панели в текущем кадре
		/// </summary>
		public virtual void Draw(SpriteBatch spriteBatch) {
			if (Visible) {
				spriteBatch.Draw(mesh, new Rectangle(sprite.Bounds.Left + (int)(Bounds.Width / 78f), sprite.Bounds.Top + (int)(Bounds.Width / 78f), sprite.Bounds.Width - (int)(Bounds.Width / 78f), sprite.Bounds.Height - (int)(Bounds.Width / 78f)), new Color(0, 0, 0, 100));
				sprite.Draw(spriteBatch);
				for (int i = 0; i < components.Count; i++) {
					if (components[i] is IDrawableComponent)
						(components[i] as IDrawableComponent).Draw(spriteBatch);
				}
			}
		}

		/// <summary>
		/// Обработка мыши
		/// </summary>
		/// <param name="ms">Текущее состояние мыши</param>
		/// <param name="oms">Предыдущее состояние мыши</param>
		/// <param name="state">Состояние MouseStates мыши</param>
		protected virtual void MouseHandler(MouseState ms, MouseState oms) {

		}

		/// <summary>
		/// Создает 3D звук одной строкой
		/// </summary>
		protected static Sound3D CreateSound3D(ContentManager content, string fname) {
			Sound sound = new Sound(content);
			sound.LoadSound(fname);
			return sound.Create3D();
		}

		/// <summary>
		/// Создает звук одной строкой
		/// </summary>
		protected static Sound CreateSound(ContentManager content, string fname) {
			Sound sound = new Sound(content);
			sound.LoadSound(fname);
			return sound;
		}

		#region Private Methods

		private void OnEnabled(EventArgs e) {
			EventHandler<EventArgs> enabled = Enabled;
			if (enabled != null)
				enabled(this, e);
		}

		private void OnDisabled(EventArgs e) {
			EventHandler<EventArgs> disabled = Disabled;
			if (disabled != null)
				disabled(this, e);
		}

		#endregion
	}
}
