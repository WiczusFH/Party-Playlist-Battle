using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Party_Playlist_Battle
{
    public class Battle
    {
        public List<user_battle_info> user_infos = new List<user_battle_info>();
        public List<user_battle_info> blocked_users = new List<user_battle_info>();
        public List<user_battle_info> active_users= new List<user_battle_info>();
        public string log;
        public int currentAdminId = -1;
        //int battleCountdown = -1;
        bool battleActive = false;

        public Battle() {

        }
        Task timer;

        public string joinBattle(user_battle_info user) {
            if (!battleActive) {
                battleActive = true;
                timer = Task.Run(startTimerAsync);
            }
            active_users.Add(user);
            //add active user

            timer.Wait();
            return log;
        }
        public virtual void startTimerAsync()
        {
            Console.WriteLine("Battle will start soon. ");
            for (int i = 15; i >= 0; i--)
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }
            start_tournament();
        }

        public void start_tournament() {
            int playerCount = active_users.Count;
            if (playerCount > 1)
            {
                for (int i = 0; i < playerCount - 1; i++)
                {
                    for (int j = i + 1; j < playerCount; j++)
                    {
                        bool block = false;
                        foreach (user_battle_info b_user in blocked_users)
                        {
                            if (active_users[i] == b_user || active_users[j] == b_user)
                            {
                                block = true;
                            }
                        }
                        if (!block)
                        {
                            fight(active_users[i], active_users[j]);
                        }
                    }
                }
                log += "Results: \r\n";
                int highest = -1;
                foreach (user_battle_info player in active_users)
                {
                    log += "  " + player.username + ": " + player.battle_score;
                    if (player.battle_score > highest)
                    {
                        highest = player.battle_score;
                    }
                }
                log += "\r\n";
                List<user_battle_info> winnerList = new List<user_battle_info>();
                foreach (user_battle_info player in active_users)
                {
                    if (player.battle_score == highest) { winnerList.Add(player); }
                }
                if (winnerList.Count > 1)
                {
                    log += "Our tournament ended in a draw between ";
                    foreach (user_battle_info player in winnerList)
                    {
                        log += player.username + " and ";
                    }
                    log = log.Remove(log.Length - 5);
                    log += ". What a travesty!!!\r\n";
                    currentAdminId = -1;
                }
                else
                {
                    log += winnerList[0].username + " is the Winner! Congrats!\r\n";
                    currentAdminId=finishResult(winnerList);
                    //currentAdminId = DB_Tools.nameToUserid(winnerList[0].username);
                    //DB_Tools.incrementUserWin(DB_Tools.nameToUserid(winnerList[0].username));
                }
            }
            else {
                log += active_users[0].username + " is the Winner! Congrats!\r\n";
                currentAdminId=finishResult(active_users);
                //currentAdminId = DB_Tools.nameToUserid(active_users[0].username);
                //DB_Tools.incrementUserWin(DB_Tools.nameToUserid(active_users[0].username));
            }
            foreach (user_battle_info user in active_users) {
                user.battle_score = 0;
            }
            blocked_users = new List<user_battle_info>();
            active_users = new List<user_battle_info>();
            battleActive = false;

        }
        public virtual int finishResult(List<user_battle_info> active_users) { 
            DB_Tools.incrementUserWin(DB_Tools.nameToUserid(active_users[0].username));
            return DB_Tools.nameToUserid(active_users[0].username); ;
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


        public void fight(user_battle_info pA, user_battle_info pB) {
            int favorA = 0;
            for (int i = 0; i < 5; i++) {
                log+= pA.username + " vs " + pB.username + "\r\n";
                favorA +=action_eval(pA.actions[i], pB.actions[i]);
                log += "   "+pA.actions[i].ToString() +" vs "+ pB.actions[i] + "\r\n";
            }
            if (favorA > 0) {
                pA.battle_score++;
                log += pA.username + " wins the round! \r\n";
            }
            if (favorA < 0) {
                pB.battle_score++;
                log += pB.username + " wins the round! \r\n";
            }
            if (favorA == 0) {
                log += "A draw?!? A conspiracy?\r\n";
                blocked_users.Add(pA);
                blocked_users.Add(pB);
            }
            log += "\r\n";
        }

    }
}