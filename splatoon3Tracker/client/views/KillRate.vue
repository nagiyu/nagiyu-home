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
    <b-table-column v-slot="props" field="id" label="ID" :visible="false">
      {{ props.row.id }}
    </b-table-column>

    <b-table-column v-slot="props" field="battle" label="バトル">
      {{ props.row.battle }}
    </b-table-column>

    <b-table-column v-slot="props" field="rule" label="ルール">
      {{ props.row.rule }}
    </b-table-column>

    <b-table-column v-slot="props" field="weapon" label="ブキ">
      {{ props.row.weapon }}
    </b-table-column>

    <b-table-column v-slot="props" field="result" label="結果">
      {{ props.row.result }}
    </b-table-column>

    <b-table-column v-slot="props" field="killRate" label="キルレ">
      {{ props.row.killRate }}
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
import { IGetKillRatesResponse } from "@splatoon3Tracker/interfaces/IGetKillRatesResponse";
import { ResultModel } from "@splatoon3Tracker/models/ResultModel";
import KillRateDetailModal from "@splatoon3Tracker/components/modals/KillRateDetailModal.vue";

@Component({
  components: {
    KillRateDetailModal
  }
})
class KillRate extends Vue {
  public killRates: ResultModel[] = [];

  public isNew: boolean = true;

  public killRate: ResultModel = new ResultModel();

  public isDetailModalActive: boolean = false;

  public async created(): Promise<void> {
    await this.FetchKillRates();
  }

  public OpenDetailModal(result?: ResultModel): void {
    if (result) {
      this.isNew = false;
      this.killRate = result;
    } else {
      this.isNew = true;
      this.killRate = new ResultModel();
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

  private async FetchKillRates(): Promise<void> {
    const response: IGetKillRatesResponse | null = await KillRateUtil.GetKillRates();
    this.killRates = [];

    if (response) {
      response.killRates.forEach(killRate => {
        var result = new ResultModel();

        result.id = killRate.id;
        result.recordType = killRate.recordType;
        result.battle = killRate.battle;
        result.rule = killRate.rule;
        result.weapon = killRate.weapon;
        result.result = killRate.result;
        result.kill = killRate.kill;
        result.assist = killRate.assist;
        result.death = killRate.death;
        result.special = killRate.special;
        result.date = dayjs(killRate.date).format('YYYY-MM-DD HH:mm');
        result.matchTime = killRate.matchTime;

        this.killRates.push(result);
      });
    }
  }
}

export default toNative(KillRate);
</script>
