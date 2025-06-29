@echo off

rem 获取当前批处理文件所在的目录，假设它就在render目录下
set SCRIPT_DIR=%~dp0
echo SCRIPT_DIR is: "%SCRIPT_DIR%"

rem 构建build目录的完整路径
set BUILD_DIR=%SCRIPT_DIR%build
echo BUILD_DIR is: "%BUILD_DIR%"

rem 检查build目录是否存在，如果不存在则创建
if not exist "%BUILD_DIR%" (
    echo Creating build directory: %BUILD_DIR%
    mkdir "%BUILD_DIR%"
) else (
    echo Build directory already exists: %BUILD_DIR%
)

rem 进入build目录
cd "%BUILD_DIR%"
echo Current directory is: %CD%

rem 运行CMake生成Visual Studio工程文件
echo Running CMake with source directory: "%SCRIPT_DIR%"
cmake -G "Visual Studio 17 2022" %SCRIPT_DIR%

if %errorlevel% neq 0 (
    echo CMake generation failed!
    pause
) else (
    echo CMake generation successful!
    echo You can now open the generated .sln file in Visual Studio from:
    echo %BUILD_DIR%
)

pause