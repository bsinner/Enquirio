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
        updateQuestion(state, question) {
            state.question.title = question.title;
            state.question.contents = question.contents;
        },
        updateAnswer(state, answer) {
            const id = answer.id;
            state.question.answers[id].title = answer.title;
            state.question.answers[id].contents = answer.contents;
        },
        removeAnswer(state, id) {
            Vue.delete(state.question.answers, id);
        },
        addAnswer(state, answer) {
            Vue.set(state.question.answers, answer.id, answer);
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
            commit("updateQuestion", updated);
        },

        async deleteQuestion({ state, rootState }) {
            const url = `${rootState.url}/deleteQuestion/${state.question.id}`;
            await Axios.delete(url);
        },

        async editAnswer({ commit, rootState }, { answer, title, contents }) {
            const url = `${rootState.url}/editAnswer`;
            const updated = ObjUtil.getCopy(answer);
            updated.title = title;
            updated.contents = contents;

            await Axios.put(url, updated);
            commit("updateAnswer", updated);
        },

        async deleteAnswer({ commit, rootState }, { answer }) {
            const id = answer.id;
            const url = `${rootState.url}/deleteAnswer/${id}`;

            await Axios.delete(url);
            commit("removeAnswer", id);
        },

        async createAnswer({ commit, rootState, state }, { title, contents }) {
            const url = `${rootState.url}/createAnswer`;

            const data = await (await Axios.post(url, {
                title: title,
                contents: contents,
                questionId: state.question.id
            })).data;
            console.log("action: " + data);
            commit("addAnswer", data);
        }
    }
};