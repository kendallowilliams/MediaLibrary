var gulp = require("gulp");
var ts = require('gulp-typescript');

var tsProject = ts.createProject('lib/app/tsconfig.json');

gulp.task('app', () => {
    var tsResult = gulp.src('./lib/app/**/*.ts').pipe(tsProject());

    return tsResult.js.pipe(gulp.dest('./lib/app'));
});