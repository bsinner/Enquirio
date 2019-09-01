<template>
    <div class="col-sm-8">
        
        <div v-if="isQuestionVisible">
            <h4>{{ question.title }}</h4>
            <p>{{ question.contents }}</p>
        </div>

        <text-post-form v-if="isEditQuestionVisible"
                @hideForm="showQuestion"
                @submitForm="submitEdit"
                :title="question.title"
                :contents="question.contents">
        </text-post-form>
        
        <br>
        <i>Asked by Author on {{ question.created }}</i>

        <br><br>
        <text-post-form v-if="isCreateAnswerVisible"
                @hideForm="showQuestion"
                @submitForm="submitAnswer">
        </text-post-form>

        <br><br>
        <text-post-buttons v-if="isQuestionBtnsVisible"
                @showEdit="showEditQuestionForm"
                @deleteItem="deleteQuestion">
            <button class="btn btn-outline-dark float-right"
                    @click="showCreateAnswerForm">Answer</button>
        </text-post-buttons>

    </div>
</template>

<script>
import TextPostButtons from "./_shared/TextPostButtons";
import TextPostForm from "../_shared/TextPostForm";
import { mapActions } from "vuex";

export default {
    props: [ "question" ],
    components: { TextPostButtons, TextPostForm },
    data() { 
        return { 
            isQuestionVisible: true,
            isQuestionBtnsVisible: true,
            isCreateAnswerVisible: false,
            isEditQuestionVisible: false
        };
    },
    methods: {
        ...mapActions("question", {
            editQuestion: "editQuestion",
            removeQuestion: "deleteQuestion",
            createAnswer: "createAnswer"
        }),
        showQuestion() {
            this.swapVisibility(true, true, false, false);
        },
        showCreateAnswerForm() {
            this.swapVisibility(true, false, true, false);
        },
        showEditQuestionForm() {
            this.swapVisibility(false, false, false, true);
        },
        swapVisibility(q, qBtns, anForm, editForm) {
            this.isQuestionVisible = q;
            this.isQuestionBtnsVisible = qBtns;
            this.isCreateAnswerVisible = anForm;
            this.isEditQuestionVisible = editForm;
        },        
        async deleteQuestion() {
            try {
                await this.removeQuestion();
                this.$router.push("/");
            } catch (err) {
                // TODO: handle not logged in/other error
            }
        },
        async submitEdit(title, contents) {
            try {
                await this.editQuestion({ 
                    question: this.question,
                    title: title,
                    contents: contents
                });
            } catch (err) {
                // TODO: handle not logged in/other error
            }

            this.showQuestion();
        },
        async submitAnswer(title, contents) {
            try {
                await this.createAnswer({
                    title: title,
                    contents: contents
                });                
            } catch (err) {
                // TODO: handle not logged in/other error
            }

            this.showQuestion();
        }
    }
}
</script>