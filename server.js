const express = require('express');
const path = require('path');
const exec = require('child_process').exec;
const fs = require('fs');

const port = process.env.PORT || 3000;

const app = express();

app.use('/static', express.static(path.join(__dirname, 'public')));
app.use('/modules', express.static(path.join(__dirname, 'node_modules')));
app.set('view engine', 'pug');

app.get('/build', (req, res) => {
    let data = req.query.data;
    data = preprocessData(data);
    const command = "./swifty/bin/Release/netcoreapp3.1/osx-x64/publish/swifty --build";
    fs.writeFileSync('buffer.t', data);
    const child = exec(command);
    child.on('close', function(err, _){
        console.log(err);
        let result = JSON.parse(fs.readFileSync('result.json', 'utf8'));
        res.send({
            result: result.compilationResult,
            error: result.error,
        });
    });
});

app.get('/run', (req, res) => {
    let data = req.query.data;
    data = preprocessData(data);
    const command = "./swifty/bin/Release/netcoreapp3.1/osx-x64/publish/swifty --build";
    fs.writeFileSync('buffer.t', data);
    const child = exec(command);
    child.on('close', function(err, _){
        console.log(err);
        let result = JSON.parse(fs.readFileSync('result.json', 'utf8'));
        res.send(result);
    });
});

app.get('/', (_, res) => {
    res.render('home', {title:'Swifty Compiler'});
});

app.listen(port, () => 
    console.log(`running on localhost:${port}`)
);

function preprocessData(data) {
    let text = data.split("\n");
    let newData = '';
    for (let i=0; i<text.length; i++) {
        if (text[i] === '') {
            continue;
        }
        newData = (newData + text[i] + '\n');
    }
    newData = `{\n${newData}}\n\n`;
    return newData;
}