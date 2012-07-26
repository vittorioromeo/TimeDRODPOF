#region
using System;
using SFMLStart;
using SFMLStart.Data;
using TimeDRODPOF.TDEditor;
using TimeDRODPOF.TDGame;
using TimeDRODPOF.TDLib;

#endregion

namespace TimeDRODPOF
{
    internal static class Program
    {
        private static void Main()
        {
            Settings.Framerate.IsLimited = true;
            Settings.Framerate.Limit = 60;

            var gameWindow = new GameWindow(1024, 768, 1);
            var game = new TDGGame(gameWindow);
            var editor = new TDEEditor(gameWindow);

            game.Editor = editor;
            editor.Game = game;

            TDLFactory.Game = game;

            gameWindow.RenderWindow.Closed += (o, e) => Environment.Exit(0);
            gameWindow.SetGame(game);
            gameWindow.Run();
        }
    }
}