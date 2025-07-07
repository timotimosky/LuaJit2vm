#!/usr/bin/env python
# -*- coding:UTF-8 -*-
# ===============================================================================
# 客户端下载
# ===============================================================================
import importlib
import os
import sys
import time
import traceback
import clr

CSharpVersion = 1


# C#调用过来
def download_call_by_c():
	print("COLOUR ***************************download_call_by_c******")
	# TODO:Mark 需要在全OS上测试以下补丁是否还需要
	#fix_python_default_coding()
	#fix_python_stdout_and_stderr()
	#fix_python_for_net_except_format()
	#fix_python_hashlib_and_ssl_conflict()
	#fix_python_dlopen_depend_bug_of_android()
	#fix_python_code_path()
	print("COLOUR **************************************************************************")
	print("RED 客户端启动:", get_os_name())
	print("COLOUR **************************************************************************")
	#init_game_info()

OSName = None
def get_os_name():
	import CSharp
	global OSName
	if OSName is None:
		OSName = CSharp.GCEnv.Instance.GetOSName()
	return OSName

# ===============================================================================
# 修正各种潜规则问题
# ===============================================================================
def fix_python_default_coding():
	importlib.reload(sys)
	getattr(sys, "setdefaultencoding")("UTF-8")

def fix_python_stdout_and_stderr():
	import CSharp
	sys.stdout = CSharp.GCIO.Instance
	sys.stderr = CSharp.GCIO.Instance

def fix_python_for_net_except_format():
	setattr(traceback, "format_tb", format_tb)

# 因为google pay会检查openssl的版本
# python的random模块在特定的情况下依赖hashlib故会导入hashlib
# 在android下hashlib依赖_hashlib模块，而_hashlib又依赖于libssl.so和libcrypto.so
# 这样会导致依赖低版本的openssl
# 修正方法是用Download模拟hashlib的接口，让random能够正确导入
# _ssl.so不能被加载【因为libssl.so被移除了】，故要这里先伪造好相关模块
# iOS上的libpython27.a里面不支持_hashlib，故也一起处理掉
def fix_python_hashlib_and_ssl_conflict():
	if get_os_name() == "StandaloneWindows64":
		return
	g = globals()
	for name in ('md5', 'sha1', 'sha224', 'sha256', 'sha384', 'sha512'):
		g[name] = None
	# 用Download伪造hashlib，让random不报错
	sys.modules["hashlib"] = __import__("Download")
	# 这里要设置为None，让 import ssl 抛异常
	sys.modules["ssl"] = None
	sys.modules["_ssl"] = None

# 有些android在后面导入python的动态模块会有诡异的dlopen找不到python27.so的问题
def fix_python_dlopen_depend_bug_of_android():
	if get_os_name() != "Android":
		return
	module_name_list = ["cPickle", "cStringIO", "binascii", "strop", "math",
						"time", "datetime", "itertools", "operator", "_socket", "_collections",
						"_bisect", "_json", "_struct", "_random", "_heapq", "_functools"]
	for module_name in module_name_list:
		try:
			__import__(module_name)
		except:
			traceback.print_exc()

def fix_python_code_path():
	# 内网构建客户端启动需要的内容
	if get_os_name() != "StandaloneWindows64":
		return
	import CSharp
	if not CSharp.GCEnv.Instance.IsDevelop():
		return
	root_folder = CSharp.GCIO.Instance.GetRootFolderPathOnWindows()
	if not root_folder:
		return
	root_folder += "/"
	# 使用编辑目录的脚本
	i = 0
	while i < len(sys.path):
		if "pyhome" in sys.path[i]:
			i += 1
			continue
		sys.path.pop(i)
	sys.path.append(root_folder + "Develop/PyCode")

def format_tb(tb, limit=None):
	l = traceback.format_list(traceback.extract_tb(tb, limit))
	return "".join(l)

def new(name, string=''):
	assert False, "disable hashlib"

# ===============================================================================
# 初始化游戏信息
# ===============================================================================
GameOperator = None
GameLogic = None
GamePackage = None
GamePackageConfig = None
GameWorld = None
GameMainSDK = None
GameLanguageIndex = 0
GameInitTime = 0
LoadingPanelMgr = None

def init_game_info():
	import random
	import UnityEngine
	import CSharp
	global GameOperator, GameLogic, GamePackage, GamePackageConfig, GameWorld, GameLanguageIndex
	global GameInitTime, LoadingPanelMgr, GameMainSDK
	LoadingPanelMgr = CSharp.LoadingPanelMgr.Instance
	gcupdate = CSharp.GCUpdate.Instance
	# 设置不睡眠，防止屏幕关闭
	UnityEngine.Screen.sleepTimeout = UnityEngine.SleepTimeout.NeverSleep
	# 初始化随机种子
	random.seed(int(time.time() * 256))
	GameInitTime = time.time()
	# 确定好游戏世界和游戏语言等常量
	GamePackage = CSharp.GCEnv.Instance.GamePackage
	print("init_game_info  GamePackage:", GamePackage)
	match_game_operator= gcupdate.MatchGameOperator
	if match_game_operator:
		GameOperator = match_game_operator
	else:
		GameOperator = gcupdate.GetClientUpdateHead("game_operator")
	GameLogic = gcupdate.GetClientUpdateHead("game_logic")
	# 设置异常文件上传站点
	#CSharp.GCHttp.Instance.UploadErrorUrl = gcupdate.GetClientUpdateHead("error_upload_url")
	# 该操作系统下的所有包信息
	os_package_dict = eval(gcupdate.GetClientUpdateHead("os_package_dict"))
	# 根据当前操作系统获取当前包的数据
	my_package_dict = os_package_dict[get_os_name()]
	print("init_game_info  os_package_dict:", my_package_dict)
	GamePackageConfig = my_package_dict[GamePackage]
	if not GamePackageConfig:
		show_error_tips_panel("0x66")
		print("GE_EXC 该包[%s]不支持登录"%GamePackage)
		return
	print("GamePackageConfig:", GamePackageConfig)
	# 确定游戏世界
	match_game_world = gcupdate.MatchGameWorld
	if match_game_world:
		GameWorld = match_game_world
	else:
		GameWorld = GamePackageConfig["game_world"]
	print("init_game_info  GameWorld:", GameWorld)
	# 使用特殊的SDK登陆
	match_game_main_sdk = gcupdate.MatchGameMainSDK
	if match_game_main_sdk:
		GameMainSDK = match_game_main_sdk
	else:
		GameMainSDK = GamePackageConfig["main_sdk"]
	# 设置IO工具元数据
	init_iotool_meta()
	# 设置AB的持久化目录
	ab_root_folder = CSharp.GCIO.Instance.GetPersistentFolderPath("ab")
	CSharp.GCAssetBundle.Instance.SetPersistenceABRootFolder(ab_root_folder)
	# 设置调试信息
	init_debug_info()
	# 构建客户端下载站点
	#build_on_client_up()
	# 检查版本
	gc_update = CSharp.GCUpdate.Instance
	if not hasattr(gc_update, "CSharpVersion"):
		print("YELLOW 请更新客户端并重新编译!")
		return
	if gc_update.CSharpVersion != CSharpVersion:
		# TODO：这里后续需要补充逻辑，提示玩家下最新的完整包
		print("YELLOW 请更新客户端并重新编译! CSharp[%d] != Python[%d]" % (CSharp.GCUpdate.Instance.CSharpVersion, CSharpVersion))
		return
	# 语言初始化
	init_game_language()
	# 播放背景音乐
	#play_down_bgm()
	# 可以开始下载了(先下载客户端信息文件)
	download_server_info()

def init_iotool_meta():
	# 连接IO工具后发送的元数据
	import CSharp
	iotool_meta = {"process_type" : "client", "process_id" : CSharp.GCEnv.Instance.GetClientIndex(),
				   "now_os" : get_os_name(), "now_language" : get_now_language(),
				   "is_developer" : CSharp.GCEnv.Instance.IsDeveloper(),
				   "log_file_path" : CSharp.GCIO.Instance.GetOutputFilePath(),
				   "pid" : CSharp.GCIO.Instance.GetProcessID(),
				   }
	CSharp.GCIO.Instance.SetIOToolMeta(repr(iotool_meta))

def init_debug_info():
	# 设置调试信息
	import CSharp
	ADD_DEBUG_INFO = CSharp.GCDebug.Instance.AddDebugInfo
	ADD_DEBUG_INFO("Download", "GamePackage", get_game_package())
	ADD_DEBUG_INFO("Download", "GameOperator", get_game_operator())
	ADD_DEBUG_INFO("Download", "GameLogic", get_game_logic())
	ADD_DEBUG_INFO("Download", "GameWorld", get_game_world())
	ADD_DEBUG_INFO("Download", "MainSDK", get_game_package_main_sdk())
	ADD_DEBUG_INFO("Download", "LanguageList", "|".join(get_language_list()))
	ADD_DEBUG_INFO("Download", "NowLanguage", get_now_language())

def init_game_language():
	global GameLanguageIndex
	language_file_path = get_language_file_path()
	if not os.path.exists(language_file_path):
		GameLanguageIndex = 0
		return
	language_file = open(language_file_path, "r")
	read_str = language_file.read()
	if read_str:
		GameLanguageIndex = int(read_str)
	language_file.close()

def show_error_tips_panel(str_key):
	import CSharp
	CSharp.LogMgr.Instance.LogTag(str_key, (int)(CSharp.FoundationLogTag.ErrorPanel))

# ===============================================================================
# 缓存全局变量
# ===============================================================================
def get_game_package():
	return GamePackage

def get_game_operator():
	return GameOperator

def get_game_logic():
	return GameLogic

def get_game_world():
	return GameWorld

def get_game_package_main_sdk():
	return GameMainSDK

def get_language_list():
	return GamePackageConfig["language_list"]

def get_now_language():
	language_list = get_language_list()
	language_count = len(language_list)
	if GameLanguageIndex >= language_count:
		return language_list[0]
	return language_list[GameLanguageIndex]

def get_default_language():
	# 初始语言
	return "SimplifiedChinese"

def get_language_file_path():
	# 当前使用的语言配置路径
	import UnityEngine
	language_file_path = UnityEngine.Application.persistentDataPath + "/language.txt"
	return language_file_path

# ===============================================================================
# 下载
# ===============================================================================
UIPacageDict = {}
ModuleNameList = []
NewMultiTable = NoneDownloadCallback = None

class DownloadOne(object):
	ErrorCount = 0
	DownloadObjSet = set()

	@classmethod
	def reset(cls):
		cls.ErrorCount = 0
		cls.DownloadObjSet.clear()

	def inc_one_error(self):
		self.ErrorCount += 1

	def __init__(self, relative_file_path, source_site_url, cdn_site_url, inner_site_url, file_check_md5, http_size):
		import CSharp
		self.relative_file_path = relative_file_path
		self.hd = hd = CSharp.GCHttp.Instance.CreateDownload()
		hd.HttpSize = http_size
		hd.CDNSiteUrl = cdn_site_url
		hd.SourceSiteUrl = source_site_url
		hd.InnerSiteUrl = inner_site_url
		hd.RelativeFilePath = relative_file_path
		hd.NeedSyncProcessing = True
		hd.NeedRemoveMetadata = True
		hd.NeedGZipDecompress = not relative_file_path.endswith(".ab")
		hd.NeedDownloadMD5 = file_check_md5
		hd.DownloadCallback4CsPy = CSharp.GCDelegate.DelegateByteArray(self.on_download_one)
		# 记录总共的下载对象
		self.DownloadObjSet.add(self)

	def __str__(self):
		return "DownloadOne(%s)" % self.relative_file_path

	def on_download_one(self, file_bytes):
		if file_bytes is None:
			print("GE_EXC download file(%s) error" % self.hd.RelativeFilePath)
			self.inc_one_error()
		self.DownloadObjSet.discard(self)
		if not is_first_download():
			hot_one(self.relative_file_path)

def do_http_request(url, callback):
	import CSharp
	hr = CSharp.GCHttp.Instance.CreateRequest()
	hr.Url = url
	hr.RequestCallback4CsPy = CSharp.GCDelegate.DelegateByteArray(callback)

def get_ui_package_dict():
	return UIPacageDict

def get_module_name_list():
	return ModuleNameList

# ===============================================================================
# 游戏信息下载
# 2个文件，一个游戏公告，一个游戏服信息
# ===============================================================================
# 回调函数[热更时用]
ONLY_DOWNLOAD_SERVER_INFO = False
ON_DOWNLOAD_SERVER_INFO_CALL_BACK = None
ONLY_DOWNLOAD_PLACARD_INFO = False
ON_DOWNLOAD_PLACARD_INFO_CALL_BACK = None

def get_server_info_table_file_name():
	# 服信息的文件
	return "client_server_%s_table.bin" % get_default_language()

def get_placard_info_table_file_name():
	# 公告信息
	return "client_placard_%s_table.bin" % get_default_language()

CLIENT_UPDATE_SITE = None
def get_client_update_site():
	# 根据不同的环境确定更新站点
	global CLIENT_UPDATE_SITE
	if not CLIENT_UPDATE_SITE:
		import CSharp
		gcenv = CSharp.GCEnv.Instance
		gcupdate = CSharp.GCUpdate.Instance
		package_update_site = gcupdate.GetClientUpdateHead("update_site")
		if gcenv.IsDeveloper() and gcupdate.ClientUpdateSite != package_update_site:
			CLIENT_UPDATE_SITE = gcupdate.ClientUpdateSite
		else:
			CLIENT_UPDATE_SITE = package_update_site
	return CLIENT_UPDATE_SITE

CLIENT_INFO_SITE = None
def get_client_info_site():
	# 客户端信息站点，用于获取服务器的信息，公告信息
	# 与更新站点分开的作用是：内网可以获取外网的信息，同时可以使用内网的资源
	global CLIENT_INFO_SITE
	if not CLIENT_INFO_SITE:
		import CSharp
		if CSharp.GCEnv.Instance.IsDeveloper():
			site = CSharp.GCUpdate.Instance.MatchInfoSite
			if site:
				CLIENT_INFO_SITE = site
		if not CLIENT_INFO_SITE:
			# 如果没有配置，就使用更新站点
			CLIENT_INFO_SITE = get_client_update_site()
	return CLIENT_INFO_SITE

def download_server_info():
	#1,先下载服信息
	uri = "Develop/Web/%s/%s" % (get_os_name(), get_server_info_table_file_name().replace("_table.bin", "_head.bin"))
	url = get_client_info_site() + uri
	do_http_request(url, on_download_server_info_head)

def on_download_server_info_head(http_bytes):
	if http_bytes is None:
		show_error_tips_panel("0x67")
		# print("YELLOW 没有构建客户端[服信息文件]head")
		return
	import CSharp
	mt_head = CSharp.MultiTable()
	mt_head.LoadFromMemory(http_bytes)
	mt_old = CSharp.MultiTable()
	mt_old.LoadFromPersistentFile(get_server_info_table_file_name())
	# 对比新旧数据,如果MD5一致，无需下载
	if mt_head.GetHeadString("table_md5") == mt_old.GetHeadString("table_md5"):
		# 如果有回调函数，则直接调用回调函数即可
		download_placard_info()
		return
	# 下载清单不一致，重新下载
	url = get_client_info_site() + "Develop/Web/%s/%s" % (get_os_name(), get_server_info_table_file_name())

	print("download_server:",url)
	do_http_request(url, on_download_server_info_table)

def on_download_server_info_table(http_bytes):
	import CSharp
	if http_bytes is None:
		show_error_tips_panel("0x68")
		# print("YELLOW 没有构建客户端[服信息文件]table")
		return
	new_table = CSharp.MultiTable()
	new_table.LoadFromMemory(http_bytes)
	# 存储到持久化目录下
	print("download_server2:",http_bytes)
	new_table.SaveToPersistentFile(get_server_info_table_file_name())
	# 尝试触发回调
	global ON_DOWNLOAD_SERVER_INFO_CALL_BACK
	if ON_DOWNLOAD_SERVER_INFO_CALL_BACK:
		ON_DOWNLOAD_SERVER_INFO_CALL_BACK()
		ON_DOWNLOAD_SERVER_INFO_CALL_BACK = None
	# 开始下载游戏公告
	download_placard_info()

def download_placard_info():
	#仅更新服信息
	global ONLY_DOWNLOAD_SERVER_INFO
	if ONLY_DOWNLOAD_SERVER_INFO:
		ONLY_DOWNLOAD_SERVER_INFO = False
		return
	# 2,下载游戏公告
	uri = "Develop/Web/%s/%s" % (get_os_name(), get_placard_info_table_file_name().replace("_table.bin", "_head.bin"))
	url = get_client_info_site() + uri
	do_http_request(url, on_download_placard_info_head)

def on_download_placard_info_head(http_bytes):
	if http_bytes is None:
		show_error_tips_panel("0x67")
		# print("YELLOW 没有构建客户端[公告文件]head")
		# 就算没有公告，也支持能继续启动
		after_finish_download_placard()
		return
	import CSharp
	mt_head = CSharp.MultiTable()
	mt_head.LoadFromMemory(http_bytes)
	mt_old = CSharp.MultiTable()
	mt_old.LoadFromPersistentFile(get_placard_info_table_file_name())
	# 对比新旧数据,如果MD5一致，无需下载
	if mt_head.GetHeadString("table_md5") == mt_old.GetHeadString("table_md5") :
		# 开始下载游戏资源
		after_finish_download_placard()
		return
	# 下载清单不一致，重新下载
	url = get_client_info_site() + "Develop/Web/%s/%s" % (get_os_name(), get_placard_info_table_file_name())
	do_http_request(url, on_download_placard_info_table)

def on_download_placard_info_table(http_bytes):
	import CSharp
	if http_bytes is None:
		print("YELLOW 没有构建客户端[公告文件]table")
		after_finish_download_placard()
		return
	new_table = CSharp.MultiTable()
	new_table.LoadFromMemory(http_bytes)
	# 存储到持久化目录下
	new_table.SaveToPersistentFile(get_placard_info_table_file_name())
	# 触发回调
	global ON_DOWNLOAD_PLACARD_INFO_CALL_BACK
	if ON_DOWNLOAD_PLACARD_INFO_CALL_BACK:
		ON_DOWNLOAD_PLACARD_INFO_CALL_BACK()
		ON_DOWNLOAD_PLACARD_INFO_CALL_BACK = None
	# 尝试触发回调
	after_finish_download_placard()
	
def after_finish_download_placard():
	# 仅更新公共
	global ONLY_DOWNLOAD_PLACARD_INFO
	if ONLY_DOWNLOAD_PLACARD_INFO:
		ONLY_DOWNLOAD_PLACARD_INFO = False
		return
	# 开始下载游戏资源
	download_start()

def get_client_info_file_tail(file_name):
	import CSharp
	from Common import Serialize
	CS_IO = CSharp.GCIO.Instance
	file_path = CS_IO.GetExistFolderFilePath(file_name)
	with open(file_path, "rb") as f:
		s = f.read()
		head_size = Serialize.bytes_to_i32(s[:4])
		body_size = Serialize.bytes_to_i32(s[4 + head_size: 4 + head_size + 4])
		tail = s[4 + head_size + 4 + body_size + 4:]
		return Serialize.pack_bytes_to_obj(tail)
# ===============================================================================
# 资源下载
# ===============================================================================
def get_client_download_table_uri_name():
	import CSharp
	return "client_download_v%s_%s_table.bin" % (CSharp.GCUpdate.Instance.ClientReleaseNumber, get_default_language())

def get_client_download_table_file_name():
	return "client_download_%s_table.bin" % get_default_language()

def download_start(download_callback=None):
	global DownloadCallback
	if is_first_download() and download_callback:
		print("GE_EXC 首次下载不用设置回调！！！")
	DownloadCallback = download_callback
	download_head()

def download_head():
	import CSharp
	uri = "Develop/Web/%s/%s" % (get_os_name(), get_client_download_table_uri_name().replace("_table.bin", "_head.bin"))
	url = CSharp.GCUpdate.Instance.ClientUpdateSite + uri
	do_http_request(url, on_download_head)

def on_download_head(http_bytes):
	if http_bytes is None:
		show_error_tips_panel("0x69")
		return
	import CSharp
	mt_head = CSharp.MultiTable()
	mt_head.LoadFromMemory(http_bytes)
	mt_old = CSharp.MultiTable()
	mt_old.LoadFromPersistentFile(get_client_download_table_file_name())
	# 下载清单一致，无需下载
	if mt_head.GetHeadString("table_md5") == mt_old.GetHeadString("table_md5"):
		check_download_file(mt_old)
	# 下载清单不一致，重新下载
	else:
		download_table()

def download_table():
	import CSharp
	gcupdate = CSharp.GCUpdate.Instance
	uri = "Develop/Web/%s/%s" % (get_os_name(), get_client_download_table_uri_name())
	hr = CSharp.GCHttp.Instance.CreateRequest()
	hr.Url = gcupdate.ClientUpdateSite + uri
	hr.RequestCallback4CsPy = CSharp.GCDelegate.DelegateByteArray(on_download_table)

def on_download_table(http_bytes):
	import CSharp
	global NewMultiTable

	if http_bytes is None:
		show_error_tips_panel("0x6a")
		return
	NewMultiTable = CSharp.MultiTable()
	NewMultiTable.LoadFromMemory(http_bytes)
	check_download_file(NewMultiTable)

def check_download_file(mt_new):
	import CSharp

	gcenv = CSharp.GCEnv.Instance
	gcupdate = CSharp.GCUpdate.Instance
	gcassetbundle = CSharp.GCAssetBundle.Instance
	mt_old = CSharp.MultiTable()
	mt_old.LoadFromPersistentFile(get_client_download_table_file_name())
	# 确定下载的源站和CDN地址
	# 有可能是开发者模式下玩家包登录模拟服，强制设置了初始更新站点是模拟服的客户端更新站点
	# 而模拟服的客户端更新站点中定义的update_site和download_site是正式服的
	# 故这种情况下需要强制将source_site和cdn_site全部使用开发者定义的
	if gcenv.IsDeveloper() and gcupdate.ClientUpdateSite != gcupdate.GetClientUpdateHead("update_site"):
		source_site = gcupdate.ClientUpdateSite
		cdn_site = gcupdate.ClientUpdateSite
	else:
		source_site = gcupdate.GetClientUpdateHead("update_site")
		cdn_site = gcupdate.GetClientUpdateHead("download_site")
	# 设置调试信息
	ADD_DEBUG_INFO = CSharp.GCDebug.Instance.AddDebugInfo
	if is_first_download():
		ADD_DEBUG_INFO("Download", "SourceSite", source_site)
		ADD_DEBUG_INFO("Download", "CDNSite", cdn_site)
	# 为旧的下载文件构建一份对比字典{}
	old_d = {}
	OLD_BODY_LINES = mt_old.GetBodyLines
	for i in range(0, 99999):
		lines = OLD_BODY_LINES(i)
		if lines is None: break
		old_path = lines[0]
		# old_uri = lines[1]
		old_md5 = lines[2]
		# old_size = int(lines[3])
		assert old_path not in old_d
		old_d[old_path] = old_md5
	else:
		assert False
	# 对比新旧清单，确定需要下载的文件
	NEW_BODY_LINES = mt_new.GetBodyLines
	DownloadOne.reset()
	UIPacageDict.clear()
	while ModuleNameList: ModuleNameList.pop()
	ui_features = "ab/UI/"
	# ui_folder = "ab/UI/%s/%s/" % (get_game_logic(), get_now_language())
	ui_start_pos = len(ui_features) + len(get_game_logic()) + 1
	ui_end_pos = len(".bytes.ab")
	for i in range(0, 99999):
		lines = NEW_BODY_LINES(i)
		if lines is None: break
		new_path = lines[0]
		new_uri = lines[1]
		new_md5 = lines[2]
		new_size = int(lines[3])
		# 记录ui的package
		if new_path.startswith(ui_features):
			ui_package_s = new_path[ui_start_pos: -ui_end_pos]
			# ab/
			ui_relative_path = new_path[3:]
			# 是描述包
			if ui_package_s.endswith("_fui"):
				ui_package_name = ui_package_s[:-4]
				ui_des_ab_path = ui_relative_path
				ui_res_ab_path = None
			# 是资源包 _fui_res
			else:
				ui_package_name = ui_package_s[:-8]
				ui_des_ab_path = None
				ui_res_ab_path = ui_relative_path
			ui_old_t = UIPacageDict.get(ui_package_name)
			if ui_old_t is None:
				UIPacageDict[ui_package_name] = (ui_des_ab_path, ui_res_ab_path)
			else:
				UIPacageDict[ui_package_name] = (ui_des_ab_path or ui_old_t[0], ui_res_ab_path or ui_old_t[1])
		# 记录客户端的模块列表
		if new_path.endswith(".pyc"):
			# len("pycode/") == 7
			ModuleNameList.append(new_path[7:-4].replace("/", "."))
		# 先判断StreamingAsset中是否存在
		inner_site_url = None
		new_uri_lower = new_uri.lower()
		in_streaming_asset = gcenv.StreamingAssetExist(new_uri_lower)
		if in_streaming_asset:
			if new_path.endswith(".ab"):
				# new_path以"ab/"开头，比如"ab/U3D/xxxxxx.ab，所以需要去掉这部分
				ab_relative_path = new_path[3:]
				gcassetbundle.RegisterStreamingAB(ab_relative_path, new_uri_lower)
				continue
			else:
				inner_site_url = new_uri_lower
		# 已经存在，无需下载
		file_path = CSharp.GCIO.Instance.GetExistFolderFilePath(new_path)
		if new_md5 == old_d.get(new_path) and os.path.isfile(file_path):
			continue
		if inner_site_url:
			new_size = 0
		# 到这里就是需要下载的资源
		DownloadOne(new_path, source_site + new_uri, cdn_site + new_uri, inner_site_url, new_md5, new_size)
	else:
		show_error_tips_panel("0x6b")
		assert False
	# 重置下载统计
	CSharp.GCHttp.Instance.ResetDownloadStatistics()
	# 下载更新
	LoadingPanelMgr.OnDownloadEndDelegate4Py = CSharp.GCDelegate.DelegateEmpty(download_end)
	LoadingPanelMgr.ReadyDownloadLoading()
	# 是首次下载
	if is_first_download():
		# 调试信息
		ADD_DEBUG_INFO("Download", "FileCount", str(len(DownloadOne.DownloadObjSet)))
		ADD_DEBUG_INFO("Download", "ByteCount", str(CSharp.GCHttp.Instance.GetNeedDownloadByteLength()))
	# 是热下载
	else:
		# 调试信息
		CSharp.GCDebug.Instance.HotDownloadFileCount = len(DownloadOne.DownloadObjSet)
		CSharp.GCDebug.Instance.HotDownloadByteCount = CSharp.GCHttp.Instance.GetNeedDownloadByteLength()
		# 尝试创建热下载
		hot_create()

def download_end():
	global NewMultiTable, DownloadCallback

	# 保存清单
	if NewMultiTable:
		NewMultiTable.SaveToPersistentFile(get_client_download_table_file_name())
		NewMultiTable = None
	# 是首次下载，初始化
	from Client.U3D import U3DMgr
	U3DMgr.after_load_module()
	if is_first_download():
		finish_first_download()
		download_then_init()
	# 是热下载，回调
	else:
		download_then_callback()

def download_then_init():
	import CSharp
	# 先设置是否开发者
	from Common import Environment
	Environment.IS_DEVELOPER = CSharp.GCEnv.Instance.IsDeveloper()
	# 初始化脚本
	from Client import GCInit
	GCInit.init()

def download_then_callback():
	global DownloadCallback
	if not DownloadCallback:
		return
	callback = DownloadCallback
	DownloadCallback = None
	callback()

FIRST_DOWNLOAD = None
def is_first_download():
	return FIRST_DOWNLOAD is not True

def finish_first_download():
	global FIRST_DOWNLOAD
	FIRST_DOWNLOAD = True

# ===============================================================================
# 热下载
# ===============================================================================
def hot_create():
	hot_one(None)

def hot_destroy():
	pass

def hot_one(relative_file_path):
	if relative_file_path is not None:
		print("***** hot download", relative_file_path)
	# 如果下载完毕，则结束
	if not DownloadOne.DownloadObjSet:
		hot_destroy()
		download_end()
