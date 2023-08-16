const path = require('path');

module.exports = {
  entry: {
    site: './src/site.js',
    register: './src/register.js',
    header: './src/header.js',
    todoList: './src/todoList.js'
  },
  module: {
    rules: [
      {
        test: /\.(js)$/,  // 編譯副檔名為js的檔案
        exclude: /(node_modules)/,  // 排除不進行編譯的程式
        use: {
            loader: 'babel-loader',
            options: {
              presets: ['@babel/preset-env']
            }
        }
      },
    ],
  },
    resolve: {
        alias: {
          vue: 'vue/dist/vue.js'
        }
    },
};