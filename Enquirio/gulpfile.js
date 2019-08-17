const { src, dest, parallel, series } = require("gulp");
const minifyCss = require("gulp-clean-css");
const minifyJs = require("gulp-uglify");
const babel = require("gulp-babel");
const rename = require("gulp-rename");
const clean = require("gulp-clean");

// Delete contents of dist
function cleanDist() {
    return src("wwwroot/dist", { allowEmpty : true })
        .pipe(clean());
}

// Compile and minify js
function js() {
    return src("wwwroot/js/**/*.js")
        .pipe(babel({ presets : ["@babel/env"] }))
        .pipe(minifyJs())
        .pipe(rename({ suffix : ".min" }))
        .pipe(dest("wwwroot/dist/js"));
}

exports.default = series(cleanDist, js);