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
    var response = await axios.post("/api/notification/push-by-subscription-ids", request);

    if (response.status !== 200) {
      throw new Error(response.data);
    }

    return;
  }

  /**
   * ログインユーザーにプッシュ通知
   * @param request リクエスト
   */
  public static async PushNotifyByLoginUser(request: PushNotifyByLoginUserRequest): Promise<void> {
    var response = await axios.post("/api/notification/push-by-login-user", request);

    if (response.status !== 200) {
      throw new Error(response.data);
    }

    return;
  }

  /**
   * Subscription ID 登録
   * @param id Subscription ID
   */
  public static async RegisterSubscriptionId(id: string): Promise<void> {
    var response = await axios.post(`/api/notification/${id}`);

    if (response.status !== 200) {
      throw new Error(response.data);
    }

    return;
  }
}
