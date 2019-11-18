const proxy = require('http-proxy-middleware');

module.exports = function(app) {
    app.use(
        proxy('/api', {
            target: 'https://localhost:5001',
            changeOrigin: true,
            secure:false,
            pathRewrite: {
                '^/api': '/',
            },
        }),
    );
};
