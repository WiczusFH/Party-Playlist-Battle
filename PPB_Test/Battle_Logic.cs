using Microsoft.VisualStudio.TestTools.UnitTesting;
using Party_Playlist_Battle;

namespace PPB_Test
{
    [TestClass]
    public class Battle_Logic
    {
        [TestMethod]
        public void action_eval() {
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
        public void joinBattle() { }
    }
}
