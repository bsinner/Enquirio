<template>
    <div>
        <slot></slot>
        <button class="btn btn-outline-info mr-2 float-right"
                :click="$emit('showEdit')">Edit
        </button>
        <button class="btn btn-outline-info mr-2 float-right"
                :click="$emit('deleteItem')">Delete
        </button>
    </div>
</template>