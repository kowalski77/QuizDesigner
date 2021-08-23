(function () {
    const leftDivId = "left";
    const rightDivId = "right";
    const innerDivClass = "quiz-data-div-inner";
    const questions = [];

    // Global export
    window.blazorColumnData = {
        initialize: function () {
            dragula([document.getElementById(leftDivId), document.getElementById(rightDivId)]);
        },
        addQuestion: function (id, text, tag) {
            if (questions.find(x => x.id === id) === undefined) {
                const question = { id: id, text: text, tag: tag };
                questions.push(question);
            }
        },
        showQuestions: function (tag) {
            clearDivContent();
            for (let i = 0; i < questions.length; i++) {
                if (questions[i].tag === tag) {
                    createDivQuestion(questions[i]);
                }
            }
        }
    };

    function clearDivContent() {
        document.getElementById(leftDivId).innerHTML = '';
    }

    function createDivQuestion(question) {
        const element = document.createElement("div");
        element.id = question.id;
        element.appendChild(document.createTextNode(question.text));
        element.classList.add(innerDivClass);
        const div = document.getElementById(leftDivId);
        div.appendChild(element);
    }
})();