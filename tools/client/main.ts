import { createApp } from 'vue';
import ConvertYahooTransit from './views/ConvertYahooTransit.vue';
import CreateBibliography from './views/CreateBibliography.vue';

// Buefy
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

// Vueアプリを作成
const convertYahooTransitApp = createApp(ConvertYahooTransit);
const createBibliographyApp = createApp(CreateBibliography);

// Buefyを使用
convertYahooTransitApp.use(Buefy as any);
createBibliographyApp.use(Buefy as any);

// アプリをマウント
convertYahooTransitApp.mount('#vue-convert-yahoo-transit');
createBibliographyApp.mount('#vue-create-bibliography');
