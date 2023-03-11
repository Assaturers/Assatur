using System;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;

namespace ModdingToolkit.AssetsExtractor
{
    public class MainThread
    {
        private Thread _thread;

        ~MainThread()
        {
            Game.Dispose();
        }

        public void Start()
        {
            _thread = new Thread(delegate(object _)
            {
                Game = new Main();
                Game.Run();
            });

            _thread.Start();
        }

        public void Stop()
        {
            Game.Exit();
            _thread.Join();
        }

        public void Join()
        {
            _thread.Join();
        }

        public bool IsAlive => _thread.IsAlive;

        public Main Game { get; private set; }
    }
}