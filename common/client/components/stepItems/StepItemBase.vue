<script lang="ts">
import ViewBase from "@common/views/ViewBase.vue";

export default class StepItemBase extends ViewBase {
  /**
   * 通知が許可されているか
   */
  public optedIn: boolean = false;

  /**
   * OneSignal の SubscriptionId
   */
  public subscriptionId: string = '';

  /**
   * 全データをリフレッシュする
   */
  protected async RefreshAllData(): Promise<void> {
    await super.RefreshAllData();

    this.optedIn = this.$OneSignal.User.PushSubscription.optedIn ?? false;
    this.subscriptionId = this.$OneSignal.User.PushSubscription.id ?? '';
  }
}
</script>
