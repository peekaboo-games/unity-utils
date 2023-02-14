using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

public class JSONUtils
{

    /**
     * <summary>保存一个json对象</summary>
     * <param name="data">JSON 对象</param>
     * <param name="fileName">文件名称</param>
     */
    public static void Save<T>(T data, string fileName)
    {
       // SaveAsync(data, fileName);
    }

    /// <summary>
    /// 异步保存数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static Task SaveAsync<T>(T data, string fileName)
    {
        return File.WriteAllTextAsync(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName, JsonConvert.SerializeObject(data));
    }

    /**
    * <summary>保存一个json对象，文件名是 typeof name</summary>
    * <param name="data">JSON 对象</param>
    */
    public static void Save<T>(T data)
    {
        Save(data, typeof(T).Name);
    }

    public static Task SaveAsync<T>(T data)
    {
        return SaveAsync(data, typeof(T).Name);
    }

    /**
     * <summary>删除一个文件</summary>
     * <param name="fileName">要删除的文件名称</param>
     * <returns>是否已删除，true 删除，false 未删除或不存在</returns>
     */
    public static bool Delete(string fileName)
    {
        FileInfo file = new(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName);
        // 如果文件不存在，则创建一个新的
        if (file.Exists)
        {
            file.Delete();
            return true;
        }
        return false;
    }

    /**
     * <summary>删除 typeof name 文件</summary>
     * <returns>删除结果</returns>
     */
    public static bool Delete<T>()
    {
        return Delete(typeof(T).Name);
    }

    /**
     * <summary>将 object 序列化成 json</summary>
     * <param name="data">要序列化的数据</param>
     */
    public static string ToJSON(object data)
    {
        return JsonConvert.SerializeObject(data);
    }

    /**
     * <summary>从文件中加载并序列化一个对象</summary>
     * <param name="fileName">文件名称</param>
     * <returns>序列化的对象，如果没有则返回nullable</returns>
     */
    public static T Load<T>(string fileName)
    {
        Task<T> task = LoadAsync<T>(fileName);
        task.Wait();
        return task.Result;
    }

    public static Task<T> LoadAsync<T>()
    {
        return LoadAsync<T>(typeof(T).Name);
    }

    public static async Task<T> LoadAsync<T>(string fileName)
    {
        FileInfo file = new(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName);
        if (!file.Exists)
        {
            return default;
        }
        string data = await File.ReadAllTextAsync(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName, Encoding.UTF8);
        return JsonConvert.DeserializeObject<T>(data);
    }

    /**
     * <summary>加载数据，文件名是 typeof name</summary>
     * <returns>加载完的数据</returns>
     */
    public static T Load<T>()
    {
        return Load<T>(typeof(T).Name);
    }

}