cmake_minimum_required(VERSION 3.10)
# 解决方案名字叫BuildLua
project(BuildLua C)

# 使用的语言标准是C89/ANSI C
set(CMAKE_C_STANDARD 90)

if (WIN32)
# 	include(GenerateExportHeader)
# 	# include_directories("C:/Program Files/Unity/Hub/Editor/2022.3.57f1c1/Editor/Data/PluginAPI")
# 	include_directories("src")


# 	file(GLOB_RECURSE SOURCES 
# 	"src/*.cpp"
# 	"src/*.c"
# 	"src/*.h" 
# 	"src/*.hpp"
# 	)

# 	set(TARGET luavmDll)
# 	add_library(${TARGET} SHARED ${SOURCES})
# #	GENERATE_EXPORT_HEADER(libflexiparts
# #		BASE_NAME flexiparts
# #		EXPORT_MACRO_NAME flexiparts_EXPORT
# #		EXPORT_FILE_NAME flexiparts_export.h
# #		STATIC_DEFINE flexiparts_BUILD_AS_STATIC
# #	)

# 	set_property(TARGET ${TARGET} PROPERTY CXX_STANDARD 14)
# 	set_property(DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR} PROPERTY VS_STARTUP_PROJECT ${TARGET})
# 	find_package(OpenGL REQUIRED)
# 	target_link_libraries(${TARGET} ${OPENGL_LIBRARIES})

# 	add_custom_command(TARGET ${TARGET} POST_BUILD
# 		COMMAND ${CMAKE_COMMAND} -E copy_if_different
# 		$<TARGET_FILE:${TARGET}>
# 		"${CMAKE_CURRENT_BINARY_DIR}/../tolua-master/Assets/Plugins/x86_64/${TARGET}.dll"
# 	)



	# 是否在项目中引入头文件（阅读源码时更方便）
	# 默认为否，如有需要将OFF改成ON
	option(STUDY_USE "Build for learning the source codes[Add head files in projects]." ON)

	# 添加附加目录
	include_directories(src)

	file(GLOB LUAEXE src/lua.c)
	file(GLOB LUACEXE src/luac.c)

	# 设置各个子项目编译需要的源文件
	if(STUDY_USE)
		file(GLOB LUALIB_SRC src/*.h src/*.c)
		list(REMOVE_ITEM LUALIB_SRC ${LUAEXE} ${LUACEXE})

		file(GLOB LUA_SRC ./src/lua.c)

		file(GLOB LUAC_SRC src/*.h src/*.c)
		list(REMOVE_ITEM LUAC_SRC ${LUAEXE})
	else()
		aux_source_directory(src LUALIB_SRC)
		list(REMOVE_ITEM LUALIB_SRC ${LUAEXE} ${LUACEXE})

		file(GLOB LUA_SRC ./src/lua.c)

		aux_source_directory(src LUAC_SRC)
		list(REMOVE_ITEM LUAC_SRC ${LUAEXE})
	endif()

	# 定义宏LUA_BUILD_AS_DLL
	add_definitions(-DLUA_BUILD_AS_DLL)

	# 创建子项目——luaXX
	# 生成动态库——luaXX.dll   XX为版本号，这里为54
	add_library(lua54 SHARED ${LUALIB_SRC})

	# 创建子项目——lua
	# 生成lua解释器——lua.exe
	add_executable(lua ${LUA_SRC})
	target_link_libraries(lua lua54)

	# 创建子项目——luac
	# 生成lua编译器——luac.exe
	add_executable(luac ${LUAC_SRC})

	# 创建子项目——luaCWrap
	# 生成lua的CWrap——luaCWrap.dll
	file(GLOB LUACWrap_SRC ./luaCWrap/*.c ./luaCWrap/*.h)
	add_library(luaCWrap SHARED ${LUACWrap_SRC})
	target_link_libraries(luaCWrap lua54)
	# install
endif(WIN32)

