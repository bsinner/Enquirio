<template>
    <div class="col-sm-8">
        <h4>{{ question.title }}</h4>
        <p>{{ question.contents }}</p>

        <br>
        <i>Asked by Author on {{ question.created }}</i>

        <br><br>
        <text-post-buttons>
            <button class="btn btn-outline-dark float-right">Answer</button>
        </text-post-buttons>

    </div>
</template>

<script>
import TextPostButtons from "./TextPostButtons";

export default {
    props: [ "question" ],
    components: { TextPostButtons }
}
</script>