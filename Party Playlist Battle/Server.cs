using System;
using System.Collections.Generic;
using System.Text;

namespace Party_Playlist_Battle
{
    public class Server
    {
        public Connection_Listener Connection_Listener {
            get => default;
            set {
            }
        }

        public Playlist Playlist {
            get => default;
            set {
            }
        }

        /// <summary>
        /// start playlist and connectionlistener, parse listener pointer for connections
        /// </summary>
        public void run() {
            throw new System.NotImplementedException();
        }
    }
}