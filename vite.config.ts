import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import path, { resolve } from 'path';

// 各機能の設定
const features = [
  {
    name: 'all',
    root: '', // TODO: とりあえず空
    entry: '', // TODO: とりあえず空
  },
  {
    name: 'common',
    root: 'common/client',
    entry: 'common/client/main.ts',
  },
  {
    name: 'home',
    root: 'home/client',
    entry: 'home/client/main.ts',
  },
  {
    name: 'policy',
    root: 'policy/client',
    entry: 'policy/client/main.ts',
  },
  {
    name: 'tools',
    root: 'tools/client',
    entry: 'tools/client/main.ts',
  },
  {
    name: 'splatoon3Tracker',
    root: 'splatoon3Tracker/client',
    entry: 'splatoon3Tracker/client/main.ts',
  },
];

export default defineConfig(({ command }) => {
  const isDev = command === 'serve';
  const target = process.env.TARGET;
  const featureConfig = features.find((f) => f.name === target);

  if (!featureConfig) {
    throw new Error(`Feature not found: ${target}`);
  }

  return {
    plugins: [vue()],
    root: isDev ? resolve(__dirname, featureConfig.root) : undefined, // デバッグ時のルートをmockにする
    build: {
      rollupOptions: {
        input: isDev
          ? resolve(__dirname, featureConfig.entry)
          : Object.fromEntries(features.filter((feature) => { return feature.name !== 'all' }).map((feature) => [feature.name, resolve(__dirname, feature.entry)])),
        output: {
          entryFileNames: '[name].js', // 出力ファイル名を設定 (ハッシュなし)
          chunkFileNames: '[name]-chunk.js', // 共通チャンクファイルの名前
          assetFileNames: 'assets/[name].[ext]', // アセット（CSSなど）の名前
        },
      },
      outDir: 'home/web/wwwroot/js/dist', // ASP.NET MVCの`wwwroot/dist`に出力
      manifest: false, // マニフェストファイルも生成しない
      emptyOutDir: true, // ビルドディレクトリをクリーンアップ
    },
    resolve: {
      alias: {
        '@common': path.resolve(__dirname, 'common/client'),
        '@auth': path.resolve(__dirname, 'auth/client'),
        '@splatoon3Tracker': path.resolve(__dirname, 'splatoon3Tracker/client'),
      },
    },
    server: {
      port: 3000, // 開発用サーバーのポート番号
      proxy: {
        // ASP.NET MVCのAPIにプロキシ
        '/api': {
          target: 'https://dev.nagiyu.com',
          changeOrigin: true, // オリジンを変更（CORS対策）
          secure: false, // HTTPSを許可（ローカル開発用）
        },
      },
    },
  };
});
