<%@include file="templates/header.jsp"%>

<br><br>
<div class="row justify-content-center">
    <div class="col-sm-6">
        <form method="POST" action="createQuestion">
            <h4>Ask Question</h4><br>

            <div class="form-group">
                <label for="title">Title</label>
                <input type="text" id="title" class="form-control" name="title">
            </div>

            <div class="form-group mt-4">
                <label for="question">Question</label>
                <textarea type="text" id="question" class="form-control" rows="6" name="content"></textarea>
            </div>

            <button class="btn btn-outline-dark" style="float:right;">Submit</button>
        </form>


    </div>
</div>

<script src="js/ask.js"></script>

<%@include file="templates/footer.jsp"%>