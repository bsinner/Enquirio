import Axios from "axios";

export default {
    namespaced: true,
    state: {
        pageNumber: 1,
        pages: 1,
        questions: []
    },
    mutations: {
        setQuestions(state, questions, page) {
            state.questions = questions;
            state.pageNumber = page;
        },
        setPages(state, pages) {
            state.pages = pages;
        }
    },
    actions: {
        async getQuestions({ commit, rootState }, page) {
            const url = `${rootState.url}/questions?p=${page}`;
            let data = (await Axios.get(url)).data;
            commit("setQuestions", data, page)
        },

        async getPages({ commit, rootState }) {
            const url = `${rootState.url}/qMaxPage`;
            let data = (await Axios.get(url)).data;
            commit("setPages", data);
        }
    }
}