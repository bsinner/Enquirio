<template>
    <div class="col-sm-8">
        
        <div v-if="showQuestion">
            <h4>{{ question.title }}</h4>
            <p>{{ question.contents }}</p>
        </div>

        <text-post-form v-if="!showQuestion"
                @hideForm="hideEdit"
                @submitForm="submitEdit"
                :title="question.title"
                :contents="question.contents">
        </text-post-form>
        
        <br>
        <i>Asked by Author on {{ question.created }}</i>

        <br><br>
        <text-post-buttons v-if="showQuestion"
                @showEdit="showEdit"
                @deleteItem="deleteQuestion">
            <button class="btn btn-outline-dark float-right">Answer</button>
        </text-post-buttons>

    </div>
</template>

<script>
import TextPostButtons from "./_shared/TextPostButtons";
import TextPostForm from "./_shared/TextPostForm";
import { mapActions } from "vuex";

export default {
    props: [ "question" ],
    components: { TextPostButtons, TextPostForm },
    data() { 
        return { showQuestion: true };
    },
    methods: {
        ...mapActions("question", {
            editQuestion: "editQuestion",
            removeQuestion: "deleteQuestion"
        }),
        showEdit() { 
            this.showQuestion = false;
        },
        hideEdit() { 
            this.showQuestion = true;
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

            this.hideEdit();
        }
    }
}
</script>