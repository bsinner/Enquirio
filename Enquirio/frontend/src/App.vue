<template>
    <div>
        <menu-bar></menu-bar>
        <router-view></router-view>
    </div>
</template>

<style>
    @import "https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css";
</style>

<script>
    import MenuBar from "./components/MenuBar";

    export default {
        components: { MenuBar }
    }
</script>
