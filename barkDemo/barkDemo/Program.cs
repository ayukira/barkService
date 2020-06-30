using PushSharp.Apple;
using System;

namespace barkDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ApplePusher applePusher = new ApplePusher();
            applePusher.OnSendSuccess += (notification) =>
            {
                Console.WriteLine(notification.DeviceToken);
            };
            applePusher.OnSendFail += ApplePusher_OnSendFail;
            applePusher.OnSend += ApplePusher_OnSend;
            applePusher.OnSendSuccess += ApplePusher_OnSendSuccess;
            applePusher.OnStop += ApplePusher_OnStop;
            applePusher.StartService();
            while (true) 
            {
                Console.WriteLine("Enter body");
                var key = Console.ReadLine();
                if (key == "end") { break; }
                Console.WriteLine("Enter title");
                string body = key;
                string title = Console.ReadLine();
                applePusher.SendMesssage(new BarkMessage(title, body));
            }
            applePusher.StopService();
            Console.WriteLine("88 World!");
            Console.ReadLine();
        }

        private static void ApplePusher_OnStop(ApnsServiceBroker obj)
        {
            Console.WriteLine($"End {DateTime.Now}");
        }

        private static void ApplePusher_OnSendSuccess(ApnsNotification apnsNotification)
        {
            Console.WriteLine($"notify:{apnsNotification.Payload.ToString()}");
        }

        private static void ApplePusher_OnSend(object sender, SendEventArgs e)
        {
            Console.WriteLine($"body:{e.barkMessage.body}");
        }

        private static void ApplePusher_OnSendFail(ApnsNotificationException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
