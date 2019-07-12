// Function for displaying errors and showing hidden elements
function QuestionPageUtils() {

    // Show one element, hide the other.
    // Params can be one element or an array of elements
    this.toggleElements = (toShow, toHide) => {
        loopOrSingle(toShow, show);
        loopOrSingle(toHide, hide);
    }

    // Highlight empty inputs and show error message, return true if errors
    // where found. Param must be an array of objects in the following format:
    // [ { elem : inputElem, msg : "optional error message" } ]
    this.errIfEmpty = inputs => {
        let results = false;

        inputs.forEach(obj => {
            let input = obj.elem;

            if (input.value === null || input.value === "") {

                results = true;
                input.value = ""; // Clear input in case of whitespace
                const small = input.parentElement.querySelector("small");

                if ("msg" in obj) {
                    this.showError(input, small, msg);
                } else {
                    this.showError(input, small);
                }
            }
        });

        return results;
    }

    // Highlight input, show error message. Error message can be
    // changed through optional msg parameter
    this.showError = (input, small, msg = null) => {
        if (msg) {
            small.innerText = msg;
        }

        addInputErr(input);
        show(small);

        input.oninput = () => {
            removeInputErr(input);
            hide(small);
        }
    }

    // Hide all of a divs small elements containing error class text-danger, 
    // un-highlight form fields with error class is-invalid
    this.clearErrors = div => {
        div.querySelectorAll("small.text-danger").forEach(elem => {
            hide(elem);
        });

        div.querySelectorAll(".is-invalid").forEach(elem => {
            removeInputErr(elem);
        });
    }

    function show(e) { e.removeAttribute("style"); }

    function hide(e) { e.setAttribute("style", "display: none;"); }

    function addInputErr(i) { i.classList.add("is-invalid"); }

    function removeInputErr(i) { i.classList.remove("is-invalid"); }

    function loopOrSingle(item, callback) {
        if (item instanceof Array) {
            item.forEach(i => {
                callback(i);
            });
        } else {
            callback(item);
        }
    }
}