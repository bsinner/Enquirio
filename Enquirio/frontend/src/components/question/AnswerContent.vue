<template>
    <div class="mt-4 row justify-content-center">
        <div class="col-sm-8">
            <hr>

            <div v-if="showAnswer">
                <h4>{{ answer.title }}</h4>
                <p>{{ answer.contents }}</p>
            </div>

            <text-post-form v-if="!showAnswer"
                    v-on:hideForm="hideEdit">
            </text-post-form>

            <br>
            <i class="text-secondary">
                Answered by Author on {{ answer.created }}            
            </i>

            <div v-if="showAnswer">
                <br><br>
                <text-post-buttons v-on:showEdit="showEdit">                     
                </text-post-buttons>
            </div>

        </div>
    </div>
</template>

<script>
import TextPostButtons from "./_shared/TextPostButtons";
import TextPostForm from "./_shared/TextPostForm";

export default {
    props: [ "answer" ],
    components: { TextPostButtons, TextPostForm },
    data() {
        return {
            showAnswer: true,            
        }
    },
    methods: {
        showEdit() {
            this.showAnswer = false;            
        },
        hideEdit() {
            this.showAnswer = true;            
        },
    }
}
</script>