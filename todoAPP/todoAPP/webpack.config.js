const path = require('path');

module.exports = {
  entry: {
    index: './src/site.js',
    register: './src/register.js',
    header: './src/header.js',
    todo: './src/todoList.js'
  },
  output: {
    path: path.resolve(__dirname, './wwwroot/js'),
    filename: '[name].js',
  },
  mode: 'development',
  resolve: {
        alias: {
            vue: 'vue/dist/vue.esm-bundler.js'
        }
    },
};