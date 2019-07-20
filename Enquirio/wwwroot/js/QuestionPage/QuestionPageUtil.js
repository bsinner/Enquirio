// Class for showing errors and showing elements
export default class QuestionPageUtil {

    constructor() {
        this.helper = new Helper();
    }

    // Show one element, hide the other.
    // Params can be one element or an array of elements
    toggleElements(toShow, toHide) {
        this.helper.loopOrSingle(toShow, show);
        this.helper.loopOrSingle(toHide, hide);
    }

    // Highlight empty inputs and show error message, return true if errors
    // where found. Param must be an array of objects in the following format:
    // [ { elem : inputElem, msg : "optional error message" } ]
    errIfEmpty(inputs) {
        let results = false;

        inputs.forEach(obj => {
            let input = obj.elem;

            if (input.value === null || input.value === "") {

                results = true;
                input.value = ""; // Clear input in case of whitespace
                const small = input.parentElement.querySelector("small");

                if ("msg" in obj) {
                    showError(input, small, msg);
                } else {
                    showError(input, small);
                }
            }
        });

        return results;
    }

    // Highlight input, show error message. Error message can be
    // changed through optional msg parameter
    showError(input, small, msg = null) {
        if (msg) {
            small.innerText = msg;
        }

        this.helper.addInputErr(input);
        this.helper.show(small);

        input.oninput = () => {
            this.helper.removeInputErr(input);
            this.helper.hide(small);
        }
    }

    // Hide all of a divs small elements containing error class text-danger, 
    // un-highlight form fields with error class is-invalid
    clearErrors(div) {
        div.querySelectorAll("small.text-danger").forEach(elem => {
            this.helper.hide(elem);
        });

        div.querySelectorAll(".is-invalid").forEach(elem => {
            this.helper.removeInputErr(elem);
        });
    }
}

class Helper {
    show(e) { e.removeAttribute("style"); }

    hide(e) { e.setAttribute("style", "display: none;"); }

    addInputErr(i) { i.classList.add("is-invalid"); }

    removeInputErr(i) { i.classList.remove("is-invalid"); }

    loopOrSingle(item, callback) {
        if (item instanceof Array) {
            item.forEach(i => {
                callback(i);
            });
        } else {
            callback(item);
        }
    }
}
