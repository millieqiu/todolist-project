/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./Pages/*.cshtml", "./Pages/Shared/*.cshtml"],
  theme: {
    extend: {},
  },
  plugins: [require("@tailwindcss/typography"), require("daisyui")],
  daisyui: {
    themes: ["cupcake", "light"],
  },
};
