import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { resolve } from 'path';
import { copyFileSync, mkdirSync, readdirSync, statSync } from 'fs';

function copyDirRecursive(src: string, dest: string) {
  mkdirSync(dest, { recursive: true });
  for (const entry of readdirSync(src)) {
    const srcPath = resolve(src, entry);
    const destPath = resolve(dest, entry);
    if (statSync(srcPath).isDirectory()) {
      copyDirRecursive(srcPath, destPath);
    } else {
      copyFileSync(srcPath, destPath);
    }
  }
}

function copyAssetsPlugin() {
  return {
    name: 'copy-assets',
    buildStart() {
      // Copy CSV files
      try {
        const csvSrc = resolve(__dirname, '../documents/English');
        const csvDest = resolve(__dirname, 'public/data');
        mkdirSync(csvDest, { recursive: true });
        for (const file of readdirSync(csvSrc)) {
          if (file.endsWith('.csv')) {
            copyFileSync(resolve(csvSrc, file), resolve(csvDest, file));
          }
        }
      } catch (e) {
        console.warn('CSV copy skipped:', e);
      }

      // Copy image files
      try {
        const imgSrc = resolve(__dirname, '../documents/image');
        const imgDest = resolve(__dirname, 'public/image');
        copyDirRecursive(imgSrc, imgDest);
      } catch (e) {
        console.warn('Image copy skipped:', e);
      }
    },
  };
}

export default defineConfig({
  plugins: [react(), copyAssetsPlugin()],
  base: '/world_traveling_logger2/',
});
