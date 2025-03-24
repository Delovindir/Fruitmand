import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/fruit": "http://localhost:32647",
      "/mand": "http://localhost:32647"
    }
  }
})
