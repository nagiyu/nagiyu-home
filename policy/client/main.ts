import { createApp } from 'vue';
import PrivacyPolicy from './PrivacyPolicy.vue';
import Terms from './Terms.vue';

// Buefy
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'

// Vueアプリを作成
const privacyPolicyApp = createApp(PrivacyPolicy);
const termsApp = createApp(Terms);

// Buefyを使用
privacyPolicyApp.use(Buefy as any);
termsApp.use(Buefy as any);

// アプリをマウント
privacyPolicyApp.mount('#vue-privacy-policy');
termsApp.mount('#vue-terms');
