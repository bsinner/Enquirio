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
                
                <!-- <page-controls></page-controls> -->            
            </div>
        </div>
    </div>
</template>

<script>
    import SearchBar from "../components/home/SearchBar";    
    import QuestionList from "../components/home/QuestionList";

    export default {
        components: { SearchBar, QuestionList },        
    }
</script>
