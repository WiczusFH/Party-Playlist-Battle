using Microsoft.VisualStudio.TestTools.UnitTesting;
using Party_Playlist_Battle;

namespace PPB_Test
{
    [TestClass]
    public class User_Communication
    {
        [TestMethod]
        public void request() { 
            string message = "GET / HTTP/1.1\r\n"+
            "User - Agent: PostmanRuntime / 7.26.10\r\n" +
            "Accept: */*\r\n" +
            "Postman-Token: ec35d5f0-6f20-4f2c-9960-e8b5d3345f81\r\n" +
            "Host: 127.0.0.1:8080\r\n" +
            "Accept-Encoding: gzip, deflate, br\r\n" +
            "Connection: keep-alive\r\n\r\n"+
            "Hello\r\n\r\n";

            Request req = new Request(message);

            Assert.AreEqual(true, req.keepalive);
            Assert.AreEqual(RESTVerbs.GET, req.verb);
            Assert.IsTrue(req.payload.StartsWith("Hello"));
        
        }
    }
}
