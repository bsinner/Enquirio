window.onload = () => {

    // Global vars
    const _questionId = document.getElementById("qid").innerText;
    const _baseUrl = document.getElementById("basePath").innerText.trim();
    const _answerIdPrefix = "an";

    // Buttons
    const _hideEditQ = document.getElementById("hideQEdit");
    const _hideNewA = document.getElementById("hideAnsFormBtn");

    // Elements
    const _newAnswerForm = document.getElementById("answerForm");
    const _questionBtns = document.getElementById("questionBtns");
    const _editQForm = document.getElementById("qEditForm");
    const _editQTitle = document.getElementById("editQTitle");
    const _editQContents = document.getElementById("editQContents");
    const _qTextBtns = [document.getElementById("questionText"), _questionBtns];
    const _qTitle = document.querySelector("#questionText > h4");
    const _qContents = document.querySelector("#questionText > p");


    // Add handlers to answers
    (function setUpAnswers() {
        document.querySelectorAll(".answer").forEach(elem => {

            // Elements
            const id = "#" + elem.getAttribute("id");
            const idInt = id.replace("#" + _answerIdPrefix, "");
            const form = document.querySelector(id + " .answerEditForm");
            const show = document.querySelector(id + " .editAnsBtn");
            const hide = document.querySelector(id + " .hideEditAnswer");
            const titleIn = form.querySelector(".editAnswerTitle");
            const contentsIn = form.querySelector(".editAnswerContents");
            const txtAndBtns = [
                document.querySelector(id + " .answerText")
                , document.querySelector(id + " .answerBtns") ];
            const titleOrig = txtAndBtns[0].querySelector("h4");
            const contentsOrig = txtAndBtns[0].querySelector("p");

            // Set up show hide buttons
            addShowEditAnswer(show, form, txtAndBtns, () => {
                titleIn.value = titleOrig.innerText;
                contentsIn.value = contentsOrig.innerText;
            });
            addHideEditAnswer(hide, form, txtAndBtns);

            // Submit button event handler
            form.querySelector(".submitEditAnswer").onclick = () => {
                editAnswerAjax(titleOrig, contentsOrig, titleIn.value, contentsIn.value
                    , idInt, hide.onclick);
            };

            // Delete button event handler
            txtAndBtns[1].querySelector(".deleteAnsBtn").onclick = () => {
                deleteAnswer(idInt, elem);
            }
        });
    })();

    // Submit answer
    document.getElementById("submitAnswer").onclick = () => {
        const newTitle = document.getElementById("newAnsTitle").value;
        const newContent = document.getElementById("newAnsContent").value;

        $.ajax({
            url : _baseUrl + "/api/createAnswer"
            , contentType : "application/json"
            , method : "POST"
            , data : JSON.stringify({
                title : newTitle
                , contents : newContent
                , questionId : _questionId
            })
            , success : (e) => { location.reload(); }
        });
    };

    // Edit question
    document.getElementById("submitQEdit").onclick = () => {
        const newTitle = _editQTitle.value;
        const newContents = _editQContents.value;

        $.ajax({
            url : _baseUrl + "/api/editQuestion"
            , contentType : "application/json"
            , method : "PUT"
            , data : JSON.stringify({
                title : newTitle
                , contents : newContents
                , id : _questionId
            })
            , success : () => {
                _qTitle.innerText = newTitle;
                _qContents.innerText = newContents;
                _hideEditQ.onclick();
            }
        });
    };

    // Submit an answer edit, hide is the hide edit form's onclick value
    function editAnswerAjax(origTitle, origContents, newTitle, newContents, anId, hide) {
        $.ajax({
            url : _baseUrl + "/api/editAnswer"
            , contentType : "application/json"
            , method : "PUT"
            , data : JSON.stringify({
                title : newTitle
                , contents : newContents
                , id : anId
            })
            , success: () => {
                origTitle.innerText = newTitle;
                origContents.innerText = newContents;
                hide();
            }
        });
    }

    // Delete an answer
    function deleteAnswer(id, answer) {
        $.ajax({
            url : _baseUrl + "/api/deleteAnswer/" + id
            , contentType : "application/json"
            , method : "DELETE"
            , success : () => {
                answer.parentNode.removeChild(answer);
            }
        });
    }

    // Hide edit question form
    _hideEditQ.onclick = () => {
        toggleElements(_qTextBtns, _editQForm);
    }

    // Show edit question form
    document.getElementById("showQEdit").onclick = () => {
        _editQTitle.value = _qTitle.innerText;
        _editQContents.value = _qContents.innerText;
        toggleElements(_editQForm, _qTextBtns);
    }

    // Hide create answer form
    _hideNewA.onclick = () => {
        toggleElements(_questionBtns, _newAnswerForm);
    };

    // Show create answer form
    document.getElementById("showAnsFormBtn").onclick = () => {
        toggleElements(_newAnswerForm, _questionBtns);
    };

    // Show answer's edit form
    function addShowEditAnswer(btn, form, txtAndBtns, callback) {
        btn.onclick = () => {
            callback();
            toggleElements(form, txtAndBtns);
        };
    };

    // Hide answer's edit form
    function addHideEditAnswer(btn, form, txtAndBtns) {
        btn.onclick = () => {
            toggleElements(txtAndBtns, form);
        };
    };

    // Show one element, hide the other
    // params can be one element or an array of elements
    function toggleElements(toShow, toHide) {
        const show = e => e.removeAttribute("style");
        const hide = e => e.setAttribute("style", "display: none;");

        if (toShow instanceof Array) {
            toShow.forEach(e => { show(e); });
        } else {
            show(toShow);
        }

        if (toHide instanceof Array) {
            toHide.forEach(e => { hide(e); });
        } else {
            hide(toHide);
        }
    }

    // Clear form errors
    function clearErrState() {}
};