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
              @changeStepsStatus="ChangeStepsStatus"
            />
          </b-step-item>
          <b-step-item step="2" label="同意" :type="isEnabledConfirmStep ? '' : 'is-success'">
            <ConfirmItem
              :isActive="stepIndex === 1"
              @changeCarouselStatus="ChangeStepsStatus"
              @openPrivacyPolicyModal="OpenPrivacyPolicyModal"
              @openTermsModal="OpenTermsModal"
            />
          </b-step-item>
          <b-step-item step="3" label="通知" :type="isEnabledNotifyStep ? '' : 'is-success'">
            <NotifyItem
              :isActive="stepIndex === 2"
              @changeStepsStatus="ChangeStepsStatus"
            />
          </b-step-item>
          <b-step-item step="4" label="ログイン" :type="isEnabledLoginStep ? '' : 'is-success'">
            <LoginItem
              :isActive="stepIndex === 3"
              @changeStepsStatus="ChangeStepsStatus"
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
import { Component, Emit, Prop, toNative } from "vue-facing-decorator";
import ViewBase from "@common/views/ViewBase.vue";
import PWAItem from "@common/components/stepItems/PWAItem.vue";
import ConfirmItem from "@common/components/stepItems/ConfirmItem.vue";
import NotifyItem from "@common/components/stepItems/NotifyItem.vue";
import LoginItem from "@common/components/stepItems/LoginItem.vue";
import StartupConst from "@common/consts/StartupConst";
import PWAUtils from "@common/utils/PWAUtils";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";
import WebUtil from "@common/utils/WebUtil";

@Component({
  components: {
    PWAItem,
    ConfirmItem,
    LoginItem,
    NotifyItem
  }
})
class StartupModal extends ViewBase {
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
   * ユーザーがログインしているかどうか
   */
  public get IsLogin(): boolean {
    return this.user !== null;
  }

  /**
   * ユーザーの OneSignal の SubscriptionId
   */
  public get UserSubscriptionId(): string {
    if (this.user === null) {
      return '';
    }

    return this.user.oneSignalSubscriptionId;
  }

  /**
   * ステップのステータスを変更する
   */
  public async ChangeStepsStatus(): Promise<void> {
    await this.AsyncWithLoading(async () => {
      await this.RefreshAllData();
    });

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
   * 全データをリフレッシュする
   */
  protected async RefreshAllData(): Promise<void> {
    await this.AsyncWithLoading(async () => {
      await super.RefreshAllData();

      this.ChangePWACaroueselStatus();
      this.ChangeConfirmCaroueselStatus();
      await this.ChangeLoginCaroueselStatus();
      await this.ChangeNotifyCaroueselStatus();

      this.ChangePrevButtonStatus();
      this.ChangeNextButtonStatus();
    });
  }

  /**
   * PWA のステップの状態を変更する
   */
  private ChangePWACaroueselStatus(): void {
    this.isEnabledPWAStep = !PWAUtils.IsPWA && LocalStorageUtil.GetItem(StartupConst.STORAGE_USE_TYPE_KEY) === null;
  }

  /**
   * Confirm のステップの状態を変更する
   */
  private ChangeConfirmCaroueselStatus(): void {
    this.isEnabledConfirmStep = LocalStorageUtil.GetItem(StartupConst.STORAGE_CONFIRM_KEY) === null;
  }

  /**
   * ログインを勧めるステップの状態を変更する
   */
  private async ChangeLoginCaroueselStatus(): Promise<void> {
    if (LocalStorageUtil.GetItem(StartupConst.STORAGE_LOGIN_KEY) !== null) {
      this.isEnabledLoginStep = false;
      return;
    }

    this.isEnabledLoginStep = LocalStorageUtil.GetItem(StartupConst.STORAGE_RECOMMEND_LOGIN_KEY) === null;
  }

  /**
   * 通知を勧めるステップの状態を変更する
   */
  private async ChangeNotifyCaroueselStatus(): Promise<void> {
    if (LocalStorageUtil.GetItem(StartupConst.STORAGE_NOTIFY_KEY) !== null) {
      this.isEnabledNotifyStep = false;
      return;
    }

    this.isEnabledNotifyStep = LocalStorageUtil.GetItem(StartupConst.STORAGE_RECOMMEND_NOTIFY_KEY) === null;
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
