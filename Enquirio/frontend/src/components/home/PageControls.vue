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

            <label v-for="(p, i) in btnArray"
                    :key="i" class="btn btn-secondary"
                    :class="{ active : p === page }">
                <input type="radio" autocomplete="off"
                        @click="toPage(p)">{{ p }}
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
 *   and shift the display multiple numbers forward, an icon for the first and/or 
 *   last page is shown, and an input for entering a page to view is shown
 */
import { mapState, mapActions } from "vuex";

const MAX_BTNS = 10;
const SHIFT_LG = 9;
const SHIFT_SM = 2;
const START_LG = 16;

export default {    
    data() {
        return { btnArray: [1] };
    },
    computed: { 
        ...mapState("home", { 
            pages: s => s.pages,
            page: s => s.pageNumber
        }),
    },
    methods: { 
        ...mapActions("home", [ "getPages", "getQuestions" ]),
        // Get the previous or next page of questions
        toPage(p) {    

            // if page is valid continue
            if (p <= this.pages) {
                const lng = this.btnArray.length - 1;
                
                // if requested page number is not one of the numbered 
                // buttons on screen update the list of buttons
                if (p >= this.btnArray[lng]) {
                
                    if (this.pages < START_LG) { 
                        this.shiftPageButtons(SHIFT_SM, p);
                    } else {
                        this.shiftPageButtons(SHIFT_LG, p);
                    }
                }
                
                this.getQuestions(p);
            }
        },
        // Update the page buttons if the last or first button was clicked
        shiftPageButtons(dist, requestedPage) {
        
            // Go forward
            if (dist > 0) {
                const remainingPages = this.pages - requestedPage;
                const lng = () => this.btnArray.length;

                if (dist > remainingPages) {                    
                    dist = remainingPages;
                }
                
                for (let i = 0; i < dist; i++) {                
                    const last = this.btnArray[lng() - 1];            
                    this.btnArray.push(last + 1);                    
                }

                this.btnArray = this.btnArray.splice(lng() - MAX_BTNS, lng());
                
            // Go backwards                
            } else {
                
            }
        }
    },
    created() {        
        this.getPages();
        this.btnArray = [...Array(this.pages + 1).keys()]
                .splice(1, MAX_BTNS);
    }
}

</script>