cmake_minimum_required(VERSION 3.10) # 建议使用 CMake 3.10 或更高版本
project(Cpp2UnityProject CXX)       # 定义项目名称，语言为 C++

# --- 设置 C++ 标准 ---
set(CMAKE_CXX_STANDARD 17) # 设置 C++ 标准为 C++17 (可根据需要调整，如 11, 14, 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF) # 禁用 GNU 扩展，保持代码更具可移植性

# --- 设置编译选项 (可选，但推荐) ---
# 开启所有警告，并视警告为错误（这有助于编写健壮的代码）
# add_compile_options(-Wall -Wextra -Werror -Wpedantic)
# 或者只开启警告，不视为错误：
# add_compile_options(-Wall -Wextra)


file(GLOB_RECURSE pipe_header_files "${CMAKE_CURRENT_SOURCE_DIR}/scripts/include/*.h")
file(GLOB SCRIPT_SOURCES "${CMAKE_CURRENT_SOURCE_DIR}/scripts/src/*.cpp")

# --- 定义库目标 ---
# add_library(库名称 类型 源文件...)
# 这里我们创建一个名为 'Cpp2UnityLib' 的静态库
# 你也可以改为 SHARED 来创建共享库 (DLL/SO)
add_library(Cpp2UnityLib STATIC ${SCRIPT_SOURCES} ${pipe_header_files})



# --- 设置头文件搜索路径 ---
# target_include_directories 告诉编译器去哪里找这个库所使用的头文件。
# PUBLIC: 表示这个路径不仅对 Cpp2UnityLib 自身的源文件有效，
#         任何链接 Cpp2UnityLib 的目标也需要知道这个头文件路径。
#         这是为了让 Unity 或其他 C++ 代码能找到 Cpp2UnityLib 的头文件。
target_include_directories(Cpp2UnityLib PUBLIC
    # 构建时，头文件路径是 Cpp2Unity/scripts/include
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/scripts/include>
    # 安装时，头文件会放到安装目录下的 include 文件夹 (如果你设置了 install 命令)
    $<INSTALL_INTERFACE:include>
)

# --- 设置输出目录 (可选，但推荐) ---
# 这会将编译出的静态库 (.lib/.a) 统一放到 build/lib 目录下
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib")
# 如果你构建的是共享库，需要设置 CMAKE_LIBRARY_OUTPUT_DIRECTORY
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/lib")
# 如果未来有可执行文件，可以设置 CMAKE_RUNTIME_OUTPUT_DIRECTORY
# set(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin")