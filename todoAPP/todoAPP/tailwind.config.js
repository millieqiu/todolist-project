/** @type {import('tailwindcss').Config} */
module.exports = {
  // 如果有任何檔案需要用到 tailwind 的語法，都要記得回來這裡加
  content: ["./Pages/*.cshtml", "./Pages/Shared/*.cshtml", "./src/js/components/*.vue", "./src/js/*/*"],
  theme: {
    extend: {},
  },
  plugins: [require("@tailwindcss/typography"), require("daisyui")],
  daisyui: {
    themes: ["cupcake", "light"],
  },
};
