<template>
    <div>
        
        <br><br>
        <div class="row justify-content-center">
            <div class="col-sm-6">

                <h4>Ask Question</h4>
                <br>

                <text-post-form 
                        :cancelBtnTxt="'Back'"
                        :rowsCount="'7'"
                        @cancelForm="back"
                        @submitForm="submitForm">                    
                </text-post-form>

            </div>
        </div>

    </div>
</template>

<script>
import TextPostForm from "../components/_shared/TextPostForm";
import { mapActions } from "vuex";

export default {
    components: { TextPostForm },
    methods: {
        ...mapActions("question", {
            createQuestion: "createQuestion"
        }),
        back() {
            this.$router.push("/");
        },
        async submitForm(title, contents) {
            try {
                const id = await this.createQuestion({
                    title: title,
                    contents: contents
                });

                this.$router.push(`/question/${id}`);
            } catch (err) {
                // TODO: handle if auth error or other error
            }
        }
    }
}
</script>