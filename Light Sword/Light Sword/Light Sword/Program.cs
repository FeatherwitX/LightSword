using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework.Content;
using SettingsLib;
using WF = System.Windows.Forms;
using Graph;

namespace Light_Sword
{
#if WINDOWS
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
			string errStr = "";

			using (Game1 game = new Game1(Settings.GetSettings())) {
                try {
                    game.Run();
                } catch (ContentLoadException e) {
                    game.IsMouseVisible = true;
                    errStr = String.Format("{0}\n{1}", e.Message, "Check game content or reinstall the game");
                } catch (PlatformNotSupportedException e) {
                    game.IsMouseVisible = true;
                    errStr = e.Message;
                } catch (Exception e) {
                    game.IsMouseVisible = true;
                    errStr = String.Format("\t{0}\n\t{1}\n{2}", "Unknown error.", e.Message, e.StackTrace);
                } finally {
                    game.Exit();
                    if (errStr != "")
                        GetError(errStr);
                }
			}
        }

		static void GetError(string message) {
			WF.MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
    }
#endif
}

