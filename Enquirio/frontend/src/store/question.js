import Axios from "axios";
import { ObjUtil } from "../util/EnqUtils";
import Vue from "vue";

export default {
    namespaced: true,
    state: {
        question: {}
    },
    mutations: {
        setQuestion(state, question) {
            state.question = question;
        },
        setAnswer(state, { id, newTitle, newContents }) {
            state.question.answers[id].title = newTitle;
            state.question.answers[id].contents = newContents;
        },
        removeAnswer(state, { id }) {
            Vue.delete(state.question.answers, id);
        }
    },
    actions: {
        async getQuestion({ commit, rootState }, id) {
            const url = `${rootState.url}/question/${id}`;
            let data = (await Axios.get(url)).data;

            ObjUtil.arrayToMap(data, "answers", "id");
            commit("setQuestion", data);
        },
        async editAnswer({ commit, rootState }, { answer, title, contents }) {
            commit("setAnswer", {
                id: answer.id,
                newTitle: title,
                newContents: contents
            });

            const url = `${rootState.url}/editAnswer`;
            await Axios.put(url, answer);
        },
        async deleteAnswer({ commit, rootState }, { answer }) {
            const id = answer.id;
            const url = `${rootState.url}/deleteAnswer/${id}`;

            await Axios.delete(url);
            commit("removeAnswer", { id });
        }
    }
};