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
                tokens[i] = "cb09b355cd2b4e85ee1fd89801e575ced7d4eb4f281f56aeec44156453103a76";
            }
            if (tokens == null) { tokens = new string[0]; }
            return tokens;
        }
    }
}
