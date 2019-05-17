<%@include file="templates/header.jsp"%>


<br><br>
<div class="row">
    <div class="col-sm-9">

        <!-- Question -->
        <div class="row justify-content-center" id="question" data-auth="${question.user.id}" data-id="${question.id}">
            <div class="col-sm-8">

                <!-- Question title and contents -->
                <div id="qText">
                    <h4 id="qTitle">${question.title}</h4>
                    <p id="qContents">
                        ${question.contents}
                    </p>
                </div>

                <!-- Hidden edit form -->
                <div id="qEditForm" style="display: none;">

                    <br>
                    <div>
                        <input type="text" id="editQTitle" class="form-control" maxlength="50">
                        <small class="text-danger" id="editQTitleMsg" style="display: none;">Title must not be left blank</small>
                    </div>

                    <br>
                    <div>
                        <textarea id="editQContents" rows="5" class="form-control"></textarea>
                        <small class="text-danger" id="editQContentsMsg" style="display: none;">Contents must not be left blank</small>
                    </div>

                    <br>
                    <button class="btn btn-outline-info float-right" id="cancelQEdit">Cancel</button>
                    <button class="btn btn-outline-dark float-right mr-2" id="submitQEdit">Edit</button>

                </div>

                <br>
                <i>Asked by ${question.user.username} - ${timeSince}</i>

                <!-- Hidden answer form -->
                <div class="form-group" id="answerForm" style="display: none;">

                    <br>
                    <div>
                        <label for="submitAnswerTitle">Title</label>
                        <input type="text" id="submitAnswerTitle" class="form-control">
                        <small class="text-danger" id="answerTitleMsg" style="display: none;">Title must not be blank</small>
                    </div>

                    <br>
                    <div>
                        <label for="submitAnswerContents" class="mt-3">Response</label>
                        <textarea id="submitAnswerContents" class="form-control" rows="5"></textarea>
                        <small class="text-danger" id="answerContentsMsg" style="display: none;">Contents must not be blank</small>
                    </div>

                    <br>
                    <button class="btn btn-outline-info float-right" id="cancelAnswer">Cancel</button>
                    <button class="btn btn-outline-dark float-right mr-2" id="answerSubmitBtn">Submit</button>
                </div>

                <!-- Show answer form -->
                <div id="answerBtns">
                    <button class="btn btn-outline-dark float-right" id="answerBtn">Answer</button>
                </div>
                <br><br>

            </div>
        </div>

        <!-- Answers -->
        <div id="answers">

            <c:forEach var="answer" items="${question.answers}">
                <div class="mt-4 answer row justify-content-center" data-auth="${answer.user.id}" data-id="${answer.id}">
                    <div class="col-sm-8">
                        <hr>

                        <!-- Answer text -->
                        <div id="answerText${answer.id}" class="answerText">
                            <h4 id="answerTitle${answer.id}">${answer.title}</h4>
                            <p id="answerContents${answer.id}">
                                ${answer.contents}
                            </p>
                        </div>

                        <!-- Hidden edit answer form -->
                        <div class="aEditForm" id="aEditForm${answer.id}" style="display: none;">

                            <br>
                            <div>
                                <input type="text" id="editATitle${answer.id}" class="form-control" maxlength="50">
                                <small class="text-danger" id="editATitleMsg${answer.id}" style="display: none;">Title must not be left blank</small>
                            </div>

                            <br>
                            <div>
                                <textarea id="editAContents${answer.id}" rows="5" class="form-control"></textarea>
                                <small class="text-danger" id="editAContentsMsg${answer.id}" style="display: none;">Contents must not be left blank</small>
                            </div>

                            <br>
                            <button class="btn btn-outline-info float-right" id="cancelAEdit${answer.id}">Cancel</button>
                            <button class="btn btn-outline-dark float-right mr-2" id="submitAEdit${answer.id}">Edit</button>

                        </div>

                        <br>
                        <i class="text-secondary">&nbsp;&nbsp;Answered by ${answer.user.username} - ${answer.getTimeSince()}</i>

                        <!-- Append edit/delete answer buttons -->
                        <div class="answerEditDelete" id="answerEditDelete${answer.id}">
                        </div>

                    </div>
                </div>
            </c:forEach>



    </div>
</div>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>


<%@include file="js-jsp/individualQuestion-js.jsp"%>

<%@include file="templates/footer.jsp"%>