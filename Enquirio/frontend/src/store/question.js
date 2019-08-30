import Axios from "axios";
import { ObjUtil } from "../util/EnqUtils";

export default {
    namespaced: true,
    state: {
        question: {}
    },
    mutations: {
        setQuestion(state, question) {
            state.question = question;
        },
        // Set 1 answer's title and contents
        setAnswer(state, { id, newTitle, newContents }) {
            state.question.answers[id].title = newTitle;
            state.question.answers[id].contents = newContents;
        }
    },
    actions: {
        async getQuestion({ commit, rootState }, id) {
            const url = `${rootState.url}/question/${id}`;
            let data = (await Axios.get(url)).data;
            ObjUtil.arrayToMap(data, "answers", "id");

            commit("setQuestion", data);
        },
        async editAnswer({ commit, state, rootState }, { answer, title, contents }) {
            commit("setAnswer", {
                id: answer.id,
                newTitle: title,
                newContents: contents
            });

            const url = `${rootState.url}/editAnswer`;
            await Axios.put(url, answer);
        }
    }
};