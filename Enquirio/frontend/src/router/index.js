import Vue from "vue";
import Router from "vue-router";
import Home from "../views/Home.vue";
import Question from "../views/Question.vue";
import CreateQuestion from "../views/CreateQuestion";

Vue.use(Router);

export default new Router({
    mode: "history",
    base: process.env.BASE_URL,
    routes: [
        { path: "/", component: Home },
        { path: "/question/:id", component: Question },
        { path: "*", redirect: "/" },
        { path: "/ask", component: CreateQuestion }
    ]
});