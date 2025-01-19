using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Service.Consts;
using Nagiyu.Common.Service.Models.Notification.Requests;
using Nagiyu.Common.Service.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nagiyu.Common.Service.Services
{
    /// <summary>
    /// プッシュ通知サービス
    /// </summary>
    public class NotificationService
    {
        /// <summary>
        /// Configuration
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// HttpClient
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="httpClient">HttpClient</param>
        public NotificationService(IConfiguration configuration, HttpClient httpClient)
        {
            this.configuration = configuration;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// 一部メンバーにプッシュ通知を送信
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="subscriptionIds">Subscription IDs</param>
        public async Task PushSubscriptions(string message, List<string> subscriptionIds)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Key {configuration["OneSignal:ApiKey"]}");
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");

            var request = new NotificationRequest
            {
                AppId = configuration["OneSignal:AppId"],
                Contents = new NotificationContents
                {
                    En = message
                },
                IncludeSubscriptionIds = subscriptionIds
            };

            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.onesignal.com/notifications?c=push", content);

            LogHelper.WriteLog(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// 全員にプッシュ通知を送信
        /// </summary>
        /// <param name="message">メッセージ</param>
        public async Task PushTotalSubscriptions(string message)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Key {configuration["OneSignal:ApiKey"]}");
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");

            var request = new NotificationRequest
            {
                AppId = configuration["OneSignal:AppId"],
                Contents = new NotificationContents
                {
                    En = message
                },
                IncludedSegments = new List<string>
                {
                    NotificationConsts.Segments.TOTAL_SUBSCRIPTIONS
                }
            };

            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.onesignal.com/notifications?c=push", content);

            LogHelper.WriteLog(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
        }
    }
}
