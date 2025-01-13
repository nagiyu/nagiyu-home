import axios, { AxiosResponse } from 'axios';
import { IGetKillRatesResponse } from '@splatoon3Tracker/interfaces/IGetKillRatesResponse';
import { ResultModel } from '@splatoon3Tracker/models/ResultModel';

export default class KillRateUtil {
  public static async GetKillRates(): Promise<IGetKillRatesResponse | null> {
    var response = await axios.get<any, AxiosResponse<IGetKillRatesResponse, any>>('/api/splatoon3/kill-rate');

    if (response.status === 200) {
      return response.data;
    }

    return null;
  }

  public static async AddKillRate(killRate: ResultModel): Promise<string> {
    var response = await axios.post<any, AxiosResponse<string, any>>('/api/splatoon3/kill-rate', killRate);

    if (response.status === 200) {
      return response.data;
    }

    return '';
  }

  public static async UpdateKillRate(id: string, killRate: ResultModel): Promise<boolean> {
    var response = await axios.put<any, AxiosResponse<string, any>>(`/api/splatoon3/kill-rate/${id}`, killRate);

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
}