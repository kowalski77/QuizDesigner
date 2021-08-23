(function () {
    const leftDivId = "left";
    const rightDivId = "right";
    const innerDivClass = "quiz-data-div-inner";
    const questionIds = [];

    // Global export
    window.blazorColumnData = {
        initialize: function () {
            dragula([document.getElementById(leftDivId), document.getElementById(rightDivId)]);
        },
        addQuestion: function (id, text) {
            if (questionIds.find(x => x.id === id) === undefined) {
                const question = { id: id, text: text };
                questionIds.push(question);
            }
        },
        showQuestions: function () {
            for (let i = 0; i < questionIds.length; i++) {
                const element = document.createElement("div");
                element.id = questionIds[i].id;
                element.appendChild(document.createTextNode(questionIds[i].text));
                element.classList.add(innerDivClass);
                const div = document.getElementById(leftDivId);
                div.appendChild(element);
            }
        },
        removeInnerDivs: function () {
            document.getElementById(leftDivId).innerHTML = '';
        }
    };
})();