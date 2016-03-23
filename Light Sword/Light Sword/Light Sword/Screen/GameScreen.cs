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
using UnitsLib;
using UnitsLib.Enums;
using UnitsLib.Surround;
using UnitsLib.Events;
using UnitsLib.Interfaces;
using SettingsLib;

namespace Light_Sword
{
	/// <summary>
	/// Экран, на котором происходит основная игра
	/// </summary>
	public class GameScreen : Screen
	{
		#region Variables

		Settings settings;
		AnalysedSettings cfg;

		SpriteFont arial;
		SpriteFont arialB;
		SpriteFont obj;

		Random random = new Random();

		Panel panel;
		Ground ground;
		MeshSelection mesh;
		Minimap minimap;
		Scene scene;
		int sceneSpeed;
		MouseStates State = MouseStates.Normal;

		Sprite BuildingRectangleSprite;
		BuildingRectangle BuildingRectangle;

        List<TObject> FocusObjects = new List<TObject>(World.MAX_FOCUSED_OBJECTS);
        bool isCtrl = false;
		bool isClick = false;

		#endregion

		public GameScreen(Game1 game, Settings settings)
			: base(game) {
			this.settings = settings;			
			Initialize();
		}

		protected override void Initialize() {
			cfg = new AnalysedSettings(this.settings);
			World.SceneMoveSpeed = cfg.SceneVelocity;
			World.SetResource(500, 300, 200, true);
			World.SetResource(500, 300, 200, false);
			World.InitCells();

			sceneSpeed = World.SceneMoveSpeed;

			player.Stop();
			player.Clear();

			base.Initialize();			
		}

		protected override void LoadContent() {
			arial = Content.Load<SpriteFont>(Fnames.Arial14);
			obj = Content.Load<SpriteFont>(Fnames.obj);
			arialB = Content.Load<SpriteFont>(Fnames.MSReferenceSansSerif14B);
			World.Flag.Image = new Sprite(Content);
			World.Flag.Image.LoadTexture(Fnames.Flag);
			World.Flag.Visible = false;

			Vector2 ScaleFactor = new Vector2(210 * ((float)Window.ClientBounds.Width / 1280), 200 * ((float)Window.ClientBounds.Height / 1024));
			panel = new Panel(Game, arial, arialB, Window.ClientBounds);
			panel.UIPanelClick += new EventHandler<UIPanelEventArgs>(panel_UIPanelClick);
			panel.ObjectIconClick += new EventHandler<ObjectIconsEventArgs>(panel_ObjectIconClick);
			ground = new Ground(World.Field, cfg.Ground);
			mesh = new MeshSelection(Game, Fnames.Mesh);
			minimap = new Minimap(Game, World.Field, new Rectangle(Right - (int)(Right / 5.2), Bottom - (int)(Bottom / 4.55), (int)ScaleFactor.X, (int)ScaleFactor.Y), ground.Tiles);
			minimap.Click += new EventHandler<MinimapMouseEventArgs>(minimap_Click);
			World.Panel = panel;
			scene = new Scene(World.Field, panel);

			#region Game.Components

			Game.Components.Clear();
			Game.Components.Add(panel);
			Game.Components.Add(mesh);
			Game.Components.Add(minimap);
			Game.Components.Add(new FPS(Game, Fnames.Arial14, new Vector2(Window.ClientBounds.Width / 2f, 0)));
			Game.Components.Add(new GameCursor(Game, Fnames.Cursor));

			#endregion

			#region Music

			Song fearNotThisNight = Content.Load<Song>(Fnames.FearNotThisNight);
			Song windGuideYou = Content.Load<Song>(Fnames.WindGuideYou);
			Song heritageOfKings = Content.Load<Song>(Fnames.HeritageOfKings);
			Song stronghold = Content.Load<Song>(Fnames.Stronghold);
			Song elfish1 = Content.Load<Song>(Fnames.Elfish1);
			Song elfish2 = Content.Load<Song>(Fnames.Elfish2);
			Song elfish3 = Content.Load<Song>(Fnames.Elfish3);

			player.Add(windGuideYou);
			player.Add(fearNotThisNight);
			player.Add(heritageOfKings);
			player.Add(stronghold);
			player.Add(elfish1);
			player.Add(elfish2);
			player.Add(elfish3);

			#endregion

			if (!cfg.MusicIsMuted) {
				player.Shuffle();
				if (random.Next(100) == 1)
					MakeBonus();
				player.Start();
			}

#if true
			for (int i = 0; i < 1000; i++) {
				new TStone(random.Next(World.Field.Width), random.Next(World.Field.Height), random.Next(500), MultiSprite.CreateSprite(Content, spriteBatch, Fnames.Stone,
					new Vector2(random.Next(World.Field.Width), random.Next(World.Field.Height)), new Vector2(60, 60), new Vector2(1, 2), World.FPS));
				string name = (random.Next(0, 2) == 0) ? Fnames.BigTree : Fnames.SmallTree;
                new TTree(random.Next(World.Field.Width), random.Next(World.Field.Height), i + 50, MultiSprite.CreateSprite(Content, spriteBatch, name,
					new Vector2(random.Next(World.Field.Width), random.Next(World.Field.Height)), new Vector2(60, 60), new Vector2(3, 1), World.FPS));
			}
			for (int i = 0; i < 1000; i++) {
                new TCSwordman(random.Next(World.Field.Width - 36), random.Next(World.Field.Height - 36), 100, 10, 3, Vector2.Zero, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.Swordman,
				   new Vector2(random.Next(World.Field.Width - 36), random.Next(World.Field.Height - 36)), new Vector2(72, 72), new Vector2(5, 24), World.FPS));
			}

#if false
			for (int i = 0; i < 1000; i++) {
				new TEnemy(random.Next(World.Field.Width), random.Next(World.Field.Height), 100, 10, 3, Vector2.Zero, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.Enemy,
				   new Vector2(random.Next(World.Field.Width), random.Next(World.Field.Height)), new Vector2(30, 30), new Vector2(6, 2), World.FPS));
			}
#endif

#endif

            BuildingRectangleSprite = new Sprite(Content);
			BuildingRectangleSprite.LoadTexture(Fnames.Mesh, Vector2.Zero, Vector2.One);
			BuildingRectangleSprite.Visible = false;
			BuildingRectangle = new BuildingRectangle(BuildingRectangleSprite);
		}

		protected override void UnloadContent() {

		}

		public override void Update(GameTime gameTime) {
			OKBS = KBS;
			KBS = Keyboard.GetState();
			OMS = MS;
			MS = Mouse.GetState();
			MouseHandler(MS, OMS, State);
			KeyboardHandler(KBS, OKBS);
			if (KBS.IsKeyDown(Keys.Escape)) {
				World.TObjects.Clear();
				World.TUnits.Clear();
				World.TBuildings.Clear();
				World.TCUnits.Clear();
				World.TEnemys.Clear();
				World.TStones.Clear();
				World.TTrees.Clear();
				World.DeadBuildings.Clear();
				World.DeadTrees.Clear();
				World.DeadUnits.Clear();
				Game.CurrentScreen = new MainMenuScreen(Game, settings);				
			}
			for (int i = World.TUnits.Count - 1; i >= 0; i--) {
				Aims aim = World.TUnits[i].Aim;
				switch (aim) {
					case Aims.Attack:
					case Aims.Download:
					case Aims.MoveToPoint:
					case Aims.Upload:
					case Aims.FindResorse:
						ImplementAims(World.TUnits[i]);
						break;
					case Aims.Stand:
                        //World.TUnits[i].P = new Vector2(random.Next(World.Field.Width), random.Next(World.Field.Height));
                        //World.TUnits[i].Aim = Aims.MoveToPoint;
						World.TUnits[i].CheckArea();
						break;
					case Aims.UploadNow:
						(World.TUnits[i] as TCWorker).Upload();
						break;
					case Aims.AttackNow:
						World.TUnits[i].Attack();
						break;
					case Aims.DownloadNow:
						(World.TUnits[i] as TCWorker).Download();
						break;
				}
			}
			for (int i = World.TBuildings.Count - 1; i >= 0; i--) {
				if (World.TBuildings[i] is ICreate)
					(World.TBuildings[i] as ICreate).Create();
			}
			DeadUnitsHandler(World.DeadUnits);
			DeadTreesHandler(World.DeadTrees);
			DeadBuildingsHandler(World.DeadBuildings);
			World.Scene = scene;

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			ground.Draw(spriteBatch, scene);
			foreach (var tree in World.DeadTrees.Keys) {
				tree.Draw(spriteBatch);
			}
			foreach (var building in World.DeadBuildings.Keys) {
				building.Draw(spriteBatch);
			}
			foreach (var unit in World.DeadUnits.Keys) {
				unit.Draw(spriteBatch);
			}
			for (int i = 0; i <= World.TObjects.Count - 1; i++) {
				World.TObjects[i].Draw(spriteBatch, gameTime, scene);
			}
			World.Flag.Image.Draw(spriteBatch);
			BuildingRectangle.Draw(spriteBatch);

			#region Debug

            spriteBatch.DrawString(arial, FocusObjects.Count.ToString(), new Vector2(50, 50), Color.Brown);
			//spriteBatch.DrawString(arial, World.TObjects.Count.ToString(), new Vector2(100, 100), Color.White);
			//spriteBatch.DrawString(arial, World.TUnits.Count.ToString(), new Vector2(500, 500), Color.Red);
			//spriteBatch.DrawString(arial, new Point(MS.X, MS.Y).ToString(), new Vector2(100, 150), Color.Red);
			//spriteBatch.DrawString(arial, "Scene Location: " + (scene.Rect.Location).ToString(), new Vector2(100, 170), Color.Blue);
			//spriteBatch.DrawString(arial, "Field Right and Field Bottom " + (World.Field.Right).ToString() + " " + (World.Field.Bottom).ToString(), new Vector2(100, 190), Color.Blue);
			//spriteBatch.DrawString(arial, "Scene Right and Scene Bottom: " + (scene.Rect.Right).ToString() + " " + (scene.Rect.Bottom).ToString(), new Vector2(100, 210), Color.Blue);
			//if (FocusObject != null) {
			////    spriteBatch.DrawString(arial, FocusObject.IsFocused.ToString(), new Vector2(100, 115), Color.White);
			////    spriteBatch.DrawString(arial, "image pos: " + FocusObject.Image.Position.ToString(), new Vector2(100, 225), Color.Black);
			////    spriteBatch.DrawString(arial, "obj location: " + FocusObject.Position.ToString() + FocusObject.Visible.ToString(), new Vector2(100, 250), Color.Black);
			////    spriteBatch.DrawString(arial, "intersect?: " + scene.Contains(FocusObject).ToString(), new Vector2(100, 275), Color.Black);
			////    if (FocusObject is TUnit)
			////        spriteBatch.DrawString(arial, (FocusObject as TUnit).Aim.ToString(), new Vector2(100, 300), Color.Red);
			//    spriteBatch.DrawString(arial, ((FocusObject as TUnit).Target != null).ToString(), new Vector2(100, 130), Color.Red);
			//}
			//spriteBatch.DrawString(arial, BuildingRectangle.Visible.ToString(), new Vector2(300, 300), Color.Red);
			//spriteBatch.DrawString(arial, BuildingRectangle.Position.ToString(), new Vector2(300, 320), Color.Red);
			//spriteBatch.DrawString(arial, BuildingRectangle.Size.ToString(), new Vector2(300, 340), Color.Red);
			//spriteBatch.DrawString(arial, BuildingRectangle.box.ToString(), new Vector2(300, 300), Color.Red);

			#endregion

			base.Draw(gameTime);

			spriteBatch.End();
		}

		/// <summary>
		/// Обработчик мыши
		/// </summary>
		/// <param name="ms">Текущее состояние мыши</param>
		/// <param name="oms">Предшествующее состояние мыши</param>
		protected override void MouseHandler(MouseState ms, MouseState oms, MouseStates state) {
			if (ms.X == this.Left) scene.MoveOn(new Vector2(-sceneSpeed, 0));
			if (ms.X == this.Right - 1) scene.MoveOn(new Vector2(sceneSpeed, 0));
			if (ms.Y == this.Top) scene.MoveOn(new Vector2(0, -sceneSpeed));
			if (ms.Y == this.Bottom - 1) scene.MoveOn(new Vector2(0, sceneSpeed));

			switch (state) {
				case MouseStates.Normal:
					MouseHandlerNormal(ms, oms);
					break;
				case MouseStates.BuildingMesh:
					MouseHandlerBuildingMesh(ms, oms);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Обработчик клавиатуры
		/// </summary>
		/// <param name="kbs">Текущее состояние калвиатуры</param>
		/// <param name="okbs">Предшествующее состояние клавиатуры</param>
		protected override void KeyboardHandler(KeyboardState kbs, KeyboardState okbs) {
			Vector2 msPos = MousePosition(MS);
            if (kbs.IsKeyDown(Keys.LeftControl) && !okbs.IsKeyDown(Keys.LeftControl)) {
                isCtrl = true;
            }
            if (kbs.IsKeyUp(Keys.LeftControl) && !okbs.IsKeyUp(Keys.LeftControl)) {
                isCtrl = false;
            }

            #region Debug

            if (kbs.IsKeyDown(Keys.W) && !okbs.IsKeyDown(Keys.W))
                new TCSwordman(msPos.X - 15, msPos.Y - 15, 100, 10, 3, Vector2.Zero, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.Swordman,
				   new Vector2(msPos.X - 15, msPos.Y - 15), new Vector2(72, 72), new Vector2(5, 24), World.FPS));
			if (kbs.IsKeyDown(Keys.E) && !okbs.IsKeyDown(Keys.E))
                new TEnemy(msPos.X - 15, msPos.Y - 15, 100, 10, 0, Vector2.Zero, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.Enemy,
					new Vector2(msPos.X - 15, msPos.Y - 15), new Vector2(30, 30), new Vector2(6, 2), World.FPS));
			if (kbs.IsKeyDown(Keys.T) && !okbs.IsKeyDown(Keys.T))
                new TStone(msPos.X - 30, msPos.Y - 30, 500, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.Stone,
					new Vector2(msPos.X - 30, msPos.Y - 30), new Vector2(60, 60), new Vector2(1, 2), World.FPS));
			if (kbs.IsKeyDown(Keys.G) && !okbs.IsKeyDown(Keys.G))
                new TTree(msPos.X - 30, msPos.Y - 30, 150, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.BigTree,
					new Vector2(msPos.X - 30, msPos.Y - 30), new Vector2(60, 60), new Vector2(3, 1), World.FPS));
			if (kbs.IsKeyDown(Keys.H) && !okbs.IsKeyDown(Keys.H))
                new TTree(msPos.X - 25, msPos.Y - 25, 100, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.SmallTree,
					new Vector2(msPos.X - 25, msPos.Y - 25), new Vector2(50, 50), new Vector2(3, 1), World.FPS));
			if (kbs.IsKeyDown(Keys.A) && !okbs.IsKeyDown(Keys.A))
                new TCityCenter(msPos.X - 70, msPos.Y - 70, 500, 5, true, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.CityCenter,
					new Vector2(msPos.X - 70, msPos.Y - 70), new Vector2(150, 140), new Vector2(3, 1), World.FPS));
			if (kbs.IsKeyDown(Keys.F) && !okbs.IsKeyDown(Keys.F)) {
                new TFarm(msPos.X - 60, msPos.Y - 34, 250, 1, true, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.Farm,
					new Vector2(msPos.X - 60, msPos.Y - 34), new Vector2(120, 67), new Vector2(3, 1), World.FPS));
			}
			if (kbs.IsKeyDown(Keys.B) && !okbs.IsKeyDown(Keys.B)) {
                new TBaracks(msPos.X - 48, msPos.Y - 46, 400, 4, true, MultiSprite.CreateSprite(Content, spriteBatch, Fnames.Baracks,
					new Vector2(msPos.X - 48, msPos.Y - 46), new Vector2(97, 92), new Vector2(3, 1), World.FPS));
			}
			if (kbs.IsKeyDown(Keys.Space) && !okbs.IsKeyDown(Keys.Space)) {
				if (TObjectUnderPoint(MS.X, MS.Y) is IHP)
					(TObjectUnderPoint(MS.X, MS.Y) as IHP).HP -= 60;
				if (TObjectUnderPoint(MS.X, MS.Y) is ISource)
					(TObjectUnderPoint(MS.X, MS.Y) as ISource).Supply -= 49;
			}
			if (kbs.IsKeyDown(Keys.Enter) && !okbs.IsKeyDown(Keys.Enter)) {
				Saver.Save();
			}
			if (kbs.IsKeyDown(Keys.RightShift) && !okbs.IsKeyDown(Keys.RightShift)) {
				Loader.Load();
			}
			if (kbs.IsKeyDown(Keys.Delete) && !okbs.IsKeyDown(Keys.Delete)) {
				if (TObjectUnderPoint(MS.X, MS.Y) != null)
					TObjectUnderPoint(MS.X, MS.Y).Dispose();
            }

            #endregion
        }

		#region Private Methods

		/// <summary>
		/// Возвращает объект под точкой (x,y)
		/// </summary>
		/// <param name="x">Координата по оси X (мировые)</param>
		/// <param name="y">Координата по оси Y (мировые)</param>
		/// <returns>Объект под точкой (x,y)</returns>
		private TObject TObjectUnderPoint(int x, int y) {
			for (int i = World.TObjects.Count - 1; i >= 0; i--) {
				TObject g = World.TObjects[i];
				if (g.PtInside(x + scene.Rect.X, y + scene.Rect.Y))
					return g;
			}
			return null;
		}

		/// <summary>
		/// Позиция мыши относительно мира, а не левого верхнего угла окна
		/// </summary>
		/// <param name="ms">Текущее состояние мыши</param>
		private Vector2 MousePosition(MouseState ms) {
			return new Vector2(ms.X + scene.Rect.X, ms.Y + scene.Rect.Y);
		}

		/// <summary>
		/// Обрабатывает словарь мертвых юнитов
		/// </summary>
		private void DeadUnitsHandler(Dictionary<MultiSprite, Vector2> units) {
			foreach (var unit in units.Keys) {
				Vector2 drawPosition = new Vector2(units[unit].X - scene.Rect.X, units[unit].Y - scene.Rect.Y);
				unit.Position = drawPosition;
			}
			if (units.Count >= 1000)
				units.Clear();
		}

		/// <summary>
		/// Обрабатывает словарь мертвых деревьев
		/// </summary>
		private void DeadTreesHandler(Dictionary<MultiSprite, Vector2> trees) {
			foreach (var tree in trees.Keys) {
				Vector2 drawPosition = new Vector2(trees[tree].X - scene.Rect.X, trees[tree].Y - scene.Rect.Y);
				tree.Position = drawPosition;
			}
			if (trees.Count >= 1000)
				trees.Clear();
		}

		/// <summary>
		/// Обрабатывает мертвые здания
		/// </summary>
		private void DeadBuildingsHandler(Dictionary<MultiSprite, Vector2> buildings) {
			foreach (var build in buildings.Keys) {
				Vector2 drawPosition = new Vector2(buildings[build].X - scene.Rect.X, buildings[build].Y - scene.Rect.Y);
				build.Position = drawPosition;
			}
			if (buildings.Count >= 100)
				buildings.Clear();
		}

		/// <summary>
		/// Обработка мыши в нормальном состоянии
		/// </summary>
		/// <param name="ms">Текущее состояние мыши</param>
		/// <param name="oms">Предшествующее состояние мыши</param>
		private void MouseHandlerNormal(MouseState ms, MouseState oms) {
			if (ms.LeftButton == ButtonState.Pressed && oms.LeftButton == ButtonState.Released && !panel.Contains(ms.X, ms.Y)) {
				if (isCtrl) {
					TObject fo = (panel.Contains(ms.X, ms.Y)) ? null : TObjectUnderPoint(ms.X, ms.Y);
					if (fo is TCUnit && fo != null && !FocusObjects.Contains(fo) && FocusObjects.All(x => x is TCUnit)) {
						FocusObjectsAdd(fo);
					} else if (FocusObjects.Contains(fo)) {
						FocusObjects.Remove(fo);
						fo.IsFocused = false;
					}
				} else {
					foreach (var obj in FocusObjects) {
						obj.IsFocused = false;
					}
					FocusObjects.Clear();
					TObject fo = (panel.Contains(ms.X, ms.Y)) ? null : TObjectUnderPoint(ms.X, ms.Y);
					if (fo != null) {
						FocusObjectsAdd(fo);
					}
				}
				if (isClick)//TODO двойной клик
					if (FocusObjects.Count == 1 && FocusObjects[0] is TCUnit) {
						FocusObjects[0].IsFocused = false;
						TCUnit fo = FocusObjects[0] as TCUnit;
						FocusObjects.Clear();
						Rectangle view = scene.Rect;
						view.Height -= (int)panel.BottomPanelSize.Y;
						var unitsInScreen = from obj in World.TCUnits
											where obj.UnitName == fo.UnitName && view.Intersects(new Rectangle((int)obj.Left, (int)obj.Top, (int)obj.Width, (int)obj.Height))
											select obj;
						foreach (var unit in unitsInScreen) {
							FocusObjectsAdd(unit);
						}
						
					}
				isClick = true;
			}
			if (ms.LeftButton == ButtonState.Pressed && oms.LeftButton == ButtonState.Pressed && !panel.Contains(ms.X, ms.Y))
				if (mesh.Vis) {
					foreach (var obj in FocusObjects) {
						obj.IsFocused = false;
					}
					FocusObjects.Clear();
					Rectangle rect = new Rectangle(mesh.Rect.X + World.Scene.Rect.X, mesh.Rect.Y + World.Scene.Rect.Y, mesh.Rect.Width, mesh.Rect.Height);
					for (int i = World.TCUnits.Count - 1; i >= 0; i--) {
						TObject obj = World.TCUnits[i];
						if (rect.Intersects(new Rectangle((int)obj.Left, (int)obj.Top, (int)obj.Width, (int)obj.Height))) {
							FocusObjectsAdd(obj);
						}

					}
				}
			if (ms.RightButton == ButtonState.Pressed && oms.RightButton == ButtonState.Released && !panel.Contains(ms.X, ms.Y)) {
				TObject objUndP = TObjectUnderPoint(ms.X, ms.Y);
				foreach (var obj in FocusObjects) {
					if (obj is TCUnit) {
						TCUnit fo = obj as TCUnit;
						if (objUndP == null) {
							fo.Aim = Aims.MoveToPoint;
							fo.P = MousePosition(ms);
							fo.Target = null;
							fo.BTarget = null;
						}
						if (objUndP is TEnemy) {
							fo.Aim = Aims.Attack;
							fo.Target = objUndP as TEnemy;
							fo.P = fo.Target.Center;
						}
						if (objUndP is TBuilding && !(objUndP as TBuilding).Side) {
							fo.Aim = Aims.Attack;
							fo.BTarget = objUndP as TBuilding;
							fo.P = fo.BTarget.Center;
						}
					}
					if (obj is TCWorker) {
						TCWorker fo = obj as TCWorker;
						if (objUndP is ISource) {
							fo.Aim = Aims.Download;
							fo.Source = objUndP as ISource;
							fo.P = fo.Source.Center;
							fo.Target = null;
							fo.BTarget = null;
						} else
							fo.Source = null;
					}
					if (obj is TCreateBuilding) {
						TCreateBuilding fo = obj as TCreateBuilding;
						if (objUndP == null && fo.Side)
							fo.P = MousePosition(ms);
					}
				}
			}
			if (ms.LeftButton == ButtonState.Released && oms.LeftButton == ButtonState.Pressed) {
				isClick = false;
			}
		}

		/// <summary>
		/// Обработка мыши в состоянии строительного прямоугольника
		/// </summary>
		/// <param name="ms">Текущее состояние мыши</param>
		/// <param name="oms">Предшествующее состояние мыши</param>
		private void MouseHandlerBuildingMesh(MouseState ms, MouseState oms) {
			if (ms.RightButton == ButtonState.Pressed && oms.RightButton == ButtonState.Released) {
				BuildingRectangle.Visible = false;
				State = MouseStates.Normal;
			}
			BuildingRectangle.Update(ms, oms, scene);
			if (ms.LeftButton == ButtonState.Pressed && oms.LeftButton == ButtonState.Released) {
				if (BuildingRectangle.Color == World.GreenBuildingRectangle) {
                    if (FocusObjects.Count == 1) {
                        TObject fo = FocusObjects[0];
                        switch (BuildingRectangle.Name) {
                            case BuildingNames.CityCenter:
                                break;
                            case BuildingNames.Farm:
                                if (fo is TCityCenter)
                                    (fo as TCityCenter).CreateBuilding(BuildingRectangle.Name, BuildingRectangle.Position);
                                break;
                            case BuildingNames.Baracks:
                                if (fo is TCityCenter)
                                    (fo as TCityCenter).CreateBuilding(BuildingRectangle.Name, BuildingRectangle.Position);
                                break;
                            default:
                                break;
                        }
                    }
					BuildingRectangle.Visible = false;
					State = MouseStates.Normal;
				}
			}
		}

		/// <summary>
		/// Обработчик события клика по UI панели
		/// </summary>
		private void panel_UIPanelClick(object sender, UnitsLib.Events.UIPanelEventArgs e) {
			Point index = e.Index;
            if (FocusObjects.Count == 1) {
                TObject fo = FocusObjects[0];
                if (index == new Point(0, 0) && fo is TCityCenter) {
                    (fo as TCityCenter).ToQueue(UnitNames.Worker);
                    return;
                }
                if (index == new Point(3, 0) && fo is TCityCenter) {
                    State = MouseStates.BuildingMesh;
                    BuildingRectangle.SetRectangle(BuildingNames.Farm);
                    fo.IsFocused = false;
                    return;
                }
                if (index == new Point(3, 1) && fo is TCityCenter) {
                    State = MouseStates.BuildingMesh;
                    BuildingRectangle.SetRectangle(BuildingNames.Baracks);
                    fo.IsFocused = false;
                    return;
                }
                if (index == new Point(0, 1) && fo is TFarm) {
                    (fo as TFarm).ToQueue(UnitNames.Peasant);
                    return;
                }
                if (index == new Point(0, 2) && fo is TBaracks) {
                    (fo as TBaracks).ToQueue(UnitNames.Swordman);
                    return;
                }
                if (index == new Point(1, 0) && fo is TCUnit) {
                    TCUnit unit = fo as TCUnit;
                    unit.Stop();
                    unit.Center = unit.Center.Round();
                    unit.Aim = Aims.Stand;
                    return;
                }
                if (index == new Point(1, 1) && fo is TCUnit) {
                    TCUnit unit = fo as TCUnit;
                    unit.HP = 0;
                    return;
                }
                if (index == new Point(2, 0) && fo is IQueue && fo is ISide && (fo as ISide).Side) {
                    (fo as IQueue).Clear();
                    return;
                }
            }
		}

		/// <summary>
		/// Обработчик события клика по иконке юнита на центральной панели
		/// </summary>
		private void panel_ObjectIconClick(object sender, ObjectIconsEventArgs e) {
			foreach (var obj in FocusObjects) {
				obj.IsFocused = false;
			}
			FocusObjects.Clear();
			FocusObjectsAdd(e.Object);
		}

		/// <summary>
		/// Реализатор целей юнита.
		/// Юнит совершает различные действия в зависимости от целей
		/// </summary>
		/// <param name="unit">Юнит, цели которого реализуются</param>
		private void ImplementAims(TUnit unit) {

			if (unit.Aim == Aims.Download) {
				if ((unit as IWorker).Source != null) {
					ISource source = (unit as IWorker).Source;
					TObject obj;
					if (source is TTree)
						obj = source as TTree;
					else
						obj = source as TStone;
					if (unit.Intersect(obj)) {
						unit.Aim = Aims.DownloadNow;
						unit.Stop();
						unit.Center = new Vector2(round(unit.Center.X), round(unit.Center.Y));
					}
				}
			} else if (unit.Aim == Aims.Upload) {
				if ((unit as IWorker).CityCenter != null)
					if (unit.Intersect((unit as IWorker).CityCenter)) {
						unit.Aim = Aims.UploadNow;
						unit.Stop();
						unit.Center = new Vector2(round(unit.Center.X), round(unit.Center.Y));
					}
			} else if (unit.Aim == Aims.Attack && unit is IAttack) {
				if (unit.Target != null)
					if (unit.Intersect(unit.Target)) {
						unit.Aim = Aims.AttackNow;
						unit.Stop();
						unit.Center = new Vector2(round(unit.Center.X), round(unit.Center.Y));
					}
				if (unit.BTarget != null)
					if (unit.Intersect(unit.BTarget)) {
						unit.Aim = Aims.AttackNow;
						unit.Stop();
						unit.Center = new Vector2(round(unit.Center.X), round(unit.Center.Y));
					}
			}

			if (unit.Aim == Aims.MoveToPoint || unit.Aim == Aims.Attack || unit.Aim == Aims.Download || unit.Aim == Aims.Upload
				|| unit.Aim == Aims.FindResorse) {
				unit.MoveToNextPoint(scene);
			}
		}

		/// <summary>
		/// Обработчик щелчка мышью по миникарте
		/// </summary>
		private void minimap_Click(object sender, MinimapMouseEventArgs e) {
			if (e.MouseState.LeftButton == ButtonState.Pressed) {
				scene = minimap.SetScene(e.Position);
				return;
			}
			if (e.MouseState.RightButton == ButtonState.Pressed) {
                foreach (var obj in FocusObjects) {
                    if (obj is TUnit) {
                        TUnit fo = obj as TUnit;
                        fo.Aim = Aims.MoveToPoint;
                        fo.Target = null;
                        fo.BTarget = null;
                    }
                    if (obj is IPoint) {
                        IPoint fo = obj as IPoint;
                        fo.P = e.Position;
                    }
                }
			}
		}

		private double sqr(double a) {
			return a * a;
		}
		private double sqrt(double a) {
			return Math.Sqrt(a);
		}
		private float abs(float a) {
			return Math.Abs(a);
		}
		private float round(float a) {
			return (float)Math.Round(a);
		}
        private void FocusObjectsAdd(TObject obj) {
            if (FocusObjects.Count < World.MAX_FOCUSED_OBJECTS) {
                FocusObjects.Add(obj);
                obj.IsFocused = true;
            }
        }

		private void MakeBonus() {
			Song sold1 = Content.Load<Song>(@"Sound\Music\Солдаты1");
			Song sold2 = Content.Load<Song>(@"Sound\Music\Солдаты2");
			Song[] songs = player.Songs.ToArray();
			player.Songs.Clear();
			player.Add(sold1);
			player.Add(sold2);
			foreach (var song in songs) {
				player.Add(song);
			}
		}

		#endregion
	}
}
