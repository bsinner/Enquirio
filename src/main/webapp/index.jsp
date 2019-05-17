<%@include file="templates/header.jsp"%>

<!-- ---------- -->
<!-- BEGIN FORM -->
<!-- ---------- -->
<br><br>
<form class="mt-2">
    <div class="form-group row justify-content-center">

        <label for="zip" class="col-sm-1 col-form-label px-0 pt-1 text-right ">Zip Code</label>
        <div class="col-sm-3">
            <input type="text" class="form-control form-control-sm" id="zip">
        </div>

        <label for="radius" class="col-sm-1 col-form-label px-0 pt-1 text-right">Within</label>
        <div class="col-sm-2">
            <select type="text" class="form-control form-control-sm" id="radius">
                <option value="5">5 miles</option>
                <option value="10">10 miles</option>
                <option value="25">25 miles</option>
                <option value="50">50 miles</option>
                <option value="100">100 miles</option>
                <option value="150">150 miles</option>
                <option value="250">250 miles</option>
                <option value="anywhere">All Questions</option>
            </select>
        </div>

    </div>
</form>

<!-- ------------- -->
<!-- BEGIN RESULTS -->
<!-- ------------- -->
<br>
<div class="row justify-content-center">
    <div class="col-sm-11">
        <h5 id="resultsTitle" class="text-center">Recent Questions Within X miles of XXXXX</h5>
        <hr>
        <div class="row justify-content-center">
            <div class="col-sm-9">

                <c:forEach var="question" items="${questions}">

                    <h6>${question.title}</h6>

                    <p class="mb-4">
                        ${question.contents}
                        <a href="question?id=${question.id}">(More...)</a>
                        <i class="text-secondary">&nbsp;&nbsp;&nbsp;${question.user.username}</i>
                    </p>
                    <hr>

                </c:forEach>

            </div>
        </div>
    </div>
</div>

<%@include file="templates/footer.jsp"%>