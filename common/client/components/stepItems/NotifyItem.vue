<template>
  <template v-if="IsPWA">
    <p>通知を設定してください。</p>
    <p>※環境によっては通知されないことがあります。</p>

    <br />

    <b-field label="設定" horizontal>
      <template v-if="!IsEnabledSubscribe">
        <b-button type="is-success" @click="PromptPush">通知設定</b-button>
      </template>
      <template v-else>
        <span>Completed</span>
      </template>
    </b-field>

    <b-field label="テスト" horizontal>
      <template v-if="isConectedSubscribe">
        <b-button type="is-success">通知</b-button>
      </template>
    </b-field>

    <br />

    <b-field position="is-centered" class="buttons">
      <b-button type="is-success" @click="PromptPush">通知設定</b-button>
      <b-button type="is-warning" @click="SetRecommendNotify">今はやめておく</b-button>
    </b-field>

    <b-loading v-model="isLoading" :is-full-page="false"></b-loading>
  </template>

  <template v-else>
    <p>通知を設定するにはアプリ化してください。</p>
  </template>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Vue, Watch } from "vue-facing-decorator";
import axios from "axios";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";
import PWAUtils from "@common/utils/PWAUtils";

@Component
class NotifyItem extends Vue {
  /**
   * Step のアクティブ状態
   */
  @Prop({
    type: Boolean,
    required: true,
    default: false
  })
  public isActive!: boolean;

  /**
   * タイプのローカルストレージのキー
   */
  @Prop({
    type: String,
    required: true,
    default: null
  })
  public recommendNotifyKey!: string;

  @Prop({
    type: Boolean,
    required: true,
    default: false
  })
  public isLogin!: boolean;

  /**
   * ユーザーの OneSignal の SubscriptionId
   */
  @Prop({
    type: String,
    required: true,
    default: ''
  })
  public userSubscriptionId!: string;

  /**
   * カルーセルのステータスを変更する
   */
  @Emit("changeCarouselStatus")
  public ChangeCarouselStatus(): void {
  }

  /**
   * ユーザーを設定する
   */
  @Emit('setUser')
  public async SetUser(): Promise<void> {
  }

  /**
   * isActive の変更時
   */
  @Watch("isActive")
  public async OnIsActiveChanged(): Promise<void> {
    await this.ChangeSubscribeStatus();
  }

  /**
   * ユーザーと SubscriptionId が紐付いているか
   */
  public isConectedSubscribe: boolean = false;

  /**
   * ローディング中か
   */
  public isLoading: boolean = false;

  /**
   * OneSignal の SubscriptionId
   */
  private subscriptionId: string = '';

  /**
   * Check if the app is running as a PWA
   */
  public get IsPWA(): boolean {
    return PWAUtils.IsPWA;
  }

  /**
   * 通知が許可されているか
   */
  public get IsEnabledSubscribe(): boolean {
    return this.subscriptionId !== '';
  }

  /**
   * Mounted フック
   */
  public mounted(): void {
    this.$OneSignal.User.PushSubscription.addEventListener('change', this.ChangeSubscribeEvent);
  }

  /**
   * 通知の許可を表示する
   */
  public async PromptPush(): Promise<void> {
    if (import.meta.env.PROD) {
      this.isLoading = true;

      await this.$OneSignal.Slidedown.promptPush({
        force: true
      });

      this.isLoading = false;
    }
  }

  /**
   * 通知の勧誘を完了に設定
   */
  public SetRecommendNotify(): void {
    LocalStorageUtil.SetItem(this.recommendNotifyKey, "completed");
    this.ChangeCarouselStatus();
  }

  /**
   * Subscribe のステータス変更時のイベント
   */
  private async ChangeSubscribeEvent(): Promise<void> {
      if (!this.isLogin) {
        return;
      }

      this.isLoading = true;

      // 確実に subscriptionId が取得できるようにするために 3 秒待つ
      await new Promise(resolve => setTimeout(resolve, 3000));

      this.subscriptionId = this.$OneSignal.User.PushSubscription.id ?? '';

      if (this.subscriptionId === '') {
        // @ts-ignore
        this.$buefy.toast.open({
            duration: 5000,
            message: 'SubscriptionID is null',
            type: 'is-danger'
        })

        this.isLoading = false;

        return;
      }

      await axios.post(`/api/notification/${this.subscriptionId}`);

      await this.SetUser();

      this.isLoading = false;

      await this.ChangeSubscribeStatus();
  }

  /**
   * Subscribe のステータスを変更する
   */
  private async ChangeSubscribeStatus(): Promise<void> {
    this.isConectedSubscribe = this.IsEnabledSubscribe && this.subscriptionId === this.userSubscriptionId;
  }
}

export default toNative(NotifyItem);
</script>
