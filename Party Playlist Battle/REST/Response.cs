using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Party_Playlist_Battle
{
    public class Response
    {

        public Response() {

        }
        public string formulateResponse() {
            string response = "HTTP/1.1 200 OK\r\n" +
            "Server: Wiczus\r\n" +
            "Content - Type: text / plain\r\n" +
            "Accept - Ranges: bytes\r\n" +
            "Connection: close\r\n" +
            "Content - Lenght: 0\r\n\r\n" + 
            "Hello this is Patrick. \r\n\r\n";
            return response;
        }

    }
}