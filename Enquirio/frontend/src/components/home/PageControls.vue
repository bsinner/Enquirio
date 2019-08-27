<template>
    <div v-if="pages > 1" class="row justify-content-center">
        <span class="btn-group btn-group-toggle" data-toggle="buttons">

            <!-- Left arrow buttons -->
            <arrow-button :direction="'left'" :count="2" 
                    v-if="showLeft" v-on:toPage="toPage(1)">                
            </arrow-button>
            <arrow-button :direction="'left'" :count="1"
                    v-if="showLeft" v-on:toPage="toPage(page - 1)">
            </arrow-button>

            <!-- Numeric buttons -->
            <page-button v-for="(p, i) in btnArray" v-on:toPage="toPage(p)"
                    :key="i" :class="{ active : p === page }">
                {{ p }}
            </page-button>

            <!-- Right arrow buttons -->
            <arrow-button :direction="'right'" :count="1"
                    v-if="showRight" v-on:toPage="toPage(page + 1)">
            </arrow-button>
            <arrow-button :direction="'right'" :count="2"
                    v-if="showRight" v-on:toPage="toPage(pages)">
            </arrow-button>

        </span>    
    </div>
</template>

<script> 
import { mapState, mapActions } from "vuex";
import ArrowButton from "./PageControls/ArrowButton";
import PageButton from "./PageControls/PageButton";

const MAX_BTNS = 10;
const SHIFT = 5;

export default {
    components: { ArrowButton, PageButton },
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
        showArrows() { return this.pages > MAX_BTNS },
        showLeft() { return this.showArrows && this.page > 1},
        showRight() { return this.showArrows && this.page < this.pages }
    },
    methods: { 
        ...mapActions("home", [ "getPages", "getQuestions" ]),
        
        // Load page of questions
        toPage(p) {
            if (p <= this.pages && p != this.page && this.page > 0) {                
                this.detectEdge(p);
                this.getQuestions(p);
            }
        },
        
        // Detect if the first or last numbered page button on the screen was clicked
        detectEdge(p) {
            const lng = this.btnArray.length - 1;

            if (p <= this.btnArray[0] || p >= this.btnArray[lng]) {
                console.log(this.btnArray);
                this.showMoreButtons(p);
            }            
        },

        // Add more page buttons to the beginning or end of the list of page buttons
        showMoreButtons(requestedPage) {                                                     
            let change = requestedPage - this.page;
            const lng = this.btnArray.length - 1;    
            const min = 1 - this.btnArray[0];
            const max = this.pages - this.btnArray[lng];
            const isNeg = change < 0;
   
            change += isNeg ? -SHIFT : SHIFT;

            if (isNeg && change < min) {
                change = min;
            } else if (change > max) {
                change = max;
            }

            this.renumberButtons(change);
        }, 

        // Renumber the page buttons to show more numbers in the direction the user clicked
        renumberButtons(change) {
            this.btnArray.forEach((btn, index) => {
                this.btnArray.splice(index, 1, btn + change);
            });
        }
    },
    created() {        
        this.getPages();    
        this.btnArray = [...Array(this.pages + 1).keys()]
                .splice(1, MAX_BTNS);
    }
}

</script>