<template>
    <div>      
        <br><br>
        <search-bar></search-bar>

        <br>
        <div class="row justify-content-center">
            <div class="col-sm-11">
                
                <h5 class="text-center">Recent Questions Within X miles of XXXXX</h5>
                <hr>

                <question-list></question-list>
                
                <br>
                <page-controls></page-controls>
                <br>

            </div>
        </div>
    </div>
</template>

<script>    
    import SearchBar    from "../components/home/SearchBar";
    import QuestionList from "../components/home/QuestionList";
    import PageControls from "../components/home/PageControls";

    export default {
        components: { SearchBar, QuestionList, PageControls },        
    }
</script>
