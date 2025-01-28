<template>
  <template v-if="IsPWA">
    <p>通知を設定してください。</p>
    <p>※環境によっては通知されないことがあります。</p>

    <br />

    <b-button @click="Debug">Debug</b-button>

    <b-field label="許可" horizontal>
      <template v-if="!optedIn">
        <b-button type="is-success" @click="OptIn">通知の許可</b-button>
      </template>
      <template v-else>
        <span>Completed</span>
      </template>
    </b-field>

    <b-field label="受け入れ" horizontal>
      <template v-if="optedIn">
        <template v-if="subscriptionId === ''">
          <b-button type="is-success" @click="PromptPush">通知の受け入れ</b-button>
        </template>
        <template v-else>
          <span>Completed</span>
        </template>
      </template>
    </b-field>

    <b-field label="テスト" horizontal>
      <template v-if="optedIn && subscriptionId !== ''">
        <b-button type="is-success" @click="TestPush">端末通知</b-button>
      </template>
    </b-field>

    <br />

    <b-field position="is-centered" class="buttons">
      <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
      <div></div>
      <b-button type="is-warning" @click="SetRecommendNotify">今はやめておく</b-button>
    </b-field>

    <b-loading v-model="isLoading" :is-full-page="false"></b-loading>
  </template>

  <template v-else>
    <p>通知を設定するにはアプリ化してください。</p>
  </template>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Watch } from "vue-facing-decorator";
import StepItemBase from "@common/components/stepItems/StepItemBase.vue";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";
import NotifyUtil from "@common/utils/NotifyUtil";
import TimeUtils from "@common/utils/TimeUtils";

@Component
class NotifyItem extends StepItemBase {
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

  /**
   * ステップのステータスを変更する
   */
  @Emit("changeStepsStatus")
  public async ChangeStepsStatus(): Promise<void> {
  }

  /**
   * isActive の変更時
   */
  @Watch("isActive")
  public async OnIsActiveChanged(): Promise<void> {
    this.AsyncWithLoading(async () => {
      await this.RefreshAllData();
    });
  }

  /**
   * Mounted フック
   */
  public mounted(): void {
    this.$OneSignal.User.PushSubscription.addEventListener('change', async () => {
      this.AsyncWithLoading(async () => {
        await TimeUtils.Sleep(2000);

        await this.RefreshAllData();
      });
    });
  }

  public Debug(): void {
    // @ts-ignore
    this.$buefy.toast.open({
      message: `permission: ${this.$OneSignal.Notifications.permission}`,
      type: "is-success"
    });
  }

  /**
   * 紐付け
   */
  public async OptIn(): Promise<void> {
    this.OnProductionAsync(async () => {
      this.AsyncWithLoading(async () => {
        await this.$OneSignal.User.PushSubscription.optIn();
      });
    });
  }

  /**
   * 通知の許可を表示する
   */
  public async PromptPush(): Promise<void> {
    this.OnProductionAsync(async () => {
      this.AsyncWithLoading(async () => {
        await this.$OneSignal.Slidedown.promptPush({
          force: true
        });
      });
    });
  }

  /**
   * テスト通知を送信
   */
  public async TestPush(): Promise<void> {
    this.OnProductionAsync(async () => {
      this.AsyncWithLoading(async () => {
        await NotifyUtil.PushBySubscriptionIds({
          subscriptionIds: [this.subscriptionId],
          message: "Test Push"
        });
      });
    });
  }

  /**
   * 通知の勧誘を完了に設定
   */
  public async SetRecommendNotify(): Promise<void> {
    LocalStorageUtil.SetItem(this.recommendNotifyKey, "completed");

    await this.RefreshAllData();
  }

  /**
   * 全データをリフレッシュする
   */
  protected async RefreshAllData(): Promise<void> {
    await super.RefreshAllData();

    this.optedIn = this.$OneSignal.User.PushSubscription.optedIn ?? false;
    this.subscriptionId = this.$OneSignal.User.PushSubscription.id ?? '';
  }
}

export default toNative(NotifyItem);
</script>
