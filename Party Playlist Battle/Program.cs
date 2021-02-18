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

namespace Party_Playlist_Battle
{
    class Program
    {

        public static void Main(string[] args) {
            //Server serv = new Server();
            //serv.run();
   
            //Connection_Listener listener = new Connection_Listener();
            //Console.WriteLine(listener.generate_token());
   
            //var connstring= "Host=localhost;Username=postgres;Database=postgres";
            //var conn = new NpgsqlConnection(connstring);
            //conn.Open();
            //Console.WriteLine("OK");

            //try {
            //    NpgsqlCommand command = new NpgsqlCommand("INSERT INTO test VALUES(456,5);", conn);
            //    NpgsqlDataReader reader = command.ExecuteReader();
            //}
            //catch (Exception e) {
            //    Console.WriteLine("Unable to execute! ");
            //}
            //var command = new NpgsqlCommand("SELECT * FROM test;", conn);
            //NpgsqlDataReader reader = command.ExecuteReader();
            //int val;
            //while (reader.Read()) {
            //    val = Int32.Parse(reader[0].ToString());
            //    Console.WriteLine(val);
            //    //do whatever you like
            //}

            //conn.Close();
        }
    }
}
