(function() {
    const leftDivId = "left";
    const rightDivId = "right";
    const innerDivClass = "quiz-data-div-inner";
    const innerDivCloseClass = "div-inner-close";

    // Global export
    window.blazorColumnData = {
        initialize: function() {
            dragula([document.getElementById(leftDivId), document.getElementById(rightDivId)],
            {
                accepts: function(el, target) {
                    const elements = target.getElementsByClassName(innerDivClass);
                    for (let i = 0; i < elements.length; i++)
                    {
                        if (elements[i].id === el.id) {
                            return false;
                        }
                    }
                    return true;
                }
            });
        },
        appendDivWithQuestion: function(id, text) {
            const element = document.createElement("div");
            element.id = id;
            element.appendChild(document.createTextNode(text));
            element.classList.add(innerDivClass);
            const div = document.getElementById(leftDivId);
            div.appendChild(element);
        },
        removeInnerDivs: function() {
            document.getElementById(leftDivId).innerHTML = '';
        }
    };
})();