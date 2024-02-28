"use strict";
const path = require("path");
const webpack = require("webpack");

module.exports = {
  context: path.resolve(__dirname, "src"),
  entry: {
    // Layout
    _Layout: "./js/_Layout.js",

    // Pages
    login: "./js/login.js",
    header: "./js/header.js",
    index: "./js/index.js",
    setting: "./js/setting.js",
  },
  output: {
    path: path.resolve(process.cwd(), "wwwroot/dist"),
    filename: "./js/[name].js",
  },
  module: {
    rules: [
      {
        test: /\.vue$/,
        loader: "vue-loader",
      },
      {
        test: /\.(js)$/, // 編譯副檔名為js的檔案
        exclude: /(node_modules)/, // 排除不進行編譯的程式
        use: {
          loader: "babel-loader",
          options: {
            presets: ["@babel/preset-env"],
          },
        },
      },
      // FIXME: dist 路徑下沒辦法打包圖片
      {
        test: /\.(jpe?g|png|gif|svg|eot|ttf|woff|woff2|otf|webp|ico)$/,
        use: [
          {
            loader: "url-loader",
            options: {
              limit: 1,
              name: "./images/[name].[ext]",
            },
          },
        ],
      },
    ],
  },
  plugins: [
    new webpack.DefinePlugin({
      __VUE_OPTIONS_API__: true,
      __VUE_PROD_DEVTOOLS__: false,
    }),
  ],
};
