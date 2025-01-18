import axios, { AxiosResponse } from 'axios';
import dayjs from 'dayjs';
import { IAddKillRateResponse } from '@splatoon3Tracker/interfaces/Responses/IAddKillRateResponse';
import { IGetKillRatesResponse } from '@splatoon3Tracker/interfaces/Responses/IGetKillRatesResponse';
import { IKillRate } from '@splatoon3Tracker/interfaces/IKillRate';
import { IKillRateRequest } from '@splatoon3Tracker/interfaces/Requests/IKillRateRequest';
import { IKillRateResponse } from '@splatoon3Tracker/interfaces/Responses/IKillRateResponse';

export default class KillRateUtil {
  public static async GetKillRates(): Promise<IGetKillRatesResponse | null> {
    var response = await axios.get<any, AxiosResponse<IGetKillRatesResponse, any>>('/api/splatoon3/kill-rate');

    if (response.status === 200) {
      return response.data;
    }

    return null;
  }

  public static async AddKillRate(request: IKillRateRequest): Promise<IAddKillRateResponse | null> {
    var response = await axios.post<any, AxiosResponse<IAddKillRateResponse, any>>('/api/splatoon3/kill-rate', request);

    if (response.status === 200) {
      return response.data;
    }

    return null;
  }

  public static async UpdateKillRate(id: string, request: IKillRateRequest): Promise<boolean> {
    var response = await axios.put<any, AxiosResponse<string, any>>(`/api/splatoon3/kill-rate/${id}`, request);

    if (response.status === 200) {
      return true;
    }

    return false;
  }

  public static async DeleteKillRate(id: string): Promise<boolean> {
    var response = await axios.delete<any, AxiosResponse<string, any>>(`/api/splatoon3/kill-rate/${id}`);

    if (response.status === 200) {
      return true;
    }

    return false;
  }

  /**
   * Create IKillRate
   * @returns IKillRate
   */
  public static CreateKillRate(): IKillRate {
    return {
      id: '',
      battle: '',
      rule: '',
      weapon: '',
      result: '',
      kill: 0,
      assist: 0,
      death: 0,
      special: 0,
      date: dayjs().format('YYYY-MM-DD HH:mm'),
      matchTime: 0
    }
  }

  /**
   * Convert IKillRateResponse to IKillRate
   * @param request IKillRateResponse
   * @returns IKillRate
   */
  public static ConvertToKillRate(request: IKillRateResponse): IKillRate {
    return {
      id: request.id,
      battle: request.battle,
      rule: request.rule,
      weapon: request.weapon,
      result: request.result,
      kill: request.kill,
      assist: request.assist,
      death: request.death,
      special: request.special,
      date: request.date,
      matchTime: request.matchTime
    }
  }

  /**
   * Convert IKillRate to IKillRateRequest
   * @param killRate IKillRate
   * @returns IKillRateRequest
   */
  public static ConvertToKillRateRequest(killRate: IKillRate): IKillRateRequest {
    return {
      battle: killRate.battle,
      rule: killRate.rule,
      weapon: killRate.weapon,
      result: killRate.result,
      kill: killRate.kill,
      assist: killRate.assist,
      death: killRate.death,
      special: killRate.special,
      date: killRate.date,
      matchTime: killRate.matchTime
    }
  }
}