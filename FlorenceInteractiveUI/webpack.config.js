// Note this only includes basic configuration for development mode.
// For a more comprehensive configuration check:
// https://github.com/fable-compiler/webpack-config-template

var path = require("path");

module.exports = {
  mode: "development",
  entry: "./src/App/App.fs.js",
  output: {
    path: path.join(__dirname, "./public"),
    filename: "bundle.js",
    publicPath: "/dist/",
    library: {
      type: "module", // or "umd", "var", etc.
    },
  },
  experiments: {
    outputModule: true, // required for type: "module"
  },
  devServer: {
    static: {
      directory: path.resolve(__dirname, "public"), // replaces contentBase
      publicPath: "/", // serve index.html from /
    },
    port: 8080,
    hot: true,
    devMiddleware: {
      writeToDisk: true,
    },
  },
};


