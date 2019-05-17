<%@include file="templates/header.jsp"%>

<br><br>
<div class="row justify-content-center">
    <div class="col-sm-6">
        <h4>Search Questions</h4><br>

        <form>
            <div class="form-group">
                <label for="keywords">Key Words</label>
                <input type="text" id="keywords" class="form-control form-control-sm">
            </div>
            <div class="form-group">
                <label for="zip">Zip Code</label>
                <input type="text" id="zip" class="form-control form-control-sm">
            </div>
            <div class="form-group">
                <label for="radius">Within</label>
                <select id="radius" class="form-control form-control-sm">
                    <option value="5">5</option>
                    <option value="10">10</option>
                    <option value="25">25</option>
                    <option value="50">50</option>
                    <option value="100">100</option>
                    <option value="150">150</option>
                    <option value="250">250</option>
                    <option value="anywhere">Any Distance</option>
                </select>
            </div>

            <button class="btn btn-outline-dark" style="float:right;">Search</button>
        </form>

    </div>
</div>

<%@include file="templates/footer.jsp"%>