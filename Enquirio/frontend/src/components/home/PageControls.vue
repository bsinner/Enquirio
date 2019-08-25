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
        
        // Load page of questions
        toPage(p) {
            if (p <= this.pages) {                
                this.updateControls(p);
                this.getQuestions(p);
            }
        },
        
        // Load more page buttons if the first or last button was clicked
        updateControls(p) {
            const lng = this.btnArray.length - 1;

            // Update buttons if last was clicked
            if (p >= this.btnArray[lng]) {
                
                if (this.pages < START_LG) { 
                    this.shiftPageButtons(SHIFT_SM, p); 
                } else { 
                    this.shiftPageButtons(SHIFT_LG, p); 
                }
            // Update buttons if first was clicked
            } else if (p <= this.btnArray[0] && p > 1) {

                if (this.pages < START_LG) {
                    this.shiftPageButtons(-SHIFT_SM, p);                
                } else {
                    this.shiftPageButtons(-SHIFT_LG, p);
                }
            }
        },

        // Show more page buttons, hide old ones
        shiftPageButtons(dist, requestedPage) {                        
            // Boolean to represent going backwards or forwards
            const isNeg = dist < 0;
            dist = Math.abs(dist);
            
            // Shorthand for .length
            const lng = () => this.btnArray.length;

            // Pages left between start/end of btnArray and total pages
            const remainingPages = isNeg 
                    ? this.btnArray[0] - 1
                    : this.pages - requestedPage;
        
            // Prevent creating more buttons than there are pages
            if (dist > remainingPages) {                    
                dist = remainingPages;
            }
            
            this.appendPageButtons(isNeg, dist, lng);
            this.removeOldButtons(isNeg, lng);
        }, 
        
        // Append more buttons to start or end of btnArray
        appendPageButtons(isNegative, distance, lng) {
            for (let i = 0; i < distance; i++) {
                const borderElement = isNegative 
                        ? this.btnArray[0]
                        : this.btnArray[lng() - 1];
                
                if (isNegative) {
                    this.btnArray.splice(0, 0, borderElement - 1);
                } else {
                    this.btnArray.push(borderElement + 1);
                }
            }                    
        }, 
        
        // Remove extra buttons from start or end of btnArray
        removeOldButtons(isNegative, lng) {
            this.btnArray = isNegative
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