@echo off
cd StarshipProject
start cmd /k "dotnet run"
cd ./ClientApp
start cmd /k "ng serve"
