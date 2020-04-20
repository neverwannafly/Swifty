const element = document.getElementById("editor");
const runButton = document.getElementById("run-icon");
const buildButton = document.getElementById("build-icon");

const codeMirrorClient = CodeMirror(element, config);

runButton.addEventListener('click', function(){
    console.log("hello");
});

buildButton.addEventListener('click', function(){
    console.log("Bob the builder! Kar ke dikhaenge!")
});