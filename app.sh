#!/bin/bash

if [ $1 = run ]
then
    dotnet run --project swifty
elif [ $1 = build ]
then
    dotnet build
elif [ $1 = test ]
then
    dotnet test
elif [ $1 = publish ]
then
    dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true 
elif [ $1 = server ]
then
    killall node && nodemon server.js & sleep 0.5 && open -a "Safari.app" 'http://localhost:3000'
fi
