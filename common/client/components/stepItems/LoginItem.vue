<template>
  <p>Googleアカウントでログインすると便利になる機能が多くあります。</p>
  <p>ログインして使用することをオススメします。</p>

  <br />

  <b-field label="ログイン" horizontal>
    <template v-if="!isLogin">
      <b-button type="is-success" @click="ClickLogin">ログイン</b-button>
    </template>
    <template v-else>
      <span>Completed</span>
    </template>
  </b-field>

  <b-field label="紐付け" horizontal>
    <template v-if="IsPWA">
      <template v-if="!isSubscribeConnected">
        <b-button type="is-success" @click="ConnectSubscribe">通知の紐付け</b-button>
      </template>
      <template v-else>
        <span>Completed</span>
      </template>
    </template>
    <template v-else>
      <span>通知を有効にするにはアプリ化してください。</span>
    </template>
  </b-field>

  <b-field label="通知テスト" horizontal>
    <template v-if="IsPWA">
      <template v-if="isSubscribeConnected">
        <b-button type="is-success" @click="TestUserPush">ユーザー通知</b-button>
      </template>
    </template>
    <template v-else>
      <span>通知を有効にするにはアプリ化してください。</span>
    </template>
  </b-field>

  <b-field position="is-centered" class="buttons">
    <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
    <div></div>
    <b-button type="is-warning" @click="SetRecommendLogin">今はやめておく</b-button>
  </b-field>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Watch } from "vue-facing-decorator";
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
   * タイプのローカルストレージのキー
   */
  @Prop({
    type: String,
    required: true,
    default: null
  })
  public recommendLoginKey!: string;

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
    this.AsyncWithLoading(async () => {
      // @ts-ignore
      this.$buefy.toast.open({
        message: "LoginItem の isActive が変更されました。",
        type: "is-info"
      });

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
    this.AsyncWithLoading(async () => {
      await NotifyUtil.RegisterSubscriptionId(this.subscriptionId);

      await this.RefreshAllData();
    });
  }

  /**
   * ユーザー通知を送信
   */
  public async TestUserPush(): Promise<void> {
    this.AsyncWithLoading(async () => {
      await NotifyUtil.PushNotifyByLoginUser({
        message: "Test User Push",
      });
    });
  }

  /**
   * ログインの勧誘を完了に設定
   */
  public async SetRecommendLogin(): Promise<void> {
    LocalStorageUtil.SetItem(this.recommendLoginKey, "completed");
    await this.ChangeStepsStatus();
  }

  /**
   * 全データをリフレッシュする
   */
  protected async RefreshAllData(): Promise<void> {
    await super.RefreshAllData();

    this.isLogin = this.user !== null;
    this.isSubscribeConnected = this.user !== null && this.user.oneSignalSubscriptionId === this.subscriptionId;
  }
}

export default toNative(LoginItem);
</script>
