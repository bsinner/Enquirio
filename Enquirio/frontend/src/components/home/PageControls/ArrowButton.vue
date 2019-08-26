<template>
    <page-button :class="'px-2'" v-on:toPage="$emit('toPage')">
        <i v-for="n in count" :key="n"
                :class="`fas fa-chevron-${direction}`">
        </i>
    </page-button>
</template>

<script>
    import PageButton from "./PageButton";

    export default {
        props: [ "direction", "count" ],
        components: { PageButton },
    }
</script>