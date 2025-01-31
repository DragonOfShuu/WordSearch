/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,js,ts}"],
  darkMode: 'selector',
  theme: {
    extend: {
      spacing: {
        "nav-margin": '3.8rem'
      },
      colors: {
        primary: {
          50: "--color-primary-50",
          100: "--color-primary-100",
          200: "--color-primary-200",
          300: "--color-primary-300",
          400: "--color-primary-400",
          500: "--color-primary-500",
          600: "--color-primary-600",
          700: "--color-primary-700",
          800: "--color-primary-800",
          900: "--color-primary-900",
          950: "--color-primary-950",
        },
        secondary: {
          50: "--color-secondary-50",
          100: "--color-secondary-100",
          200: "--color-secondary-200",
          300: "--color-secondary-300",
          400: "--color-secondary-400",
          500: "--color-secondary-500",
          600: "--color-secondary-600",
          700: "--color-secondary-700",
          800: "--color-secondary-800",
          900: "--color-secondary-900",
          950: "--color-secondary-950",
        }
      }
    },
  },
  plugins: [],
}

