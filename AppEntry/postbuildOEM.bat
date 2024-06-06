@echo off
SETLOCAL

REM Debug Echos

REM The arguments from MSBuild
SET TARGET_PATH=%~1
SET TARGET_DIR=%~2
SET TARGET_NAME=%~3
SET PUBLIC_DIR=%~4
SET APPDATA_DIR=%~5
SET CONFIGURATION=%~6

REM Set the paths to the tools and certificate
SET SIGNTOOL_PATH=C:\Users\Public\Documents\06_Zertifikat\signtool.exe
SET CERTIFICATE_PATH=C:\Users\Public\Documents\06_Zertifikat\TinLine.pfx
SET CERTIFICATE_PASSWORD=4ABs84iuh.s9C6LNNhNJbwYHwFGnAo
SET TIMESTAMP_URL=http://timestamp.digicert.com

SET VERSION=2025
SET OEM_PROJECT_NAME=TinCAD

ECHO "TARGET_PATH="%TARGET_PATH%
ECHO "TARGET_DIR="%TARGET_DIR%
ECHO "TARGET_NAME="%TARGET_NAME%
ECHO "PUBLIC_DIR="%PUBLIC_DIR%
ECHO "APPDATA_DIR="%APPDATA_DIR%
ECHO "CONFIGURATION"=%CONFIGURATION%


REM Check if the DLL exists
IF NOT EXIST "%TARGET_DIR%%TARGET_NAME%.dll" (
    ECHO DLL not found: "%TARGET_DIR%%TARGET_NAME%.dll"
    GOTO End
)

SET "INSTALL_DIRECTORY=%PUBLIC_DIR%\Documents\AutoCAD OEM %VERSION%\Build\%OEM_PROJECT_NAME%"
ECHO "INSTALL_DIRECTORY="%INSTALL_DIRECTORY%

REM Check if the target directory exists, if not, create it
IF NOT EXIST "%INSTALL_DIRECTORY%" (
    ECHO Install directory does not exist. Creating directory...
    GOTO End
)

SET "BINDTOOLEXE=C:\Program Files\Autodesk\AutoCAD OEM %VERSION% - English\Toolkit\bindmgd.exe"
SET "BINDTOOL_PATH=C:\Program Files\Autodesk\AutoCAD OEM %VERSION% - English\Toolkit\"

IF NOT EXIST "%BINDTOOLEXE%" (
    ECHO %BINDTOOLEXE%
    ECHO Bindtool exe e.g bindmgd.exe is missing...
    GOTO End
)

SET "SNK_FILE_PATH=%PUBLIC_DIR%\Documents\AutoCAD OEM %VERSION%\Projects\%OEM_PROJECT_NAME%\Toolkit\%OEM_PROJECT_NAME%.snk"
SET "DLL_FILE=%TARGET_DIR%%TARGET_NAME%.dll"

ECHO "DLL_FILE="%DLL_FILE%
ECHO "SNK_FILE_PATH="%SNK_FILE_PATH%

IF NOT EXIST "%SNK_FILE_PATH%" (
    ECHO %SNK_FILE_PATH%
    ECHO SNK File is missing...
    GOTO End
)

REM ---------------------------------------------------------------------
REM xcopy "%SNK_FILE_PATH%" "%TARGET_DIR%"
REM xcopy "%BINDTOOLEXE%" "%TARGET_DIR%"

REM Execute Bindtool
REM cd %TARGET_DIR%
REM bindmgd.exe -b %TARGET_NAME%.dll %OEM_PROJECT_NAME%.snk
REM ---------------------------------------------------------------------

REM set current directory
cd %BINDTOOL_PATH%

bindmgd.exe -b "%DLL_FILE%" "%SNK_FILE_PATH%"
REM xcopy "%BINDTOOL_PATH%%TARGET_NAME%.dll.sig" "%TARGET_DIR%"

REM Copy the DLL and dll.sig to the public documents folder
ECHO Copying DLL...
xcopy "%TARGET_DIR%%TARGET_NAME%.dll" "%INSTALL_DIRECTORY%" /F /R /Y /I
xcopy "%TARGET_DIR%%TARGET_NAME%.dll.sig" "%INSTALL_DIRECTORY%" /F /R /Y /I

REM xcopy "C:\Users\MarcoCovolan\Projects\Repos\TinLine.OEM25\AppPrototype\bin\%CONFIGURATION%\net8.0-windows\AppPrototype.dll" "%INSTALL_DIRECTORY%"  /F /R /Y /I
REM xcopy "C:\Users\MarcoCovolan\Projects\Repos\TinLine.OEM25\AppPrototype\bin\%CONFIGURATION%\net8.0-windows\AppInterface.dll" "%INSTALL_DIRECTORY%" /F /R /Y /I


REM xcopy "%TARGET_DIR%\CommunityToolkit.Mvvm.dll" "%LIBRARIES_DIRECTORY%" /F /R /Y /I
REM xcopy "%TARGET_DIR%\GongSolutions.WPF.DragDrop.dll" "%INSTALL_DIRECTORY%" /F /R /Y /I


IF ERRORLEVEL 1 (
    ECHO Failed to copy DLL to public documents folder
    GOTO End
)

REM Sign the target file
REM "%SIGNTOOL_PATH%" sign /f "%CERTIFICATE_PATH%" /p %CERTIFICATE_PASSWORD% /fd SHA256 /td SHA256 /tr "%TIMESTAMP_URL%" "%TARGET_PATH%"

:End

ENDLOCAL