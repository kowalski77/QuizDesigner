(function () {
    const leftDivId = "left";
    const rightDivId = "right";
    const innerDivClass = "quiz-data-div-inner";
    var questions = [];

    // Global export
    window.blazorColumnData = {
        initialize: function () {
            dragula([document.getElementById(leftDivId), document.getElementById(rightDivId)])
                .on('drop', function (el, container) {
                    setQuestionAsDropped(el.id, container);
                });
        },
        addQuestion: function (id, text, tag) {
            if (questions.find(x => x.id === id) === undefined) {
                const question = { id: id, text: text, tag: tag, dropped: false };
                questions.push(question);
            }
        },
        showQuestions: function (tag) {
            clearDivContent();
            const questionsByTag = questions.filter(x => x.tag === tag && !x.dropped);
            questionsByTag.forEach(question => {
                createDivQuestion(question);
            });
        },
        clearQuestions: function() {
            questions = [];
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

    function setQuestionAsDropped(questionId, container) {
        const question = questions.find(x => x.id === questionId);
        if (container.id === rightDivId) {
            question.dropped = true;
        } else {
            question.dropped = false;
        }
    }
})();