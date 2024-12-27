<template>
  <div>
    <h1>プライバシーポリシー</h1>
    <ul>
      <li v-for="(item, index) in policyItems" :key="index">
        {{ item }}
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { Component, Vue, toNative } from "vue-facing-decorator";

@Component
class PrivacyPolicy extends Vue {
  public policyItems: string[] = [
    "ユーザーの個人情報は適切に管理します。",
    "第三者に情報を提供することはありません。",
    "サービス改善のため、データを活用します。",
    "法令に基づいて情報を取り扱います。",
  ];
}

export default toNative(PrivacyPolicy);
</script>

<style scoped>
h1 {
  font-size: 24px;
  color: #333;
}
ul {
  list-style-type: disc;
  padding-left: 20px;
}
li {
  margin-bottom: 8px;
  color: #555;
}
</style>
