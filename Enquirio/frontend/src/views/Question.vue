<template>
    <div>
        <br><br>

        <div class="row">
            <div class="col-sm-9">
                
                <div class="row justify-content-center">
                    <!-- <div class="col-sm-8">
                        <h4>{{ question.title }}</h4>
                        <p>{{ question.contents }}</p>

                        <br>
                        <i>Asked by Author on {{ question.created }}</i>

                        <br><br>
                        <button-group>

                        </button-group>

                    </div> -->
                    <question-contents :question="question">
                    </question-contents>

                </div>

            </div>
        </div>

    </div>
</template>

<script>
import { mapActions, mapState } from "vuex"
import QuestionContents from "../components/question/QuestionContents";

export default {
    components: { QuestionContents },
    computed: { 
        ...mapState("question", { question: s => s.question }) 
    },
    methods: {
        ...mapActions("question", { getQuestion: "getQuestion" })
    },
    created() {
        this.getQuestion(this.$route.params.id);
    }
}
</script>