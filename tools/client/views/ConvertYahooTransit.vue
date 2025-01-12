<template>
  <b-tabs v-model="activeTab" position="is-centered" type="is-boxed" expanded>
    <b-tab-item label="Before">
      <b-field>
        <b-input v-model="input" type="textarea"></b-input>
      </b-field>
      <b-field position="is-centered" class="buttons">
        <b-button type="is-info" @click="read">Read</b-button>
        <b-button type="is-primary" @click="convert">Convert</b-button>
      </b-field>
    </b-tab-item>

    <b-tab-item label="After">
      <b-field>
        <b-input v-model="output" type="textarea"></b-input>
      </b-field>
      <b-field position="is-centered">
        <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
        <div></div>
        <b-button type="is-success" @click="copy">Copy</b-button>
      </b-field>
    </b-tab-item>
  </b-tabs>
</template>

<script lang="ts">
import { Component, Vue, toNative } from "vue-facing-decorator";

@Component
class ConvertYahooTransit extends Vue {
  public activeTab: number = 0;
  public input: string = "";
  public output: string = "";

  public convert() {
    this.output = this.input
      .slice(this.input.indexOf('■'), this.input.indexOf('(運賃内訳)'))
      .replace('---\n', '');

    this.activeTab = 1;
  }

  /**
   * 読み込み
   */
  public async read() {
    this.input = await navigator.clipboard.readText();
  }

  /**
   * コピー
   */
  public async copy() {
    await navigator.clipboard.writeText(this.output);
  }
}

export default toNative(ConvertYahooTransit);
</script>
