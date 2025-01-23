<template>
  <b-modal v-model="isStartupModalActive" has-modal-card :can-cancel="false">
    <div class="modal-content" :style="modalStyle">
      <header class="modal-card-head">
        <p class="modal-card-title">
          確認
        </p>
      </header>

      <b-carousel 
        v-model="carousel"
        :autoplay="false"
        :arrow="false"
        :indicator="false"
      >
        <b-carousel-item v-if="IsEnabledPWACarouesel()">
          <section class="modal-card-body" :style="carouselStyle">
            <PWAItem
              :useTypeKey="USE_TYPE_KEY"
              @changeCarouselStatus="ChangeCarouselStatus"
            />
          </section>
        </b-carousel-item>

        <b-carousel-item v-if="IsEnabledConfirmCarouesel()">
          <section class="modal-card-body" :style="carouselStyle">
            <ConfirmItem
              :confirmKey="CONFIRM_KEY"
              @changeCarouselStatus="ChangeCarouselStatus"
            />
          </section>
        </b-carousel-item>

        <b-carousel-item v-if="IsEnabledLoginCarouesel()">
          <section class="modal-card-body" :style="carouselStyle">
            <LoginItem
              :recommendLoginKey="RECOMMEND_LOGIN_KEY"
              @changeCarouselStatus="ChangeCarouselStatus"
              @openStartupModal="OpenStartupModal"
              @closeStartupModal="CloseStartupModal"
            />
          </section>
        </b-carousel-item>

        <b-carousel-item v-if="IsEnabledNotifyCarouesel()">
          <section class="modal-card-body" :style="carouselStyle">
            <NotifyItem
              :recommendNotifyKey="RECOMMEND_NOTIFY_KEY"
              @changeCarouselStatus="ChangeCarouselStatus"
            />
          </section>
        </b-carousel-item>
      </b-carousel>

      <footer class="modal-card-foot">
        <b-button type="is-primary" :disabled="!IsEnabledPrevButton()" @click="PrevCarousel"><</b-button>
        <b-button type="is-primary" :disabled="!IsEnabledNextButton()" @click="NextCarousel">></b-button>
      </footer>
    </div>
  </b-modal>
</template>

<script lang="ts">
import { Component, Emit, Prop, Vue, toNative } from "vue-facing-decorator";
import PWAItem from "@common/components/carouselItems/PWAItem.vue";
import ConfirmItem from "@common/components/carouselItems/ConfirmItem.vue";
import NotifyItem from "@common/components/carouselItems/NotifyItem.vue";
import LoginItem from "@common/components/carouselItems/LoginItem.vue";
import PWAUtils from "@common/utils/PWAUtils";
import LocalStorageUtil from "@common/utils/LocalStorageUtil";
import WebUtil from "@common/utils/WebUtil";

@Component({
  components: {
    PWAItem,
    ConfirmItem,
    LoginItem,
    NotifyItem,
  }
})
class StartupModal extends Vue {
  /**
   * カルーセルのスタイル
   */
  public readonly carouselStyle = { height: '50vh' };

  /**
   * モーダル
   */
  public readonly modalStyle = WebUtil.IsMobile()
    ? { width: '90vw' }
    : { width: '30vw' };

  /**
   * タイプのローカルストレージのキー
   */
  public readonly USE_TYPE_KEY = "UseType";

  /**
   * 確認のローカルストレージのキー
   */
  public readonly CONFIRM_KEY = "Confirm";

  /**
   * ログインを勧めるカルーセルのローカルストレージのキー
   */
  public readonly RECOMMEND_LOGIN_KEY = "RecommendLogin";

  /**
   * 通知を勧めるカルーセルのローカルストレージのキー
   */
  public readonly RECOMMEND_NOTIFY_KEY = "RecommendNotify";

  /**
   * カルーセルのインデックス
   */
  public carousel: number = 0;

  /**
   * モーダルの表示状態
   */
  @Prop({
    type: Boolean,
    required: true,
    default: false
  })
  public isStartupModalActive: boolean = false;

  /**
   * マウント時の処理
   */
  public mounted(): void {
    this.ChangeCarouselStatus();
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
   * カルーセルのアイテム数
   */
  public CarouselItemCount(): number {
    var count = 0;

    if (this.IsEnabledPWACarouesel()) {
      count++;
    }

    if (this.IsEnabledConfirmCarouesel()) {
      count++;
    }

    if (this.IsEnabledLoginCarouesel()) {
      count++;
    }

    if (this.IsEnabledNotifyCarouesel()) {
      count++;
    }

    return count;
  }

  /**
   * PWA のカルーセルが有効かどうか
   */
  public IsEnabledPWACarouesel(): boolean {
    return !PWAUtils.IsPWA && LocalStorageUtil.GetItem(this.USE_TYPE_KEY) === null;
  }

  /**
   * Confirm のカルーセルが有効かどうか
   */
  public IsEnabledConfirmCarouesel(): boolean {
    return LocalStorageUtil.GetItem(this.CONFIRM_KEY) === null;
  }

  /**
   * ログインを勧めるカルーセルが有効かどうか
   */
  public IsEnabledLoginCarouesel(): boolean {
    return LocalStorageUtil.GetItem(this.RECOMMEND_LOGIN_KEY) === null;
  }

  /**
   * 通知を勧めるカルーセルが有効かどうか
   */
  public IsEnabledNotifyCarouesel(): boolean {
    return PWAUtils.IsPWA && LocalStorageUtil.GetItem(this.RECOMMEND_NOTIFY_KEY) === null;
  }

  /**
   * 前に戻るボタンが有効かどうか
   */
  public IsEnabledPrevButton(): boolean {
    return this.CarouselItemCount() !== 1 && this.carousel > 0;
  }

  /**
   * 次に進むボタンが有効かどうか
   */
  public IsEnabledNextButton(): boolean {
    return this.CarouselItemCount() !== 1 && this.carousel < this.CarouselItemCount() - 1;
  }

  /**
   * カルーセルのステータスを変更する
   */
  public ChangeCarouselStatus(): void {
    this.CloseStartupModal();

    this.$nextTick(() => {
      if (this.CarouselItemCount() > 0) {
        this.OpenStartupModal();
      }
    });
  }

  /**
   * カルーセルを前に戻す
   */
  public PrevCarousel(): void {
    this.carousel--;
  }

  /**
   * カルーセルを次に進める
   */
  public NextCarousel(): void {
    this.carousel++;
  }
}

export default toNative(StartupModal)
</script>
