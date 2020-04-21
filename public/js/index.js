const runButton = document.getElementById("run-icon");
const buildButton = document.getElementById("build-icon");
const editor = document.getElementById("editor");
const output = document.getElementById("output")
const tree = document.getElementById("tree")

CodeMirror.defineSimpleMode('swifty-mode', swiftyMode);

const editorClient = CodeMirror(editor, editorConfig);
$("#editor").children()[0].id = "editor-client";

const outputClient = CodeMirror(output, outputConfig);
$("#output").children()[0].id = "output-client";

const treeClient = CodeMirror(tree, treeConfig);
$("#tree").children()[0].id = "tree-client";

buildButton.addEventListener('click', function(){
    const code = editorClient.getValue();
    const lines = editorClient.lineCount();
    $.ajax({
        url: '/build',
        data: {
            data: code,
            lines: lines,
        },
        success: function(data) {
            let text = ``;
            if (data.result) {
                text = text + `${data.result[data.result.length-1]}\n`;
            }
            if (data.error) {
                text = text + `${data.error.join("\n")}`;
            }
            outputClient.setValue(text);
            treeClient.setValue('');
        },
        error: function(err) {
            console.log(err);
        }
    });
});

runButton.addEventListener('click', function(){
    const code = editorClient.getValue();
    const lines = editorClient.lineCount();
    $.ajax({
        url: '/run',
        data: {
            data: code,
            lines: lines,
        },
        success: function(data) {
            let text = ``;
            if (data.compilationResult) {
                text = text + `${data.compilationResult[data.compilationResult.length-1]}\n`;
            }
            if (data.error) {
                text = text + `${data.error.join("\n")}\n`;
            } else {
                text = text + `${data.result.join("\n")}\n`;
            }
            let tree = `${data.syntaxTree.join("\n")}\n`;
            outputClient.setValue(text);
            treeClient.setValue(tree);
        },
        error: function(err) {
            console.log(err);
        }
    });
});