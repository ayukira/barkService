using System;
using System.Collections.Generic;
using System.Text;

namespace barkDemo
{
    public class DeviceData
    {
        static IDictionary<string, string> deviceTokens = new Dictionary<string, string>();

        public static string[] DeviceTokens()
        {
            int tokenCount = 1;
            string[] tokens = new string[tokenCount];
            for (int i = 0; i < tokenCount; i++)
            {
                tokens[i] = "deviceTokenCode";
            }
            if (tokens == null) { tokens = new string[0]; }
            return tokens;
        }
    }
}
