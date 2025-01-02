<template>
  <h1>利用規約</h1>

  <p>この利用規約（以下，「本規約」といいます。）は，音和 柚羽（以下，「なぎゆー」といいます。）がこのウェブサイト上で提供するサービス（以下，「本サービス」といいます。）の利用条件を定めるものです。登録ユーザーの皆さま（以下，「ユーザー」といいます。）には，本規約に従って，本サービスをご利用いただきます。</p>

  <template v-for="(item, index) in termItems" :key="index">
    <h2>第{{ index + 1 }}条 ({{ item.title }})</h2>
    <ol>
      <template v-for="content in item.contents">
        <li>
          {{ content.mainContent }}
          <template v-if="content.subItems">
            <ol>
              <template v-for="subItem in content.subItems">
                <li>
                  {{ subItem }}
                </li>
              </template>
            </ol>
          </template>
        </li>
      </template>
    </ol>
  </template>

  <p>以上</p>
</template>

<script lang="ts">
import axios, { AxiosResponse } from "axios";
import { Component, Vue, toNative } from "vue-facing-decorator";

@Component
class Terms extends Vue {
  public termItems: ITerm[] = [];

  public async created() {
    var response = await axios.get<any, AxiosResponse<ITerm[], any>>('/api/terms');

    if (response.status !== 200) {
      return;
    }

    this.termItems = response.data;
  }
}

export default toNative(Terms);
</script>