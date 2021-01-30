using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
namespace Party_Playlist_Battle
{
    public class Active_Connection
    {
        public Socket user_socket {
            get => default;
            set {
            }
        }

        public int token {
            get => default;
            set {
            }
        }

        public Active_User Active_User {
            get => default;
            set {
            }
        }

        public Library_Manager Library_Manager {
            get => default;
            set {
            }
        }

        public Login_Manager Login_Manager {
            get => default;
            set {
            }
        }

        public Profile_Manager Profile_Manager {
            get => default;
            set {
            }
        }

        public Playlist_Manager Playlist_Manager {
            get => default;
            set {
            }
        }

        public Battle Battle {
            get => default;
            set {
            }
        }

        /// <summary>
        /// create instance of login manager, until a login is completed, create active user when logged in.
        /// </summary>
        public void run() {
            throw new System.NotImplementedException();
        }

        public void ip_block() {
            throw new System.NotImplementedException();
        }

        public void receive_message() {
            throw new System.NotImplementedException();
        }

        public void join_battle() {
            throw new System.NotImplementedException();
        }

        public void display_scoreboard() {
            throw new System.NotImplementedException();
        }
    }
}