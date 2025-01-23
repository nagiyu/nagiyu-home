/**
 * PWA Utils
 */
export default class PWAUtils {
  /**
   * Check if the app is running as a PWA
   * @returns boolean - true if the app is running as a PWA
   */
  public static get IsPWA(): boolean {
    return window.matchMedia('(display-mode: standalone)').matches;
  }
}