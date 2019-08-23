import Vue from "vue";
import Vuex from "vuex";
import Axios from "axios";

Vue.use(Vuex);

export default new Vuex.Store({
    strict : process.env.NODE_ENV !== "production",
    state : {
        pageNumber : 0,
        pageSize : 10,
        currentPosts : []
    },
    mutations : {
        setPosts(state, posts, page) {
            state.currentPosts = posts;
            state.pageNumber = page;
        }
    },
    actions : {
        async getPosts({ commit, state }, page) {
            const curr = state.pageNumber * state.pageSize;
            let data = (await Axios.get(`...\/api\/allQuestions?s=${curr}&e=${curr + state.pageSize}`)).data;
            commit("setPosts", data, page);
        }
    }
})
