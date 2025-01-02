import { createApp } from 'vue';
import Header from './Header.vue';
import Footer from './Footer.vue';

// Buefy
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

// Vueアプリを作成
const headerApp = createApp(Header);
const footerApp = createApp(Footer);

// Buefyを使用
headerApp.use(Buefy as any);
footerApp.use(Buefy as any);

// アプリをマウント
headerApp.mount('#vue-header');
footerApp.mount('#vue-footer');
