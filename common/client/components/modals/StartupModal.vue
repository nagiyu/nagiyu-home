<template>
  <b-modal v-model="isStartupModalActive" has-modal-card :can-cancel="false">
    <div class="modal-content" :style="modalStyle">
      <header class="modal-card-head">
        <p class="modal-card-title">
          初期設定
        </p>
      </header>

      <section class="modal-card-body" :style="MODAL_CARD_BODY_STYLE">
        <b-steps v-model="stepIndex" :has-navigation="false" mobile-mode="compact">
          <b-step-item step="1" label="アプリ化" :type="isEnabledPWAStep ? '' : 'is-success'">
            <PWAItem
              :useTypeKey="USE_TYPE_KEY"
              @changeStepsStatus="ChangeStepsStatus"
            />
          </b-step-item>
          <b-step-item step="2" label="同意" :type="isEnabledConfirmStep ? '' : 'is-success'">
            <ConfirmItem
              :isActive="stepIndex === 1"
              :confirmKey="CONFIRM_KEY"
              @changeCarouselStatus="ChangeStepsStatus"
              @openPrivacyPolicyModal="OpenPrivacyPolicyModal"
              @openTermsModal="OpenTermsModal"
            />
          </b-step-item>
          <b-step-item step="3" label="ログイン" :type="isEnabledLoginStep ? '' : 'is-success'">
            <LoginItem
              :recommendLoginKey="RECOMMEND_LOGIN_KEY"
              :isLogin="IsLogin"
              @changeCarouselStatus="ChangeStepsStatus"
            />
          </b-step-item>
          <b-step-item step="4" label="通知" :type="isEnabledNotifyStep ? '' : 'is-success'">
            <NotifyItem
              :recommendNotifyKey="RECOMMEND_NOTIFY_KEY"
              @changeCarouselStatus="ChangeStepsStatus"
            />
          </b-step-item>
        </b-steps>
      </section>

      <footer class="modal-card-foot">
        <b-button type="is-danger" :disabled="!isEnabledPrevButton" @click="ClickPrevButton"><</b-button>
        <b-button type="is-success" :disabled="!isEnabledNextButton" @click="ClickNextButton">></b-button>
      </footer>
    </div>
  </b-modal>
</template>

<script lang="ts">
import { Component, Emit, Prop, Vue, toNative } from "vue-facing-decorator";
import PWAItem from "@common/components/stepItems/PWAItem.vue";
import ConfirmItem from "@common/components/stepItems/ConfirmItem.vue";
import NotifyItem from "@common/components/stepItems/NotifyItem.vue";
import LoginItem from "@common/components/stepItems/LoginItem.vue";
import PWAUtils from "@common/utils/PWAUtils";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";
import WebUtil from "@common/utils/WebUtil";
import AuthUtil from "@auth/utils/AuthUtil";

@Component({
  components: {
    PWAItem,
    ConfirmItem,
    LoginItem,
    NotifyItem
  }
})
class StartupModal extends Vue {
  /**
   * Modal Card Body のスタイル
   */
  public readonly MODAL_CARD_BODY_STYLE = { height: '50vh' };

  /**
   * モーダル
   */
  public readonly modalStyle = WebUtil.IsMobile()
    ? { width: '90vw' }
    : { width: '30vw' };

  /**
   * タイプのローカルストレージのキー
   */
  public readonly USE_TYPE_KEY = "UseType";

  /**
   * 確認のローカルストレージのキー
   */
  public readonly CONFIRM_KEY = "Confirm";

  /**
   * ログインを勧めるカルーセルのローカルストレージのキー
   */
  public readonly RECOMMEND_LOGIN_KEY = "RecommendLogin";

  /**
   * 通知を勧めるカルーセルのローカルストレージのキー
   */
  public readonly RECOMMEND_NOTIFY_KEY = "RecommendNotify";

  /**
   * モーダルの表示状態
   */
  @Prop({
    type: Boolean,
    required: true,
    default: false
  })
  public isStartupModalActive: boolean = false;

  /**
   * モーダルを開く
   */
  @Emit('openStartupModal')
  public OpenStartupModal(): void {
    return;
  }

  /**
   * モーダルを閉じる
   */
  @Emit('closeStartupModal')
  public CloseStartupModal(): void {
    return;
  }

  /**
   * プライバシーポリシーモーダルを開く
   */
  @Emit('openPrivacyPolicyModal')
  public OpenPrivacyPolicyModal(): void {
    return;
  }

  /**
   * 利用規約モーダルを開く
   */
  @Emit('openTermsModal')
  public OpenTermsModal(): void {
    return;
  }

  /**
   * ステップのインデックス
   */
  public stepIndex: number = 0;

  /**
   * PWA のステップが有効かどうか
   */
  public isEnabledPWAStep: boolean = false;

  /**
   * Confirm のステップが有効かどうか
   */
  public isEnabledConfirmStep: boolean = false;

  /**
   * ログインを勧めるステップが有効かどうか
   */
  public isEnabledLoginStep: boolean = false;

  /**
   * Notify のステップが有効かどうか
   */
  public isEnabledNotifyStep: boolean = false;

  /**
   * 前に戻るボタンが有効かどうか
   */
  public isEnabledPrevButton: boolean = false;

  /**
   * 次に進むボタンが有効かどうか
   */
  public isEnabledNextButton: boolean = false;

  /**
   * ユーザー
   */
  private user: IUserAuthBase | null = null;

  /**
   * ユーザーがログインしているかどうか
   */
  public get IsLogin(): boolean {
    return this.user !== null;
  }

  /**
   * Mounted フック
   */
  public async mounted(): Promise<void> {
    this.user = await AuthUtil.GetUser<IUserAuthBase>();

    await this.ChangeStepsStatus();
  }

  /**
   * ステップのステータスを変更する
   */
  public async ChangeStepsStatus(): Promise<void> {
    this.ChangePWACaroueselStatus();
    this.ChangeConfirmCaroueselStatus();
    await this.ChangeLoginCaroueselStatus();
    await this.ChangeNotifyCaroueselStatus();

    this.ChangePrevButtonStatus();
    this.ChangeNextButtonStatus();

    if (this.isEnabledPWAStep && this.isEnabledConfirmStep && this.isEnabledLoginStep && this.isEnabledNotifyStep) {
      this.CloseStartupModal();
    } else {
      this.OpenStartupModal();
    }
  }

  /**
   * ステップを前に戻す
   */
  public ClickPrevButton(): void {
    this.stepIndex--;

    this.ChangePrevButtonStatus();
    this.ChangeNextButtonStatus();
  }

  /**
   * ステップを次に進める
   */
  public ClickNextButton(): void {
    this.stepIndex++;

    this.ChangePrevButtonStatus();
    this.ChangeNextButtonStatus();
  }

  /**
   * PWA のカルーセルの状態を変更する
   */
  private ChangePWACaroueselStatus(): void {
    this.isEnabledPWAStep = !PWAUtils.IsPWA && LocalStorageUtil.GetItem(this.USE_TYPE_KEY) === null;
  }

  /**
   * Confirm のカルーセルの状態を変更する
   */
  private ChangeConfirmCaroueselStatus(): void {
    this.isEnabledConfirmStep = LocalStorageUtil.GetItem(this.CONFIRM_KEY) === null;
  }

  /**
   * ログインを勧めるカルーセルの状態を変更する
   */
  private async ChangeLoginCaroueselStatus(): Promise<void> {
    await this.SetUser();

    if (this.user !== null) {
      this.isEnabledLoginStep = false;
      return;
    }

    this.isEnabledLoginStep = LocalStorageUtil.GetItem(this.RECOMMEND_LOGIN_KEY) === null;
  }

  /**
   * 通知を勧めるカルーセルの状態を変更する
   */
  private async ChangeNotifyCaroueselStatus(): Promise<void> {
    if (!PWAUtils.IsPWA) {
      this.isEnabledNotifyStep = false;
      return;
    }

    var subscriptionId = await this.GetSubscriptionId();

    if (subscriptionId !== '') {
      this.isEnabledNotifyStep = false;
      return;
    }

    this.isEnabledNotifyStep = LocalStorageUtil.GetItem(this.RECOMMEND_NOTIFY_KEY) === null;
  }

  /**
   * ユーザーを設定する
   */
  private async SetUser(): Promise<void> {
    if (this.user !== null) {
      return;
    }

    this.user = await AuthUtil.GetUser<IUserAuthBase>();
  }

  /**
   * OneSignal の SubscriptionId を取得する
   */
  private async GetSubscriptionId(): Promise<string> {
    if (import.meta.env.PROD) {
      await this.$OneSignal.User.PushSubscription.optIn();
      return this.$OneSignal.User.PushSubscription.id ?? '';
    } else {
      return '';
    }
  }

  /**
   * 前に戻るボタンの状態を変更する
   */
  private ChangePrevButtonStatus(): void {
    this.isEnabledPrevButton = this.stepIndex > 0;
  }

  /**
   * 次に進むボタンの状態を変更する
   */
  private ChangeNextButtonStatus(): void {
    this.isEnabledNextButton = this.stepIndex < 3;
  }
}

export default toNative(StartupModal)
</script>
