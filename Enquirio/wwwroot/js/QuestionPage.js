window.onload = () => {

    // Global vars
    const _questionId = document.getElementById("qid").innerText;
    const _baseUrl = document.getElementById("basePath").innerText.trim();

    // Buttons
    const _hideEditQ = document.getElementById("hideQEdit");
    const _hideNewA = document.getElementById("hideAnsFormBtn");
    // const _hideEditA = document.getElementById("");

    // Elements
    const _newAnswerForm = document.getElementById("answerForm");
    const _questionBtns = document.getElementById("questionBtns");
    const _editQForm = document.getElementById("qEditForm");
    const _qTextBtns = [document.getElementById("questionText"), _questionBtns];
    const _qTitle = document.querySelector("#questionText > h4");
    const _qContents = document.querySelector("#questionText > p");

    // Submit answer
    document.getElementById("submitAnswer").onclick = () => {
        const newTitle = document.getElementById("newAnsTitle").value;
        const newContent = document.getElementById("newAnsContent").value;

        $.ajax({
            url : _baseUrl + "/api/createAnswer/" + _questionId
            , contentType : "application/json"
            , method : "POST"
            , data : JSON.stringify({
                title : newTitle
                , contents : newContent
            })
            , success : (e) => { location.reload(); }
        });
    };

    // Edit question
    document.getElementById("submitQEdit").onclick = () => {
        const newTitle = document.getElementById("editQTitle").value;
        const newContent = document.getElementById("editQContents").value;

        $.ajax({
            url : _baseUrl + "/api/editQuestion"
            , contentType : "application/json"
            , method : "PUT"
            , data : JSON.stringify({
                title : newTitle
                , contents : newContent
                , id : _questionId
            })
            , success : () => {
                _qTitle.innerText = newTitle;
                _qContents.innerText = newContent;
                _hideEditQ.onclick();
            }
        });
    };

    // Hide edit question form
    _hideEditQ.onclick = () => {
        toggleElements(_qTextBtns, _editQForm);
    }

    // Show edit question form
    document.getElementById("showQEdit").onclick = () => {
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