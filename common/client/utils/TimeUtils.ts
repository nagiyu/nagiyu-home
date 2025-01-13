export default class TimeUtils {
  public static GetMinutesAndSeconds(matchTime: number): { minutes: number, seconds: number } {
    const minutes = Math.floor(matchTime / 60);
    const seconds = matchTime % 60;
    return { minutes, seconds };
  }

  public static GetMatchTime(minutes: number, seconds: number): number {
    return minutes * 60 + seconds;
  }
}