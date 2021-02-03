using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace Party_Playlist_Battle
{
    class Program
    {

        public static void Main(string[] args) {
            Server serv = new Server();
            serv.run();
            //TcpListener listener = new TcpListener(IPAddress.Loopback, 8080);
            //listener.Start();
            //TcpClient client = listener.AcceptTcpClient();
            //byte[] buffer = new byte[4096];
            //Stream clistream = client.GetStream();

            //int readsize=clistream.Read(buffer, 0, buffer.Length);
            //Console.WriteLine(ASCIIEncoding.ASCII.GetString(buffer,0,readsize));
            //Response resp = new Response();
            //clistream.Write(ASCIIEncoding.ASCII.GetBytes(resp.formulateResponse()),0,ASCIIEncoding.ASCII.GetBytes(resp.formulateResponse()).Length);
            //clistream.Close();
            //client.Close();
        }
    }
}
