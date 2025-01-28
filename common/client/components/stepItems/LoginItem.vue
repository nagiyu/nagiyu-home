<template>
  <p>Googleアカウントでログインすると便利になる機能が多くあります。</p>
  <p>ログインして使用することをオススメします。</p>

  <br />

  <b-field label="ログイン" horizontal>
    <template v-if="optedIn">
      <template v-if="!isLogin">
        <b-button type="is-success" @click="ClickLogin">ログイン</b-button>
      </template>
      <template v-else>
        <span>Completed</span>
      </template>
    </template>
    <template v-else>
      <span>通知を許可してからログインしてください。</span>
    </template>
  </b-field>

  <b-field label="紐付け" horizontal>
    <template v-if="isEnableNotify">
      <template v-if="!isSubscribeConnected">
        <b-button type="is-success" @click="ConnectSubscribe">通知の紐付け</b-button>
      </template>
      <template v-else>
        <span>Completed</span>
      </template>
    </template>
    <template v-else>
      <span>通知を有効にしてから設定してください。</span>
    </template>
  </b-field>

  <b-field label="通知テスト" horizontal>
    <template v-if="isEnableNotify">
      <template v-if="isSubscribeConnected">
        <b-button type="is-success" @click="TestUserPush">ユーザー通知</b-button>
      </template>
    </template>
    <template v-else>
      <span>通知を有効にしてから設定してください。</span>
    </template>
  </b-field>

  <b-field position="is-centered" class="buttons">
    <b-button type="is-warning" @click="SetRecommendLogin">今はやめておく</b-button>
    <b-button type="is-success" :disabled="!isTestPushCompleted" @click="SetLoginCompleted">完了</b-button>
  </b-field>

  <b-loading v-model="isLoading" :is-full-page="false"></b-loading>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Watch } from "vue-facing-decorator";
import StartupConst from "@common/consts/StartupConst";
import StepItemBase from "@common/components/stepItems/StepItemBase.vue";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";
import NotifyUtil from "@common/utils/NotifyUtil";

@Component
class LoginItem extends StepItemBase {
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
   * ユーザーがログインしているかどうか
   */
  public isLogin: boolean = false;

  /**
   * Subscribe が接続されているかどうか
   */
  public isSubscribeConnected: boolean = false;

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
   * 通知が有効かどうか
   */
  public isEnableNotify: boolean = false;

  /**
   * テスト通知の完了フラグ
   */
  public isTestPushCompleted: boolean = false;

  /**
   * マウント時の処理
   */
  public async mounted(): Promise<void> {
    await this.AsyncWithLoading(async () => {
      await this.UpdateUser();
      await this.RefreshAllData();
    });
  }

  /**
   * ログインボタンのクリックイベント
   */
  public ClickLogin(): void {
    window.location.href = "/Account/Login";
  }

  /**
   * 通知の紐付け
   */
  public async ConnectSubscribe(): Promise<void> {
    await this.AsyncWithLoading(async () => {
      await NotifyUtil.RegisterSubscriptionId(this.subscriptionId);

      await this.UpdateUser();
      await this.RefreshAllData();
    });
  }

  /**
   * ユーザー通知を送信
   */
  public async TestUserPush(): Promise<void> {
    await this.AsyncWithLoading(async () => {
      await NotifyUtil.PushNotifyByLoginUser({
        message: "Test User Push",
      });

      this.isTestPushCompleted = true;
    });
  }

  /**
   * ログインを完了に設定
   */
  public async SetLoginCompleted(): Promise<void> {
    LocalStorageUtil.SetItem(StartupConst.STORAGE_LOGIN_KEY, "completed");

    this.AsyncWithLoading(async () => {
      await this.ChangeStepsStatus();
    });
  }

  /**
   * ログインの勧誘を完了に設定
   */
  public async SetRecommendLogin(): Promise<void> {
    LocalStorageUtil.SetItem(StartupConst.STORAGE_RECOMMEND_LOGIN_KEY, "completed");

    this.AsyncWithLoading(async () => {
      await this.ChangeStepsStatus();
    });
  }

  /**
   * 全データをリフレッシュする
   */
  protected async RefreshAllData(): Promise<void> {
    await this.AsyncWithLoading(async () => {
      await super.RefreshAllData();

      this.isLogin = this.user !== null;
      this.isSubscribeConnected = this.user !== null && this.user.oneSignalSubscriptionId !== '' && this.user.oneSignalSubscriptionId === this.subscriptionId;
      this.isEnableNotify = LocalStorageUtil.GetItem(StartupConst.STORAGE_NOTIFY_KEY) !== null;
    });
  }
}

export default toNative(LoginItem);
</script>
