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
        /// 1 win, 0 draw, -1 lose
        /// </summary>
        public static int action_eval(Battle_Actions action_1, Battle_Actions action_2) {
            if (action_1 == Battle_Actions.Lizard) {
                if (action_2 == Battle_Actions.Lizard) {
                    return 0;
                }
                if (action_2 == Battle_Actions.Spock || action_2 == Battle_Actions.Paper) {
                    return 1;
                }
                return -1;
            }

            if (action_1 == Battle_Actions.Spock) {
                if (action_2 == Battle_Actions.Spock) {
                    return 0;
                }
                if (action_2 == Battle_Actions.Rock || action_2 == Battle_Actions.Scissors) {
                    return 1;
                }
                return -1;
            }

            if (action_1 == Battle_Actions.Scissors) {
                if (action_2 == Battle_Actions.Scissors) {
                    return 0;
                }
                if (action_2 == Battle_Actions.Paper || action_2 == Battle_Actions.Lizard) {
                    return 1;
                }
                return -1;
            }

            if (action_1 == Battle_Actions.Rock) {
                if (action_2 == Battle_Actions.Rock) {
                    return 0;
                }
                if (action_2 == Battle_Actions.Scissors || action_2 == Battle_Actions.Lizard) {
                    return 1;
                }
                return -1;
            }

            if (action_1 == Battle_Actions.Paper) {
                if (action_2 == Battle_Actions.Paper) {
                    return 0;
                }
                if (action_2 == Battle_Actions.Rock || action_2 == Battle_Actions.Spock) {
                    return 1;
                }
                return -1;
            }

            return 0;
        }
    }
}