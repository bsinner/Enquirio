<template>
    <label class="btn btn-secondary">
        <input @click="$emit('toPage')"
                 type="radio" autocomplete="off">
        <slot></slot>
    </label>
</template>