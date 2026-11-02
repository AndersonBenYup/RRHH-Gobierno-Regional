/** SC-011: Tailwind convive con Bootstrap.
 *  - prefix "tw-": las utilidades no chocan con las clases de Bootstrap (.border, .shadow, .hidden...).
 *  - preflight desactivado: no resetea los estilos base que usan las vistas actuales.
 *  - darkMode por el atributo data-bs-theme que ya usa el sitio. */
module.exports = {
  content: ['./Views/**/*.cshtml', './wwwroot/js/site.js'],
  prefix: 'tw-',
  darkMode: ['selector', '[data-bs-theme="dark"]'],
  corePlugins: { preflight: false },
  theme: {
    extend: {
      colors: {
        petroleo: { 50: '#F1F6F7', 100: '#E1EDF0', 200: '#BFD8DE', 400: '#5C9AA9', 600: '#1B7186', 700: '#145E70', 800: '#0F4C5C', 900: '#0B3440', 950: '#082630' },
        piedra: { 50: '#FAF9F7', 100: '#F5F4F1', 200: '#E9E6E0', 300: '#DAD6CE', 500: '#7C8087', 600: '#5E646B', 800: '#2E3338', 900: '#1F2328' },
        ocre: { 600: '#9A6516', 100: '#F6ECDB' }
      },
      fontFamily: { sans: ['"IBM Plex Sans"', 'system-ui', 'sans-serif'] }
    }
  }
};
