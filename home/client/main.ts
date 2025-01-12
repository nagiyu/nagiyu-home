import { createApp } from 'vue';
import Home from './views/Home.vue';

// Buefy
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

// Vueアプリを作成
const homeApp = createApp(Home);

// Buefyを使用
homeApp.use(Buefy as any);

// アプリをマウント
homeApp.mount('#vue-home');
