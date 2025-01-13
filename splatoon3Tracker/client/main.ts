import { createApp } from 'vue';
import KillRate from './views/KillRate.vue';

// Buefy
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

// Vueアプリを作成
const killRateApp = createApp(KillRate);

// Buefyを使用
killRateApp.use(Buefy as any);

// アプリをマウント
killRateApp.mount('#vue-kill-rate');
