using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Party_Playlist_Battle
{
    public class Response
    {
        Status_Code status;
        Request req;
        string[] additionalHeader;
        string additionalPayload;
        public Response(Request request, Status_Code Status, string[] AdditionalHeader=null, string AdditionalPayload = null) {
            req = request;
            status = Status;
            additionalHeader = AdditionalHeader;
            additionalPayload = AdditionalPayload;
        }

        public string formulateResponse() {
            string response = $"HTTP/1.1 200 {status}\r\n" +
            "Server: Wiczus\r\n" +
            "Content - Type: Application/json\r\n" +
            "Connection: close\r\n" +
            "Content - Lenght: 0\r\n";
            if (additionalHeader != null)
            {
                foreach (string line in additionalHeader)
                {
                    response += line + "\r\n";
                }
            }
            response+="\r\n";
            if (additionalPayload != null)
            {
                response += additionalPayload;
            }
            response += "\r\n\r\n";
            return response;
        }

    }
}