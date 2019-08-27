<template>
    <div>
        
        <br>
        <div>
            <input type="text" class="form-control">
            <small class="text-danger" v-if="titleError">
                Title must not be blank
            </small>
        </div>

        <br>
        <div>
            <textarea rows="5" class="form-control"></textarea>
            <small class="text-danger" v-if="contentsError">
                Contents must not be blank
            </small>
        </div>

        <br>
        <button class="btn btn-outline-info float-right" 
                @click="$emit('hideForm')">Cancel
        </button>
        <button class="btn btn-outline-dark float-right mr-2"
                @click="$emit('submitForm')">Submit
        </button>

    </div>
</template>

<script>
export default {
    data() { 
        return {
            titleError: false,
            contentsError: false
        }
    }
}
</script>