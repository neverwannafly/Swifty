const element = document.getElementById("editor");
const runButton = document.getElementById("run-icon");
const buildButton = document.getElementById("build-icon");

const codeMirrorClient = CodeMirror(element, config);

buildButton.addEventListener('click', function(){
    const code = codeMirrorClient.getValue();
    const lines = codeMirrorClient.lineCount();
    $.ajax({
        url: '/build',
        data: {
            data: code,
            lines: lines,
        },
        success: function(data) {
            console.log(data);
        },
        error: function(err) {
            console.log(err);
        }
    });
});

runButton.addEventListener('click', function(){
    console.log("Bob the builder! Kar ke dikhaenge!")
});