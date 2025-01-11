import { createApp } from 'vue';
import ConvertYahooTransit from './views/ConvertYahooTransit.vue';

// Buefy
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

// Vueアプリを作成
const convertYahooTransitApp = createApp(ConvertYahooTransit);

// Buefyを使用
convertYahooTransitApp.use(Buefy as any);

// アプリをマウント
convertYahooTransitApp.mount('#vue-convert-yahoo-transit');
