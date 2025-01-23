<template>
  <div v-if="isPrivacyPolicyModalActive" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5);">
  </div>

  <b-modal v-model="isPrivacyPolicyModalActive" has-modal-card :can-cancel="false">
    <div class="modal-content" :style="modalStyle">
      <header class="modal-card-head">
        <p class="modal-card-title">
          プライバシーポリシー
        </p>
      </header>

      <section class="modal-card-body" style="height: 80vh;">
        <div class="content">
          <PrivacyPolicy />
        </div>
      </section>

      <footer class="modal-card-foot">
        <b-button type="is-primary" @click="ClosePrivacyPolicyModal">閉じる</b-button>
      </footer>
    </div>
  </b-modal>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Vue } from "vue-facing-decorator";
import PrivacyPolicy from "@common/components/PrivacyPolicy.vue";
import WebUtil from "@common/utils/WebUtil";

@Component({
  components: {
    PrivacyPolicy
  }
})
class PrivacyPolicyModal extends Vue {
  /**
   * モーダル
   */
  public readonly modalStyle = WebUtil.IsMobile()
    ? { width: '90vw' }
    : { width: '30vw' };

  /**
   * プライバシーポリシーモーダルの表示状態
   */
  @Prop({
    type: Boolean,
    required: true,
    default: false
  })
  public isPrivacyPolicyModalActive!: boolean;

  /**
   * プライバシーポリシーモーダルを閉じる
   */
  @Emit("closePrivacyPolicyModal")
  public ClosePrivacyPolicyModal(): void {
    return;
  }
}

export default toNative(PrivacyPolicyModal)
</script>
