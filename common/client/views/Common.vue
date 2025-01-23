<template>
  <StartupModal 
    :isStartupModalActive="isStartupModalActive"
    @openStartupModal="OpenStartupModal"
    @closeStartupModal="CloseStartupModal"
    @openPrivacyPolicyModal="OpenPrivacyPolicyModal"
    @openTermsModal="OpenTermsModal"
  />

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
import { Component, Vue, toNative } from "vue-facing-decorator";
import StartupModal from "@common/components/modals/StartupModal.vue";
import PrivacyPolicyModal from "@common/components/modals/PrivacyPolicyModal.vue";
import TermsModal from "@common/components/modals/TermsModal.vue";

@Component({
  components: {
    StartupModal,
    PrivacyPolicyModal,
    TermsModal
  }
})
class Common extends Vue {
  /**
   * スタートアップモーダルの表示状態
   */
  public isStartupModalActive: boolean = false;

  /**
   * プライバシーポリシーモーダルの表示状態
   */
  public isPrivacyPolicyModalActive: boolean = false;

  /**
   * 利用規約モーダルの表示状態
   */
  public isTermsModalActive: boolean = false;

  /**
   * スタートアップモーダルを開く
   */
  public OpenStartupModal(): void {
    this.isStartupModalActive = true;
  }

  /**
   * スタートアップモーダルを閉じる
   */
  public CloseStartupModal(): void {
    this.isStartupModalActive = false;
  }

  /**
   * プライバシーポリシーモーダルを開く
   */
  public OpenPrivacyPolicyModal(): void {
    this.isStartupModalActive = false;
    this.isPrivacyPolicyModalActive = true;
  }

  /**
   * プライバシーポリシーモーダルを閉じる
   */
  public ClosePrivacyPolicyModal(): void {
    this.isPrivacyPolicyModalActive = false;
    this.isStartupModalActive = true;
  }

  /**
   * 利用規約モーダルを開く
   */
  public OpenTermsModal(): void {
    this.isStartupModalActive = false;
    this.isTermsModalActive = true;
  }

  /**
   * 利用規約モーダルを閉じる
   */
  public CloseTermsModal(): void {
    this.isTermsModalActive = false;
    this.isStartupModalActive = true;
  }
}

export default toNative(Common)
</script>
