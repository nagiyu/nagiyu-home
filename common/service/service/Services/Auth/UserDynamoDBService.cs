using Microsoft.Extensions.Configuration;

namespace Nagiyu.Common.Service.Services.Auth
{
    /// <summary>
    /// ユーザー情報の DynamoDB サービス
    /// </summary>
    public class UserDynamoDBService : DynamoDBServiceBase
    {
        public UserDynamoDBService(IConfiguration configuration) : base(configuration)
        {
            TableName = "User";
        }
    }
}
