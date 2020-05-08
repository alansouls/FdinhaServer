using System;
using System.Collections.Generic;
using System.Text;

namespace FdinhaServer.Messages
{
    public class CreateRoomResponseMessage
    {
        public Guid Id { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
