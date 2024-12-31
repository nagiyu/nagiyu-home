<template>
  <h1>プライバシーポリシー</h1>

  <p>音和 柚羽（以下，「なぎゆー」といいます。）は，本ウェブサイト上で提供するサービス（以下,「本サービス」といいます。）における，ユーザーの個人情報の取扱いについて，以下のとおりプライバシーポリシー（以下，「本ポリシー」といいます。）を定めます。</p>

  <template v-for="(item, index) in policyItems" :key="index">
    <h2>第{{ index + 1 }}条 ({{ item.title }})</h2>
    <ol>
      <template v-for="content in item.contents">
        <li>
          {{ content.mainContent }}
          <template v-if="content.subContents">
            <ol>
              <template v-for="subContent in content.subContents">
                <li>
                  {{ subContent.subContent }}
                  <template v-if="subContent.subItems">
                    <ol>
                      <template v-for="subItem in subContent.subItems">
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
          <template v-if="content.link">
            <a :href="content.link" target="_blank" rel="noreferrer">リンク</a>
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
class PrivacyPolicy extends Vue {
  public policyItems: IPrivacyPolicy[] = [];

  public async created() {
    var response = await axios.get<any, AxiosResponse<IPrivacyPolicy[], any>>('/api/privacy-policy');

    if (response.status !== 200) {
      return;
    }

    this.policyItems = response.data;
  }
}

export default toNative(PrivacyPolicy);
</script>
