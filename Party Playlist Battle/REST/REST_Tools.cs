using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace Party_Playlist_Battle
{
    public class REST_Tools
    {
        public static void Handler(Request req){
            switch (req.verb) {
                case RESTVerbs.POST:
                    if (req.loc.StartsWith("/users")) { register(req); }
                    if (req.loc.StartsWith("/sessions")) { login(req); }
                    if (req.loc.StartsWith("/lib")) { addToLib(req); }
                    if (req.loc.StartsWith("/playlist")) { addToPlaylist(req); }
                    if (req.loc.StartsWith("/battles")) { joinBattle(req); }//
                    if (req.loc.StartsWith("/actions")) { setActions(req); }//
                    break;
                case RESTVerbs.GET:
                    if (req.loc.StartsWith("/users")) { getUserData(req); }
                    if (req.loc.StartsWith("/stats")) { getUserStats(req); }
                    if (req.loc.StartsWith("/score")) { getScoreboard(req); }
                    if (req.loc.StartsWith("/lib")) { getUserLib(req); }//
                    if (req.loc.StartsWith("/playlist")) { getPlaylist(req); }
                    if (req.loc.StartsWith("/actions")) { getUserActions(req); }//
                    break;
                case RESTVerbs.DEL:
                    if (req.loc.StartsWith("/lib")) { }//
                    break;
                case RESTVerbs.PUT:
                    if (req.loc.StartsWith("/playlist")) { }
                    if (req.loc.StartsWith("/actions")) { }//
                    break;
                default: break;
            }
        }
        public static bool editPlaylist(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) { 
                if (req.ctype == "json")
                {
                    JObject jObject = JObject.Parse(req.payload);
                    if (DB_Tools.reorderPlaylist(Int32.Parse(jObject.GetValue("FromPosition").ToString()), Int32.Parse(jObject.GetValue("ToPosition").ToString())))
                    {
                        //send message success
                        return true;
                    }
                }
            }
            //send message fail
            return false;
        }
        public static bool editActions(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) { }
            //send message fail
            return false;
        }

        public static bool deleteFromLib(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) {
                int mcid = DB_Tools.nameToMcid(req.loc.Substring(7).Trim());
                DB_Tools.removeFromLib(userid,mcid);
            }
            
            //send message fail
            return false;
        }

        public static bool getUserActions(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) { }
            //send message fail
            return false;
        }

        public static bool getPlaylist(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) { 
                List<Object[]> playlist=DB_Tools.getPlaylist();
                //foreach (Object[] abc in playlist) {
                //    Console.WriteLine(abc[0]+" "+ abc[1]);
                //}
                //send success;
                return true;
            }
            //send message fail

            return false;
        }

        public static bool getUserLib(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) 
            {
                List<string[]> library= DB_Tools.getLib(userid);
                if (library != null)
                {
                    //send back
                    return true;
                }
            }
            //send message fail
            return false;
        }

        public static bool getScoreboard(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                List<int[]> scoreboard = DB_Tools.getScoreboard();
                if (scoreboard != null)
                {
                    //send back
                    return true;
                }
            }
            //send message fail
            return false;
        }

        public static bool getUserStats(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                int userData = DB_Tools.getUserStats(userid);
                if (userData != -1) {
                    //send back
                    return true;
                }
            }

            //send message fail
            return false;
        }

        public static bool getUserData(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) {
                List<string> userData=DB_Tools.userData(userid);
                //send back
                return true;
            }
            //send message fail
            return false;
        }

        //GET
        public static bool setActions(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            { }
                if (req.ctype == "json")
            {

            }
            //send message fail
            return false;
        }

        public static bool joinBattle(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            { }
                if (req.ctype == "json")
            {

            }
            //send message fail
            return false;
        }

        public static bool addToPlaylist(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                if (req.ctype == "json")
                {
                    JObject jObject = JObject.Parse(req.payload);
                    if (jObject.GetValue("Name") != null)
                    {
                        DB_Tools.addToPlaylist(DB_Tools.nameToMcid(jObject.GetValue("Name").ToString()));
                    }
                }
            }
            //send message fail
            return false;
        }

        public static bool register(Request req) {
            if (req.ctype == "json"){
                JObject jObject = JObject.Parse(req.payload);
                if (DB_Tools.db_register(jObject.GetValue("Username").ToString(), jObject.GetValue("Password").ToString())) {
                    //send message success
                    return true;
                }
            }
                //send message fail
                return false;
        }

        public static bool login(Request req)
        {
            if (req.ctype == "json")
            {
                JObject jObject = JObject.Parse(req.payload);
                if (DB_Tools.db_login(jObject.GetValue("Username").ToString(), jObject.GetValue("Password").ToString()))
                {
                    generate_token(req);
                    //send token
                    //make active
                    //send message success
                    return true;
                }
            }
            //send message fail
            return false;
        }

        public static bool addToLib(Request req)
        {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            { 
                if (req.ctype == "json")
                {
                    JObject jObject = JObject.Parse(req.payload);
                    if (jObject.GetValue("Name") != null && jObject.GetValue("Url") != null) {
                        if (true)
                        {
                            string name=null,filepath = null, genre = null, title = null, album = null;
                            float rating = -1;
                            int lenght = -1;
                            string filetype = "Url"; //no local files support
                            name = jObject.GetValue("Name").ToString();
                            filepath = jObject.GetValue("Url").ToString();
                            if (jObject.GetValue("Genre") != null) { genre= jObject.GetValue("Genre").ToString(); }
                            if (jObject.GetValue("Rating") != null) { rating=float.Parse(jObject.GetValue("Rating").ToString()); }
                            if (jObject.GetValue("Title") != null) { title=jObject.GetValue("Title").ToString(); }
                            if (jObject.GetValue("Album") != null) { album=jObject.GetValue("Album").ToString(); }
                            if (jObject.GetValue("Lenght") != null) { lenght=Int32.Parse(jObject.GetValue("Lenght").ToString()); }
                            if (DB_Tools.addToLib(userid, name, filepath, rating, genre, title, album, lenght, filetype))
                            {
                                //send message success
                                return true;
                            }
                        }
                    }
                }
            }
            //send message fail
            return false;
        }

        //Tools

        public static string generate_token(Request req)
        {
            JObject jObject = JObject.Parse(req.payload);
            string token = $"Basic {jObject.GetValue("Username")}-ppbToken";
            return token;
        }
        public static int tokenToUserId(string token) {
            return DB_Tools.nameToUserid(token.Substring(6, token.Length - 15));
        }
    }
}