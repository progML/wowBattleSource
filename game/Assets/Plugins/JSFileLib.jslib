var JSFileInteractPlugin = {
    $StoredParams: {
        get: "",
        userkey: "",
    },
    CreateInputForm: function (id, format, route, callbackObject, callbackFunction) {
        var inputFile = document.createElement('input');
        inputFile.setAttribute('type', 'file');
        inputFile.setAttribute('id', id);
        inputFile.setAttribute('accept', format);
        inputFile.style.visibility = 'hidden';
        inputFile.onclick = function (event) {
            this.value = null;
        };
        inputFile.onchange = function (evt) {
            evt.stopPropagation();
            var fileInput = evt.target.files;
            if (!fileInput || !fileInput.length) {
                return null;
            }
            var formData = new FormData();
            formData.append("file", fileInput[0]);
            return;
        };
        document.getElementsByTagName("body")[0].insertAdjacentElement("beforeend", inputFile);
        return inputFile;
    },
    CreateInputFormFull: function (id, format, route, callbackObject, callbackFunction) {
        var inputFile = document.createElement('input');
        inputFile.setAttribute('type', 'file');
        inputFile.setAttribute('id', id);
        inputFile.setAttribute('accept', format);
        inputFile.style.visibility = 'hidden';
        inputFile.onclick = function (event) {
            this.value = null;
        };
        inputFile.onchange = function (evt) {
            evt.stopPropagation();
            var fileInput = evt.target.files;
            if (!fileInput || !fileInput.length) {
                return null;
            }
            var formData = new FormData();
            formData.append("file", fileInput[0]);
            fetch((route), {method: "PUT", body: formData, headers: {Authorization: StoredParams.userkey}}).then(function(response) {

                if (callbackObject && callbackFunction) {
                    gameInstance.SendMessage(callbackObject, callbackFunction);
                }
            });
            return;
        };
        document.getElementsByTagName("body")[0].insertAdjacentElement("beforeend", inputFile);
        return inputFile;
    },
    SendImage: function(rawroute){
        var route = Pointer_stringify(rawroute);
        var fileInput = document.getElementById("UserImageUpload").files;
        if (!fileInput || !fileInput.length) {
            return null;
        }
        var formData = new FormData();
        formData.append("file", fileInput[0]);
        fetch((route), {method: "PUT", body: formData, headers: {Authorization: StoredParams.userkey}}).then(function(response) {

            if (callbackObject && callbackFunction) {
                gameInstance.SendMessage(callbackObject, callbackFunction);
            }
        });
    },
    InitForms__deps: ['CreateInputForm', 'CreateInputFormFull'],
    InitForms: function (jsonParams) {
        var params = Pointer_stringify(jsonParams);
        _CreateInputForm("UserImageUpload", "image/jpeg", "", "[Network]", "OnImageUploadedJS");
        _CreateInputFormFull("UserImageUploadFull", "image/jpeg", params, "[Network]", "OnImageUploadedJS");
    },
    InjectUserKey: function(userkey)
    {
        StoredParams.userkey = "Bearer "+Pointer_stringify(userkey)
    },
    RequestImage: function () {
        document.getElementById('UserImageUpload').click();
    },
    FullUploadImage: function(){
        document.getElementById('UserImageUploadFull').click();
    }
};
autoAddDeps(JSFileInteractPlugin, '$StoredParams');
mergeInto(LibraryManager.library, JSFileInteractPlugin);