export { ObjUtil };

/*
 * Convert a nested array of objects in an object into a map, for example:
 * { foo: [ {id:4, x:"a4"}, {id:99, x:"a99"} ] }
 * becomes
 * { foo: { 4: {id:4, x:"a4"}, 99: {id:99, x:"a99"} } }
 * Also map can be converted back with mapToArray
 */
const ObjUtil = (function() {

    function arrayToMap(obj, array, key) {
        const temp = {};

        obj[array].forEach(item => {
            temp[item[key]] = item;
        });

        obj[array] = temp;
    }

    function mapToArray(obj, map) {
        const temp = [];

        for (let [key, val] of Object.entries(obj[map])) {
            temp.push(val);
        }

        obj[map] = temp;
    }

    return {
        arrayToMap: arrayToMap,
        mapToArray: mapToArray
    };
})();