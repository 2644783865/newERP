@echo off

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Name the SQL scripts so that they alphabetically sort in the order in which you want 
:: to run them.
::
:: 01 My First Script.sql
:: 02 The second one.sql
:: 03 My third.sql
::
:: To use: open a command prompt and execute "UpdateDB" you will be prompted for the 
:: server name and database name. We assume that Integrated Security is being used,
:: hence we don't prompt for user names or passwords.
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

echo.
setlocal
pushd "%~dp0"

:: Project Title
set PROJECT_TITLE=Infobasis

:: Files to be deployed must match this filemask...
set SCRIPT_FILEMASK="*usp*.sql"^
,"ufn*.sql"

:: Default DB Server and Database can optionally be defined here...
set USER_DEFAULT=sa
set PASSWORD_DEFAULT=salary
set SERVER_DEFAULT=localhost
set DATABASE_DEFAULT=EasyHr
set SQLServerAuth_DEFAULT=y
set AUTHCONFIG=-E

:: Test that we can write to TEMP
set TEMPTEST=%TEMP%\%RANDOM%.tmp
echo Testing > "%TEMPTEST%"
if not exist "%TEMPTEST%" (
   echo.
   echo FATAL ERROR: Couldn't write to %%TEMP%% ^(%TEMP%^)
   echo.
   goto END
)
del "%TEMPTEST%"


:: Use newer SQL 2005 SQLCMD if available. 
:: * osql -n switch is for "no line numers" and only used by OSQL
:: * sqlcmd -x ignores special processing of "$(xyz)" scripting variables which may appear in our scripts

set SQLCMD=OSQL.exe -n

:: Search for sqlcmd.exe in PATH
for %%I in (sqlcmd.exe) do set SQLCMDPATH="%%~$PATH:I"

if exist %SQLCMDPATH% (
    echo [Info: using SQLCMD from '%SQLCMDPATH%']
	echo.
    set SQLCMD=SQLCMD.exe -x
) else (
    echo.
    echo -------------------------------------------------------------------------------
    echo WARNING: 
    echo Using legacy OSQL tool. Consider installing SQL 2005 [or later] client tools 
    echo which include the SQLCMD tool.
    echo -------------------------------------------------------------------------------
    echo.
    pause
)

set /p SQLServerAuth=SQL Server Authentication - y/n (%SQLServerAuth_DEFAULT%)?
if "%SQLServerAuth%" == "" set SQLServerAuth=%SQLServerAuth_DEFAULT%

if "%SQLServerAuth%" == "n" goto InputDBInfo

set /p USER=Enter user name(%USER_DEFAULT%):
if "%USER%" == "" set USER=%USER_DEFAULT%

set /p PASSWORD=Enter password (%PASSWORD_DEFAULT%):
if "%PASSWORD%" == "" set PASSWORD=%PASSWORD_DEFAULT%

set AUTHCONFIG=-U %USER% -P %PASSWORD%

:InputDBInfo


set /p SERVER=Enter SQL Server name (%SERVER_DEFAULT%): 
if "%SERVER%" == "" (
    set SERVER=%SERVER_DEFAULT%
) else (
    set DATABASE_DEFAULT=
)

set /p DATABASE=Enter database name (%DATABASE_DEFAULT%): 
if "%DATABASE%" == "" set DATABASE=%DATABASE_DEFAULT%

echo.
echo Ready to update %SERVER%\%DATABASE%.

:AskContinue
	set /p CONTINUE=Continue? (y/n): 
	if "%CONTINUE%" == "n" GOTO End
	if "%CONTINUE%" == "y" GOTO DoWork
	echo Unrecognised response '%CONTINUE%'
	goto AskContinue


:DoWork
	::call :CHECK_UNICODE
	::if ERRORLEVEL 1 (
	::	echo.
	::	echo ********************************************
	::	echo ERROR: one or more SQL files are not Unicode
	::	echo Batch terminated.
	::	echo ********************************************
	::	echo.
	::	goto END
	::)

:Create_SYtbUpdateScriptHistory
	%SQLCMD% -S %SERVER% %AUTHCONFIG% -b -d %DATABASE% -Q "IF NOT EXISTS (SELECT * FROM sysobjects where name = 'SYtbUpdateScriptHistory' AND xtype = 'U') BEGIN CREATE TABLE SYtbUpdateScriptHistory (SYtbUpdateScriptHistoryID int IDENTITY(1,1) NOT NULL, ScriptName varchar(255) NULL, ErrorMessage nvarchar(max) NULL, DateApplied datetime NULL CONSTRAINT DF_SYtbUpdateScriptHistory_DateApplied DEFAULT (getdate()), CONSTRAINT PK_SYtbUpdateScriptHistory PRIMARY KEY CLUSTERED (SYtbUpdateScriptHistoryID ASC) ) PRINT 'SYtbUpdateScriptHistory created' END ELSE PRINT 'SYtbUpdateScriptHistory already exists'"

	IF ERRORLEVEL 1 (
		echo.
		echo *****************************************
		echo ERROR creating SYtbUpdateScriptHistory table.
		echo Batch terminated.
		echo *****************************************
		echo.
		goto END
	)


	for %%S in (%SCRIPT_FILEMASK%) do (
		echo.
		echo -------------------------------------------------------------------------------
		echo %%~fS
		echo -------------------------------------------------------------------------------
		%SQLCMD% -S %SERVER% -I %AUTHCONFIG% -b -d %DATABASE% -i "%%~fS" > "%TEMP%\UpdateDb.out"
		
		IF ERRORLEVEL 1 (
			:: Error occurred
			echo Error executing script "%%~fS":
			type %TEMP%\UpdateDb.out
			
			:: Create SQL script to insert error message into SYtbUpdateScriptHistory
			call :LOG_FAIL %%S
			
			echo *************************
			echo ERROR: Batch terminated.
			echo *************************
			echo.
			goto END
		) ELSE (
			type %TEMP%\UpdateDb.out
			call :LOG_SUCCESS %%S
			echo Update successful.
		)
		echo.
	)
	echo Finished.
	goto END

:LOG_FAIL
	:: Create SQL script to insert error message into SYtbUpdateScriptHistory
	echo INSERT INTO SYtbUpdateScriptHistory(ScriptName, ErrorMessage) VALUES('%*', '> "%TEMP%\UpdateDbError.sql"
	call :REPLACE_QUOTES "%TEMP%\UpdateDb.out" "%TEMP%\UpdateDb-Quoted.out"
	type "%TEMP%\UpdateDb-Quoted.out" >> "%TEMP%\UpdateDbError.sql"
	echo ')>> "%TEMP%\UpdateDbError.sql"
	:: Exec script
	%SQLCMD% -S %SERVER% %AUTHCONFIG% -b -d %DATABASE% -i "%TEMP%\UpdateDbError.sql"
	del "%TEMP%\UpdateDbError.sql"
	del "%TEMP%\UpdateDb-Quoted.out"
	goto :EOF

:REPLACE_QUOTES
	:: Replaces single quotes in file %1 with double single quotes and writes to file %2
	echo WScript.Echo Trim(Replace(WScript.StdIn.ReadAll(), "'", "''")) > "%temp%\UpdateDb-replacequotes.vbs"
	type %1 | cscript //nologo "%temp%\UpdateDb-ReplaceQuotes.vbs" > %2
	del "%temp%\UpdateDb-ReplaceQuotes.vbs"
	goto :EOF


:LOG_SUCCESS
	%SQLCMD% -S %SERVER% %AUTHCONFIG% -b -d %DATABASE% -Q "INSERT INTO SYtbUpdateScriptHistory(ScriptName) VALUES('%*')"
	goto :EOF


:CHECK_UNICODE
	echo.
	set CHECK_UNICODE_VBS="%TEMP%\CheckUnicode%RANDOM%.vbs"
	
	echo Set fso = CreateObject("Scripting.FileSystemObject") >> %CHECK_UNICODE_VBS%
	echo errCount = 0 >> %CHECK_UNICODE_VBS%
	echo For Each file in fso.GetFolder(".").Files >> %CHECK_UNICODE_VBS%
	echo 	If LCase(fso.GetExtensionName(file.Name)) = "sql" Then >> %CHECK_UNICODE_VBS%
	echo 		lengthDiff = Len(fso.OpenTextFile(file.Name, 1, false, -1).ReadAll()) - Len(fso.OpenTextFile(file.Name, 1, false, -2).ReadAll()) >> %CHECK_UNICODE_VBS%
	echo 		If lengthDiff ^<^> 0 Then  >> %CHECK_UNICODE_VBS%
	echo 			WScript.Echo "Error: '" + file.Name + "' is not Unicode" >> %CHECK_UNICODE_VBS%
	echo 			errCount = errCount + 1 >> %CHECK_UNICODE_VBS%
	echo 		End if >> %CHECK_UNICODE_VBS%
	echo 	End if >> %CHECK_UNICODE_VBS%
	echo Next >> %CHECK_UNICODE_VBS%
	echo WScript.Quit errCount >> %CHECK_UNICODE_VBS%

	cscript //nologo %CHECK_UNICODE_VBS%
	set CHECK_UNICODE_RESULT=%ERRORLEVEL%
	del %CHECK_UNICODE_VBS%
	EXIT /B %CHECK_UNICODE_RESULT%
	goto :EOF


:END
	endlocal
	popd  
	pause