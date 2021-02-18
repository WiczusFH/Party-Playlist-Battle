using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace Party_Playlist_Battle
{
    public class DB_Connect
    {
        NpgsqlConnection conn;
        DB_Connect() {
            var connstring = "Host=localhost;Username=postgres;Database=postgres";
            conn = new NpgsqlConnection(connstring);
            conn.Open();
        }
        int get_mcid_by_name() { return 0; }
        string get_name_from_mcid() { return ""; }

        void insert(Db_Tables table, string[] values) {
        
        }
        string select() {


            return "";
        }
        ~DB_Connect() {
            conn.Close();
        }
        
    }
}