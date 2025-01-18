export interface IKillRateResponse {
  id: string;
  battle: string;
  rule: string;
  weapon: string;
  result: string;
  kill: number;
  assist: number;
  death: number;
  special: number;
  date: string;
  matchTime: number;
}