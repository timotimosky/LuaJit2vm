cmake_minimum_required(VERSION 3.10)
project(unity_nativeDll)

if (WIN32)
	include(GenerateExportHeader)
	# include_directories("C:/Program Files/Unity/Hub/Editor/2022.3.57f1c1/Editor/Data/PluginAPI")
	include_directories("LuaLib/src")


	file(GLOB_RECURSE SOURCES "LuaLib/src/*/*.cpp" "LuaLib/src/*/*.c" "LuaLib/src/*/*.h" "LuaLib/src/*/*.hpp")

	set(TARGET luavmDll)
	add_library(${TARGET} SHARED ${SOURCES})
#	GENERATE_EXPORT_HEADER(libflexiparts
#		BASE_NAME flexiparts
#		EXPORT_MACRO_NAME flexiparts_EXPORT
#		EXPORT_FILE_NAME flexiparts_export.h
#		STATIC_DEFINE flexiparts_BUILD_AS_STATIC
#	)

	set_property(TARGET ${TARGET} PROPERTY CXX_STANDARD 14)
	set_property(DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR} PROPERTY VS_STARTUP_PROJECT ${TARGET})
	find_package(OpenGL REQUIRED)
	target_link_libraries(${TARGET} ${OPENGL_LIBRARIES})

	add_custom_command(TARGET ${TARGET} POST_BUILD
		COMMAND ${CMAKE_COMMAND} -E copy_if_different
		$<TARGET_FILE:${TARGET}>
		"${CMAKE_CURRENT_BINARY_DIR}/../tolua-master/Assets/Plugins/x86_64/${TARGET}.dll"
	)

endif(WIN32)
