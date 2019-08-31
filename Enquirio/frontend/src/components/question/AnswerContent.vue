<template>
    <div class="mt-4 row justify-content-center">
        <div class="col-sm-8">
            <hr>

            <div v-if="showAnswer">
                <h4>{{ answer.title }}</h4>
                <p>{{ answer.contents }}</p>
            </div>

            <text-post-form v-if="!showAnswer"
                    @hideForm="hideEdit"
                    @submitForm="submitEdit"
                    :title="answer.title"
                    :contents="answer.contents">
            </text-post-form>

            <br>
            <i class="text-secondary">
                Answered by Author on {{ answer.created }}            
            </i>

            <div v-if="showAnswer">
                <br><br>
                <text-post-buttons
                        @showEdit="showEdit"
                        @deleteItem="deleteAnswer">
                </text-post-buttons>
            </div>

        </div>
    </div>
</template>

<script>
import TextPostButtons from "./_shared/TextPostButtons";
import TextPostForm from "./_shared/TextPostForm";
import { mapActions } from "vuex";

export default {
    props: [ "answer" ],
    components: { TextPostButtons, TextPostForm },
    data() {
        return {
            showAnswer: true,            
        }
    },
    methods: {
        ...mapActions("question", { 
            editAnswer: "editAnswer",
            delAnswer: "deleteAnswer"
        }),
        showEdit() {
            this.showAnswer = false;
        },
        hideEdit() {
            this.showAnswer = true;
        },
        async submitEdit(title, contents) {
            try {
                await this.editAnswer({
                    answer: this.answer, 
                    title: title, 
                    contents: contents
                });
            } catch (err) {
                // TODO: show error if not logged in or other error                
            }

            this.hideEdit();
        },
        async deleteAnswer() {
            try {
                await this.delAnswer({ answer: this.answer });
            } catch (err) {                
                // TODO: show error if not logged in or other error
            }
        }
    }
}
</script>
