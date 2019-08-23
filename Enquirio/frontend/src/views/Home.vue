<template>
    <div>      
        <br><br>
        <search-bar></search-bar>
        <!-- foreach loop that displays question components using data from created() -->
    </div>
</template>

<script>
    import SearchBar from "../components/home/SearchBar";

    export default {
        components: { SearchBar }
    }
</script>
