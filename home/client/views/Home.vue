<template>
  <div class="buttons">
    <template v-for="item in items">
      <b-button tag="a" :href="item.link" style="height: auto;">
        <h4>{{ item.title }}</h4>
        <p>{{ item.description }}</p>
      </b-button>
    </template>
  </div>
</template>

<script lang="ts">
import { Component, Vue, toNative } from "vue-facing-decorator";

interface IItem {
  title: string;
  description: string;
  link: string;
}

@Component
class Home extends Vue {
  public items: IItem[] = [
    {
      title: "乗り換え変換",
      description: "Yahooの乗り換えをコピペしやすい形に変換します。",
      link: "/tools/convert-yahoo-transit",
    },
    {
      title: "参考文献つくーる",
      description: "参考文献を作ります。",
      link: "/tools/create-bibliography",
    },
    {
      title: "キルレ記録",
      description: "スプラトゥーン3のキルレを記録します。",
      link: "/splatoon3/kill-rate",
    },
  ];
}

export default toNative(Home);
</script>
