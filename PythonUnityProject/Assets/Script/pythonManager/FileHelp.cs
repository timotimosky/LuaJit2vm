using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
public class FileHelp 
{
 

    public static string GetDirFromAllFile(string filePath)
    {
        int a = filePath.LastIndexOf('/');
        if (a < 0)
            return filePath;

        return filePath.Remove(a);
    }

    /// <summary>
    /// 创建二进制文件 .比如模型、音频
    /// </summary>
    public static void WriteModelFile(string fullname, byte[] info)
    {
        Stream sw;
        string filePartPath = GetDirFromAllFile(fullname);
        //路径是否存在
        NoDirectCreate(filePartPath);
        FileInfo t = new FileInfo(fullname);
        if (!t.Exists)
        {
            sw = t.Create();
        }
        else
        {
            //DebugTool.LogJL("已经存在，删除它 还是覆盖他");
            //sw = t.Append();
            sw = t.OpenWrite();
        }
        //以行的形式写入信息
        //sw.WriteLine(info);
        sw.Write(info, 0, info.Length);
        sw.Flush();
        sw.Close();
        sw.Dispose();
    }


    public static void WriteModelFile(string path, string name, byte[] info)
    {
        //DebugTool.LogFormat("==CreateModelFile=======name=====" , name ," path=== " , path);
        //文件流信息
        //StreamWriter sw;
        Stream sw;
        string filePath  =Path.Combine(path , name);
        //string filePartPath = GetDirFromAllFile(filePath);
        //路径是否存在
        NoDirectCreate(path);
        FileInfo t = new FileInfo(filePath);
        if (!t.Exists)
        {
            sw = t.Create();
        }
        else
        {
            //DebugTool.LogJL("已经存在，删除它 还是覆盖他");
            //sw = t.Append();
            sw = t.OpenWrite();
        }
        //以行的形式写入信息
        //sw.WriteLine(info);
        sw.Write(info, 0, info.Length);
        sw.Flush();
        sw.Close();
        sw.Dispose();
    }

/**
 * 专用于文本创建
* path：文件创建目录
* name：文件的名称
*  info：写入的内容
*/
    void WriteFile(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + name);
       // new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }

        //以行的形式写入信息
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }



    /**
    * path：删除文件的路径
    * name：删除文件的名称
    */
    public static void DeleteFile(string path, string name)
    {
        FileInfo t = new FileInfo(path + name);
        if (!t.Exists)
        {
            return;
        }
        File.Delete(path + name);
       
    }


    public static void DeleteFile(string name)
    {
        FileInfo t = new FileInfo(name);
        if (!t.Exists)
        {
            return;
        }
        File.Delete(name);
    }

    //删除文件夹
    public static void DeleteDirs(DirectoryInfo dirs)
    {
        UnlockDir(dirs);
        if (dirs == null || (!dirs.Exists))
        {
            return;
        }

        DirectoryInfo[] subDir = dirs.GetDirectories();
        if (subDir != null)
        {
            for (int i = 0; i < subDir.Length; i++)
            {
                if (subDir[i] != null)
                {
                    DeleteDirs(subDir[i]);
                }
            }
            subDir = null;
        }

        FileInfo[] files = dirs.GetFiles();
        if (files != null)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != null)
                {
                    //Debug.Log("删除文件:" + files[i].FullName + "__over");
                    Unlock(files[i].FullName);
                    files[i].Delete();
                    files[i] = null;
                }
            }
            files = null;
        }

        // Debug.Log("删除文件夹:" + dirs.FullName + "__over");
        dirs.Delete();
    }



    public static void NoDirectCreate(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void WriteText( string path, string fileName, string info)
    {
        NoDirectCreate(path);
        string filePath = path + "/" + fileName;
        using( StreamWriter sw = new StreamWriter( filePath, false))
        {
            sw.Write(info);
        }
    }

    public static void WriteText(string path, string fileName, byte[] info)
    {
        NoDirectCreate(path);
        string filePath = path + "/" + fileName;
        if (File.Exists(filePath))
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Truncate))
            {
                fs.Write(info, 0, info.Length);
            }
        }
        else
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(info, 0, info.Length);
            }
        }
    }

    public static void WriteText(string AllfileName, byte[] info)
    {
        if (info ==null)
        {
            return;
        }
        string filePartPath = Path.GetDirectoryName(AllfileName);

        NoDirectCreate(filePartPath);
        if (File.Exists(AllfileName))
        {
            using (FileStream fs = new FileStream(AllfileName, FileMode.Truncate))
            {
                fs.Write(info, 0, info.Length);
            }
        }
        else
        {
            using (FileStream fs = new FileStream(AllfileName, FileMode.Create))
            {
                fs.Write(info, 0, info.Length);
            }
        }
    }

    public static bool WriteFileText(string fileName, string text)
    {
        string filePartPath = GetDirFromAllFile(fileName);
        //路径
        NoDirectCreate(filePartPath);

        try
        {
            FileStream stream = new FileStream(fileName, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(text);
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 写行文本
    /// </summary>
    public static void WriteFileText(string fileName, byte[] info)
    {
        string filePartPath = GetDirFromAllFile(fileName);

        //DebugTool.LogFormat("获取文件名:filePartPath>" , filePartPath);

        filePartPath = Path.GetDirectoryName(fileName);

       // DebugTool.LogFormat("====再次====获取文件名:filePartPath>" , filePartPath);

        //路径
        NoDirectCreate(filePartPath);
#if UNITY_WEBPLAYER
#else
        File.WriteAllBytes(fileName, info);
#endif
    }

    /// <summary>
    /// 取得行文本
    /// </summary>
    public static string ReadFileText(string path)
    {
        return File.ReadAllText(path);
    }

    /**
 * 读取文本文件
 * path：读取文件的路径
 * name：读取文件的名称
 */
    public static List<string> ReadFile(string path, string name)
    {
        string filePath = path + "//" + name;
        return ReadFile(filePath);
    }


    public static List<string> ReadFile(string filePath)
    {
        //使用流的形式读取
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            //路径与名称未找到文件则直接返回空
            return null;
        }
        string line;
        List<string> arrlist = new List<string>();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取
            //将每一行的内容存入数组链表容器中
            arrlist.Add(line);
        }
        //关闭流
        sr.Close();
        //销毁流
        sr.Dispose();
        //将数组链表容器返回
        return arrlist;
    }

    /// <summary>
    /// 磁盘路径转换为Assets相对路径.
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>

    public static string FullPath2AssetPath(ref string src)
    {
        if (string.IsNullOrEmpty(src))
            return string.Empty;

        int index = src.IndexOf("Assets", System.StringComparison.Ordinal);
        return src.Substring(index, src.Length - index);
    }


    /// <summary>
    /// Assets相对路径转换为磁盘路径.
    /// </summary>
    /// <param name="src">Assets相对路径</param>
    /// <returns></returns>

    public static string AssetPath2FullPath(ref string src)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), src.Substring("Assets/".Length));
    }


    /// <summary>
    /// 获取目录名称
    /// </summary>
    /// <param name="src">路径</param>
    /// <returns></returns>

    public static string GetFolderName(string src)
    {
        if (Directory.Exists(src))
        {
            return new DirectoryInfo(src).Name;
        }
        return string.Empty;
    }


    /// <summary>
    /// 是否是Assets相对路径
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>

    public static bool IsAssetPath(ref string src)
    {
        return src.StartsWith("Assets");
    }


    /// <summary>
    /// 是否是目录
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>

    public static bool IsFolder(ref string src)
    {
        if (Directory.Exists(src))
            return true;
        return false;
    }


    /// <summary>
    /// 这个路径是否包含子目录.
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>

    public static bool HasSubFolder(ref string src)
    {
        if (Directory.Exists(src))
        {
            return Directory.GetDirectories(src).Length > 0;
        }

        return false;
    }


    public static void SaveXml(string filePath, object data)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(writer, data);
        }
    }


    public static T LoadXml<T>(string filePath) where T : class
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return xmlSerializer.Deserialize(reader) as T;
            }
        }
        return null;
    }


    public static string LoadTextAsset(string vPath)
    {
        TextAsset textAsset = Resources.Load(vPath) as TextAsset;

        if (textAsset == null)
        {
            //DebugTool.LogErrorFormatAndWrite("can not find :" , vPath);
            return null;
        }

        return textAsset.text;
    }
    public static void Unlock(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }

        var info = new FileInfo(path);
        if (info.IsReadOnly)
        {
           Debug.LogError("要保存的配置文件 Check out " + path);
           info.IsReadOnly = false;
        }
    }
    public static void UnlockDir(DirectoryInfo DirInfo)
    {
        DirInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
    }

    public static string CullPathExtension(string vPath)
    {
        int dotIndex = vPath.LastIndexOf(".");
        if (dotIndex > 0)
        {
            return vPath.Substring(0, vPath.LastIndexOf("."));
        }
        else
        {
            return vPath;
        }

    }
}
