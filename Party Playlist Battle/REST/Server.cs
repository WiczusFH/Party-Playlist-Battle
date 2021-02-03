using System;
using System.Collections.Generic;
using System.Text;

namespace Party_Playlist_Battle
{
    public class Server
    {
        public Connection_Listener listener;

        public Playlist Playlist {
            get => default;
            set {
            }
        }

        /// <summary>
        /// start playlist and connectionlistener, parse listener pointer for connections
        /// </summary>
        public void run() {
            listener = new Connection_Listener();
            listener.start();

            //commands
            bool keepalive = true;
            string input;
            while (keepalive) {
                input = Console.ReadLine();
                if (input.StartsWith("kill ")) {
                    listener.kill_active_connection(Int32.Parse(input.Substring(5)));
                }
                if (input.StartsWith("list")) {
                    listener.list_active_connections();
                }
                if (input == "quit" || input == "q") {
                    keepalive = false;
                }
            }
        }
    }
}