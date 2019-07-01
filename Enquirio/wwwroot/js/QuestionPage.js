window.onload = () => {

    // Elements
    const _newAnswerForm = document.getElementById("answerForm");
    const _questionBtns = document.getElementById("questionBtns");

    // Buttons
    const _showAnswerFormBtn = document.getElementById("showAnsFormBtn");

    _showAnswerFormBtn.onclick = () => {
        toggleElements(_newAnswerForm, _questionBtns);
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