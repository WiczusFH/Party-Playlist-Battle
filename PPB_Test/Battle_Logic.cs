using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using Party_Playlist_Battle;

namespace PPB_Test
{
    [TestClass]
    public class Battle_Logic
    {
        [TestMethod]
        public void createUserInfoSuccess()
        {
            user_battle_info userA = new user_battle_info("A");
            userA.setActions("RRRRR");

            Assert.AreEqual(userA.actions.Length, 5);
            for(int i = 0; i < userA.actions.Length; i++) {
                Assert.AreEqual(Battle_Actions.Rock, userA.actions[i]);
            }
        }
        [TestMethod]
        public void createUserInfoFail()
        {
            user_battle_info userA = new user_battle_info("A");
            userA.setActions("Hello");
         
            for (int i = 0; i < userA.actions.Length; i++)
            {
                Assert.AreEqual(Battle_Actions.NULL, userA.actions[i]);
            }
        }
        [TestMethod]
        public void createUserInfoPersistence()
        {
            user_battle_info userA = new user_battle_info("A");
            userA.setActions("RRRRR");
            userA.setActions("ABCDE");

            Assert.AreEqual(userA.actions.Length, 5);
            for (int i = 0; i < userA.actions.Length; i++)
            {
                Assert.AreEqual(Battle_Actions.Rock, userA.actions[i]);
            }
        }
        [TestMethod]
        public void action_eval() {
            //25 methoden wäre sinnlos
            Assert.AreEqual(0, Battle.action_eval(Battle_Actions.Spock,Battle_Actions.Spock));
            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Spock,Battle_Actions.Rock));
            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Spock,Battle_Actions.Scissors));
            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Spock,Battle_Actions.Paper));
            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Spock,Battle_Actions.Lizard));

            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Rock, Battle_Actions.Spock));
            Assert.AreEqual(0, Battle.action_eval(Battle_Actions.Rock, Battle_Actions.Rock));
            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Rock, Battle_Actions.Scissors));
            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Rock, Battle_Actions.Paper));
            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Rock, Battle_Actions.Lizard));

            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Scissors, Battle_Actions.Spock));
            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Scissors, Battle_Actions.Rock));
            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Scissors, Battle_Actions.Paper));
            Assert.AreEqual(0, Battle.action_eval(Battle_Actions.Scissors, Battle_Actions.Scissors));
            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Scissors, Battle_Actions.Lizard));

            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Paper, Battle_Actions.Spock));
            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Paper, Battle_Actions.Rock));
            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Paper, Battle_Actions.Lizard));
            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Paper, Battle_Actions.Scissors));
            Assert.AreEqual(0, Battle.action_eval(Battle_Actions.Paper, Battle_Actions.Paper));

            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Lizard, Battle_Actions.Spock));
            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Lizard, Battle_Actions.Rock));
            Assert.AreEqual(1, Battle.action_eval(Battle_Actions.Lizard, Battle_Actions.Paper));
            Assert.AreEqual(-1, Battle.action_eval(Battle_Actions.Lizard, Battle_Actions.Scissors));
            Assert.AreEqual(0, Battle.action_eval(Battle_Actions.Lizard, Battle_Actions.Lizard));

        }
        [TestMethod]
        public void fightdraw() {
            Battle battle=new Battle();
            user_battle_info userA = new user_battle_info("A");
            user_battle_info userB = new user_battle_info("B");
            userA.setActions("RRRRR");
            userB.setActions("RRRRR");
            battle.active_users.Add(userA);
            battle.active_users.Add(userB);
            
            battle.fight(userA, userB);

            Assert.AreEqual(userA.battle_score, 0);
            Assert.AreEqual(userB.battle_score, 0);
        }
        [TestMethod]
        public void fightwinA()
        {
            Battle battle = new Battle();
            user_battle_info userA = new user_battle_info("A");
            user_battle_info userB = new user_battle_info("B");
            userA.setActions("PRRRR");
            userB.setActions("RRRRR");
            battle.active_users.Add(userA);
            battle.active_users.Add(userB);

            battle.fight(userA, userB);

            Assert.AreEqual(userA.battle_score, 1);
            Assert.AreEqual(userB.battle_score, 0);
        }
        [TestMethod]
        public void fightwinB()
        {
            Battle battle = new Battle();
            user_battle_info userA = new user_battle_info("A");
            user_battle_info userB = new user_battle_info("B");
            userA.setActions("SRRRR");
            userB.setActions("RRRRR");
            battle.active_users.Add(userA);
            battle.active_users.Add(userB);

            battle.fight(userA, userB);

            Assert.AreEqual(userA.battle_score, 0);
            Assert.AreEqual(userB.battle_score, 1);
        }
        [TestMethod]
        public void tournament2winner()
        {
            Mock<Battle> moq = new Mock<Battle>();
            moq.Setup(x => x.finishResult(new List<user_battle_info>())).Returns(0);
            user_battle_info userA = new user_battle_info("A");
            user_battle_info userB = new user_battle_info("B");
            userA.setActions("PRRRR");
            userB.setActions("RRRRR");
            moq.Object.active_users.Add(userA);
            moq.Object.active_users.Add(userB);

            moq.Object.start_tournament();

            string[] logsplit = moq.Object.log.Split("\r\n");
            Assert.AreEqual("A is the Winner! Congrats!",logsplit[logsplit.Length-2]);
        }
        [TestMethod]
        public void tournament3winner()
        {
            Mock<Battle> moq = new Mock<Battle>();
            moq.Setup(x => x.finishResult(new List<user_battle_info>())).Returns(0);
            user_battle_info userA = new user_battle_info("A");
            user_battle_info userB = new user_battle_info("B");
            user_battle_info userC = new user_battle_info("C");
            userA.setActions("PRRRR");
            userB.setActions("RRRRR");
            userC.setActions("PPPPP");
            moq.Object.active_users.Add(userA);
            moq.Object.active_users.Add(userB);
            moq.Object.active_users.Add(userC);

            moq.Object.start_tournament();

            string[] logsplit = moq.Object.log.Split("\r\n");
            Assert.AreEqual("C is the Winner! Congrats!", logsplit[logsplit.Length - 2]);
        }
        [TestMethod]
        public void tournament3draw()
        {
            Mock<Battle> moq = new Mock<Battle>();
            moq.Setup(x => x.finishResult(new List<user_battle_info>())).Returns(0);
            user_battle_info userA = new user_battle_info("A");
            user_battle_info userB = new user_battle_info("B");
            user_battle_info userC = new user_battle_info("C");
            userA.setActions("PRRRR");
            userB.setActions("RRRRR");
            userC.setActions("SRRRR");
            moq.Object.active_users.Add(userA);
            moq.Object.active_users.Add(userB);
            moq.Object.active_users.Add(userC);

            moq.Object.start_tournament();

            string[] logsplit = moq.Object.log.Split("\r\n");
            Assert.AreEqual("Our tournament ended in a draw between A and B and C. What a travesty!!!", logsplit[logsplit.Length - 2]);
        }
        [TestMethod]
        public void tournament2draw()
        {
            Mock<Battle> moq = new Mock<Battle>();
            moq.Setup(x => x.finishResult(new List<user_battle_info>())).Returns(0);
            user_battle_info userA = new user_battle_info("A");
            user_battle_info userB = new user_battle_info("B");
            userA.setActions("RRRRR");
            userB.setActions("RRRRR");
            moq.Object.active_users.Add(userA);
            moq.Object.active_users.Add(userB);

            moq.Object.start_tournament();
            string log = moq.Object.log;
            string[] logsplit = log.Split("\r\n");
            Assert.AreEqual("Our tournament ended in a draw between A and B. What a travesty!!!", logsplit[logsplit.Length-2]);
        }
        [TestMethod]
        public void tournament1winner()
        {
            Mock<Battle> moq = new Mock<Battle>();
            moq.Setup(x => x.finishResult(new List<user_battle_info>())).Returns(0);
            user_battle_info userA = new user_battle_info("A");
            userA.setActions("PRRRR");
            moq.Object.active_users.Add(userA);

            moq.Object.start_tournament();

            string[] logsplit = moq.Object.log.Split("\r\n");
            Assert.AreEqual("A is the Winner! Congrats!", logsplit[logsplit.Length - 2]);
        }
        [TestMethod]
        public void joinBattle() {
            Mock<Battle> moq = new Mock<Battle>();
            moq.Setup(x => x.startTimerAsync());
            user_battle_info userA = new user_battle_info("A");
            moq.Object.joinBattle(userA);

            Assert.AreEqual(1, moq.Object.active_users.Count);
        }
        [TestMethod]
        public void joinBattle3()
        {
            Mock<Battle> moq = new Mock<Battle>();
            moq.Setup(x => x.startTimerAsync());
            user_battle_info userA = new user_battle_info("A");
            user_battle_info userB = new user_battle_info("B");
            user_battle_info userC = new user_battle_info("C");
            moq.Object.joinBattle(userA);
            moq.Object.joinBattle(userB);
            moq.Object.joinBattle(userC);

            Assert.AreEqual(3, moq.Object.active_users.Count);
        }


    }
}
