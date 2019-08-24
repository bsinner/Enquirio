import Vue from "vue";
import Vuex from "vuex";
import home from "./home";

const IS_PROD = process.env.NODE_ENV === "production";

Vue.use(Vuex);

export default new Vuex.Store({
    strict: !IS_PROD,
    modules: { home },
    state: {
        url: IS_PROD ? process.env.BASE_URL : "http://localhost:5000/api"
    },
});