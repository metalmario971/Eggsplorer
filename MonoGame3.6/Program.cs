﻿using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Core
{
    public class DesktopGameSystem : GameSystem
    {
        public override Platform GetPlatform()
        {
            return Core.Platform.Desktop;
        }

        public DesktopGameSystem(GameBase g) :
            base(g)
        {
        }
        public override void Exit()
        {
            Game.Exit();
        }
        public override bool LoadData(string filename, out string data)
        {
            data = "";
            try
            {
                IsolatedStorageFile f = GetIsolatedStorage();
                using (IsolatedStorageFileStream isoStream =
                    new IsolatedStorageFileStream(filename, FileMode.Open, f))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        data = reader.ReadLine();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Game.Log(ex.ToString());
            }

            return false;
        }
        public override bool SaveData(string filename, string data)
        {
            try
            {
                IsolatedStorageFile f = GetIsolatedStorage();
                using (IsolatedStorageFileStream isoStream =
                    new IsolatedStorageFileStream(filename, FileMode.Create, f))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine(data);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Game.Log(ex.ToString());
            }

            return false;
        }
        public IsolatedStorageFile GetIsolatedStorage()
        {
            IsolatedStorageFile f = null;
            try
            {
                f = IsolatedStorageFile.GetUserStoreForApplication();
            }
            catch (Exception ex)
            {
                Game.Log(ex.ToString());
                //On UWP this doesn't work.
                //error CS0117: 'IsolatedStorageFile' does not contain a definition for 'GetMachineStoreForAssembly'
                try
                {
                    f = IsolatedStorageFile.GetMachineStoreForAssembly();
                }
                catch (Exception ex2)
                {
                    Game.Log(ex2.ToString());
                }
            }
            return f;
        }
        public override void HideNav()
        {
        }
    }
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            using (var game = new MainGame())
            {
                game.Init(true, new DesktopGameSystem(game));
                game.Run();
            }
        }
    }
}
