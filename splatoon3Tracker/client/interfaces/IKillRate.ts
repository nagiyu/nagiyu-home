export interface IKillRate {
  id: string;
  recordType: string;
  battle: string;
  rule: string;
  weapon: string;
  result: string;
  kill: number;
  assist: number;
  death: number;
  special: number;
  date: Date;
  matchTime: number;
}