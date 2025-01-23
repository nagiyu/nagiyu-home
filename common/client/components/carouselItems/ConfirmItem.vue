<template>
  <p>
    <a @click="OpenPrivacyPolicyModal">プライバシーポリシー</a>と<a @click="OpenTermsModal">利用規約</a>に同意してください。
  </p>

  <br />

  <b-field position="is-centered" class="buttons">
    <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
    <div></div>
    <b-button type="is-success" @click="SetConfirm">同意する</b-button>
  </b-field>

  <PrivacyPolicyModal
    :isPrivacyPolicyModalActive="isPrivacyPolicyModalActive"
    @closePrivacyPolicyModal="ClosePrivacyPolicyModal"
  />

  <TermsModal
    :isTermsModalActive="isTermsModalActive"
    @closeTermsModal="CloseTermsModal"
  />
</template>

<script lang="ts">
import { Component, Emit, Prop, Vue, toNative } from "vue-facing-decorator";
import PrivacyPolicyModal from "@common/components/modals/PrivacyPolicyModal.vue";
import TermsModal from "@common/components/modals/TermsModal.vue";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";

@Component({
  components: {
    PrivacyPolicyModal,
    TermsModal
  }
})
class ConfirmItem extends Vue {
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
   * プライバシーポリシーモーダルの表示状態
   */
  public isPrivacyPolicyModalActive: boolean = false;

  /**
   * 利用規約モーダルの表示状態
   */
  public isTermsModalActive: boolean = false;

  /**
   * カルーセルのステータスを変更する
   */
  @Emit("changeCarouselStatus")
  public ChangeCarouselStatus(): void {
    return;
  }

  /**
   * モーダルを開く
   */
  @Emit('openStartupModal')
  public OpenStartupModal(): void {
    return;
  }

  /**
   * モーダルを閉じる
   */
  @Emit('closeStartupModal')
  public CloseStartupModal(): void {
    return;
  }

  /**
   * タイプに Web を設定
   */
  public SetConfirm(): void {
    LocalStorageUtil.SetItem(this.confirmKey, "confirm");
    this.ChangeCarouselStatus();
  }

  /**
   * プライバシーポリシーモーダルを開く
   */
  public OpenPrivacyPolicyModal(): void {
    this.CloseStartupModal();

    this.$nextTick(() => {
      this.isPrivacyPolicyModalActive = true;
    });
  }

  /**
   * プライバシーポリシーモーダルを閉じる
   */
  public ClosePrivacyPolicyModal(): void {
    this.isPrivacyPolicyModalActive = false;

    this.$nextTick(() => {
      this.OpenStartupModal();
    });
  }

  /**
   * 利用規約モーダルを開く
   */
  public OpenTermsModal(): void {
    this.isTermsModalActive = true;
  }

  /**
   * 利用規約モーダルを閉じる
   */
  public CloseTermsModal(): void {
    this.isTermsModalActive = false;
  }
}

export default toNative(ConfirmItem)
</script>
