<%@include file="templates/header.jsp"%>

<br><br>
<div class="row justify-content-center">
    <div class="col-sm-6">
        <form method="POST" action="doEditQuestion">
            <h4>Edit Question</h4><br>

            <div class="form-group">
                <label for="title">Title</label>
                <input type="text" id="title" class="form-control" name="title" value="${question.title}">
            </div>

            <div class="form-group mt-4">
                <label for="content">Question</label>
                <textarea type="text" id="content" class="form-control" rows="6" name="content">${question.title}</textarea>
            </div>

            <%--TODO remove hidden feilds--%>
            <input style="display: none;" type="text" name="id" id="id" value="${question.id}">

            <button class="btn btn-outline-dark float-right">Edit</button>
        </form>


    </div>
</div>



<%@include file="templates/footer.jsp"%>