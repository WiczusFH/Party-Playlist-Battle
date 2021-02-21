using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Threading;
using Npgsql;
using System.Threading.Tasks;


namespace Party_Playlist_Battle
{
    public class communication
    {
        public static Response Handler(Request req,Battle battle){
            switch (req.verb) {
                case RESTVerbs.POST:
                    if (req.loc.StartsWith("/users")) {  return register(req); }
                    if (req.loc.StartsWith("/sessions")) { return login(req); }
                    if (req.loc.StartsWith("/lib")) { return addToLib(req); }
                    if (req.loc.StartsWith("/playlist")) { return addToPlaylist(req); }
                    if (req.loc.StartsWith("/battles")) { return joinBattle(req,battle); }
                    if (req.loc.StartsWith("/actions")) { return setActions(req,battle); }
                    break;
                case RESTVerbs.GET:
                    if (req.loc.StartsWith("/users")) { return getUserData(req); }
                    if (req.loc.StartsWith("/stats")) { return getUserStats(req); }
                    if (req.loc.StartsWith("/score")) { return getScoreboard(req); }
                    if (req.loc.StartsWith("/lib")) { return getUserLib(req); }
                    if (req.loc.StartsWith("/playlist")) { return getPlaylist(req); }
                    if (req.loc.StartsWith("/actions")) { return getUserActions(req,battle); }
                    break;
                case RESTVerbs.DEL:
                    if (req.loc.StartsWith("/lib")) { return deleteFromLib(req); }
                    break;
                case RESTVerbs.PUT:
                    if (req.loc.StartsWith("/playlist")) { return editPlaylist(req,battle); }
                    if (req.loc.StartsWith("/users")) { return editUserData(req); }
                    if (req.loc.StartsWith("/actions")) { return setActions(req, battle); }
                    break;
                default: break;
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Not Found or Implemented. ");
        }

        public static Response editUserData(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                if (DB_Tools.nameToUserid(req.loc.Substring(7).Trim()) == userid) { 

                    if (req.ctype == "json")
                    {
                        JObject jObject = JObject.Parse(req.payload);
                        string nickname=null, bio=null, email=null, image=null;
                        if (jObject.GetValue("Nickname")!=null) { nickname = jObject.GetValue("Nickname").ToString(); }
                        if (jObject.GetValue("Bio") != null) { bio = jObject.GetValue("Bio").ToString(); }
                        if (jObject.GetValue("Email") != null) { email = jObject.GetValue("Email").ToString(); }
                        if (jObject.GetValue("Image") != null) { image = jObject.GetValue("Image").ToString(); }
                        DB_Tools.editUserData(userid, nickname,bio,email,image);
                        return new Response(req, Status_Code.OK, AdditionalPayload: "Edited. ");
                    }
                    return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid type. ");
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Unauthorised request. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't verify Connection. ");
        }
        public static Response editPlaylist(Request req, Battle battle) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) {
                if (battle.currentAdminId == userid)
                {
                    if (req.ctype == "json")
                    {
                        JObject jObject = JObject.Parse(req.payload);
                        if (DB_Tools.reorderPlaylist(Int32.Parse(jObject.GetValue("FromPosition").ToString()), Int32.Parse(jObject.GetValue("ToPosition").ToString())))
                        {
                            return new Response(req, Status_Code.OK, AdditionalPayload: "Swapped. ");
                        }
                        return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid Arguments. ");
                    }
                    return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid type. ");
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Need Admin rights. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't verify Connection. ");
        }

        public static Response deleteFromLib(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) {
                int mcid = DB_Tools.nameToMcid(req.loc.Substring(5).Trim());
                if (mcid != -1)
                {
                    if (DB_Tools.removeFromLib(userid, mcid))
                    {
                        return new Response(req, Status_Code.NOK, AdditionalPayload: "Deleted. ");
                    }
                    return new Response(req, Status_Code.NOK, AdditionalPayload: "Database error. ");
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't find Song. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't verify Connection. ");
        }

        public static Response getUserActions(Request req, Battle battle) {

            int userid = tokenToUserId(req.token);
            if (userid != -1) {
                foreach (user_battle_info user in battle.user_infos) {
                    if (user != null)
                    {
                        if (user.username == DB_Tools.userIdToName(userid))
                        {
                            string values = "";
                            foreach (Battle_Actions action in user.actions)
                            {
                                values += action.ToString()+", ";
                            }
                            values += ". ";
                            return new Response(req, Status_Code.OK, AdditionalPayload: values);
                        }
                    }
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't find the user actions. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't verify Connection. ");
        }

        public static Response getPlaylist(Request req) {
            List<Object[]> playlist=DB_Tools.getPlaylist();
            JObject payload = new JObject();
            int i = 0;
            foreach (Object[] song in playlist)
            {
                JObject JSong = new JObject();
                JSong.Add(song[1].ToString(), song[0].ToString());
                payload.Add(i.ToString(), JSong);
                i++;
            }
            return new Response(req, Status_Code.OK, AdditionalPayload: payload.ToString());
        }

        public static Response getUserLib(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) 
            {
                List<string[]> library= DB_Tools.getLib(userid);
                if (library != null)
                {
                    JObject payload=new JObject();
                    foreach (string[] song in library) {
                        payload.Add(song[0], song[1]);
                    }
                   return new Response(req, Status_Code.OK, AdditionalPayload: payload.ToString());
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Database error. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't verify Connection. ");
        }

        public static Response getScoreboard(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                List<Object[]> scoreboard = DB_Tools.getScoreboard();
                if (scoreboard != null)
                {
                    JObject payload = new JObject();
                    int i = 0;
                    foreach (Object[] obj in scoreboard) {
                        i++;
                        JObject userObj = new JObject();
                        userObj.Add(obj[0].ToString(), obj[1].ToString());
                        payload.Add(i.ToString(), userObj);
                    }
                    return new Response(req, Status_Code.OK, AdditionalPayload: payload.ToString());
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Database error. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't verify Connection. ");
        }

        public static Response getUserStats(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                int wins = DB_Tools.getUserStats(userid);
                if (wins!= -1) {
                    return new Response(req, Status_Code.OK, AdditionalPayload: "Win count: "+wins.ToString());
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Database Error. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't verify Connection. ");
        }

        public static Response getUserData(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1) {
                if (DB_Tools.nameToUserid(req.loc.Substring(7).Trim()) == userid)
                {
                    JObject payload = new JObject();
                    List<string> userData = DB_Tools.userData(DB_Tools.nameToUserid(req.loc.Substring(7).Trim()));
                    foreach (string data in userData)
                    {
                        string[] lines = data.Split("\r\n");
                        if (lines.Length == 2)
                        {
                            payload.Add(lines[0], lines[1]);
                        }
                    }
                    return new Response(req, Status_Code.OK, AdditionalPayload: payload.ToString());
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Unauthorised request. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Couldn't verify Connection. ");
        }

        //GET
        public static Response setActions(Request req, Battle battle) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                if (req.ctype == "json")
                {
                    user_battle_info thisUser = null;
                    foreach (user_battle_info user in battle.user_infos) {
                        if (user.username == DB_Tools.userIdToName(userid)) {
                            thisUser = user;
                        }
                    }
                    if (thisUser == null) {
                        thisUser = new user_battle_info(DB_Tools.userIdToName(userid));
                        battle.user_infos.Add(thisUser);
                    }
                    int status= thisUser.setActions(JObject.Parse(req.payload).GetValue("actions").ToString());
                    if (status == -2) { 
                        return new Response(req, Status_Code.NOK, AdditionalPayload: "Please use Rock Paper Scissor Lizard Spock initials. ");
                    }
                    if (status == -1)
                    {
                        return new Response(req, Status_Code.NOK, AdditionalPayload: "Please use 5 Characters. ");
                    }
                    if (status == 0)
                    {
                        return new Response(req, Status_Code.OK, AdditionalPayload: "Actions Set. ");
                    }

                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid type. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Coulnd't verify connection. ");
        }

        public static Response joinBattle(Request req, Battle battle) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                foreach (user_battle_info user in battle.user_infos)
                {
                    if (user.username == DB_Tools.userIdToName(userid))
                    {
                        foreach (Battle_Actions action in user.actions) {
                            if (action == Battle_Actions.NULL) { 
                                return new Response(req, Status_Code.NOK, AdditionalPayload: "Set the actions first!. ");
                            }
                        }
                        string payload=battle.joinBattle(user);
                        return new Response(req, Status_Code.NOK, AdditionalPayload: payload);
                    }
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Set the actions first!. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Coulnd't verify connection. ");
        }

        public static Response addToPlaylist(Request req) {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            {
                if (req.ctype == "json")
                {
                    JObject jObject = JObject.Parse(req.payload);
                    if (jObject.GetValue("Name") != null)
                    {
                        int mcid = DB_Tools.nameToMcid(jObject.GetValue("Name").ToString());
                        if (mcid != -1)
                        {
                            DB_Tools.addToPlaylist(mcid);
                            return new Response(req, Status_Code.OK, AdditionalPayload: "Added to Playlist. ");
                        }
                        return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid Song Name. Make sure it is spelled correctly. ");
                    }
                    return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid Arguments. Please specify name. ");
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid Type. Please use Json. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Coulnd't verify connection. ");
        }

        public static Response register(Request req) {
            if (req.ctype == "json"){
                JObject jObject = JObject.Parse(req.payload);
                if (DB_Tools.db_register(jObject.GetValue("Username").ToString(), jObject.GetValue("Password").ToString())) {
                    return new Response(req,Status_Code.OK, AdditionalPayload:"User Created. ");
                }
                return new Response(req, Status_Code.NOK,AdditionalPayload:"User Exists. ");
            }
            return new Response(req, Status_Code.NOK);
        }

        public static Response login(Request req)
        {
            if (req.ctype == "json")
            {
                JObject jObject = JObject.Parse(req.payload);
                if (DB_Tools.db_login(jObject.GetValue("Username").ToString(), jObject.GetValue("Password").ToString()))
                {
                    JObject payload = new JObject();
                    payload.Add("Authorization", generate_token(req));
                    return new Response(req, Status_Code.OK,AdditionalPayload:(payload.ToString()));
                }
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload:"Login Failed. ");
        }

        public static Response addToLib(Request req)
        {
            int userid = tokenToUserId(req.token);
            if (userid != -1)
            { 
                if (req.ctype == "json")
                {
                    JObject jObject = JObject.Parse(req.payload);
                    if (jObject.GetValue("Name") != null && jObject.GetValue("Url") != null) {
                        string name=null,filepath = null, genre = null, title = null, album = null;
                        float rating = -1;
                        int lenght = -1;
                        string filetype = "Url"; //no local files support
                        name = jObject.GetValue("Name").ToString().Trim();
                        filepath = jObject.GetValue("Url").ToString();
                        if (jObject.GetValue("Genre") != null) { genre= jObject.GetValue("Genre").ToString().Trim(); }
                        if (jObject.GetValue("Rating") != null) { rating=float.Parse(jObject.GetValue("Rating").ToString()); }
                        if (jObject.GetValue("Title") != null) { title=jObject.GetValue("Title").ToString().Trim(); }
                        if (jObject.GetValue("Album") != null) { album=jObject.GetValue("Album").ToString().Trim(); }
                        if (jObject.GetValue("Lenght") != null) { lenght=Int32.Parse(jObject.GetValue("Lenght").ToString()); }
                        if (DB_Tools.addToLib(userid, name, filepath, rating, genre, title, album, lenght, filetype))
                        {
                            return new Response(req, Status_Code.OK, AdditionalPayload:"Added to Library. ");
                        }
                        return new Response(req, Status_Code.NOK, AdditionalPayload: "Already Exists. ");
                    }
                    return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid Arguments. ");
                }
                return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid Type. ");
            }
            return new Response(req, Status_Code.NOK, AdditionalPayload: "Invalid Token. ");
        }

        //Tools

        public static string generate_token(Request req)
        {
            JObject jObject = JObject.Parse(req.payload);
            string token = $"Basic {jObject.GetValue("Username")}-ppbToken";
            return token;
        }

        public static int tokenToUserId(string token) {
            if (token != null)
            {
                return DB_Tools.nameToUserid(token.Substring(6, token.Length - 15));
            }
            return -1;
        }
    }
}