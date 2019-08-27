<template>
    <div>
        <menu-bar></menu-bar>
        <router-view></router-view>
    </div>
</template>

<style>
    @import "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.9.0/css/fontawesome.min.css";
    @import "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.9.0/css/solid.min.css";
    @import "https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css";
</style>

<script>
    import MenuBar from "./components/_shared/MenuBar";

    export default {
        components: { MenuBar }
    }
</script>
