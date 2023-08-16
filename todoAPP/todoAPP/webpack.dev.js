const path = require('path');
const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = merge(common, {
  mode: 'development',
  watch: true,
  watchOptions: {
    aggregateTimeout: 500
},
output: {
  path: path.resolve(__dirname, './wwwroot/dist'),
  filename: '[name].js',
},
  devtool: 'inline-source-map',
  module: {
    rules: [
      {
        test: /\.(scss)$/,
        use: [
          {
            loader:'style-loader'
          },
          {
            loader: 'css-loader'
          }, 
          {
            loader: 'sass-loader'
          }]
      }
    ],
  },
});