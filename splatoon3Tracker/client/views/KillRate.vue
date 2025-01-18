<template>
  <KillRateDetailModal 
    :isDetailModalActive="isDetailModalActive"
    :isNew="isNew"
    :result="killRate"
    @closeDetailModal="CloseDetailModal"
  />

  <b-field class="buttons">
    <b-button type="is-primary" @click="OpenDetailModal()">新規作成</b-button>
  </b-field>

  <b-table :data="killRates">
    <b-table-column v-slot="props" field="battle" label="バトル">
      {{ GetBattleLabel(props.row.battle) }}
    </b-table-column>

    <b-table-column v-slot="props" field="rule" label="ルール">
      {{ GetRuleLabel(props.row.rule) }}
    </b-table-column>

    <b-table-column v-slot="props" field="weapon" label="ブキ">
      {{ GetWeaponLabel(props.row.weapon) }}
    </b-table-column>

    <b-table-column v-slot="props" field="result" label="結果">
      {{ props.row.result }}
    </b-table-column>

    <b-table-column v-slot="props" field="killRate" label="キルレ">
      {{ CalcKillRate(props.row.kill, props.row.death) }}
    </b-table-column>

    <b-table-column v-slot="props" field="matchTime" label="試合時間">
      {{ props.row.matchTime }}
    </b-table-column>

    <b-table-column v-slot="props" field="date" label="日付">
      {{ props.row.date }}
    </b-table-column>

    <b-table-column v-slot="props" label="操作">
      <b-field class="buttons">
        <b-button type="is-info" @click="OpenDetailModal(props.row)">
          <i class="fa-solid fa-pen-to-square"></i>
        </b-button>
        <b-button type="is-danger" @click="DeleteConfirm(props.row.id)">
          <i class="fa-solid fa-trash"></i>
        </b-button>
      </b-field>
    </b-table-column>
  </b-table>
</template>

<script lang="ts">
import { Component, Vue, toNative } from "vue-facing-decorator";
import dayjs from "dayjs";
import KillRateUtil from "@splatoon3Tracker/utils/KillRateUtil";
import KillRateDetailModal from "@splatoon3Tracker/components/modals/KillRateDetailModal.vue";
import Splatoon3Utils from "@splatoon3Tracker/utils/Splatoon3Utils";
import { IGetKillRatesResponse } from "@splatoon3Tracker/interfaces/Responses/IGetKillRatesResponse";
import { IKillRate } from "@splatoon3Tracker/interfaces/IKillRate";

@Component({
  components: {
    KillRateDetailModal
  }
})
class KillRate extends Vue {
  public killRates: IKillRate[] = [];

  public isNew: boolean = true;

  public killRate: IKillRate = KillRateUtil.CreateKillRate();

  public isDetailModalActive: boolean = false;

  public async created(): Promise<void> {
    await this.FetchKillRates();
  }

  public OpenDetailModal(result?: IKillRate): void {
    if (result) {
      this.isNew = false;
      this.killRate = result;
    } else {
      this.isNew = true;
      this.killRate = KillRateUtil.CreateKillRate();
    }

    this.isDetailModalActive = true;
  }

  public async CloseDetailModal(): Promise<void> {
    await this.FetchKillRates();
    this.isDetailModalActive = false;
  }

  public async DeleteConfirm(id: string): Promise<void> {
    // @ts-ignore
    this.$buefy.dialog.confirm({
      message: '削除しますか？',
      onConfirm: async () => {
        await KillRateUtil.DeleteKillRate(id);
        await this.FetchKillRates();
      }
    })
  }

  public GetBattleLabel(battleType: string): string {
    var battle = Splatoon3Utils.GetBattleTypes().find(b => b.type === battleType);
    return battle ? battle.label : '';
  }

  public GetRuleLabel(ruleType: string): string {
    var rule = Splatoon3Utils.GetRuleTypes().find(b => b.type === ruleType);
    return rule ? rule.label : '';
  }

  public GetWeaponLabel(weaponType: string): string {
    var weapon = Splatoon3Utils.GetWeapons().find(b => b.type === weaponType);
    return weapon ? weapon.label : '';
  }

  public CalcKillRate(kill: number, death: number): string {
    if (death === 0) {
      return kill.toFixed(2);
    }

    return (kill / death).toFixed(2);
  }

  private async FetchKillRates(): Promise<void> {
    const response: IGetKillRatesResponse | null = await KillRateUtil.GetKillRates();
    this.killRates = [];

    if (response) {
      response.killRates.forEach(killRateResponse => {
        var killRate = KillRateUtil.ConvertToKillRate(killRateResponse);
        this.killRates.push(killRate);
      });
    }
  }
}

export default toNative(KillRate);
</script>
