<template>
  <p>通知を許可してください。</p>
  <p>※環境によっては通知されないことがあります。</p>
  <p>後から設定を変更することも可能です。</p>

  <br />

  <b-field position="is-centered" class="buttons">
    <b-button type="is-success" @click="PromptPush">通知の可否を表示する</b-button>
    <b-button type="is-warning" @click="SetRecommendNotify">今はやめておく</b-button>
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
   * 通知の許可を表示する
   */
  public async PromptPush(): Promise<void> {
    if (this.$OneSignal) {
      await this.$OneSignal.Slidedown.promptPush();
    } else {
      console.error("OneSignal is not initialized.");
    }
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
