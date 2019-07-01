window.onload = () => {

    const _newAnswerForm = document.getElementById("answerForm");
    const _showAnswerFormBtn = document.getElementById("showAnsFormBtn");

    _showAnswerFormBtn.onclick = () => {
        toggleElements(_newAnswerForm, _showAnswerFormBtn);
    };

    document.getElementById("hideAnsFormBtn").onclick = () => {
        toggleElements(_showAnswerFormBtn, _newAnswerForm);
    };

    // Show one element, hide the other
    function toggleElements(toShow, toHide) {
        toShow.removeAttribute("style");
        toHide.setAttribute("style", "display: none;");
    }

};