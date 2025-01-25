<template>
  <p>Googleアカウントでログインすると便利になる機能が多くあります。</p>
  <p>ログインして使用することをオススメします。</p>

  <br />

  <b-field position="is-centered" class="buttons">
    <b-button type="is-success" @click="ClickLogin">ログイン</b-button>
    <b-button type="is-warning" @click="SetRecommendLogin">今はやめておく</b-button>
  </b-field>
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
   * カルーセルのステータスを変更する
   */
  @Emit("changeCarouselStatus")
  public ChangeCarouselStatus(): void {
    return;
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
