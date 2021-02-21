using System;
using System.Collections.Generic;
using System.Text;

namespace Party_Playlist_Battle
{
    public class user_battle_info
    {
        public user_battle_info(string Username) {
            actions = new Battle_Actions[5];
            for (int i = 0; i < 5; i++) {
                actions[i] = Battle_Actions.NULL;
            }
            username = Username;
            battle_score = 0;
        }
        public string username;

        public int round_score;
        public Battle_Actions[] actions;
        public int setActions(string input) {
            input = input.ToLower();
            if (input.Length >= 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    switch (input[i])
                    {
                        case 'r': actions[i] = Battle_Actions.Rock; break; 
                        case 'p': actions[i] = Battle_Actions.Paper; break;
                        case 's': actions[i] = Battle_Actions.Scissors; break;
                        case 'l': actions[i] = Battle_Actions.Lizard; break;
                        case 'v': actions[i] = Battle_Actions.Spock; break;
                        default: return -2;
                    }
                }
                return 0;
            }
            else {
                return -1;
            }
        }
        public int battle_score;
    }
}