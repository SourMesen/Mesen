hugo
del public\js\searchjson.js
echo var searchjson = >>public\js\searchjson.js
type public\index.json>>public\js\searchjson.js
