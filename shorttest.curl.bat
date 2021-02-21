@echo off

REM --------------------------------------------------
REM Party Playlist Battle
REM --------------------------------------------------
title Party Playlist Battle
echo CURL Testing for Party Playlist Battle
echo.





REM --------------------------------------------------
echo 14) set actions (kienboec)
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"actions\": \"RRRRR\"}"
curl -X GET http://localhost:10001/actions --header "Authorization: Basic kienboec-ppbToken"

echo set actions (altenhof):
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"actions\": \"SSSSS\"}"
curl -X GET http://localhost:10001/actions --header "Authorization: Basic altenhof-ppbToken"

echo.
echo.

REM --------------------------------------------------
echo 15) battle (kienboec starts the 15 seconds tournament)
start /b "kienboec battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic kienboec-ppbToken"
start /b "altenhof battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic altenhof-ppbToken"
ping localhost -n 20 >NUL 2>NUL
echo.
echo.


echo.

REM --------------------------------------------------
echo 19) set actions to simulate a draw
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"actions\": \"RRRRR\"}"
echo.
echo.

REM --------------------------------------------------
echo 20) battle (no admin)
start /b "kienboec battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic kienboec-ppbToken"
start /b "altenhof battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic altenhof-ppbToken"
ping localhost -n 20 >NUL 2>NUL
echo.
echo.

REM --------------------------------------------------
echo 21) reorder (should fail)
curl -X PUT http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"FromPosition\": 1, \"ToPosition\": 3}"
curl -X PUT http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"FromPosition\": 1, \"ToPosition\": 3}"
echo.
echo.


pause
@echo on
