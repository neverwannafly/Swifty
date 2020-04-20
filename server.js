const express = require('express');
const path = require('path');

const port = process.env.PORT || 3000;

const app = express();

app.use('/static', express.static(path.join(__dirname, 'public')));
app.use('/modules', express.static(path.join(__dirname, 'node_modules')));
app.set('view engine', 'pug');

app.get('/', (req, res) => {
    res.render('home', {title:'Swifty Compiler'});
});

app.listen(port, () => 
    console.log(`running on localhost:${port}`)
);