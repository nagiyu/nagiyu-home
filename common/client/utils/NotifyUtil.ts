import axios from "axios";
import PushNotifyByLoginUserRequest from "@common/models/requests/PushNotifyByLoginUserRequest";
import PushNotifyBySubscriptionIdRequest from "@common/models/requests/PushNotifyBySubscriptionIdRequest";

/**
 * Notify Utility
 */
export default class NotifyUtil {
  /**
   * Subscription ID によるプッシュ通知
   * @param request リクエスト
   */
  public static async PushBySubscriptionIds(request: PushNotifyBySubscriptionIdRequest): Promise<void> {
    await axios.post("/api/notification/push-by-subscription-ids", request);
  }

  /**
   * ログインユーザーにプッシュ通知
   * @param request リクエスト
   */
  public static async PushNotifyByLoginUser(request: PushNotifyByLoginUserRequest): Promise<void> {
    await axios.post("/api/notification/push-by-login-user", request);
  }

  /**
   * Subscription ID 登録
   * @param id Subscription ID
   */
  public static async RegisterSubscriptionId(id: string): Promise<void> {
    await axios.post(`/api/notification/${id}`);
  }
}
