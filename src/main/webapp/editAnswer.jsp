<%@include file="templates/header.jsp"%>

<br><br>
<div class="row justify-content-center">
    <div class="col-sm-6">
        <form method="POST" action="doEditAnswer">
            <h4>Edit Answer</h4><br>

            <div class="form-group">
                <label for="title">Title</label>
                <input type="text" id="title" class="form-control" name="title" value="${answer.title}">
            </div>

            <div class="form-group mt-4">
                <label for="answer">Answer</label>
                <textarea type="text" id="answer" class="form-control" rows="6" name="content">${answer.title}</textarea>
            </div>

            <input type="text" style="display:none;" id="id" name="id" value="${answer.id}">
            <button class="btn btn-outline-dark float-right">Edit</button>
        </form>


    </div>
</div>



<%@include file="templates/footer.jsp"%>