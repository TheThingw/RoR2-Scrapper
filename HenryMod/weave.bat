REM original version https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/C%23-Programming/Networking/UNet/
REM open this in vs it'll be so much nicer

REM call with ./weave.bat $(ConfigurationName)
echo "seting"
set Target=Scrapper.dll
set Output=bin\%1\netstandard2.1
set Libs=.\Weaver\libs

xcopy %Output%\%Target% %Output%\%Target%.prepatch /Y

REM le epic networking patch
.\Weaver\Unity.UNetWeaver.exe %Libs%\UnityEngine.CoreModule.dll %Libs%\com.unity.multiplayer-hlapi.Runtime.dll %Output% %Output%\%Target% %Libs%

REM del Weaver\%Target%
REM del %Output%\%Target%.prepatch

REM that's it. This is meant to pretend we just built a dll like any other time except this one is networked
REM add your postbuilds in vs like it's any other project