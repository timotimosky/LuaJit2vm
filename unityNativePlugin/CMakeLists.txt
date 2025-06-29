cmake_minimum_required(VERSION 3.10)
# 解决方案名字叫BuildPinvoke，使用C++语言和C语言
project(BuildPinvoke CXX C)

# 不要错误的设置成C语言了。CMAKE_C_STANDARD
set(CMAKE_CXX_STANDARD 17)

if (WIN32)
	# 是否在项目中引入头文件（阅读源码时更方便）
	# 默认为否，如有需要将OFF改成ON
	option(STUDY_USE "Build for learning the source codes[Add head files in projects]." ON)

	# 添加附加目录,用于查找头文件
	include_directories("${CMAKE_CURRENT_SOURCE_DIR}/src")
	include_directories(
    "${SRC_DIR}/RHI"
    "${SRC_DIR}/openglEx"
    "${SRC_DIR}/RHI/gl3w"
    "${SRC_DIR}/AsyncGPUReadbackPlugin"
    "${SRC_DIR}/cpp_example_dll"
    "${SRC_DIR}/particles"
	)

	# PENGLEX_SOURCES
    file(GLOB RHI_SOURCES 
    "${CMAKE_CURRENT_SOURCE_DIR}/src/rhi/*/*.c"
     "${CMAKE_CURRENT_SOURCE_DIR}/src/rhi/*/*.cpp"
     "${CMAKE_CURRENT_SOURCE_DIR}/src/rhi/*/*.h"
    )

 message(STATUS "RHI_SOURCES 的值是: ${RHI_SOURCES}")

	file(GLOB OPENGLEX_SOURCES  
    "${CMAKE_CURRENT_SOURCE_DIR}/src/openglEx/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/openglEx/*.cpp"
    )
 message(STATUS "OPENGLEX_SOURCES 的值是: ${OPENGLEX_SOURCES}")

	file(GLOB ASYNCGPUREADBACK_SOURCES 
    "${CMAKE_CURRENT_SOURCE_DIR}/src/AsyncGPUReadbackPlugin/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/AsyncGPUReadbackPlugin/*.cpp"

     )

	file(GLOB CPP_EXAMPLE_DLL_SOURCES 
     "${CMAKE_CURRENT_SOURCE_DIR}/src/cpp_example_dll/*.h"
     "${CMAKE_CURRENT_SOURCE_DIR}/src/cpp_example_dll/*.cpp"
    )

	file(GLOB PARTICLES_SOURCES  
    "${CMAKE_CURRENT_SOURCE_DIR}/src/particles/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/particles/*.cpp"

    "${CMAKE_CURRENT_SOURCE_DIR}/src/openglEx/*.h"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/openglEx/*.cpp"
    )


   #  include_directories("C:/Program Files/Unity/Hub/Editor/2022.3.62f1/Editor/Data/PluginAPI")

	# 生成动态库——RHI
	 add_library(RHI SHARED ${RHI_SOURCES})

    # 生成静态库——openglEx
	#add_library(openglEx STATIC ${OPENGLEX_SOURCES})

	# Build AsyncGPUReadbackPlugin as a shared library (DLL).
	add_library(AsyncGPUReadbackPlugin SHARED ${ASYNCGPUREADBACK_SOURCES})
	# AsyncGPUReadbackPlugin depends on RHI, so we link it.
	target_link_libraries(AsyncGPUReadbackPlugin PRIVATE RHI)

	# Build cpp_example_dll as a shared library (DLL).
	# It has no dependencies, so no target_link_libraries call is needed.
	add_library(cpp_example_dll SHARED ${CPP_EXAMPLE_DLL_SOURCES})

	# Build particles as a shared library (DLL).
	add_library(particles SHARED ${PARTICLES_SOURCES})
	# particles depends on openglEx, so we link it.
	#target_link_libraries(particles PRIVATE openglEx)


endif(WIN32)

