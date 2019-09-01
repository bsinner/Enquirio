<template>
    <div>
        
        <br><br>
        <div class="row justify-content-center">
            <div class="col-sm-6">

                <h4>Ask Question</h4>
                <br>

                <text-post-form :cancelBtnTxt="'Back'"
                        :rowsCount="'7'">                    
                </text-post-form>

            </div>
        </div>

    </div>
</template>

<script>
import TextPostForm from "../components/_shared/TextPostForm";

export default {
    components: { TextPostForm }
}
</script>