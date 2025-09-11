/** @type {import('tailwindcss').Config} */
import withFlowbiteReact from 'flowbite-react/plugin/nextjs';
module.exports = {
  content: [
    "./app/**/*.{js,ts,jsx,tsx}",
    "./components/**/*.{js,ts,jsx,tsx}",
    "./node_modules/flowbite-react/**/*.js",
    "./node_modules/flowbite/**/*.js"
  ],
  theme: {
    extend: {},
  },
  plugins: [
    withFlowbiteReact
  ],
};