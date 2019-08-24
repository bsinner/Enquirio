<template>
    <div>
        
    </div>
</template>

<script>
import { mapState, mapActions } from "vuex";

export default {
    computed: { ...mapState("home", { 
        pages: s => s.pages 
    }) },
    methods: { ...mapActions("home", [ "getPages" ]) },
    created() {
        this.getPages();
    }
}

</script>