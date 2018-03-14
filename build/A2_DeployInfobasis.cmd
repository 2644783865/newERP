
robocopy "C:\Work\NET2\Infobasis\Infobasis" "C:\Work\Build\Infobasis" ^
    /S ^
    /XF *.cs *.sln *.user *.*proj *.cmd *.Settings *.nupkg ^
    /XD obj _* .svn ^
    /XX
 
pause

::web.config app.js QF.config log4net.xml