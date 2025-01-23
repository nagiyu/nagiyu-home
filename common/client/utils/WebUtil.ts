/**
 * WebUtil
 */
export default class WebUtil {
  /**
   * モバイル端末かどうかを判定する
   */
  public static IsMobile(): boolean {
    return window.matchMedia('(max-width: 767px)').matches;
  }
}
