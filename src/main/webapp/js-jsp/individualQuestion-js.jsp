<script>

$(document).ready(function() {

    /*
    * Add edit and delete buttons
    */
    loadEditDeleteButtons();
    setUpQuestion();

    /*
     * Set up the post new answer form
     */
    function setUpQuestion() {
        const answerFormElems = ["answerForm"];
        const answerBtnElems = ["answerBtns"];
        const answerBtn = document.getElementById("answerBtn");
        const title = document.getElementById("submitAnswerTitle");
        const submitBtn = document.getElementById("answerSubmitBtn");
        const contents = document.getElementById("submitAnswerContents");

        // Show post answer form
        answerBtn.onclick = () => { hideShow(answerBtnElems, answerFormElems); };

        // Hide post answer form
        document.getElementById("cancelAnswer").onclick = () => {
            hideShow(answerFormElems, answerBtnElems);
        };

        // Submit an answer
        submitBtn.onclick = () => {

            if (title.value.length === 0 || contents.value.length === 0) {
                showErrorState({
                    "submitAnswerTitle" : "answerTitleMsg"
                    , "submitAnswerContents" : "answerContentsMsg"
                });
                return;
            }

            fetch("<%=request.getContextPath()%>/api/answer?q_id=${question.id}", {
                credentials : "same-origin"
                , method : "POST"
                , headers : {
                    "answer-title" : title.value
                    , "answer-contents" : contents.value
                }
            }).then(res => {
                if (!res.ok) return JSON.parse("{}");
                return res.json()
            }).then(data => {
                location.reload(true);
            });

        };
    }

    /*
     * Show edit delete buttons if needed
     */
    function loadEditDeleteButtons() {

        fetch("<%=request.getContextPath()%>/api/currUserId", {credentials : "same-origin"})
                .then(res => {
                    if (!res.ok) {
                        return "";
                    }
                    return res.text();
                }).then(id => {
                    if (id === "") return;
                    addQuestionButtons(id);
                    addAnswerButtons(id);
                });

    }

    // Add buttons to the page's question if needed
    function addQuestionButtons(uId) {
        const qe = document.getElementById("question");
        const qId = qe.getAttribute("data-id");
        const qFk = qe.getAttribute("data-auth");

        if (qFk === uId) {
            $(
                "<button class='btn btn-outline-info mr-2 float-right' id='qEdit'>Edit</button>" +
                "<button class='btn btn-outline-info mr-2 float-right' id='qDel'>Delete</button>"
            ).appendTo("#answerBtns");

            const edit = document.getElementById("qEdit");
            const del = document.getElementById("qDel");

            addEditDeleteQuestionHandlers(edit, del, qId);
        }
    }

    // Loop all answers, add buttons if needed
    function addAnswerButtons(id) {

        document.querySelectorAll(".answer").forEach((item) => {
            const fk = item.getAttribute("data-auth");

            if (id === fk) {
                const answerId = item.getAttribute("data-id");
                setUpOneAnswerBtnGroup(answerId);
            }

        });
    }

    // Append buttons to passed in answer
    function setUpOneAnswerBtnGroup(answerId) {
        const editId = "aEdit" + answerId;
        const deleteId = "aDel" + answerId;

        $(
            "<button class='btn float-right btn-outline-info' id='" + editId + "'>Edit</button>" +
            "<button class='btn float-right btn-outline-info mr-2' id='" + deleteId + "'>Delete</button>"
        ).appendTo("#answerEditDelete" + answerId);

        setUpAnswerBtnHandlers(editId, deleteId, answerId);
    }

    /*
     * Set up edit/delete for a passed in question
     */
    function addEditDeleteQuestionHandlers(editBtn, delBtn, qId) {
        const inputTitle = document.getElementById("editQTitle");
        const inputContents = document.getElementById("editQContents");
        const originalTitle = document.getElementById("qTitle");
        const originalContents = document.getElementById("qContents");
        const textElems = ["qText", "answerBtns"];
        const formElems = ["qEditForm"];

        // Show edit form
        editBtn.onclick = () => {
            inputTitle.setAttribute("value", originalTitle.innerText);
            inputContents.innerText = originalContents.innerText;
            hideShow(textElems, formElems);
        };

        // Hide edit form
        document.getElementById("cancelQEdit").onclick = () => {
            hideShow(formElems, textElems);
        };

        // Submit an edit
        document.getElementById("submitQEdit").onclick = () => {

            if (inputTitle.value.length === 0 || inputContents.value.length === 0) {
                showErrorState({
                    "editQTitle" : "editQTitleMsg"
                    , "editQContents" : "editQContentsMsg"
                });
                return;
            }

            const url = "<%=request.getContextPath()%>/api/editQuestion?q_id=" + qId;
            const props = {
                credentials : "same-origin"
                , method : "POST"
                , headers : {
                    "question-title" : inputTitle.value
                    , "question-contents" : inputContents.value
                }
            };

            // Submit the edit, update the page, hide the form
            fetch(url, props)
                .then(res => {
                    if (!res.ok) return JSON.parse("{}");
                    return res.json();
                })
                .then(data => {
                    if (Object.keys(data).length > 1) {
                        originalTitle.innerText = data.title;
                        originalContents.innerText = data.contents;
                    }
                    hideShow(formElems, textElems);
                });
        };

        // Delete an element
        delBtn.onclick = () => {
            const url = "<%=request.getContextPath()%>/api/deleteQuestion?q_id=" + qId;
            const props = { credentials : "same-origin", method : "DELETE" };

            fetch(url, props)
                    .then(res => res.ok)
                    .then(status => {
                        if (status) window.location = "home";
                        // TODO: handling of errors
                    })
        };
    }

    /*
     * Answer editing functions
     */
    function setUpAnswerBtnHandlers(editId, deleteId, id) {
        const inputTitle = document.getElementById("editATitle" + id);
        const inputContents = document.getElementById("editAContents" + id);
        const originalTitle = document.getElementById("answerTitle" + id);
        const originalContents = document.getElementById("answerContents" + id);
        const textElems = ["answerText" + id, "answerEditDelete" + id];
        const formElems = ["aEditForm" + id];

        // Show the edit answer form
        document.getElementById(editId).onclick = () => {
            inputTitle.setAttribute("value", originalTitle.innerText);
            inputContents.innerText = originalContents.innerText;
            hideShow(textElems, formElems);
        };

        // Submit an edited answer
        document.getElementById("submitAEdit" + id).onclick = () => {

            if (inputTitle.value.length === 0 || inputContents.value === 0) {
                showErrorState({
                    ["editATitle" + id] : "editATitleMsg" + id
                    , ["editAContents" + id] : "editAContentsMsg" + id
                });
                return;
            }

            const url = "<%=request.getContextPath()%>/api/editAnswer?a_id=" + id;
            const props = {
                credentials : "same-origin"
                , method : "POST"
                , headers : {
                    "answer-title" : inputTitle.value
                    , "answer-contents" : inputContents.value
                }
            };

            fetch(url, props)
                    .then(res => {
                        if (!res.ok) return JSON.parse("{}");
                        return res.json();
                    })
                    .then(data => {
                        if (Object.keys(data).length > 1) {
                            originalTitle.innerText = data.title;
                            originalContents.innerText = data.contents;
                        }
                        hideShow(formElems, textElems);
                    });
        };

        // Cancel editing an answer
        document.getElementById("cancelAEdit" + id).onclick = () => {
            hideShow(formElems, textElems);
        };

        // Delete an answer
        document.getElementById(deleteId).onclick = () => {
            const url = "<%=request.getContextPath()%>/api/deleteAnswer?a_id=" + id;
            const props = { credentials : "same-origin", method : "DELETE" };

            fetch(url, props)
                    .then(res => res.ok)
                    .then(status => {
                        if (status) {
                            $(".answer[data-id='" + id + "']").remove();
                        }
                        // TODO: add error handling
                    });
        };
    }

    /*
     * Highlight inputs and show their messages. Pass object
     * in this format { input_id : input_msg_id }
     */
    function showErrorState(inputs) {
        Object.keys(inputs).forEach(k => {
            const input = $("#" + k);
            const msg = document.getElementById(inputs[k]);

            input.addClass("is-invalid");
            msg.removeAttribute("style");

            input.on("input", () => {
                input.removeClass("is-invalid");
                msg.setAttribute("style", "display: none;");
            });
        });
    }

    /*
     * Hide/show elements by adding display:none; to style attribute,
     * or removing style attribute. Inputs must be array of css id
     * names to hide/show. Empty arrays can be passed in to preform only a hide or show
     */
    function hideShow(hide, show) {
        if (hide.length > 0) {
            hide.forEach(e => {
                document.getElementById(e)
                    .setAttribute("style", "display: none;");
            });
        }

        if (show.length > 0) {
            show.forEach(e => {
                document.getElementById(e).removeAttribute("style");
            });
        }
    }

});
</script>