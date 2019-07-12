function QuestionPageRequests(baseUrl) {
    const _baseUrl = baseUrl;
    const _json = "application/json";

    this.createAnswer = (newTitle, newContents, qId, callback) => {
        $.ajax({
            url : `${_baseUrl}/api/createAnswer`
            , contentType : _json
            , method : "POST"
            , data : JSON.stringify({
                title : newTitle
                , contents : newContents
                , questionId : qId
            })
            , success : () => { callback(); }
        });
    };

    this.editQuestion = (newTitle, newContents, qId, callback) => {
        $.ajax({
            url : `${_baseUrl}/api/editQuestion`
            , contentType : _json
            , method : "PUT"
            , data : JSON.stringify({
                title : newTitle
                , contents : newContents
                , id : qId
            }),
            success : () => { callback(); }
        });
    };

    this.editAnswer = (newTitle, newContents, ansId, callback) => {
        $.ajax({
            url : `${_baseUrl}/api/editAnswer`
            , contentType : _json
            , method : "PUT"
            , data : JSON.stringify({
                title : newTitle
                , contents : newContents
                , id : ansId
            })
            , success : () => { callback(); }
        });
    }

    this.deleteAnswer = (ansId, callback) => {
        $.ajax({
            url : `${_baseUrl}/api/deleteAnswer/${ansId}`
            , method : "DELETE"
            , success : () => { callback(); }
        });
    };
}