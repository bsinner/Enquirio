<template>
    <div class="row justify-content-center">
        <div class="col-sm-9">        
            <div v-for="(q, index) in questions"
                v-bind:key="index">

                    <h6>{{ q.title }}</h6>

                    <p class="mb-4">
                        {{ q.contents }}
                        <router-link :to="`/question/${ q.id }`">
                            (More...)
                        </router-link>            
                        <i class="text-secondary">
                            &nbsp;&nbsp;&nbsp;Username - {{ q.created }}
                        </i>
                    </p>
                    <hr>

            </div>
        </div>
    </div>
</template>

<script>
import { mapActions, mapState } from "vuex";

export default {
    computed: { ...mapState("home", { 
        questions: s => s.questions, 
        pageNumber: s => s.pageNumber
    }) },
    methods: { ...mapActions("home", [ "getQuestions" ]) },
    created() {
        this.getQuestions(this.pageNumber);
    }
}
</script>