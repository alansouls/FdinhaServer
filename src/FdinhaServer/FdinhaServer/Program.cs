﻿using FdinhaServer.Core;
using System;

namespace FdinhaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new GameServer(8965);
            server.StartServer();
        }
    }
}
