import Vue from "vue";
import Vuex from "vuex";
import Axios from "axios";

const IS_PROD = process.env.NODE_ENV === "production";

Vue.use(Vuex);

export default new Vuex.Store({
    strict: !IS_PROD,
    state: {
        pageNumber: 1,
        pageSize: 15,
        currentPosts: [],
        url: IS_PROD ? process.env.BASE_URL : "http://localhost:5000/api"
    },
    mutations: {
        setPosts(state, posts, page) {
            state.currentPosts = posts;
            state.pageNumber = page;
        }
    },
    actions: {
        async getPosts({ commit, state }, page) {
            const curr = state.pageNumber * state.pageSize;
            let data = (await Axios.get(`${url}/questions?p=${pageNumber}`)).data;
            commit("setPosts", data, page);
        }
    }
})