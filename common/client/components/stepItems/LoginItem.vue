<template>
  <template v-if="!isLogin">
    <p>Googleアカウントでログインすると便利になる機能が多くあります。</p>
    <p>ログインして使用することをオススメします。</p>

    <br />

    <b-field position="is-centered" class="buttons">
      <b-button type="is-success" @click="ClickLogin">ログイン</b-button>
      <b-button type="is-warning" @click="SetRecommendLogin">今はやめておく</b-button>
    </b-field>
  </template>

  <template v-else>
    <p>ログインありがとうございます！</p>
  </template>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Vue } from "vue-facing-decorator";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";

@Component
class LoginItem extends Vue {
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
  @Prop({
    type: Boolean,
    required: true,
    default: false
  })
  public isLogin: boolean = false;

  /**
   * カルーセルのステータスを変更する
   */
  @Emit("changeCarouselStatus")
  public ChangeCarouselStatus(): void {
  }

  /**
   * ログインボタンのクリックイベント
   */
  public ClickLogin(): void {
    window.location.href = "/Account/Login";
  }

  /**
   * ログインの勧誘を完了に設定
   */
  public SetRecommendLogin(): void {
    LocalStorageUtil.SetItem(this.recommendLoginKey, "completed");
    this.ChangeCarouselStatus();
  }
}

export default toNative(LoginItem);
</script>
