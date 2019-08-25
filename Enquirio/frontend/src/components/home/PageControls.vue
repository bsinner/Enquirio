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
const START_LG = 21;

export default {    
    data() {
        return { 
            btnArray: [1] // A list of numbers to print on pagination buttons
        };
    },
    computed: { 
        ...mapState("home", { 
            pages: s => s.pages,
            page: s => s.pageNumber
        }),
    },
    methods: { 
        ...mapActions("home", [ "getPages", "getQuestions" ]),
        // Get a page of questions
        toPage(p) {    

            // if page isn't greater than total number of pages continue
            if (p <= this.pages) {
                const lng = this.btnArray.length - 1;
                
                // if requested page number is not on the screen update the controls
                if (p >= this.btnArray[lng]) {
                    
                    // Update to show going forward
                    if (this.pages < START_LG) { 
                        this.shiftPageButtons(SHIFT_SM, p);
                    } else {
                        this.shiftPageButtons(SHIFT_LG, p);
                    }
                } else if (p <= this.btnArray[0] && p > 1) {
                    
                    // Update to show going back
                    if (this.pages < START_LG) {
                        this.shiftPageButtons(-SHIFT_SM, p);
                    } else {
                        this.shiftPageButtons(-SHIFT_LG, p);
                    }
                }
                                
                this.getQuestions(p);
            }
        },
        
        // Show more page buttons if the first or last button was clicked
        shiftPageButtons(dist, requestedPage) {                        
            // Boolean to represent going backwards or forwards
            const isNeg = dist < 0;
            dist = Math.abs(dist);
            
            // Pages left between start/end of btnArray and total pages
            const remainingPages = isNeg 
                    ? this.btnArray[0] - 1
                    : this.pages - requestedPage;

            // Shorthand for .length
            const lng = () => this.btnArray.length;

            // if trying to go more pages than there are left 
            // change distance to amount of remaing pages
            if (dist > remainingPages) {                    
                dist = remainingPages;
            }
            
            // Append new buttons
            for (let i = 0; i < dist; i++) {
                // Last/first element in btnArray
                const borderElement = isNeg 
                        ? this.btnArray[0]
                        : this.btnArray[lng() - 1];
                
                if (isNeg) {
                    this.btnArray.splice(0, 0, borderElement - 1);
                } else {
                    this.btnArray.push(borderElement + 1);
                }                
            }
            
            // Trim old buttons
            this.btnArray = isNeg 
                    ? this.btnArray.splice(0, MAX_BTNS)
                    : this.btnArray.splice(lng() - MAX_BTNS, lng());                                                
        }
    },
    created() {        
        this.getPages();
        this.btnArray = [...Array(this.pages + 1).keys()]
                .splice(1, MAX_BTNS);
    }
}

</script>