<template>
  <b-modal v-model="isDetailModalActive" has-modal-card :can-cancel="false"  >
    <div class="modal-card">
      <header class="modal-card-head">
        <p class="modal-card-title">
          {{ isNew ? "新規登録" : "編集" }}
        </p>
      </header>

      <section class="modal-card-body">
        <b-field label="バトル" horizontal>
          <b-select v-model="result.battle" expanded>
            <option v-for="(type, index) in BATTLE_TYPE_LIST" :key="index" :value="type.type">
              {{ type.label }}
            </option>
          </b-select>
        </b-field>

        <b-field label="ルール" horizontal>
          <b-select v-model="result.rule" expanded>
            <option v-for="(type, index) in RULE_TYPE_LIST" :key="index" :value="type.type">
              {{ type.label }}
            </option>
          </b-select>
        </b-field>

        <b-field label="ブキ" horizontal>
          <b-autocomplete 
            v-model="selectedWeapon"
            :data="filteredWeapons"
            field="label"
            :open-on-focus="true"
            @select="SelectWeapon"
          />
        </b-field>

        <b-field label="結果" horizontal>
          <b-select v-model="result.result" expanded>
            <option v-for="(type, index) in ['Win', 'Lose']" :key="index" :value="type">
              {{ type }}
            </option>
          </b-select>
        </b-field>

        <b-field label="キル" horizontal>
          <b-input v-model="result.kill" type="number" />
        </b-field>

        <b-field label="アシスト" horizontal>
          <b-input v-model="result.assist" type="number" />
        </b-field>

        <b-field label="デス" horizontal>
          <b-input v-model="result.death" type="number" />
        </b-field>

        <b-field label="スペシャル" horizontal>
          <b-input v-model="result.special" type="number" />
        </b-field>

        <b-field label="日付" horizontal>
          <b-input v-model="result.date" type="datetime-local" />
        </b-field>

        <b-field label="試合時間" horizontal>
          <b-input v-model="minutes" type="number" />
          <span> : </span>
          <b-input v-model="seconds" type="number" />
        </b-field>
      </section>

      <footer class="modal-card-foot">
        <b-button type="is-success" @click="Submit">
          {{ isNew ? "登録" : "更新" }}
        </b-button>
        <b-button type="is-primary" @click="CloseDetailModal">閉じる</b-button>
      </footer>
    </div>
  </b-modal>
</template>

<script lang="ts">
import { Component, Emit, Prop, Vue, Watch, toNative } from "vue-facing-decorator";
import dayjs from "dayjs";
import TimeUtils from "@common/utils/TimeUtils";
import ISplatoon3ConstType from "@splatoon3Tracker/interfaces/ISplatoon3ConstType";
import KillRateUtil from "@splatoon3Tracker/utils/KillRateUtil";
import Splatoon3Utils from "@splatoon3Tracker/utils/Splatoon3Utils";
import { IKillRate } from "@splatoon3Tracker/interfaces/IKillRate";

@Component
class KillRateDetailModal extends Vue {
  public readonly BATTLE_TYPE_LIST: ISplatoon3ConstType[] = Splatoon3Utils.GetBattleTypes();
  public readonly RULE_TYPE_LIST: ISplatoon3ConstType[] = Splatoon3Utils.GetRuleTypes();
  public readonly WEAPON_LIST: ISplatoon3ConstType[] = Splatoon3Utils.GetWeapons();

  // #region Fields
  public minutes: string = "0";
  public seconds: string = "0";
  public selectedWeapon: string = '';
  // #endregion

  @Prop({ 
    type: Boolean,
    required: true,
    default: false
  })
  public isDetailModalActive: boolean = false;

  @Prop({
    type: Boolean,
    required: true,
    default: true
  })
  public isNew: boolean = true;

  @Prop({
    type: Object,
    required: true,
    default: KillRateUtil.CreateKillRate()
  })
  public result!: IKillRate;

  public get filteredWeapons(): ISplatoon3ConstType[] {
    return this.WEAPON_LIST.filter(w => w.label.indexOf(this.selectedWeapon) >= 0);
  }

  @Emit('closeDetailModal')
  public async CloseDetailModal(): Promise<void> {
    return;
  }

  @Watch('isDetailModalActive')
  public OnIsDetailModalActiveChanged(value: boolean): void {
    if (value) {
      const times = TimeUtils.GetMinutesAndSeconds(this.result.matchTime);
      this.minutes = times.minutes.toString();
      this.seconds = times.seconds.toString();
      this.selectedWeapon = this.WEAPON_LIST.find(w => w.type === this.result.weapon)?.label || '';
    }
  }

  public SelectWeapon(weapon: ISplatoon3ConstType): void {
    this.selectedWeapon = weapon.label;
    this.result.weapon = weapon.type;
  }

  public async Submit(): Promise<void> {
    this.result.date = dayjs(this.result.date).format('YYYY-MM-DD HH:mm');
    this.result.matchTime = TimeUtils.GetMatchTime(parseInt(this.minutes), parseInt(this.seconds));

    var request = KillRateUtil.ConvertToKillRateRequest(this.result);

    if (this.isNew) {
      var response = await KillRateUtil.AddKillRate(request);

      if (response) {
        this.CloseDetailModal();
      }
    } else {
      if (!this.result.id) {
        return;
      }

      var result = await KillRateUtil.UpdateKillRate(this.result.id, request);

      if (result) {
        this.CloseDetailModal();
      }
    }
  }
}

export default toNative(KillRateDetailModal);
</script>
