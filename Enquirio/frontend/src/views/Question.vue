<template>
    <div>
        <br><br>

        <div class="row">
            <div class="col-sm-9">
                
                <div class="row justify-content-center">                    
                    <question-content :question="question">
                    </question-content>
                </div>

                <answer-content v-for="an in question.answers"
                        v-bind:key="an.id" :answer="an">                    
                </answer-content>            

            </div>
        </div>

    </div>
</template>

<script>
import { mapActions, mapState } from "vuex"
import QuestionContent from "../components/question/QuestionContent";
import AnswerContent from "../components/question/AnswerContent";

export default {
    components: { QuestionContent, AnswerContent },
    computed: { 
        ...mapState("question", { question: s => s.question }) 
    },
    methods: {
        ...mapActions("question", { getQuestion: "getQuestion" })
    },
    async created() {
        await this.getQuestion(this.$route.params.id);
    }
}
</script>