<template>
  <template v-if="IsPWA">
    <p>通知を設定してください。</p>
    <p>※環境によっては通知されないことがあります。</p>

    <br />

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
      <b-button type="is-warning" @click="SetRecommendNotify">今はやめておく</b-button>
      <b-button type="is-success" :disabled="!isTestPushCompleted" @click="SetNotifyCompleted">完了</b-button>
    </b-field>

    <b-loading v-model="isLoading" :is-full-page="false"></b-loading>
  </template>

  <template v-else>
    <p>通知を設定するにはアプリ化してください。</p>
  </template>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Watch } from "vue-facing-decorator";
import StartupConst from "@common/consts/StartupConst";
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
    if (this.isActive) {
      await this.AsyncWithLoading(async () => {
        await this.RefreshAllData();
      });
    }
  }

  /**
   * テスト通知の完了フラグ
   */
  public isTestPushCompleted: boolean = false;

  /**
   * Mounted フック
   */
  public async mounted(): Promise<void> {
    await this.AsyncWithLoading(async () => {
      await super.mounted();
    });

    this.$OneSignal.User.PushSubscription.addEventListener('change', async () => {
      await this.AsyncWithLoading(async () => {
        await TimeUtils.Sleep(3000);
        await this.RefreshAllData();
      });
    });
  }

  /**
   * 紐付け
   */
  public async OptIn(): Promise<void> {
    await this.OnProductionAsync(async () => {
      await this.AsyncWithLoading(async () => {
        await this.$OneSignal.User.PushSubscription.optIn();
      });
    });
  }

  /**
   * 通知の許可を表示する
   */
  public async PromptPush(): Promise<void> {
    await this.OnProductionAsync(async () => {
      await this.AsyncWithLoading(async () => {
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
    await this.AsyncWithLoading(async () => {
      await NotifyUtil.PushBySubscriptionIds({
        subscriptionIds: [this.subscriptionId],
        message: "Test Push"
      });

      this.isTestPushCompleted = true;
    });
  }

  /**
   * 通知を完了に設定
   */
  public async SetNotifyCompleted(): Promise<void> {
    LocalStorageUtil.SetItem(StartupConst.STORAGE_NOTIFY_KEY, "completed");

    this.AsyncWithLoading(async () => {
      await this.ChangeStepsStatus();
    });
  }

  /**
   * 通知の勧誘を完了に設定
   */
  public async SetRecommendNotify(): Promise<void> {
    LocalStorageUtil.SetItem(StartupConst.STORAGE_RECOMMEND_NOTIFY_KEY, "completed");

    this.AsyncWithLoading(async () => {
      await this.ChangeStepsStatus();
    });
  }
}

export default toNative(NotifyItem);
</script>
