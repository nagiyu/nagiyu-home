/**
 * LocalStorage Utility
 */
export default class LocalStorageUtil {
  /**
   * Get an item from LocalStorage
   * @param key - the key of the item to get
   * @returns string - the item from LocalStorage
   */
  public static GetItem(key: string): string | null {
    return localStorage.getItem(key);
  }

  /**
   * Set an item in LocalStorage
   * @param key - the key of the item to set
   * @param value - the value of the item to set
   */
  public static SetItem(key: string, value: string): void {
    localStorage.setItem(key, value);
  }
}