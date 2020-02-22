requirejs.config({
    baseUrl: 'lib/app',
    paths: {
        jquery: '../jquery/jquery.min',
        'jquery-ui': '../jquery-ui-1.12.1.custom/jquery-ui.min',
        bootstrap: '../bootstrap/dist/js/bootstrap.bundle.min'
    },
    deps: ['jquery', 'jquery-ui', 'bootstrap']
});

requirejs(['app'], function (app) { });