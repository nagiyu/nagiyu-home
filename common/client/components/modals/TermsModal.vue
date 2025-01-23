<template>
  <b-modal v-model="isTermsModalActive" has-modal-card :can-cancel="false">
    <div class="modal-content" :style="modalStyle">
      <header class="modal-card-head">
        <p class="modal-card-title">
          利用規約
        </p>
      </header>

      <section class="modal-card-body">
        <div class="content">
          <Terms />
        </div>
      </section>

      <footer class="modal-card-foot">
        <b-button type="is-primary" @click="CloseTermsModal">閉じる</b-button>
      </footer>
    </div>
  </b-modal>
</template>

<script lang="ts">
import { Component, Emit, Prop, toNative, Vue } from "vue-facing-decorator";
import Terms from "@common/components/Terms.vue";
import WebUtil from "@common/utils/WebUtil";

@Component({
  components: {
    Terms
  }
})
class TermsModal extends Vue {
  /**
   * モーダル
   */
  public readonly modalStyle = WebUtil.IsMobile() ? { width: '95vw' } : { width: '30vw' };

  /**
   * 利用規約モーダルの表示状態
   */
  @Prop({
    type: Boolean,
    required: true,
    default: false
  })
  public isTermsModalActive!: boolean;

  /**
   * 利用規約モーダルを閉じる
   */
  @Emit("closeTermsModal")
  public CloseTermsModal(): void {
    return;
  }
}

export default toNative(TermsModal)
</script>
