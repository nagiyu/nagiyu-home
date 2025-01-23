<template>
  <b-modal v-model="isPrivacyPolicyModalActive" has-modal-card :can-cancel="false">
    <div class="modal-content">
      <header class="modal-card-head">
        <p class="modal-card-title">
          プライバシーポリシー
        </p>
      </header>

      <section class="modal-card-body">
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

@Component({
  components: {
    PrivacyPolicy
  }
})
class PrivacyPolicyModal extends Vue {
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
