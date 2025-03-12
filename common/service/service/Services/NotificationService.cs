using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Service.Consts;
using Nagiyu.Common.Service.Interfaces.Auth;
using Nagiyu.Common.Service.Models.DB.Auth;
using Nagiyu.Common.Service.Models.DB.Notification;
using Nagiyu.Common.Service.Models.Notification.Requests;
using Nagiyu.Common.Service.Services.Auth;
using Nagiyu.Common.Service.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        /// 認証サービス
        /// </summary>
        private readonly IAuthService authService;

        /// <summary>
        /// ユーザー情報の DynamoDB サービス
        /// </summary>
        private readonly UserDynamoDBService dbService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration">Configuration</param>
        /// <param name="httpClient">HttpClient</param>
        /// <param name="authService">認証サービス</param>
        /// <param name="userDynamoDBService">ユーザー情報の DynamoDB サービス</param>
        public NotificationService(IConfiguration configuration, HttpClient httpClient, IAuthService authService, UserDynamoDBService userDynamoDBService)
        {
            this.configuration = configuration;
            this.httpClient = httpClient;
            this.authService = authService;
            dbService = userDynamoDBService;
        }

        /// <summary>
        /// システムユーザーにプッシュ通知を送信
        /// </summary>
        /// <param name="message">メッセージ</param>
        public async Task PushNotifyOnlySystemRole(string message)
        {
            var users = await authService.GetUsersByRole(SystemRoleEnums.SystemRole.Admin);

            var subscriptionIds = new List<string>();

            foreach (var user in users)
            {
                var subscriptionIdList = await GetSubscriptionIdsByUserId(user.UserId);

                subscriptionIds.AddRange(subscriptionIdList);
            }

            await PushSubscriptions(message, subscriptionIds);
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

            var request = new NotificationSubscriptionRequest
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
        public async Task PushNotifyAll(string message)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Key {configuration["OneSignal:ApiKey"]}");
            httpClient.DefaultRequestHeaders.Add("accept", "application/json");

            var request = new NotificationSegmentsRequest
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

        /// <summary>
        /// ユーザーに SubscriptionID を追加する
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="subscriptionId">SubscriptionID</param>
        public async Task AddSubscriptionIdToUser(Guid userId, string subscriptionId)
        {
            var expressions = new Dictionary<string, string>
            {
                { nameof(UserRecord.UserId), userId.ToString() }
            };
            var records = await dbService.GetRecords<NotificationUserRecord>(expressions: expressions);
            var record = records.Item1.FirstOrDefault();

            ValidateUser.ValidateUserRecord(record);

            var id = Guid.Parse(record.Id);

            var user = NotificationUserConverter.ConvertToNotificationUser(record);

            if (!user.OneSignalSubscriptionIdList.Contains(subscriptionId))
            {
                user.OneSignalSubscriptionIdList.Add(subscriptionId);

                var userRecord = NotificationUserConverter.ConvertToNotificationUserRecord(user);

                await dbService.UpdateRecord(id, userRecord);
            }
        }

        /// <summary>
        /// ユーザーから SubscriptionID を削除する
        /// </summary>
        /// <param name="userId">ユーザーID</param>
        /// <param name="subscriptionId">SubscriptionID</param>
        public async Task RemoveSubscriptionIdFromUser(Guid userId, string subscriptionId)
        {
            var expressions = new Dictionary<string, string>
            {
                { nameof(UserRecord.UserId), userId.ToString() }
            };
            var records = await dbService.GetRecords<NotificationUserRecord>(expressions: expressions);
            var record = records.Item1.FirstOrDefault();

            ValidateUser.ValidateUserRecord(record);

            var id = Guid.Parse(record.Id);

            var user = NotificationUserConverter.ConvertToNotificationUser(record);

            if (user.OneSignalSubscriptionIdList.Contains(subscriptionId))
            {
                user.OneSignalSubscriptionIdList.Remove(subscriptionId);

                var userRecord = NotificationUserConverter.ConvertToNotificationUserRecord(user);

                await dbService.UpdateRecord(id, userRecord);
            }
        }

        /// <summary>
        /// UserID から Subscription ID を取得する
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <returns>Subscription ID リスト</returns>
        public async Task<List<string>> GetSubscriptionIdsByUserId(Guid userId)
        {
            var expressions = new Dictionary<string, string>
            {
                { nameof(UserRecord.UserId), userId.ToString() }
            };
            var records = await dbService.GetRecords<NotificationUserRecord>(expressions: expressions);
            var record = records.Item1.FirstOrDefault();

            ValidateUser.ValidateUserRecord(record);

            var user = NotificationUserConverter.ConvertToNotificationUser(record);

            return user.OneSignalSubscriptionIdList;
        }
    }
}
