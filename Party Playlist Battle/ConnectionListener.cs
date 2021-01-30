using System;
using System.Collections.Generic;
using System.Text;

namespace Party_Playlist_Battle
{
    public class Connection_Listener
    {
        public List<Active_Connection> active_connection_list {
            get => default;
            set {
            }
        }

        public Active_Connection Active_Connection {
            get => default;
            set {
            }
        }

        /// <summary>
        /// creates instance of active_connection class
        /// </summary>
        public int create_active_connection() {
            throw new System.NotImplementedException();
        }

        public void start_listening() {
            throw new System.NotImplementedException();
        }

        public void terminate_active_connection() {
            throw new System.NotImplementedException();
        }

        public void generate_token() {
            throw new System.NotImplementedException();
        }
    }
}