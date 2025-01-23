<template>
  <p>通知を許可してください。</p>
  <p>※環境によっては通知されないことがあります。</p>
  <p>後から設定を変更することも可能です。</p>

  <br />

  <b-field position="is-centered" class="buttons">
    <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
    <div></div>
    <b-button type="is-success" @click="SetRecommendNotify">OK</b-button>
  </b-field>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Vue } from "vue-facing-decorator";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";

@Component
class NotifyItem extends Vue {
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
   * カルーセルのステータスを変更する
   */
  @Emit("changeCarouselStatus")
  public ChangeCarouselStatus(): void {
    return;
  }

  /**
   * 通知の勧誘を完了に設定
   */
  public SetRecommendNotify(): void {
    LocalStorageUtil.SetItem(this.recommendNotifyKey, "completed");
    this.ChangeCarouselStatus();
  }
}

export default toNative(NotifyItem);
</script>
