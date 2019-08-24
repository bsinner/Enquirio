<template>
    <div class="row justify-content-center">
        <div class="btn-group btn-group-toggle" data-toggle="buttons">
            
            <label v-for="(p, i) in pageArray"
                    :key="i" class="btn btn-secondary"
                    :class="{ active : p === page }">

                <input type="radio" autocomplete="off"
                        @click="getQuestions(p)">{{ p }}

            </label>

        </div>    
    </div>
</template>

<script> 
import { mapState, mapActions } from "vuex";

export default {
    computed: { 
        ...mapState("home", { 
            pages: s => s.pages,
            page: s => s.pageNumber
        }),
        pageArray() { 
            return [...Array(this.pages + 1).keys()].splice(1); 
        },
    },
    methods: { 
        ...mapActions("home", [ "getPages", "getQuestions" ])
    },
    created() {
        this.getPages();
    }
}

</script>