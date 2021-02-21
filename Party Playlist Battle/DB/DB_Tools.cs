using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Reflection;
using System.Security.Cryptography;

namespace Party_Playlist_Battle
{
    public static class DB_Tools
    {
        static string connstring = "Host=localhost;Username=postgres;Database=postgres";

        public static bool db_register(string username, string password)
        {
            using (SHA256 hasher = SHA256.Create()) {
                password = BitConverter.ToString(hasher.ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", string.Empty);
            }
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;

            command = new NpgsqlCommand("SELECT userid FROM users;", conn);
            reader = command.ExecuteReader();
            //Simulate autoincrement
            int val = 0;
            while (reader.Read() && val == Int32.Parse(reader[0].ToString()))
            {
                val += 1;
            }
            reader.Close();
            try
            {
                command = new NpgsqlCommand("PREPARE registration (int, text, text) AS INSERT INTO users (userid,login,password) VALUES ($1,$2,$3);", conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                reader.Close();
                conn.Close();
                return false;
            }
            reader.Close();
            Console.WriteLine($"EXECUTE registration ({val},'{username}','{password}');");
            try
            {
                command = new NpgsqlCommand($"EXECUTE registration ({val},'{username}','{password}');", conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                reader.Close();
                conn.Close();
                return false;
            }
            reader.Close();
            try
            {
                command = new NpgsqlCommand($"INSERT INTO scoreboard VALUES ({val},0);", conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                reader.Close();
                conn.Close();
                return false;
            }
            reader.Close();
            conn.Close();
            return true;
        }

        public static bool db_login(string username, string password) {
            using (SHA256 hasher = SHA256.Create())
            {
                password = BitConverter.ToString(hasher.ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", string.Empty);
            }
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand("SELECT userid,login,password FROM users;", conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader[1].ToString() == username) {
                    if (reader[2].ToString() == password) {
                        reader.Close();
                        return true;
                    }
                    reader.Close();
                    return false;
                }
            }
            reader.Close();
            return false;
        }

        public static List<string> userData(int userid) {
            List<string> usrdata = new List<string>(); ;
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            try
            {
                command = new NpgsqlCommand($"SELECT * FROM users WHERE userid={userid};", conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
            if (reader.Read())
            {
                for (int i = 3; i < reader.FieldCount; i++)
                {
                    usrdata.Add(reader.GetName(i) + "\r\n" + reader[i].ToString());
                }
            }
            return usrdata;
        }

        public static bool editUserData(int userId, string nickname = null, string about = null, string email = null, string image = null) {
            //prepare
            string prepare = "PREPARE editUserData (";
            if (nickname != null) { prepare += "text, "; };
            if (about != null) { prepare += "text, "; };
            if (email != null) { prepare += "text, "; };
            if (image != null) { prepare += "text, "; };
            if (prepare.EndsWith(", ")) { prepare = prepare.Remove(prepare.Length - 2); }
            if (prepare.EndsWith("(")) { return true; }
            prepare += ") AS UPDATE users SET ";
            int count = 0;
            if (nickname != null) { count += 1; prepare += "nickname=$" + count + ", "; };
            if (about != null) { count += 1; prepare += "about=$" + count + ", "; };
            if (email != null) { count += 1; prepare += "email=$" + count + ", "; };
            if (image != null) { count += 1; prepare += "image=$" + count + ", "; };
            if (prepare.EndsWith(", ")) { prepare = prepare.Remove(prepare.Length - 2); }
            prepare += $" WHERE userid={userId};";
            //execute
            string execute = "EXECUTE editUserData(";
            if (nickname != null) { execute = execute + "'" + nickname + "', "; };
            if (about != null) { execute = execute + "'" + about + "', "; };
            if (email != null) { execute = execute + "'" + email + "', "; };
            if (image != null) { execute = execute + "'" + image + "', "; };
            if (execute.EndsWith(", ")) { execute = execute.Remove(execute.Length - 2); }
            execute += ");";
            Console.WriteLine(prepare);
            Console.WriteLine(execute);
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            //Prepare
            try
            {
                command = new NpgsqlCommand(prepare, conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            reader.Close();
            //Execute
            try
            {
                command = new NpgsqlCommand(execute, conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            reader.Close();
            conn.Close();
            return true;
        }

        public static int getUserStats(int userId) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand($"SELECT wins FROM scoreboard WHERE userid={userId};", conn);
            reader = command.ExecuteReader();
            int wins;
            try
            {
                reader.Read();
                wins = Int32.Parse(reader[0].ToString());
            }
            catch (Exception e) {
                Console.WriteLine(e);
                reader.Close();
                conn.Close();
                return -1;
            }
            reader.Close();
            conn.Close();
            return wins;
        }

        public static bool incrementUserWin(int userId)
        {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand($"SELECT wins FROM scoreboard WHERE userid={userId};", conn);
            reader = command.ExecuteReader();
            int wins;
            try
            {
                reader.Read();
                wins = Int32.Parse(reader[0].ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                reader.Close();
                conn.Close();
                return false;
            }
            reader.Close();
            wins += 1;
            try
            {
                command = new NpgsqlCommand($"UPDATE scoreboard SET wins={wins} WHERE userid={userId}", conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                reader.Close();
                conn.Close();
                return false;
            }
            reader.Close();
            conn.Close();
            return true;
        }

        public static List<Object[]> getScoreboard() {
            List<Object[]> scoreboard = new List<Object[]>();

            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand($"SELECT userid,wins FROM scoreboard ORDER BY wins ASC;", conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                scoreboard.Add(new Object[2] { DB_Tools.userIdToName(Int32.Parse(reader[0].ToString())), Int32.Parse(reader[1].ToString()) });
            }
            return scoreboard;
        }

        private static bool addToLib_mcid(int userId, int mcid) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand($"SELECT * FROM library WHERE userid={userId} AND mcid={mcid};", conn);
            reader = command.ExecuteReader();
            if (reader.Read()) {
                return false;
            }
            reader.Close();

            try
            {
                command = new NpgsqlCommand("PREPARE addMmc AS INSERT INTO library(userid, mcid) VALUES($1,$2);", conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                conn.Close();
                return false;
            }
            reader.Close();
            //Execute
            try
            {
                command = new NpgsqlCommand($"EXECUTE addMmc ({userId},{mcid});", conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                conn.Close();
                return false;
            }
            reader.Close();
            conn.Close();
            return true;
        }
        public static bool addToLib(int userid, string name, string filepath, float rating=-1, string genre=null, string title=null, string album=null, int lenght=-1, string filetype=null) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand("SELECT mcid,name FROM multimedia_content;", conn);
            reader = command.ExecuteReader();
            //Simulate autoincrement
            int mmcid = 0;
            while (reader.Read() && mmcid == Int32.Parse(reader[0].ToString()))
            {
                if (reader[1].ToString() == name)
                {
                    reader.Close();
                    conn.Close();
                    if (addToLib_mcid(userid, mmcid)) { return true; } else { return false; }
                }
                mmcid += 1;
            }
            reader.Close();
            //Prepare
            string prepare = "PREPARE addMmc AS INSERT INTO multimedia_content(";
            prepare += "mcid, name, filepath";
            if (rating!=-1) { prepare += ", rating"; };
            if (genre!=null) { prepare += ", genre"; };
            if (title!=null) { prepare += ", title"; };
            if (album!=null) { prepare += ", album"; };
            if (lenght!= -1) { prepare += ", lenght"; };
            if (filetype!= null) { prepare += ", filetype"; };
            prepare += ") VALUES(";
            int count = 3;
            prepare += "$1,$2,$3";
            if (rating != -1) { count += 1; prepare += ",$"+count; };
            if (genre != null) { count += 1; prepare += ",$" + count; };
            if (title != null) { count += 1; prepare += ",$" + count; };
            if (album != null) { count += 1; prepare += ",$" + count; };
            if (lenght != -1) { count += 1; prepare += ",$" + count; };
            if (filetype != null) { count += 1; prepare += ",$" + count; };
            prepare += ");";
            //Execute
            string execute = $"EXECUTE addMmc ({mmcid},'{name}','{filepath}'";
            if (rating != -1) { execute += $", replace('{rating}',',','.')::float"; };
            if (genre != null) { execute += $", '{genre}'"; };
            if (title != null) { execute += $", '{title}'"; };
            if (album != null) { execute += $", '{album}'"; };
            if (lenght != -1) { execute += $", {lenght}"; };
            if (filetype != null) { execute += $", '{filetype}'"; };
            execute += ");";
            Console.WriteLine(prepare);
            Console.WriteLine(execute);
            try
            {
                command = new NpgsqlCommand(prepare, conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                reader.Close();
                conn.Close();
                return false;
            }
            reader.Close();
            try
            {
                command = new NpgsqlCommand(execute, conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                reader.Close();
                conn.Close();
                return false;
            }
            reader.Close();
            conn.Close();
            if (addToLib_mcid(userid, mmcid)) { return true; } else { return false; }
        }

        public static List<string[]> getLib(int userid) {
            List<string[]> lib= new List<string[]>();
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;

            command = new NpgsqlCommand($"SELECT multimedia_content.name, multimedia_content.filepath FROM library,multimedia_content WHERE multimedia_content.mcid=library.mcid AND library.userid={userid};", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                lib.Add(new string[2] { reader[0].ToString(), reader[1].ToString()});
            }
            return lib;
        }

        public static bool removeFromLib(int userId, int mcid) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;

            command = new NpgsqlCommand($"SELECT * FROM library WHERE userid={userId} AND mcid={mcid}", conn);
            reader = command.ExecuteReader();
            if (!reader.Read()) {
                reader.Close();
                conn.Close();
                return false;
            }
            reader.Close();

            command = new NpgsqlCommand($"DELETE FROM library WHERE userid={userId} AND mcid={mcid}", conn);
            reader = command.ExecuteReader();
            reader.Close();
            conn.Close();
            return true;
        }

        public static bool addToPlaylist(int mcid) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;

            command = new NpgsqlCommand("SELECT song_order FROM playlist;", conn);
            reader = command.ExecuteReader();
            //Simulate autoincrement
            //ASSUMPTION: order will restart correctly if someone tries inserting more than max int values overwriting the old values - no handling required.
            int order=0;
            while (reader.Read())
            {
                order = Int32.Parse(reader[0].ToString());
            }
            reader.Close();
            order += 1;
            try{
                command = new NpgsqlCommand($"INSERT INTO playlist VALUES ({mcid},{order})", conn);
                reader = command.ExecuteReader();
            } catch (Exception e){
                Console.WriteLine(e);
                conn.Close();
                return false;
            }
            reader.Close();
            conn.Close();
            return true;
        }

        public static bool reorderPlaylist(int swap_from, int swap_to) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand($"SELECT song_order FROM playlist WHERE song_order={swap_from};", conn);
            reader = command.ExecuteReader();
            if (!reader.Read()) { conn.Close();return false; }
            reader.Close();
            command = new NpgsqlCommand($"SELECT song_order FROM playlist WHERE song_order={swap_to};", conn);
            reader = command.ExecuteReader();
            if (!reader.Read()) { conn.Close(); return false; }
            reader.Close();

            command = new NpgsqlCommand($"UPDATE playlist SET song_order={-1} WHERE song_order={swap_from};", conn);
            reader = command.ExecuteReader();
            reader.Close();

            command = new NpgsqlCommand($"UPDATE playlist SET song_order={swap_from} WHERE song_order={swap_to};", conn);
            reader = command.ExecuteReader();
            reader.Close();

            command = new NpgsqlCommand($"UPDATE playlist SET song_order={swap_to} WHERE song_order={-1};", conn);
            reader = command.ExecuteReader();
            reader.Close();
            conn.Close();
            return true;
        }

        public static List<Object[]> getPlaylist() {
            List<Object[]> playlist = new List<Object[]>();
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            try
            {
                command = new NpgsqlCommand($"SELECT mcid,song_order FROM playlist ORDER BY song_order ASC;", conn);
                reader = command.ExecuteReader();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                conn.Close();
                return null;
            }
            while (reader.Read()) {
                playlist.Add(new Object[2] {mcidToName(Int32.Parse(reader[0].ToString())), Int32.Parse(reader[1].ToString()) });
            }
            conn.Close();
            return playlist;
        }
        //conversion tools

        public static int nameToMcid(string name) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;

            command = new NpgsqlCommand("PREPARE findName AS SELECT mcid FROM multimedia_content WHERE name=$1;", conn);
            reader = command.ExecuteReader();
            reader.Close();


            command = new NpgsqlCommand($"EXECUTE findName('{name}');", conn);
            reader = command.ExecuteReader();
            if (reader.Read()) {
                int val = Int32.Parse(reader[0].ToString());
                conn.Close();
                return val;
            }
            conn.Close();
            return -1;
        }

        public static string mcidToName(int mcid) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand($"SELECT name FROM multimedia_content WHERE mcid={mcid};", conn);
            reader = command.ExecuteReader();
            if (reader.Read()) {
                string name = reader[0].ToString();
                conn.Close();
                return name;
            }
            conn.Close();
            return null;
        }
        public static int nameToUserid(string name)
        {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;

            command = new NpgsqlCommand("PREPARE findUsername AS SELECT userid FROM users WHERE login=$1;", conn);
            reader = command.ExecuteReader();
            reader.Close();


            command = new NpgsqlCommand($"EXECUTE findUsername('{name}');", conn);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                int val = Int32.Parse(reader[0].ToString());
                conn.Close();
                return val;
            }
            conn.Close();
            return -1;
        }

        public static string userIdToName(int userid) {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            NpgsqlCommand command;
            NpgsqlDataReader reader;
            command = new NpgsqlCommand($"SELECT login FROM users WHERE userid={userid};", conn);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                string name = reader[0].ToString();
                conn.Close();
                return name;
            }
            conn.Close();
            return null;
        }
    }
}