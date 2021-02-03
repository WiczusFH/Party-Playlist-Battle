using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Party_Playlist_Battle
{
    public class Request
    {
        public bool keepalive=true;
        public RESTVerbs verb;
        public string payload;
        public Request(string message) {
            //verb
            if (message.StartsWith("POST")) {
                verb = RESTVerbs.POST;
            }
            else if (message.StartsWith("GET")) {
                verb = RESTVerbs.GET;
            }
            else if (message.StartsWith("DEL")) {
                verb = RESTVerbs.DEL;
            }
            else if (message.StartsWith("PUT")) {
                verb = RESTVerbs.PUT;
            }
            else {
                verb = RESTVerbs.NULL;
            }
            if (message.ToLower().Contains("connection: keep-alive")) {
                keepalive = true;
            } else {
                keepalive = false;   
            }
            payload=message.Split("\r\n\r\n", 2)[1];


        }


        
    }
}