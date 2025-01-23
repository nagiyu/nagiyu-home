import { createApp } from 'vue';
import Header from './views/Header.vue';
import Footer from './views/Footer.vue';
import Common from './views/Common.vue';

// Buefy
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

// Vueアプリを作成
const headerApp = createApp(Header);
const footerApp = createApp(Footer);
const commonApp = createApp(Common);

// Buefyを使用
headerApp.use(Buefy as any);
footerApp.use(Buefy as any);
commonApp.use(Buefy as any);

// アプリをマウント
headerApp.mount('#vue-header');
footerApp.mount('#vue-footer');
commonApp.mount('#vue-common');
