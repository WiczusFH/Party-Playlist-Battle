using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using Npgsql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Party_Playlist_Battle;
using System.Security.Cryptography;

namespace Party_Playlist_Battle
{
    class Program
    {


        public static void Main(string[] args) {
            //DB TOOLS TEST

            //DB_Tools.db_register("kienboec", "nobo");
            //Console.WriteLine(DB_Tools.db_login("kienboec", "nobo"));
            //Console.WriteLine(DB_Tools.nameToUserid("kienboec"));
            //Console.WriteLine(DB_Tools.editUserData(DB_Tools.nameToUserid("kienboec"), email: "testemail@gmail.com"));
            //Console.WriteLine(DB_Tools.incrementUserWin(3));
            //Console.WriteLine(DB_Tools.getUserStats(1));
            //List<int[]> test=DB_Tools.getScoreboard();
            //foreach (int[] abc in test) {
            //    Console.WriteLine("Id: "+abc[0]+" Wins: " + abc[1]);

            //}
            //Console.WriteLine(DB_Tools.addToLib(1, "xd", "https://www.youtube.com/watch?v=inMCT9fXjg4", rating: 3.4f));
            //Console.WriteLine("test, ".Remove("test, ".Length-2));
            //List<string[]> test = DB_Tools.getLib(0);
            //foreach (string[] abc in test) {
            //    Console.WriteLine("Title: "+abc[0]+" Path: " + abc[1]);

            //}
            //Console.WriteLine(DB_Tools.removeFromLib(0,4));
            //Console.WriteLine(DB_Tools.addToLib(0,"Title","youtube.com"));
            //Console.WriteLine(DB_Tools.addToPlaylist(3));
            //Console.WriteLine(DB_Tools.reorderPlaylist(2, 3));
            //List<int[]> test = DB_Tools.getPlaylist();
            //foreach (int[] abc in test) {
            //    Console.WriteLine("Mcid: "+abc[0]+" Order: "+abc[1]);
            //List<string> best = DB_Tools.userData(0);
            //foreach (string abc in best) {
            //    Console.WriteLine(abc);
            //}
            //REST TEST

            //string exampleReq= "GET /playlist HTTP/1.1\r\n" +
            //    "Host: localhost: 6000\r\n" +
            //    "User - Agent: curl / 7.55.1\r\n" +
            //    "Authorisation: blipblop\r\n" +
            //    "Accept: */*\r\n" +
            //    "Content-Type: application/json\r\n" +
            //    "Content-Length: 44\r\n" +
            //    "\r\n" +
            //    "{\"Name\":\"Yoo\"}\r\n"+
            //    "\r\n";


            //REST_Tools.tokenToUserId("Basic kienboec-ppbToken");
            //Request test = new Request(exampleReq);
            //Console.WriteLine(test.verb);
            //Console.WriteLine(test.loc);
            //Console.WriteLine(test.payload);
            //Console.WriteLine(test.token);
            //Console.WriteLine(test.ctype);

            //REST_Tools.Handler(test);






            //TcpListener listener = new TcpListener(IPAddress.Loopback, 6000);
            //listener.Start();
            //TcpClient client = listener.AcceptTcpClient();
            //byte[] buffer = new byte[4096];
            //Stream clistream = client.GetStream();
            //int readsize;
            //readsize = clistream.Read(buffer, 0, buffer.Length);
            //Console.WriteLine(ASCIIEncoding.ASCII.GetString(buffer, 0, readsize));


            Server serv = new Server();
            serv.run();

            //Console.WriteLine(REST_Tools.tokenToUserId("Basic kienboec-ppbToken"));

            //Battle
            //user_battle_info batinfoA = new user_battle_info("A");
            //user_battle_info batinfoB = new user_battle_info("B");
            //user_battle_info batinfoC = new user_battle_info("C");
            //user_battle_info batinfoD = new user_battle_info("D");
            //batinfoA.setActions("RRRRR");
            //batinfoB.setActions("RRRRR");
            //batinfoC.setActions("RPPLS");
            //batinfoD.setActions("VVSLS");
            //Battle battle = new Battle();
            //battle.user_infos.Add(batinfoA);
            //battle.user_infos.Add(batinfoB);
            //battle.user_infos.Add(batinfoC);
            //battle.user_infos.Add(batinfoD);
            //battle.start_tournament();
            //Console.WriteLine(battle.log);
        }


    }
}
