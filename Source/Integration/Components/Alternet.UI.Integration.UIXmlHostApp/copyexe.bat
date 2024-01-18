set UIXmlHostAppFolder=C:\Documents and Settings\Sergiy\AppData\Local\Microsoft\VisualStudio\17.0_2de07248\Extensions\djsu3mpq.53s\UIXmlHostApp\
set exename=Alternet.UI.Integration.UIXmlHostApp.exe


set UIXmlHostAppFolder6=%UIXmlHostAppFolder%\net6.0\%exename%
set UIXmlHostAppFolder7=%UIXmlHostAppFolder%\net7.0\%exename%
set UIXmlHostAppFolder462=%UIXmlHostAppFolder%\net462\%exename%
set UIXmlHostAppFolder480=%UIXmlHostAppFolder%\net480\%exename%

ECHO %UIXmlHostAppFolder6%
ECHO %UIXmlHostAppFolder7%
ECHO %UIXmlHostAppFolder462%
ECHO %UIXmlHostAppFolder480%

del "%UIXmlHostAppFolder6%"
del "%UIXmlHostAppFolder7%"
del "%UIXmlHostAppFolder462%"
del "%UIXmlHostAppFolder480%"

set src="E:\DIMA\AlternetUI\Source\Integration\Components\Alternet.UI.Integration.UIXmlHostApp\bin\Debug\net6.0-windows\Alternet.UI.Integration.UIXmlHostApp.exe"

copy %src% "%UIXmlHostAppFolder6%"
copy %src% "%UIXmlHostAppFolder7%"
copy %src% "%UIXmlHostAppFolder462%"
copy %src% "%UIXmlHostAppFolder480%"
