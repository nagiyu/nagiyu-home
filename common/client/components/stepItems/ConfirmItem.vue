<template>
  <template v-if="!isConfirmed">
    <p>
      <a @click="OpenPrivacyPolicyModal">プライバシーポリシー</a>と<a @click="OpenTermsModal">利用規約</a>に同意してください。
    </p>

    <br />

    <b-field position="is-centered" class="buttons">
      <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
      <div></div>
      <b-button type="is-success" @click="SetConfirm">同意する</b-button>
    </b-field>
  </template>

  <template v-else>
    <p>同意ありがとうございます！</p>
  </template>
</template>

<script lang="ts">
import { Component, Emit, Prop, Vue, Watch, toNative } from "vue-facing-decorator";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";

@Component
class ConfirmItem extends Vue {
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
  public confirmKey!: string;

  /**
   * カルーセルのステータスを変更する
   */
  @Emit("changeCarouselStatus")
  public ChangeCarouselStatus(): void {
    return;
  }

  /**
   * プライバシーポリシーモーダルを開く
   */
  @Emit('openPrivacyPolicyModal')
  public OpenPrivacyPolicyModal(): void {
    return;
  }

  /**
   * 利用規約モーダルを開く
   */
  @Emit('openTermsModal')
  public OpenTermsModal(): void {
    return;
  }

  /**
   * isActive の変更時
   */
  @Watch("isActive")
  public OnIsActiveChanged(): void {
    this.ChangeConfirmStatus();
  }

  /**
   * プライバシーポリシーモーダルの表示状態
   */
  public isPrivacyPolicyModalActive: boolean = false;

  /**
   * 利用規約モーダルの表示状態
   */
  public isTermsModalActive: boolean = false;

  /**
   * Confirm が完了しているかどうか
   */
  public isConfirmed: boolean = false;

  /**
   * タイプに Web を設定
   */
  public SetConfirm(): void {
    LocalStorageUtil.SetItem(this.confirmKey, "confirm");
    this.ChangeConfirmStatus();
    this.ChangeCarouselStatus();
  }

  /**
   * Confirm のステータスを変更する
   */
  private ChangeConfirmStatus(): void {
    this.isConfirmed = LocalStorageUtil.GetItem(this.confirmKey) === "confirm";
  }
}

export default toNative(ConfirmItem)
</script>
