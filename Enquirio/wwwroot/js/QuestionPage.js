window.onload = () => {

    // Global vars
    const _questionId = document.getElementById("qid").innerText;
    const _baseUrl = document.getElementById("basePath").innerText.trim();

    // Elements
    const _newAnswerForm = document.getElementById("answerForm");
    const _questionBtns = document.getElementById("questionBtns");

    // Buttons
    const _showAnswerFormBtn = document.getElementById("showAnsFormBtn");
    const _questionDeleteBtn = document.getElementById("deleteQuestionBtn");
    const _submitAnswer = document.getElementById("submitAnswer");

    // Show create answer form
    _showAnswerFormBtn.onclick = () => {
        toggleElements(_newAnswerForm, _questionBtns);
    };

    // Submit answer
    _submitAnswer.onclick = () => {
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

    document.getElementById("hideAnsFormBtn").onclick = () => {
        toggleElements(_questionBtns, _newAnswerForm);
    };

    // Show one element, hide the other
    function toggleElements(toShow, toHide) {
        toShow.removeAttribute("style");
        toHide.setAttribute("style", "display: none;");
    }

};