using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using static PushSharp.Apple.ApnsConfiguration;

namespace barkDemo
{
    public class ApplePusher
    {
        #region Event
        //private EventHandler<SendEventArgs> OnSendHandler() => OnSend;
        /// <summary>
        /// 发送信息事件
        /// </summary>
        public event EventHandler<SendEventArgs> OnSend;
        /// <summary>
        /// 发送成功事件
        /// </summary>
        public event Action<ApnsNotification> OnSendSuccess;
        /// <summary>
        /// 发送异常事件
        /// </summary>
        public event Action<ApnsNotificationException> OnSendFail;
        /// <summary>
        /// 发送内部异常事件
        /// </summary>
        public event Action<Exception> OnSendInnerFail;
        /// <summary>
        /// 启动事件
        /// </summary>
        public event Action<ApnsServiceBroker> OnStart;
        /// <summary>
        /// 停止事件
        /// </summary>
        public event Action<ApnsServiceBroker> OnStop;
        #endregion

        IConfiguration Configuration = new ConfigurationBuilder().Add(new JsonConfigurationSource
        {
            Path = "cert.json",
            ReloadOnChange = true
        }).Build();
        ApnsConfiguration config;
        ApnsServiceBroker apnsBroker;
        string CertPath = string.Empty;
        string CertPassWord = string.Empty;
        int DeviceTokenLength = 64;
        ApnsServerEnvironment apnsServerEnvironment = ApnsServerEnvironment.Production;

        public ApplePusher()
        {
            CertPath = Configuration.GetSection("certpath").Value;
            CertPassWord = Configuration.GetSection("certpassword").Value;
            DeviceTokenLength = Configuration.GetValue("devicetokenlength", 64);
            apnsServerEnvironment = Configuration.GetValue("apnsserver", ApnsServerEnvironment.Production);
            config = new ApnsConfiguration(apnsServerEnvironment, CertPath, CertPassWord);
            apnsBroker = new ApnsServiceBroker(config);
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    // See what kind of exception it was to further diagnose 判断例外，进行诊断
                    if (ex is ApnsNotificationException notificationException)
                    {
                        // Deal with the failed notification 处理失败的通知
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;
                        Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");
                        OnSendFail?.Invoke(notificationException);
                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException		内部异常
                        Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                        OnSendInnerFail?.Invoke(ex);
                    }
                    // Mark it as handled 标记为处理
                    return true;
                });
            };
            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("Apple Notification Sent ! " + notification.DeviceToken);
                OnSendSuccess?.Invoke(notification);
            };
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        public void StartService()
        {
            if (apnsBroker != null)
            {
                apnsBroker.Start();
                OnStart?.Invoke(apnsBroker);
            }
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void StopService()
        {
            if (apnsBroker != null)
            {
                apnsBroker.Stop();
                OnStop?.Invoke(apnsBroker);
            }
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="barkMessage">信息</param>
        /// <param name="MY_DEVICE_TOKENS">device token 数组</param>
        /// <returns></returns>
        public bool SendMesssage(BarkMessage barkMessage, string[] MY_DEVICE_TOKENS = null)
        {
            if (apnsBroker == null)
            {
                return false;
            }
            if (barkMessage == null)
            {
                return false;
            }
            if (MY_DEVICE_TOKENS == null)
            {
                MY_DEVICE_TOKENS = DeviceData.DeviceTokens();
            }
            if (MY_DEVICE_TOKENS == null)
            {
                return false;
            }
            if (MY_DEVICE_TOKENS.Length <= 0)
            {
                return false;
            }
            Dictionary<string, object> payload = new Dictionary<string, object>();
            Dictionary<string, object> aps = new Dictionary<string, object>();
            Dictionary<string, object> alert = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(barkMessage.body))
            {
                alert.Add("body", barkMessage.body);
            }
            if (!string.IsNullOrEmpty(barkMessage.title))
            {
                alert.Add("title", barkMessage.title);
            }
            aps.Add("category", "myNotificationCategory");
            aps.Add("sound", "1");
            aps.Add("badge", "0");
            aps.Add("mutable-content", "1");
            aps.Add("alert", alert);
            //1 自动保存 0 不保存
            payload.Add("isarchive", barkMessage.isArchive);
            //url地址
            if (!string.IsNullOrEmpty(barkMessage.url)) { payload.Add("url", barkMessage.url); }
            //推送内容会自动复制到粘贴板
            payload.Add("automaticallycopy", barkMessage.autoMaticallyCopy);
            //复制字段
            if (!string.IsNullOrEmpty(barkMessage.copy)) { payload.Add("copy", barkMessage.copy); }
            payload.Add("aps", aps);
            var jsonObject = JObject.FromObject(payload);

            foreach (var deviceToken in MY_DEVICE_TOKENS)
            {
                SendNotification(jsonObject, deviceToken, barkMessage);
            }
            return true;
        }

        private void SendNotification(JObject payload, string deviceToken, BarkMessage barkMessage)
        {
            if (apnsBroker == null)
            {
                return;
            }
            if (deviceToken.Length != DeviceTokenLength)
            {
                return;
            }
            try
            {
                // 队列发送一个通知
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,//这里的deviceToken是ios端获取后传递到数据库统一记录管理的，有效的Token才能保证推送成功
                    Payload = payload
                });
                OnSend?.Invoke(this, new SendEventArgs(payload, deviceToken, barkMessage));
            }
            catch (Exception) { }
        }
    }
}
