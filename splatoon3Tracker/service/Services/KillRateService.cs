using Nagiyu.Common.Auth.Service.Interfaces;
using Nagiyu.Splatoon3Tracker.Service.Exceptions;
using Nagiyu.Splatoon3Tracker.Service.Models.Requests;
using Nagiyu.Splatoon3Tracker.Service.Models.Responses;
using Nagiyu.Splatoon3Tracker.Service.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nagiyu.Splatoon3Tracker.Service.Services
{
    public class KillRateService
    {
        private readonly DynamoDBAccessor dynamoDBAccessor;
        private readonly IAuthService authService;

        public KillRateService(DynamoDBAccessor dynamoDBAccessor, IAuthService authService)
        {
            this.dynamoDBAccessor = dynamoDBAccessor;
            this.authService = authService;
        }

        /// <summary>
        /// KillRate の取得
        /// </summary>
        /// <returns>レスポンス</returns>
        public async Task<GetKillRatesResponse> GetKillRates()
        {
            var killRateRecords = await dynamoDBAccessor.GetKillRates();

            var result = new GetKillRatesResponse()
            {
                KillRates = new List<KillRateResponse>()
            };

            var userId = await GetUserId();

            foreach (var killRateRecord in killRateRecords)
            {
                var killRate = ModelUtil.ConvertToKillRate(killRateRecord);

                if (killRate.UserId != userId)
                {
                    continue;
                }

                var killRateResponse = ModelUtil.ConvertToKillRateResponse(killRate);
                result.KillRates.Add(killRateResponse);
            }

            return result;
        }

        /// <summary>
        /// KillRate の追加
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <returns>レスポンス</returns>
        public async Task<AddKillRateResponse> AddKillRate(KillRateRequest request)
        {
            var guid = Guid.NewGuid();
            var userId = await GetUserId();
            var killRate = ModelUtil.ConvertToKillRate(guid, userId, request);
            var killRateRecord = ModelUtil.ConvertToKillRateRecord(killRate);

            var id = await dynamoDBAccessor.AddKillRate(killRateRecord);

            return new AddKillRateResponse { Id = id };
        }

        /// <summary>
        /// KillRate の更新
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="request">リクエスト</param>
        public async Task UpdateKillRate(string id, KillRateRequest request)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                throw new ParameterException("Id is invalid.");
            }

            var userId = await GetUserId();
            var killRate = ModelUtil.ConvertToKillRate(guid, userId, request);
            var killRateRecord = ModelUtil.ConvertToKillRateRecord(killRate);

            await dynamoDBAccessor.UpdateKillRate(id, killRateRecord);
        }

        /// <summary>
        /// KillRate の削除
        /// </summary>
        /// <param name="id">ID</param>
        public async Task DeleteKillRate(string id)
        {
            if (!Guid.TryParse(id, out _))
            {
                throw new ParameterException("Id is invalid.");
            }

            await dynamoDBAccessor.DeleteKillRate(id);
        }

        /// <summary>
        /// ユーザー ID の取得
        /// </summary>
        /// <returns>UserID</returns>
        private async Task<Guid> GetUserId()
        {
            //var user = await authService.GetUser<UserAuthBase>();
            //return user.UserId;
            return Guid.Parse("acaa64d6-3da6-49a7-9c52-d57cfe8bdb34");
        }
    }
}
