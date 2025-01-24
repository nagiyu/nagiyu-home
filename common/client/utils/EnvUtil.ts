/**
 * 環境変数のための Utility
 */
export default class EnvUtil {
  /**
   * 環境変数から値を取得
   * @param key - 取得する値のキー
   * @returns string - 環境変数から取得した値
   */
  public static GetEnv(key: string): string | undefined {
    return import.meta.env[key];
  }
}
