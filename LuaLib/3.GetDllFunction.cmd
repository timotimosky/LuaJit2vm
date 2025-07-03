@echo off

@echo off

:: 设置你的 Visual Studio 版本和版次（例如：2022\Community, 2019\Professional 等）
set "VS_VERSION_PATH=2022\Community"

:: 构建 VsDevCmd.bat 的完整路径
:: 注意：ProgramFiles 通常是 C:\Program Files，ProgramFiles(x86) 是 C:\Program Files (x86)
:: 大多数新版 VS 安装在 Program Files 下，旧版可能在 Program Files (x86)

set BUILD_DIR="%ProgramFiles%\Microsoft Visual Studio\%VS_VERSION_PATH%\Common7\Tools\VsDevCmd.bat"

echo Current directory is: %BUILD_DIR%

if exist %BUILD_DIR% (
    echo set =======================path
    set "VSCMD_PATH=%ProgramFiles%\Microsoft Visual Studio\%VS_VERSION_PATH%\Common7\Tools\VsDevCmd.bat"
) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\%VS_VERSION_PATH%\Common7\Tools\VsDevCmd.bat" (
    set "VSCMD_PATH=%ProgramFiles(x86)%\Microsoft Visual Studio\%VS_VERSION_PATH%\Common7\Tools\VsDevCmd.bat"
) else (
    echo Error:NoFind VsDevCmd.bat. Check VS_VERSION_PATH
    echo 示例路径：C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\Tools\VsDevCmd.bat

)

:: 启动 VsDevCmd.bat 并在此会话中设置环境变量
call "%VSCMD_PATH%"

:: 切换到 libnativeparticles.dll 所在的目录 (可选)
:: 如果 libnativeparticles.dll 不在当前目录，你需要指定它的完整路径，或者切换到它所在的目录。
:: 例如：cd /d "C:\你的项目\libnativeparticles.dll 所在目录"
:: 如果 libnativeparticles.dll 就在当前目录，可以省略这行。

:: 执行 dumpbin 命令
echo.
set SCRIPT_DIR=%~dp0
echo now do == dumpbin.exe /exports luaCWrap.dll...
set DLL_DIR=%SCRIPT_DIR%build\Debug
echo "%DLL_DIR%"

cd %DLL_DIR%

set BUILD_DIR=%SCRIPT_DIR%unityNativePlugin\Assets\Plugins\x86_64
echo BUILD_DIR is: "%BUILD_DIR%"
cd  %BUILD_DIR%

dumpbin.exe /exports ./luaCWrap.dll
echo.
echo now do  over ==
pause