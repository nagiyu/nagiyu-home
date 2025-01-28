<script lang="ts">
import { Vue } from "vue-facing-decorator";
import PWAUtils from "@common/utils/PWAUtils";
import AuthUtil from "@auth/utils/AuthUtil";

export default class ViewBase extends Vue {
  /**
   * ローディング中か
   */
  public isLoading: boolean = false;

  /**
   * ユーザー
   */
  protected user: IUserAuthBase | null = null;

  /**
   * 本番ビルドか
   */
  public get IsProduction(): boolean {
    return import.meta.env.PROD;
  }

  /**
   * Check if the app is running as a PWA
   */
  public get IsPWA(): boolean {
    return PWAUtils.IsPWA;
  }

  /**
   * ローディング中に非同期処理を実行する
   * @param func 非同期処理
   */
  protected async AsyncWithLoading<T>(func: () => Promise<T>): Promise<T> {
    // 既にローディング中であれば、何もしない
    if (this.isLoading) {
      return await func();
    }

    this.isLoading = true;
    try {
      return await func();
    } finally {
      this.isLoading = false;
    }
  }

  /**
   * 本番ビルド時のみ処理を実行する
   * @param func 処理
   */
  protected OnProduction<T>(func: () => T): T | null {
    if (!this.IsProduction) {
      return null;
    }

    return func();
  }

  /**
   * 本番ビルド時のみ処理を実行する
   * @param func 非同期処理
   */
  protected async OnProductionAsync<T>(func: () => Promise<T>): Promise<T | null> {
    if (!this.IsProduction) {
      return null;
    }

    return await func();
  }

  /**
   * 全データをリフレッシュする
   */
  protected async RefreshAllData(): Promise<void> {
    this.AsyncWithLoading(async () => {
      if (this.user === null) {
        await this.UpdateUser();

        this.$forceUpdate();
      }
    });
  }

  /**
   * ユーザーを更新する
   */
  protected async UpdateUser(): Promise<void> {
    this.AsyncWithLoading(async () => {
      this.user = await AuthUtil.GetUser<IUserAuthBase>();
    });
  }
}
</script>
