<template>
  <template v-if="!IsPWA">
    <p>このサイトはWebでも閲覧できますが、アプリ化した方が便利な機能が多くあります。</p>
    <p>ぜひ、アプリ化してご利用ください。</p>

    <br />

    <b-collapse v-model="isOpenNotifyIOS" aria-id="notify-iso">
      <template #trigger="props">
        <b-button label="iOSの場合" type="is-primary" aria-controls="notify-iso" :aria-expanded="props.open" />
      </template>
      <div class="notification">
        <div class="content">
          <ol>
            <li>
              <p>本ページをSafariで開いてください。</p>
            </li>
            <li>
              <p>「共有」→「ホーム画面に追加」で、アプリとして追加してください。</p>
            </li>
          </ol>
          <p>※iOSのバージョンによって変わることがあります。</p>
          <p>※Safariから追加しないと正しく動作しないことがあります。</p>
        </div>
      </div>
    </b-collapse>

    <br />

    <b-collapse v-model="isOpenNotifyAndroid" aria-id="notify-android">
      <template #trigger="props">
        <b-button label="Androidの場合" type="is-primary" aria-controls="notify-android" :aria-expanded="props.open" />
      </template>
      <div class="notification">
        <div class="content">
          <ol>
            <li>
              <p>本ページをGoogleアプリで開いてください。</p>
            </li>
            <li>
              <p>「インストール」で、アプリとしてインストールしてください。</p>
              <p>※「オプション」→「ホーム
                画面に追加」でも可能です。</p>
            </li>
          </ol>
          <p>※Androidのバージョンによって変わることがあります。</p>
          <p>※Googleアプリから追加しないと正しく動作しないことがあります。</p>
        </div>
      </div>
    </b-collapse>

    <br />

    <b-collapse v-model="isOpenNotifyPC" aria-id="notify-pc">
      <template #trigger="props">
        <b-button label="PCの場合" type="is-primary" aria-controls="notify-pc" :aria-expanded="props.open" />
      </template>
      <div class="notification">
        <div class="content">
          <ol>
            <li>
              <p>アプリを利用するボタンからインストールしてください。</p>
              <p>※ChromeやEdgeの場合、URLとお気に入りボタンの間にあります。</p>
            </li>
          </ol>
          <p>※ブラウザの種類、バージョンによって変わることがあります。</p>
        </div>
      </div>
    </b-collapse>

    <br />

    <b-field position="is-centered" class="buttons">
      <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
      <div></div>
      <b-button type="is-success" @click="SetUseWeb">今はやめておく</b-button>
    </b-field>
  </template>

  <template v-else>
    <p>お使いの環境はアプリ化されています！</p>
  </template>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Vue } from "vue-facing-decorator";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";
import PWAUtils from "@common/utils/PWAUtils";

@Component
class PWAItem extends Vue {
  /**
   * タイプのローカルストレージのキー
   */
  @Prop({
    type: String,
    required: true,
    default: null
  })
  public useTypeKey!: string;

  /**
   * カルーセルのステータスを変更する
   */
  @Emit("changeStepsStatus")
  public ChangeStepsStatus(): void {
  }

  /**
   * iOS通知の表示状態
   */
  public isOpenNotifyIOS: boolean = false;

  /**
   * Android通知の表示状態
   */
  public isOpenNotifyAndroid: boolean = false;

  /**
   * PC通知の表示状態
   */
  public isOpenNotifyPC: boolean = false;

  /**
   * PWA かどうか
   */
  public get IsPWA(): boolean {
    return PWAUtils.IsPWA;
  }

  /**
   * タイプに Web を設定
   */
  public SetUseWeb(): void {
    LocalStorageUtil.SetItem(this.useTypeKey, "web");
    this.ChangeStepsStatus();
  }
}

export default toNative(PWAItem);
</script>
