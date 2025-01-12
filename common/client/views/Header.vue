<template>
  <b-navbar>
    <template #brand></template>

    <template #start>
      <template v-for="item in leftMenuItems">
        <b-navbar-item :href="item.link">
          <b-button :type="item.type">{{ item.label }}</b-button>
        </b-navbar-item>
      </template>
    </template>

    <template #end>
      <template v-if="UserName">
        <b-navbar-item tag="div">
          Welcome, {{ UserName }}!!!
        </b-navbar-item>
      </template>
      <template v-else>
        <template v-for="item in RightMenuItems">
          <b-navbar-item :href="item.link">
            <b-button :type="item.type">{{ item.label }}</b-button>
          </b-navbar-item>
        </template>
      </template>
    </template>
  </b-navbar>
</template>

<script lang="ts">
import AuthUtil from "@auth/utils/AuthUtil";
import { Component, Vue, toNative } from "vue-facing-decorator";

interface MenuItem {
  label: string;
  type: string;
  link: string;
}

@Component
class Header extends Vue {
  /**
   * 左側のメニューボタン
   */
  public leftMenuItems: MenuItem[] = [
    { label: "ホーム", type: "is-link", link: "/" },
  ];

  /**
   * ユーザー情報
   */
  private user: IUserAuthBase | null = null;

  /**
   * 右側のメニューボタン
   */
  public get RightMenuItems(): MenuItem[] {
    if (this.user === null) {
      return [
        { label: "Login", type: "is-light", link: "/Account/Login" },
        { label: "Register", type: "is-primary", link: "/Account/Register" },
      ];
    } else if (this.user.userName === '') {
      return [
        { label: "Register", type: "is-primary", link: "/Account/Register" },
      ];
    } else {
      return [];
    }
  }

  /**
   * ユーザー名
   */
  public get UserName(): string {
    return this.user?.userName ?? "";
  }

  /**
   * コンポーネントが作成されたときに呼び出されるライフサイクルフック
   */
  public async created() {
    this.user = await AuthUtil.GetUser<IUserAuthBase>();
  }
}

export default toNative(Header)
</script>
