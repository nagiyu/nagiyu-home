<template>
  <p>このサイトはWebでも閲覧できますが、アプリ化した方が便利な機能が多くあります。</p>
  <p>ぜひ、アプリ化してご利用ください。</p>

  <br />

  <b-field position="is-centered" class="buttons">
    <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
    <div></div>
    <b-button type="is-success" @click="SetUseWeb">今はやめておく</b-button>
  </b-field>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Vue } from "vue-facing-decorator";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";

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
  @Emit("changeCarouselStatus")
  public ChangeCarouselStatus(): void {
    return;
  }

  /**
   * タイプに Web を設定
   */
  public SetUseWeb(): void {
    LocalStorageUtil.SetItem(this.useTypeKey, "web");
    this.ChangeCarouselStatus();
  }
}

export default toNative(PWAItem);
</script>
