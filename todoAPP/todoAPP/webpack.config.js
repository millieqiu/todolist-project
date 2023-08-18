const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
    watch: true,
    watchOptions: {
        aggregateTimeout: 500
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
            {
                test: /\.(scss)$/,
                use: [
                    {
                        loader: 'style-loader'
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
    entry: {
        site: './src/site.js',
        register: './src/register.js',
        header: './src/header.js',
        todoList: './src/todoList.js'
    },
    output: {
        path: path.resolve(__dirname, './wwwroot/js'),
        filename: '[name].js',
    },
    mode: 'development',
    resolve: {
        alias: {
            vue: 'vue/dist/vue.js'
        }
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: 'all.css',
        }),
    ],
};