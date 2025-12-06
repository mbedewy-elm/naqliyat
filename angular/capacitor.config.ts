import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'com.naqliyat.app',
  appName: 'Naqliyat',
  webDir: 'dist/Naqliyat/browser',
  android: {
    allowMixedContent: true
  }
};

export default config;
