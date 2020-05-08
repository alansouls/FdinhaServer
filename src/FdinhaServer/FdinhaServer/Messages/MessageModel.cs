using FdinhaServer.Entities;
using System;

namespace FdinhaServer.Messages
{
    [Serializable]
    public class MessageModel
    {
        public string MessageId;

        public ActionObject Action;
    }
}
