<template>
  <StartupModal 
    :isStartupModalActive="isStartupModalActive"
    @openStartupModal="OpenStartupModal"
    @closeStartupModal="CloseStartupModal"
  />
</template>

<script lang="ts">
import { Component, Vue, toNative } from "vue-facing-decorator";
import StartupModal from "@common/components/modals/StartupModal.vue";

@Component({
  components: {
    StartupModal
  }
})
class Common extends Vue {
  /**
   * スタートアップモーダルの表示状態
   */
  public isStartupModalActive: boolean = false;

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
}

export default toNative(Common)
</script>
