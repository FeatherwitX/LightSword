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
using UnitsLib.Interfaces;
using UnitsLib.Enums;
using UnitsLib.Events;
using UnitsLib.Exceptions;


namespace UnitsLib.Surround
{
	/// <summary>
	/// Игровая панель, представленная ввиде HUDа, меню и UI панели
	/// </summary>
	public sealed class Panel : Microsoft.Xna.Framework.DrawableGameComponent
    {
		const int UI_X_OFFSET = 3;
		const int UI_Y_OFFSET = 3;

        struct Subpanel
        {
            public Rectangle Bounds;
            public Rectangle LeftPath;
            public Rectangle CentralPath;
            public Rectangle RightPath;
        }

        #region Variables

        Subpanel bottomPanel;
        Rectangle bottomRect;
		Rectangle topRect;

		Sprite hud;

		Sprite Prev;
		Sprite ManPrev;
		Sprite SwordmanPrev;
		Sprite StonePrev;
		Sprite TreePrev;
		Sprite CityCenterPrev;
		Sprite FarmPrev;
		Sprite BaracksPrev;

		Rectangle bounds;
		SpriteBatch spriteBatch;

		SpriteFont topFont;
		SpriteFont bottomFont;

		MouseState MS, OMS;

		int gold, wood, stone;

		hpRect statucRect;
		hpRect progressRect;
		string attack = "";
		string armor = "";
		string hp = "";
		string bag = "";

        List<TObject> objList;

		/// <summary>
		/// [0][x] - unit icons
		/// [1][x] - unit commands icons
		/// [2][x] - building commands icons
		/// [3][x] - building icons
		/// </summary>
		List<List<Sprite>> UiPanel;

		List<UnitNames> Names;
		List<Sprite> Pictures_UnitIcons;
		List<IcoWithHP> Pictures_FocusedObj;

		static Texture2D hpTexture = World.Content.Load<Texture2D>(Fnames.HpPrev);
		static Texture2D hpBorder = World.Content.Load<Texture2D>(Fnames.HpBorderPrev);
		static Texture2D progressTexture = World.Content.Load<Texture2D>(Fnames.ProgressPrev);
		static Texture2D progressBorder = World.Content.Load<Texture2D>(Fnames.ProgressBorderPrev);

        #endregion

        /// <summary>
		/// Размер нижней панели (только для чтения)
		/// </summary>
		public Vector2 BottomPanelSize { get { return new Vector2(hud.Size.X, hud.Size.Y / 4.096f); } }
		/// <summary>
		/// Позиция левого верхнего угла нижней панели (только для чтения)
		/// </summary>
		public Vector2 BottomPanelPosition { get { return new Vector2(1, hud.Size.Y - BottomPanelSize.Y); } }
        /// <summary>
        /// Список объектов, зафиксированных на панели
        /// </summary>
        public List<TObject> Objects {
            get { return objList; }
            set {
                objList = value;
                OnObjectChanged(new EventArgs());
            }
        }
		/// <summary>
		/// UI панель (только для чтения)
		/// </summary>
		internal List<List<Sprite>> UIPanel { get { return UiPanel; } }

		/// <summary>
		/// Клик мышью по элементу UI панели
		/// </summary>
		public event EventHandler<UIPanelEventArgs> UIPanelClick;
        /// <summary>
        /// Событие изменения объекта, закрепленного на панели
        /// </summary>
		private event EventHandler<EventArgs> ObjectChanged;
		/// <summary>
		/// Событие клика по иконке юнита на центральной панели
		/// </summary>
		public event EventHandler<ObjectIconsEventArgs> ObjectIconClick;

		/// <summary>
		/// Создает новую игровую панель с указанными параметрами
		/// </summary>
		/// <param name="game">Игра, к которой привязан этот компонент</param>
		/// <param name="topFont">Шрифт для верхней части панели</param>
		/// <param name="bottomFont">Шрифт для нижней части панели</param>
		/// <param name="bounds">Границы экрана, на котором существует панель</param>
		public Panel(Game game, SpriteFont topFont, SpriteFont bottomFont, Rectangle bounds)
			: base(game) {
			hud = new Sprite(game.Content);
			ManPrev = new Sprite(game.Content);
			SwordmanPrev = new Sprite(game.Content);
			StonePrev = new Sprite(game.Content);
			TreePrev = new Sprite(game.Content);
			CityCenterPrev = new Sprite(game.Content);
			FarmPrev = new Sprite(game.Content);
			BaracksPrev = new Sprite(game.Content);
			this.topFont = topFont;
			this.bottomFont = bottomFont;
			this.bounds = bounds;
			this.ObjectChanged += new EventHandler<EventArgs>(Panel_ObjectChanged);

			List<Sprite> unitIcons = new List<Sprite>();
			Sprite workerIcon = new Sprite(game.Content);
			Sprite peasantIcon = new Sprite(game.Content);
			Sprite swordmanIcon = new Sprite(game.Content);
			unitIcons.Add(workerIcon);
			unitIcons.Add(peasantIcon);
			unitIcons.Add(swordmanIcon);

			List<Sprite> unitCommandsIcons = new List<Sprite>();
			Sprite stopIcon = new Sprite(game.Content);
			Sprite deadIcon = new Sprite(game.Content);
			unitCommandsIcons.Add(stopIcon);
			unitCommandsIcons.Add(deadIcon);

			List<Sprite> buildingCommandIcons = new List<Sprite>();
			Sprite clearIcon = new Sprite(game.Content);
			buildingCommandIcons.Add(clearIcon);

			List<Sprite> buildingIcons = new List<Sprite>();
			Sprite farmIcon = new Sprite(game.Content);
			Sprite baracksIcon = new Sprite(game.Content);
			buildingIcons.Add(farmIcon);
			buildingIcons.Add(baracksIcon);

			UiPanel = new List<List<Sprite>>();
			UiPanel.Add(unitIcons);
			UiPanel.Add(unitCommandsIcons);
			UiPanel.Add(buildingCommandIcons);
			UiPanel.Add(buildingIcons);

			Names = new List<UnitNames>(World.MAX_QUEUE_SIZE);
			Pictures_UnitIcons = new List<Sprite>(World.MAX_QUEUE_SIZE);
			Pictures_FocusedObj = new List<IcoWithHP>(World.MAX_FOCUSED_OBJECTS);
            objList = new List<TObject>();
		}

		public override void Initialize() {

			base.Initialize();
		}

		protected override void LoadContent() {
			hud.LoadTexture(Fnames.HUD, Vector2.Zero, new Vector2(bounds.Width, bounds.Height));
			ManPrev.LoadTexture(Fnames.ManPrev, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 4f, BottomPanelPosition.Y + (50f).FormatH(Game)), new Vector2(150, 150));
			SwordmanPrev.LoadTexture(Fnames.SwordmanPrev, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 4f, BottomPanelPosition.Y + (50f).FormatH(Game)), new Vector2(150, 150));
			TreePrev.LoadTexture(Fnames.TreePrev, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 4f, BottomPanelPosition.Y + (50f).FormatH(Game)), new Vector2(150, 150));
			StonePrev.LoadTexture(Fnames.StonePrev, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 4f, BottomPanelPosition.Y + (50f).FormatH(Game)), new Vector2(150, 150));
			CityCenterPrev.LoadTexture(Fnames.CityCenterPrev, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 4f, BottomPanelPosition.Y + (50f).FormatH(Game)), new Vector2(150, 150));
			FarmPrev.LoadTexture(Fnames.FarmPrev, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 4f, BottomPanelPosition.Y + (70f).FormatH(Game)), new Vector2(150, 100));
			BaracksPrev.LoadTexture(Fnames.BaracksPrev, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 4f, BottomPanelPosition.Y + (50f).FormatH(Game)), new Vector2(150, 150));

			Rectangle RectPrev = new Rectangle((int)BottomPanelPosition.X + (int)(BottomPanelSize.X / 4f), (int)BottomPanelPosition.Y + (int)((50f).FormatH(Game)), 150, 150);

			UiPanel[0][0].LoadTexture(Fnames.WorkerIcon, new Vector2(RectPrev.X + RectPrev.Width + 40, RectPrev.Y), new Vector2(50, 50));
			UiPanel[0][1].LoadTexture(Fnames.PeasantIcon, UiPanel[0][0].Position, new Vector2(50, 50));
			UiPanel[0][2].LoadTexture(Fnames.SwordmanIcon, UiPanel[0][0].Position, new Vector2(50, 50));

			UiPanel[1][0].LoadTexture(Fnames.StopIcon, new Vector2(ManPrev.Position.X + ManPrev.Size.X + 40, ManPrev.Position.Y), new Vector2(50, 50));
			UiPanel[1][1].LoadTexture(Fnames.DeadIcon, UiPanel[1][0].Position + new Vector2(UiPanel[1][0].Size.X * 2 + 15, 0), new Vector2(50, 50));

			UiPanel[2][0].LoadTexture(Fnames.ClearIcon, new Vector2(UiPanel[0][0].Position.X, UiPanel[0][0].Position.Y + UiPanel[0][0].Size.Y + 20), new Vector2(50, 50));

			UiPanel[3][0].LoadTexture(Fnames.FarmIcon, new Vector2(UiPanel[0][0].Position.X + UiPanel[0][0].Size.X + 15, UiPanel[0][0].Position.Y), new Vector2(50, 50));
			UiPanel[3][1].LoadTexture(Fnames.BaracksIcon, new Vector2(UiPanel[3][0].Position.X + UiPanel[3][0].Size.X + 15, UiPanel[3][0].Position.Y), new Vector2(50, 50));

			spriteBatch = new SpriteBatch(GraphicsDevice);

			bottomRect = new Rectangle((int)BottomPanelPosition.X - 5, (int)BottomPanelPosition.Y, (int)BottomPanelSize.X + 5, (int)BottomPanelSize.Y + 5);
			topRect = new Rectangle(0, 0, bottomRect.Width, 35);

            bottomPanel.Bounds = bottomRect;
            bottomPanel.LeftPath = new Rectangle((int)BottomPanelPosition.X, (int)BottomPanelPosition.Y, (int)(BottomPanelSize.X / 4), (int)BottomPanelSize.Y);
            bottomPanel.CentralPath = new Rectangle((int)bottomPanel.LeftPath.Right, (int)bottomPanel.LeftPath.Top, (int)(BottomPanelSize.X / 4 * 2), (int)BottomPanelSize.Y);
            bottomPanel.RightPath = new Rectangle((int)bottomPanel.CentralPath.Right, (int)bottomPanel.CentralPath.Top, (int)(BottomPanelSize.X / 4), (int)BottomPanelSize.Y);

			base.LoadContent();
		}

		public override void Update(GameTime gameTime) {
			OMS = MS;
			MS = Mouse.GetState();
			MouseHandler(MS, OMS);
			gold = World.CResource.Gold;
			wood = World.CResource.Wood;
			stone = World.CResource.Stone;

			AnalyseObject(objList);

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			spriteBatch.Begin();
			hud.Draw(spriteBatch);
			if (Prev != null)
				Prev.Draw(spriteBatch);
			if (statucRect.Vis) {
				spriteBatch.Draw(hpTexture, statucRect.Rect, statucRect.Rect, Color.Green);
				spriteBatch.Draw(hpBorder, statucRect.Border, Color.White);
			}
			if (progressRect.Vis) {
				spriteBatch.Draw(progressTexture, progressRect.Rect, progressRect.Rect, Color.Blue);
				spriteBatch.Draw(progressBorder, progressRect.Border, Color.White);
			}
			foreach (var list in UiPanel) {
				foreach (var sprite in list) {
					sprite.Draw(spriteBatch);
				}
			}
			foreach (var pict in Pictures_UnitIcons) {
				pict.Draw(spriteBatch);
			}
			foreach (var pict in Pictures_FocusedObj) {
				pict.Draw(spriteBatch);
			}
			spriteBatch.DrawString(bottomFont, hp, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 20, BottomPanelPosition.Y + BottomPanelSize.Y / 10 * 2), Color.Black);
			spriteBatch.DrawString(bottomFont, bag, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 20, BottomPanelPosition.Y + BottomPanelSize.Y / 10 * 4), Color.Black);
			spriteBatch.DrawString(bottomFont, attack, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 20, BottomPanelPosition.Y + BottomPanelSize.Y / 10 * 6), Color.Black);
			spriteBatch.DrawString(bottomFont, armor, new Vector2(BottomPanelPosition.X + BottomPanelSize.X / 20, BottomPanelPosition.Y + BottomPanelSize.Y / 10 * 8), Color.Black);
			spriteBatch.DrawString(topFont, gold.ToString(), new Vector2(63, 10).Format(Game), Color.White);
			spriteBatch.DrawString(topFont, wood.ToString(), new Vector2(166, 10).Format(Game), Color.White);
			spriteBatch.DrawString(topFont, stone.ToString(), new Vector2(269, 10).Format(Game), Color.White);
			spriteBatch.End();
			base.Draw(gameTime);
		}

		private void MouseHandler(MouseState ms, MouseState oms) {
			for (int i = UiPanel.Count - 1; i >= 0; i--) {
				for (int j = UiPanel[i].Count - 1; j >= 0; j--) {
					if (ms.LeftButton == ButtonState.Released && oms.LeftButton == ButtonState.Pressed && UiPanel[i][j].Contains(ms.X, ms.Y) && UiPanel[i][j].Visible) {
						Point index = new Point(i, j);
						OnUIPanelClick(new UIPanelEventArgs(index));
						return;
					}
				}
			}
			for (int i = 0; i < Pictures_FocusedObj.Count; i++) {
				if (ms.LeftButton == ButtonState.Pressed && oms.LeftButton == ButtonState.Released && Pictures_FocusedObj[i].Image.Contains(ms.X, ms.Y) && Pictures_FocusedObj[i].Visible) {
					OnObjectIconClick(new ObjectIconsEventArgs(objList[i]));
				}
			}
		}

		/// <summary>
		/// Анализ объекта
		/// </summary>
        private void AnalyseObject(List<TObject> objList) {
            /*Отчистка UI панели и списка Pictures*/
            foreach (var list in UiPanel) {
                foreach (var sprite in list) {
                    sprite.Visible = false;
                }
            }
            foreach (var sprite in Pictures_UnitIcons) {
                sprite.Visible = false;
            }
			foreach (var icon in Pictures_FocusedObj) {
				icon.Visible = false;
			}
			Pictures_FocusedObj.Clear();
            Pictures_UnitIcons.Clear();
            /*Отчистка всей нижней панели, если кол-во объектов, закрепленных на панели, не равно 1*/
            if (objList.Count != 1) {
                Prev = null;
                statucRect.Vis = false;
                progressRect.Vis = false;
                attack = "";
                armor = "";
                hp = "";
                bag = "";
                foreach (var list in UiPanel) {
                    foreach (var sprite in list) {
                        sprite.Visible = false;
                    }
                }
                Names.Clear();
                /*Анализ списка объектов, закрепленных на панели, если кол-во объектов больше 1*/
                if (objList.Count > 1) {
                    objList.Sort(TObject.Comparer);
                    for (int i = 0; i < objList.Count; i++) {
						if (objList[i] is TUnit) {
							switch ((objList[i] as TUnit).UnitName) {
								case UnitNames.Worker:
									Pictures_FocusedObj.Add(new IcoWithHP((Sprite)UIPanel[0][0].Clone()));
									break;
								case UnitNames.Peasant:
									Pictures_FocusedObj.Add(new IcoWithHP((Sprite)UIPanel[0][1].Clone()));
									break;
								case UnitNames.Swordman:
									Pictures_FocusedObj.Add(new IcoWithHP((Sprite)UIPanel[0][2].Clone()));
									break;
								case UnitNames.Archer:

									break;
								case UnitNames.Undefined:
									throw new UndefinedUnitNameException("Встречено неопределенное имя юнита");
							}
							Pictures_FocusedObj[i].Size = Pictures_FocusedObj[i].Image.InitSize.Format(Game);
							Pictures_FocusedObj[i].HPRect = new hpRect(new Rectangle((int)(Pictures_FocusedObj[i].Position.X), (int)Pictures_FocusedObj[i].Position.Y + (int)Pictures_FocusedObj[i].Size.Y - 3, (int)Pictures_FocusedObj[i].Size.X, 5));
							Pictures_FocusedObj[i].SetHp((objList[i] as IHP).MaxHP, (objList[i] as IHP).HP);
							Pictures_FocusedObj[i].Visible = true;
						}
                    }
					FormateUIPanel(unitIcons: Pictures_FocusedObj);
                }
            } /*Иначе анализ списка объектов, закрепленных на панели, если кол-во объектов равно 1*/
            else if (objList.Count == 1) {
                TObject obj = objList[0];
                if (obj is TUnit) {
                    if (obj is TCSwordman)
                        Prev = SwordmanPrev;
                    else
                        Prev = ManPrev;
                }
                if (obj is TStone) {
                    Prev = StonePrev;
                }
                if (obj is TTree) {
                    Prev = TreePrev;
                }
                if (obj is TCityCenter) {
                    Prev = CityCenterPrev;
                    if ((obj as TCityCenter).Side) {
                        UiPanel[0][0].Visible = true;
                        UiPanel[3][0].Visible = true;
                        UiPanel[3][1].Visible = true;
                    } else {
                        Names.Clear();
                        foreach (var sprite in Pictures_UnitIcons) {
                            sprite.Visible = false;
                        }
                    }
                }
                if (obj is TFarm) {
                    Prev = FarmPrev;
                    if ((obj as TFarm).Side) {
                        UiPanel[0][1].Visible = true;
                    } else {
                        Names.Clear();
                        foreach (var sprite in Pictures_UnitIcons) {
                            sprite.Visible = false;
                        }
                    }
                }
                if (obj is TBaracks) {
                    Prev = BaracksPrev;
                    if ((obj as TBaracks).Side) {
                        UiPanel[0][2].Visible = true;
                    } else {
                        Names.Clear();
                        foreach (var sprite in Pictures_UnitIcons) {
                            sprite.Visible = false;
                        }
                    }
                }
                if (obj is TCUnit) {
                    UiPanel[1][0].Visible = true;
                    UiPanel[1][1].Visible = true;
                }
                if (obj is ISource || obj is IHP) {
                    bool isHP = (obj is IHP);
                    statucRect = new hpRect(new Rectangle((int)Prev.Position.X, (int)Prev.Position.Y + (int)Prev.Size.Y + 5, (int)Prev.Size.X, 10));
                    statucRect.Rect.Width = (isHP) ? (int)(((obj as IHP).HP * Prev.Size.X) / (obj as IHP).MaxHP) : (int)(((obj as ISource).Supply * Prev.Size.X) / (obj as ISource).MaxSupply);
                    hp = (isHP) ? String.Format("Hp: {0}/{1}", (obj as IHP).HP, (obj as IHP).MaxHP) : String.Format("Supply: {0}/{1}", (obj as ISource).Supply, (obj as ISource).MaxSupply);
                    statucRect.Vis = true;
                }
                if (obj is IAttack) {
                    attack = "Attack: " + (obj as IAttack).Atack.ToString();
                }
                if (obj is IArmor) {
                    armor = "Armor: " + (obj as IArmor).Armor.ToString();
                }
                if (obj is IBag) {
                    bag = String.Format("Bag : {0}/{1}", (obj as IBag).Bag, (obj as IBag).MaxBag);
                    if ((obj as IBag).Bag > 0)
                        bag += "  (" + (obj as IBag).SourceType.ToString() + ")";
                }
                if (obj is ISide && (obj as ISide).Side) {
                    if (obj is ICreate) {
                        progressRect = new hpRect(new Rectangle((int)Prev.Position.X, (int)Prev.Position.Y + (int)Prev.Size.Y + 5 + statucRect.Height + 5, (int)Prev.Size.X, 16));
                        progressRect.Rect.Width = (int)(((obj as ICreate).Progress * Prev.Size.X) / (obj as ICreate).MaxProgress);
                        progressRect.Vis = true;
                    }
                    if (obj is IQueue) {
                        Names = new List<UnitNames>((obj as IQueue).Q);
                        Pictures_UnitIcons.Clear();
                        for (int i = 0; i < Names.Count; i++) {
                            switch (Names[i]) {
                                case UnitNames.Swordman:
                                    Pictures_UnitIcons.Add((Sprite)UiPanel[0][2].Clone());
                                    Pictures_UnitIcons[i].Position = new Vector2(UiPanel[0][0].Position.X + 20 * i, UiPanel[0][0].Position.Y + UiPanel[0][0].Size.Y + 20);
                                    break;
                                case UnitNames.Worker:
                                    Pictures_UnitIcons.Add((Sprite)UiPanel[0][0].Clone());
                                    Pictures_UnitIcons[i].Position = new Vector2(UiPanel[0][0].Position.X + 20 * i, UiPanel[0][0].Position.Y + UiPanel[0][0].Size.Y + 20);
                                    break;
                                case UnitNames.Peasant:
                                    Pictures_UnitIcons.Add((Sprite)UiPanel[0][1].Clone());
                                    Pictures_UnitIcons[i].Position = new Vector2(UiPanel[0][0].Position.X + 20 * i, UiPanel[0][0].Position.Y + UiPanel[0][0].Size.Y + 20);
                                    break;
                                case UnitNames.Archer:
                                    break;
                                case UnitNames.Undefined:
									throw new UndefinedUnitNameException("Встречено неопределенное имя юнита");
                            }
                        }
                        if ((obj as IQueue).Q.Count > 0) {
                            UiPanel[2][0].Visible = true;
                            UiPanel[2][0].Position = Pictures_UnitIcons[Pictures_UnitIcons.Count - 1].Position + new Vector2(Pictures_UnitIcons[Pictures_UnitIcons.Count - 1].Size.X + 20, 0);
                        } else
                            UiPanel[2][0].Visible = false;
                    }
                }
            }
			if (Prev != null)
				Prev.Size = Prev.InitSize.Format(Game);
        }

		/// <summary>
		/// Форматирует UI панель
		/// </summary>
		/// <param name="pictures">Список иконок юнитов, которые должны быть отформатированы на центральной панели</param>
		/// <param name="uiIcons">Список UI иконок, которые должны быть отформатированы</param>
		private void FormateUIPanel(List<Sprite> pictures = null, List<Sprite> uiIcons = null, List<IcoWithHP> unitIcons = null) {
			if (pictures != null) {
				Vector2 pos = new Vector2(bottomPanel.CentralPath.Left, bottomPanel.CentralPath.Top + (30f).FormatH(Game));
				int countInLine = bottomPanel.CentralPath.Width / ((int)pictures.AverrageSize().X + UI_X_OFFSET);
				int lines = pictures.Count / countInLine + ((pictures.Count % countInLine == 0) ? 0 : 1);
				if (pictures.Count != 0 && countInLine != 0 && lines != 0) {
					int currentCount = 0;
					int currentLine = 0;
					for (int i = 0; i < pictures.Count; i++) {
						pictures[i].Position = new Vector2(pos.X + (pictures[i].Size.X + UI_X_OFFSET) * currentCount++, pos.Y + (pictures[i].Size.Y + UI_Y_OFFSET) * currentLine);
						if (currentCount == countInLine) {
							currentCount = 0;
							currentLine++;
						}
					}
				}
			}
			if (uiIcons != null) {
				throw new NotImplementedException();
			}
			if (unitIcons != null) {
				Vector2 pos = new Vector2(bottomPanel.CentralPath.Left, bottomPanel.CentralPath.Top + (30f).FormatH(Game));
				int countInLine = bottomPanel.CentralPath.Width / ((int)unitIcons.AverrageSize().X + UI_X_OFFSET);
				int lines = unitIcons.Count / countInLine + ((unitIcons.Count % countInLine == 0) ? 0 : 1);
				if (unitIcons.Count != 0 && countInLine != 0 && lines != 0) {
					int currentCount = 0;
					int currentLine = 0;
					for (int i = 0; i < unitIcons.Count; i++) {
						unitIcons[i].Position = new Vector2(pos.X + (unitIcons[i].Size.X + UI_X_OFFSET) * currentCount++, pos.Y + (unitIcons[i].Size.Y + UI_Y_OFFSET) * currentLine);
						if (currentCount == countInLine) {
							currentCount = 0;
							currentLine++;
						}
					}
				}
			}
		}

		/// <summary>
		/// Возвращает True, если точка (x,y) находится в пределах панели и False в противном случае
		/// </summary>
		/// <param name="x">Растояние по оси X</param>
		/// <param name="y">Растояние по оси Y</param>
		public bool Contains(float x, float y) {
			Point p = new Point((int)x, (int)y);
			return bottomRect.Contains(p) || topRect.Contains(p);
		}

		/// <summary>
		/// Возвращает True, если точка (x,y) лежит в пределах элемента UI панели и False в противном случае
		/// </summary>
		/// <param name="x">Растояние по оси X</param>
		/// <param name="y">Растояние по оси Y</param>
		public bool UIContains(float x, float y) {
			bool ui = false;
			foreach (var list in UiPanel) {
				foreach (var sprite in list) {
					ui = sprite.Contains(x, y);
					if (ui)
						return ui;
				}
			}
			return ui;
		}

		private void OnObjectChanged(EventArgs e) {
			EventHandler<EventArgs> objectChanged = ObjectChanged;
			if (objectChanged != null)
				objectChanged(this, e);
		}

		private void Panel_ObjectChanged(object sender, EventArgs e) {
			AnalyseObject(Objects);
		}

		private void OnUIPanelClick(UIPanelEventArgs e) {
			EventHandler<UIPanelEventArgs> uiPanelClick = UIPanelClick;
			if (uiPanelClick != null)
				uiPanelClick(this, e);
		}

		private void OnObjectIconClick(ObjectIconsEventArgs e) {
			EventHandler<ObjectIconsEventArgs> objectIconClick = ObjectIconClick;
			if (objectIconClick != null)
				objectIconClick(this, e);
		}
	}
}
