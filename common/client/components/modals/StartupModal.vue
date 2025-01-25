<template>
  <b-modal v-model="isStartupModalActive" has-modal-card :can-cancel="false">
    <div class="modal-content" :style="modalStyle">
      <header class="modal-card-head">
        <p class="modal-card-title">
          確認
        </p>
      </header>

      <b-carousel 
        v-model="carousel"
        :autoplay="false"
        :arrow="false"
        :indicator="false"
      >
        <b-carousel-item v-if="isEnabledPWACarouesel">
          <section class="modal-card-body" :style="carouselStyle">
            <PWAItem
              :useTypeKey="USE_TYPE_KEY"
              @changeCarouselStatus="ChangeCarouselStatus"
            />
          </section>
        </b-carousel-item>

        <b-carousel-item v-if="isEnabledConfirmCarouesel">
          <section class="modal-card-body" :style="carouselStyle">
            <ConfirmItem
              :confirmKey="CONFIRM_KEY"
              @changeCarouselStatus="ChangeCarouselStatus"
              @openPrivacyPolicyModal="OpenPrivacyPolicyModal"
              @openTermsModal="OpenTermsModal"
            />
          </section>
        </b-carousel-item>

        <b-carousel-item v-if="isEnabledLoginCarouesel">
          <section class="modal-card-body" :style="carouselStyle">
            <LoginItem
              :recommendLoginKey="RECOMMEND_LOGIN_KEY"
              @changeCarouselStatus="ChangeCarouselStatus"
            />
          </section>
        </b-carousel-item>

        <b-carousel-item v-if="isEnabledNotifyCarouesel">
          <section class="modal-card-body" :style="carouselStyle">
            <NotifyItem
              :recommendNotifyKey="RECOMMEND_NOTIFY_KEY"
              @changeCarouselStatus="ChangeCarouselStatus"
            />
          </section>
        </b-carousel-item>
      </b-carousel>

      <footer class="modal-card-foot">
        <b-button type="is-primary" :disabled="!isEnabledPrevButton" @click="PrevCarousel"><</b-button>
        <b-button type="is-primary" :disabled="!isEnabledNextButton" @click="NextCarousel">></b-button>
      </footer>
    </div>
  </b-modal>
</template>

<script lang="ts">
import { Component, Emit, Prop, Vue, toNative } from "vue-facing-decorator";
import PWAItem from "@common/components/carouselItems/PWAItem.vue";
import ConfirmItem from "@common/components/carouselItems/ConfirmItem.vue";
import NotifyItem from "@common/components/carouselItems/NotifyItem.vue";
import LoginItem from "@common/components/carouselItems/LoginItem.vue";
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
   * カルーセルのスタイル
   */
  public readonly carouselStyle = { height: '50vh' };

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
   * カルーセルのインデックス
   */
  public carousel: number = 0;

  /**
   * PWA のカルーセルが有効かどうか
   */
  public isEnabledPWACarouesel: boolean = false;

  /**
   * Confirm のカルーセルが有効かどうか
   */
  public isEnabledConfirmCarouesel: boolean = false;

  /**
   * ログインを勧めるカルーセルが有効かどうか
   */
  public isEnabledLoginCarouesel: boolean = false;

  /**
   * Notify のカルーセルが有効かどうか
   */
  public isEnabledNotifyCarouesel: boolean = false;

  /**
   * 前に戻るボタンが有効かどうか
   */
  public isEnabledPrevButton: boolean = false;

  /**
   * 次に進むボタンが有効かどうか
   */
  public isEnabledNextButton: boolean = false;

  /**
   * マウント時の処理
   */
  public mounted(): void {
    this.ChangeCarouselStatus();
  }

  /**
   * カルーセルのステータスを変更する
   */
  public async ChangeCarouselStatus(): Promise<void> {
    this.ChangePWACaroueselStatus();
    this.ChangeConfirmCaroueselStatus();
    this.ChangeLoginCaroueselStatus();
    await this.ChangeNotifyCaroueselStatus();

    await this.ChangePrevButtonStatus();
    await this.ChangeNextButtonStatus();

    if (await this.CarouselItemCount() === 0) {
      this.CloseStartupModal();
    } else {
      this.OpenStartupModal();
    }
  }

  /**
   * カルーセルを前に戻す
   */
  public async PrevCarousel(): Promise<void> {
    this.carousel--;

    await this.ChangePrevButtonStatus();
    await this.ChangeNextButtonStatus();
  }

  /**
   * カルーセルを次に進める
   */
  public async NextCarousel(): Promise<void> {
    this.carousel++;

    await this.ChangePrevButtonStatus();
    await this.ChangeNextButtonStatus();
  }

  /**
   * カルーセルのアイテム数
   */
  private async CarouselItemCount(): Promise<number> {
    this.ChangePWACaroueselStatus();
    this.ChangeConfirmCaroueselStatus();
    this.ChangeLoginCaroueselStatus();
    await this.ChangeNotifyCaroueselStatus();

    var count = 0;

    if (this.isEnabledPWACarouesel) {
      count++;
    }

    if (this.isEnabledConfirmCarouesel) {
      count++;
    }

    if (this.isEnabledLoginCarouesel) {
      count++;
    }

    if (this.isEnabledNotifyCarouesel) {
      count++;
    }

    return count;
  }

  /**
   * PWA のカルーセルの状態を変更する
   */
  private ChangePWACaroueselStatus(): void {
    this.isEnabledPWACarouesel = !PWAUtils.IsPWA && LocalStorageUtil.GetItem(this.USE_TYPE_KEY) === null;
  }

  /**
   * Confirm のカルーセルの状態を変更する
   */
  private ChangeConfirmCaroueselStatus(): void {
    this.isEnabledConfirmCarouesel = LocalStorageUtil.GetItem(this.CONFIRM_KEY) === null;
  }

  /**
   * ログインを勧めるカルーセルの状態を変更する
   */
  private ChangeLoginCaroueselStatus(): void {
    this.isEnabledLoginCarouesel = LocalStorageUtil.GetItem(this.RECOMMEND_LOGIN_KEY) === null;
  }

  /**
   * 通知を勧めるカルーセルの状態を変更する
   */
  private async ChangeNotifyCaroueselStatus(): Promise<void> {
    if (!PWAUtils.IsPWA) {
      this.isEnabledNotifyCarouesel = false;
      return;
    }

    var subscriptionId = this.GetSubscriptionId();

    var user = await AuthUtil.GetUser<IUserAuthBase>();
    var userSubscriptionId = user ? user.oneSignalSubscriptionId : null;

    if (subscriptionId !== null && userSubscriptionId !== null && subscriptionId === userSubscriptionId) {
      this.isEnabledNotifyCarouesel = false;
      return;
    }

    this.isEnabledNotifyCarouesel = LocalStorageUtil.GetItem(this.RECOMMEND_NOTIFY_KEY) === null;
  }

  /**
   * 前に戻るボタンの状態を変更する
   */
  private async ChangePrevButtonStatus(): Promise<void> {
    this.isEnabledPrevButton = await this.CarouselItemCount() !== 1 && this.carousel > 0;
  }

  /**
   * 次に進むボタンの状態を変更する
   */
  private async ChangeNextButtonStatus(): Promise<void> {
    this.isEnabledNextButton = await this.CarouselItemCount() !== 1 && this.carousel < await this.CarouselItemCount() - 1;
  }

  /**
   * SubscriptionID を取得する
   */
  private GetSubscriptionId(): string | null {
    if (this.$OneSignal) {
      return this.$OneSignal.User.PushSubscription.id ?? null;
    }

    return null;
  }
}

export default toNative(StartupModal)
</script>
