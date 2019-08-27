<template>
    <div class="mt-4 row justify-content-center">
        <div class="col-sm-8">
            <hr>

            <h4>{{ answer.title }}</h4>
            <p>{{ answer.contents }}</p>

            <br>
            <i class="text-secondary">
                &nbsp;&nbsp;Answered by Author on {{ answer.created }}            
            </i>

            <br><br>
            <text-post-buttons></text-post-buttons>

        </div>
    </div>
</template>

<script>
import TextPostButtons from "./_shared/TextPostButtons";

export default {
    props: [ "answer" ],
    components: { TextPostButtons }
}
</script>