import Axios from "axios";

export default {
    namespaced: true,
    state: {
        pageNumber: 1,
        questions: []
    },
    mutations: {
        setQuestions(state, questions, page) {
            state.questions = questions;
            state.pageNumber = page;
        }
    },
    actions: {
        async getPosts({ commit, rootState }, page) {
            const url = `${rootState.url}/questions?p=${page}`;
            let data = (await Axios.get(url)).data;
            commit("setQuestions", data, page)
        }
    }
}