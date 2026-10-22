import tailwindcss from '@tailwindcss/vite'
import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    // El navegador solo habla con Vite (5173) y Vite reenvía /api al backend,
    // así ambos comparten origen y el backend no necesita CORS.
    proxy: {
      '/api': 'http://localhost:5286',
    },
  },
})
