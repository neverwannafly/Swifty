let editorConfig = {
    theme: "twilight",
    smartIndent: false,
    lineNumbers: true,
    autofocus: true,
};

let outputConfig = {
    theme : "twilight",
    readOnly: 'nocursor',
}

let treeConfig = {
    theme: "twilight",
    readOnly: "nocursor",
}

let swiftyMode =  {
    start: [
        {regex: /"(?:[^\\]|\\.)*?(?:"|$)/, token: "string"},
        {regex: /(function)(\s+)([a-z$][\w$]*)/, token: ["keyword", null, "variable-2"]},
        {regex: /(?:int|bool|const|to|if|for|while|else)\b/, token: "keyword"},
        {regex: /true|false|null/, token: "atom"},
        {regex: /0x[a-f\d]+|[-+]?(?:\.\d+|\d+\.?\d*)(?:e[-+]?\d+)?/i, token: "number"},
        {regex: /\/\/.*/, token: "comment"},
        {regex: /\/(?:[^\\]|\\.)*?\//, token: "variable-3"},
        {regex: /\/\*/, token: "comment", next: "comment"},
        {regex: /[-+\/:&!^}|{*=<>]+/, token: "operator"},
        {regex: /[\{\[\(]/, indent: true},
        {regex: /[\}\]\)]/, dedent: true},
        {regex: /[a-z$][\w$]*/, token: "variable"},
        {regex: /<</, token: "meta", mode: {spec: "xml", end: />>/}}
    ],
    comment: [
        {regex: /.*?\*\//, token: "comment", next: "start"},
        {regex: /.*/, token: "comment"}
    ],
    meta: {
        dontIndentStates: ["comment"],
        lineComment: "//"
    }
};