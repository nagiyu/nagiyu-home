export default class TimeUtils {
  /**
   * Sleep for a given amount of time
   * @param ms milliseconds to sleep
   */
  public static async Sleep(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  public static GetMinutesAndSeconds(matchTime: number): { minutes: number, seconds: number } {
    const minutes = Math.floor(matchTime / 60);
    const seconds = matchTime % 60;
    return { minutes, seconds };
  }

  public static GetMatchTime(minutes: number, seconds: number): number {
    return minutes * 60 + seconds;
  }
}