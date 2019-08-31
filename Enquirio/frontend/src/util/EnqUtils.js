export { ObjUtil };

const ObjUtil = (function() {

    /*
     * Convert an array of objects within an object into a map
     * { foo: [ {id:4, x:"a4"}, {id:99, x:"a99"} ]
     * becomes
     * { foo: { 4: {id:4, x:"a4"}, 99: {id:99, x:"a99"} } 
     */
    function arrayToMap(obj, array, key) {
        const temp = {};

        obj[array].forEach(item => {
            temp[item[key]] = item;
        });

        obj[array] = temp;
    }

    /* 
     * Convert a map within an object created by arrayToMap 
     * back into an array
     */
    function mapToArray(obj, map) {
        const temp = [];

        _loopObject(obj[map], (k, value) => {
            temp.push(value);
        });

        obj[map] = temp;
    }

    /*
     * Get a shallow copy of an object
     */
    function getCopy(obj) {
        const temp = {};
        _loopObject(obj, (k, v) => {
            temp[k] = v;
        });
        return temp;
    }

    /*
     * Get a shallow copy of an object and skip given properties,
     * toSkip can be an array or a single string
     */
    function getWithoutProperties(obj, toSkip) {
        const temp = {};
        let skipKey = toSkip instanceof Array ?
            k => toSkip.includes(k) :
            k => toSkip === k;

        _loopObject(obj, (key, value) => {
            if (!skipKey(key)) {
                temp[key] = value;
            }
        });

        return temp;
    }

    function _loopObject(obj, callback) {
        for (let [k, v] of Object.entries(obj)) {
            callback(k, v);
        }
    }

    return {
        arrayToMap: arrayToMap,
        mapToArray: mapToArray,
        getWithoutProperties: getWithoutProperties,
        getCopy: getCopy
    };
})();