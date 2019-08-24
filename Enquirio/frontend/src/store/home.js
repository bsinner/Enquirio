import Axios from "axios";

export default {
    namespaced: true,
    state: {
        pageNumber: 1,
        pages: 1,
        questions: []
    },
    mutations: {
        setQuestions(state, data) {
            state.questions = data.questions;
            state.pageNumber = data.page;
        },
        setPages(state, pages) {
            state.pages = pages;
        }
    },
    actions: {
        async getQuestions({ commit, rootState }, pageNumber) {
            const url = `${rootState.url}/questions?p=${pageNumber}`;
            let data = (await Axios.get(url)).data;
            commit("setQuestions", { questions: data, page: pageNumber });
        },

        async getPages({ commit, rootState }) {
            const url = `${rootState.url}/qMaxPage`;
            let data = (await Axios.get(url)).data;
            commit("setPages", data);
        }
    }
}