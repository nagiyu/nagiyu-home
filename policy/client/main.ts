import { createApp } from 'vue';
import PrivacyPolicy from './PrivacyPolicy.vue';

// Buefy
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

// Vueアプリを作成
const privacyPolicyApp = createApp(PrivacyPolicy);

// Buefyを使用
privacyPolicyApp.use(Buefy as any);

// アプリをマウント
privacyPolicyApp.mount('#vue-privacy-policy');
