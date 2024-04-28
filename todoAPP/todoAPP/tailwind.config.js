/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Pages/*.cshtml", "./Pages/Shared/*.cshtml", "./src/js/components/*.vue"],
  theme: {
    extend: {},
  },
  plugins: [require("@tailwindcss/typography"), require("daisyui")],
  daisyui: {
    themes: ["cupcake", "light"],
  },
};
