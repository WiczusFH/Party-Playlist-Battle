using System;
using System.Collections.Generic;
using System.Text;

namespace Party_Playlist_Battle
{
    class headerDescAttribute:Attribute {
        public string descriptor;
        public headerDescAttribute(string descriptor) {
            this.descriptor = descriptor;
        }
    }
    public enum RespHeaderParams
    {
        [headerDesc("Server")]
        Server,
        [headerDesc("Content-Type")]
        cType,
        [headerDesc("Accept-Ranges")]
        aRanges,
        [headerDesc("Connection")]
        Connection,

    }
}
