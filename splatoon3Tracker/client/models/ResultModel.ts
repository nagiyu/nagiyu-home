import dayjs from "dayjs";

export class ResultModel {
  public id?: string;
  public recordType: string = "";
  public battle: string = "";
  public rule: string = "";
  public weapon: string = "";
  public result: string = "";
  public kill: number = 0;
  public assist: number = 0;
  public death: number = 0;
  public special: number = 0;
  public date: string = dayjs().format('YYYY-MM-DD HH:mm');
  public matchTime: number = 0;

  public get killRate(): number {
    if (this.death === 0) {
      return this.kill;
    }

    return this.kill / this.death;
  }
}