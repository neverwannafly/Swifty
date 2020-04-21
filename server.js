const express = require('express');
const path = require('path');
const exec = require('child_process').exec;
const fs = require('fs');
const rateLimit = require('express-rate-limit');
const config = require('./config');

const port = process.env.PORT || 3000;

const app = express();
const limiter = new rateLimit({
    windowMs: 1*60*1000, // 1 minute
    max: 120,
});

app.use(limiter);

app.use('/static', express.static(path.join(__dirname, 'public')));
app.use('/modules', express.static(path.join(__dirname, 'node_modules')));
app.set('view engine', 'pug');

app.get('/build', (req, res) => {
    let data = req.query.data;
    if (data != '') {
        data = preprocessData(data);
        const command = `${config.exec_path} --build`;
        fs.writeFileSync(`${config.buffer_file}`, data);
        const child = exec(command);
        child.on('close', function(err, _){
            console.log(err);
            let result = JSON.parse(fs.readFileSync('result.json', 'utf8'));
            res.send({
                result: result.compilationResult,
                error: result.error,
            });
        });
    }
});

app.get('/run', (req, res) => {
    let data = req.query.data;
    if (data != '') {
        data = preprocessData(data);
        const command = `${config.exec_path} --build`;
        fs.writeFileSync(`${config.buffer_file}`, data);
        const child = exec(command);
        child.on('close', function(err, _){
            console.log(err);
            let result = JSON.parse(fs.readFileSync('result.json', 'utf8'));
            res.send(result);
        });
    }
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