<template>
  <b-tabs v-model="activeTab" position="is-centered" type="is-boxed" expanded>
    <b-tab-item label="変換元">
      <b-select v-model="baseType">
        <option v-for="(type, index) in TYPE_LIST" :key="index" :value="type.type">
          {{ type.label }}
        </option>
      </b-select>

      <template v-if="baseType === WEB_TYPE">
        <b-field label="URL" horizontal>
          <b-input v-model="web.url" type="url" />
        </b-field>

        <b-field label="ページタイトル" horizontal>
          <b-input v-model="web.page_title" />
        </b-field>

        <b-field label="サイトタイトル" horizontal>
          <b-input v-model="web.cite_title" />
        </b-field>

        <b-field label="閲覧日" horizontal>
          <b-input v-model="web.read" type="date" />
        </b-field>
      </template>

      <template v-else-if="baseType === BOOK_TYPE">
        <b-field label="タイトル" horizontal>
          <b-input v-model="book.title" />
        </b-field>

        <b-field label="著者" horizontal>
          <b-taginput v-model="book.authors">
          </b-taginput>
        </b-field>

        <b-field label="出版日" horizontal>
          <b-input v-model="book.created" type="date" />
        </b-field>

        <b-field label="閲覧日" horizontal>
          <b-input v-model="book.read" type="date" />
        </b-field>
      </template>

      <template v-else-if="baseType === JOURNAL_TYPE">
        <b-field label="タイトル" horizontal>
          <b-input v-model="journal.title" />
        </b-field>

        <b-field label="著者" horizontal>
          <b-taginput v-model="journal.authors">
          </b-taginput>
        </b-field>

        <b-field label="出版日" horizontal>
          <b-input v-model="journal.created" type="date" />
        </b-field>

        <b-field label="閲覧日" horizontal>
          <b-input v-model="journal.read" type="date" />
        </b-field>
      </template>

      <b-field position="is-centered" class="buttons">
        <!-- TODO: 要素が1つだとセンタリングされないので暫定追加 -->
        <div></div>
        <b-button v-if="baseType === WEB_TYPE" type="is-primary" @click="urlConvert">URL変換</b-button>
        <b-button type="is-info" @click="clear">Clear</b-button>
      </b-field>
    </b-tab-item>

    <b-tab-item label="変換先">
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
import dayjs from "dayjs";

type BaseType = "web" | "book" | "journal";

interface IType {
  type: BaseType;
  label: string;
}

interface WebStyle {
  page_title: string;
  cite_title: string;
  url: string;
  authors?: Array<string>;
  created?: string;
  read: string;
}

interface BookStyle {
  title: string;
  authors: Array<string>;
  state?: string;
  created: string;
  startPage?: number;
  endPage?: number;
  read: string;
}

interface JournalStyle {
  title: string;
  authors: Array<string>;
  state?: string;
  created: string;
  startPage?: number;
  endPage?: number;
  read: string;
}

@Component
class CreateBibliography extends Vue {
  public readonly WEB_TYPE: BaseType = "web";
  public readonly BOOK_TYPE: BaseType = "book";
  public readonly JOURNAL_TYPE: BaseType = "journal";

  public readonly TYPE_LIST: IType[] = [
    { type: this.WEB_TYPE, label: "Webページ" },
    { type: this.BOOK_TYPE, label: "書籍" },
    { type: this.JOURNAL_TYPE, label: "論文" },
  ];

  public activeTab: number = 0;

  public baseType: BaseType = this.WEB_TYPE;

  public web: WebStyle = {
    page_title: "",
    cite_title: "",
    url: "",
    authors: [],
    created: dayjs('0000-00-00').format('YYYY-MM-DD'),
    read: dayjs().format('YYYY-MM-DD'),
  };

  public book: BookStyle = {
    title: "",
    authors: [],
    state: "",
    created: dayjs('0000-00-00').format('YYYY-MM-DD'),
    startPage: 0,
    endPage: 0,
    read: dayjs().format('YYYY-MM-DD'),
  };

  public journal: JournalStyle = {
    title: "",
    authors: [],
    state: "",
    created: dayjs('0000-00-00').format('YYYY-MM-DD'),
    startPage: 0,
    endPage: 0,
    read: dayjs().format('YYYY-MM-DD'),
  };

  public urlConvert(): void {
    this.web.url = decodeURI(this.web.url);
  }

  public get output(): string {
    switch (this.baseType) {
      case this.WEB_TYPE:
        return [
          `『${this.web.page_title}』`,
          this.web.cite_title,
          this.web.url,
          this.web.read,
        ].join(', ');

      case this.BOOK_TYPE:
        return [
          `『${this.book.title}』`,
          this.book.authors.map((author) => {
            return author;
          }),
          this.book.created,
          this.book.read,
        ].join(', ');

      case this.JOURNAL_TYPE:
        return [
          `『${this.journal.title}』`,
          this.journal.authors.map((author) => {
            return author;
          }),
          this.journal.created,
          this.journal.read,
        ].join(', ');

      default:
        return '';
    }
  }

  public clear(): void {
    this.web = {
      page_title: "",
      cite_title: "",
      url: "",
      authors: [],
      created: dayjs('0000-00-00').format('YYYY-MM-DD'),
      read: dayjs().format('YYYY-MM-DD'),
    };

    this.book = {
      title: "",
      authors: [],
      state: "",
      created: dayjs('0000-00-00').format('YYYY-MM-DD'),
      startPage: 0,
      endPage: 0,
      read: dayjs().format('YYYY-MM-DD'),
    };

    this.journal = {
      title: "",
      authors: [],
      state: "",
      created: dayjs('0000-00-00').format('YYYY-MM-DD'),
      startPage: 0,
      endPage: 0,
      read: dayjs().format('YYYY-MM-DD'),
    };
  }
}

export default toNative(CreateBibliography);
</script>
