<template>
    <div v-if="pages > 1" class="row justify-content-center">
        <div class="btn-group btn-group-toggle" data-toggle="buttons">
            
            <label v-if="pages >= 10 && page > 1" 
                    class="btn btn-secondary"
            >
                <input @click="toPage(page - 1)" 
                        type="radio" autocomplete="off"
                >
                <i class="fas fa-chevron-left"></i>
            </label>

            <label v-for="(p, i) in pageArray"
                    :key="i" class="btn btn-secondary"
                    :class="{ active : p === page }">
                <input type="radio" autocomplete="off"
                        @click="getQuestions(p)">{{ p }}
            </label>

            <label v-if="pages >= 10 && page < pages" 
                    class="btn btn-secondary"
            >
                <input @click="toPage(page + 1)" 
                        type="radio" autocomplete="off"
                >
                <i class="fas fa-chevron-right"></i>
            </label>

        </div>    
    </div>
</template>

<script> 
/*
 * Possible pagination displays
 * - If there is only one page of results don't display buttons
 * - If pages is greater than 1 and less than 10 display a button for
 *   each page number
 * - If pages is greater than 9 and less than 16 show 10 buttons at a time
 *   and include arrow buttons to move left and right
 * - If pages is greater than 15, left and right arrows move one page forward
 *   and shift the display 10 numbers forward, an icon for the first and/or 
 *   last page is shown, and an input for entering a page to view is shown
 */
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
        ...mapActions("home", [ "getPages", "getQuestions" ]),
        toPage(p) {
            this.getQuestions(p);
        }
    },
    created() {
        this.getPages();
    }
}

</script>