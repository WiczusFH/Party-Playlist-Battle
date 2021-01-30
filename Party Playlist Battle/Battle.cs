using System;
using System.Collections.Generic;
using System.Text;

namespace Party_Playlist_Battle
{
    public class Battle
    {
        public System.Collections.Generic.List<user_battle_info> user_infos {
            get => default;
            set {
            }
        }

        public user_battle_info user_battle_info {
            get => default;
            set {
            }
        }

        public int start_tournament() {
            throw new System.NotImplementedException();
        }

        public void announce_results() {
            throw new System.NotImplementedException();
        }

        public void start_round() {
            throw new System.NotImplementedException();
        }

        public void prompt() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// win=1, draw=0, lose=-1
        /// </summary>
        public int action_eval() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 1 win, 0 draw, -1 lose
        /// </summary>
        public static int action_eval(Battle_Actions action_1, Battle_Actions action_2) {

            return 0;
        }
    }
}