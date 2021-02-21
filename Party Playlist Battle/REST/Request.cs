using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Party_Playlist_Battle
{
    public class Request
    {
        public string message;
        public bool keepalive=true;
        public RESTVerbs verb;
        public string payload;
        public string loc;
        public string ctype;
        public string token;
        public Request(string Request_Message) {
            message = Request_Message;
            findVerb();
            findLoc();
            findPayld();
            findKeepAlive();
            findToken();
            findCType();
        }
        void findLoc() {
            if (message != null)
            {
                string firstLine = message.Split("\r\n")[0];
                string[] splitLine = firstLine.Split(" ");
                if (splitLine.Length > 1)
                {
                    loc = splitLine[1];
                }
            }
        }
        void findPayld() {
            if (message != null)
            {
                string[] lines = message.Split("\r\n\r\n");
                if (lines.Length > 1)
                {
                    payload = lines[1];
                }
            }
        }
        void findKeepAlive() {
            if (message.ToLower().Contains("connection: keep-alive"))
            {
                keepalive = true;
            }
            else
            {
                keepalive = false;
            }
        }
        void findToken() {
            string[] lines = message.Split("\r\n");
            foreach(string line in lines)
            {
                if (line.ToLower().Contains("authorisation:"))
                {
                    token = line.Substring(14).Trim();
                    break;
                }
            }
        }
        void findCType() {
            string[] lines = message.Split("\r\n");
            foreach (string line in lines)
            {
                if (line.ToLower().Contains("content-type:"))
                {
                    if (line.Contains("json")) {
                        ctype = "json";
                        break;
                    }
                    if (line.Contains("html"))
                    {
                        ctype = "html";
                        break;
                    }
                    ctype="text";
                    break;
                }
            }
        }
        void findVerb() {
            if (message.StartsWith("POST"))
            {
                verb = RESTVerbs.POST;
            }
            else if (message.StartsWith("GET"))
            {
                verb = RESTVerbs.GET;
            }
            else if (message.StartsWith("DEL"))
            {
                verb = RESTVerbs.DEL;
            }
            else if (message.StartsWith("PUT"))
            {
                verb = RESTVerbs.PUT;
            }
            else
            {
                verb = RESTVerbs.NULL;
            }
        }

        
    }
}