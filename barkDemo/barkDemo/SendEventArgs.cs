using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace barkDemo
{
    public class SendEventArgs : EventArgs
    {
        public JObject payload { get; }
        public string deviceToken { get; }
        public BarkMessage barkMessage { get; }
        public SendEventArgs(JObject payload, string deviceToken, BarkMessage barkMessage)
        {
            this.payload = payload;
            this.deviceToken = deviceToken;
            this.barkMessage = barkMessage;
        }
    }
}
