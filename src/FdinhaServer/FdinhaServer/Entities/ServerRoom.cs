﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FdinhaServer.Entities
{
    public class ServerRoom
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public bool Open { get; set; }
    }
}
