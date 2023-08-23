const path = require('path');
const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = merge(common, {
    mode: 'development',
    watch: true,
    watchOptions: {
        aggregateTimeout: 300
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
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader'
                    },
                    {
                        loader: 'sass-loader'
                    }]
            }
        ],
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: 'css/[name].css',
        })
    ],
});