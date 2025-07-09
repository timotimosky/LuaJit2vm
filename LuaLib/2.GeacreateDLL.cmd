@echo off

@echo off

:: 设置你的 Visual Studio 版本和版次（例如：2022\Community, 2019\Professional 等）
set "VS_VERSION_PATH=2022\Enterprise"

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
    set "VS_VERSION_PATH=2022\Enterprise"
    set BUILD_DIR="%ProgramFiles%\Microsoft Visual Studio\%VS_VERSION_PATH%\Common7\Tools\VsDevCmd.bat"
    echo New Current directory is: %BUILD_DIR%
    if exist %BUILD_DIR% (
    echo set =======================path
    set "VSCMD_PATH=%ProgramFiles%\Microsoft Visual Studio\%VS_VERSION_PATH%\Common7\Tools\VsDevCmd.bat"
    ) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\%VS_VERSION_PATH%\Common7\Tools\VsDevCmd.bat" (
        set "VSCMD_PATH=%ProgramFiles(x86)%\Microsoft Visual Studio\%VS_VERSION_PATH%\Common7\Tools\VsDevCmd.bat"
    ) else (
        echo Error :Cannot find VsDevCmd.bat。请检查 VS_VERSION_PATH 设置。
        echo 示例路径:C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\Tools\VsDevCmd.bat

    )

)

:: 启动 VsDevCmd.bat 并在此会话中设置环境变量
call "%VSCMD_PATH%"

:: 切换到 libnativeparticles.dll 所在的目录 (可选)
:: 如果 libnativeparticles.dll 不在当前目录，你需要指定它的完整路径，或者切换到它所在的目录。
:: 例如：cd /d "C:\你的项目\libnativeparticles.dll 所在目录"
:: 如果 libnativeparticles.dll 就在当前目录，可以省略这行。

:: 执行 dumpbin 命令
echo.
echo now do == msbuild BuildLua.sln...


set SCRIPT_DIR=%~dp0
set BUILD_DIR=%SCRIPT_DIR%build
echo BUILD_DIR is: "%BUILD_DIR%"
cd  %BUILD_DIR%
msbuild BuildLua.sln
echo.
echo now msbuild BuildLua.sln  over ==
pause