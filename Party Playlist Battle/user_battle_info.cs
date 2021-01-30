using System;
using System.Collections.Generic;
using System.Text;

namespace Party_Playlist_Battle
{
    public struct user_battle_info
    {
        public int uid {
            get => default;
            set {
            }
        }

        public int round_score {
            get => default;
            set {
            }
        }

        /// <summary>
        /// max 5
        /// </summary>
        public Battle_Actions[] actions {
            get => default;
            set {
            }
        }

        public int battle_score {
            get => default;
            set {
            }
        }
    }
}