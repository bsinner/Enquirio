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
        updateQuestion(state, { question }) {
            state.question.title = question.title;
            state.question.contents = question.contents;
        },
        updateAnswer(state, { answer }) {
            const id = answer.id;
            state.question.answers[id].title = answer.title;
            state.question.answers[id].contents = answer.contents;
        },
        removeAnswer(state, { id }) {
            Vue.delete(state.question.answers, id);
        }
    },
    actions: {
        async getQuestion({ commit, rootState }, id) {
            const url = `${rootState.url}/question/${id}`;
            let data = (await Axios.get(url)).data;

            // Answers come in an array, convert to object where answer
            // IDs are keys to make answers easier to reference in mutations
            ObjUtil.arrayToMap(data, "answers", "id");
            commit("setQuestion", data);
        },

        async editQuestion({ commit, rootState }, { question, title, contents }) {
            const url = `${rootState.url}/editQuestion`;
            const updated = ObjUtil.getWithoutProperties(question, "answers");
            updated.title = title;
            updated.contents = contents;

            await Axios.put(url, updated);
            commit("updateQuestion", { question: updated })
        },

        async editAnswer({ commit, rootState }, { answer, title, contents }) {
            const url = `${rootState.url}/editAnswer`;
            const updated = ObjUtil.getCopy(answer);
            updated.title = title;
            updated.contents = contents;

            await Axios.put(url, updated);
            commit("updateAnswer", { answer: updated });
        },

        async deleteAnswer({ commit, rootState }, { answer }) {
            const id = answer.id;
            const url = `${rootState.url}/deleteAnswer/${id}`;

            await Axios.delete(url);
            commit("removeAnswer", { id });
        }
    }
};