<template>
    <div>
        
        <br>
        <div>
            <input v-model="localTitle"
                    @input="titleError = false"
                    type="text" class="form-control">
            <small class="text-danger" v-if="titleError">
                Title must not be blank
            </small>
        </div>

        <br>
        <div>
            <textarea v-model="localContents"
                    @input="contentsError = false"                   
                    rows="5" class="form-control"></textarea>
            <small class="text-danger" v-if="contentsError">
                Contents must not be blank
            </small>
        </div>

        <br>
        <button class="btn btn-outline-info float-right" 
                @click="$emit('hideForm')">Cancel
        </button>
        <button class="btn btn-outline-dark float-right mr-2"
                @click="submitForm">Submit
        </button>

    </div>
</template>

<script>
export default {
    props: [ "title", "contents" ],
    data() { 
        return {
            titleError: false,
            contentsError: false,
            localTitle: "",
            localContents: ""
        }
    }, methods: {
        submitForm($event) {
            // If neither feild is empty emit title and contents          
            if (this.localTitle && this.localContents) {                
                this.$emit("submitForm", 
                        this.localTitle , this.localContents                             
                );
            // Else highlight the empty fields
            } else {
                this.titleError = !this.localTitle 
                        ? true : false;
                this.contentsError = !this.localContents 
                        ? true : false;
            }
        },
        updateContents($event) {        
            this.localContents = $event.target.value;            
        },
        updateTitle($event) {
            this.localTitle = $event.target.value;
        }
    // Form title and contents are stored localy instead of bound to 
    // the parent because the parent should update when the form 
    // submits instead of on input
    }, created() {
        this.localTitle = this.title;
        this.localContents = this.contents;
    }
}
</script>