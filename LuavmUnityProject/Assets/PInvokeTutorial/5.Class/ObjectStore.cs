
/// JacksonDunstan, http://JacksonDunstan.com/articles/3908

//结构体可以不受GC影响，并更有效地使用CPU数据缓存。
//但结构体不能包含一些Unity和.NET类，如MonoBehaviour和string。
//如果您的结构中包含任何这些字段，则无法再使用sizeof(MyStruct)。
/*
那是因为string是一个托管类，我们不知道它的实例的大小。因此，如果我们想要使用任何托管类，我们需要一个解决方法。
这基本上是一个给定的类string，因为类MonoBehaviour在日常Unity编程中是必不可少的。
解决方法是 添加一个间接层。   我们存储的东西不是直接存储在string结构中，而是存储可用于获取string的东西。所以我们使用一个简单的int
因此，我们将在结构体外部存储托管对象，并使用它int来识别个体object。
我们只需要一个存储对象数组和另一个可用int句柄数组，我们将其视为一个堆栈。
1.要存储对象，从堆栈中弹出一个句柄，并将其用作对象数组的索引，以将对象存储在那里。
2.要获取存储对象，只需使用句柄索引数组！
3.要删除对象，使用句柄索引到数组并将其设置为null，然后将句柄推入堆栈。
*/

//存储对象并允许通过int访问它们。该类是线程安全的。
public static class ObjectStore
{
    // Stored objects. The first is always null.
    private static object[] objects;

    // Stack of available handles
    private static int[] handles;

    // Index of the next available handle
    private static int nextHandleIndex;

    //初始化对象存储并重置句柄
    //存储对象的最大数量。必须是积极的。
    public static void InitObjects(int maxObjects)
    {
        //将对象初始化为null，并初始化为空。
        objects = new object[maxObjects + 1];

        //初始化句柄堆栈为1,2,3，…
        handles = new int[maxObjects];
        for (int i = 0, handle = maxObjects;i < maxObjects; ++i, --handle)
        {
            //用数组模拟栈
            handles[i] = handle;
        }
        nextHandleIndex = maxObjects - 1;
    }

    //存储对象obj，存储对象的句柄可以是空的。
    // 如果Init尚未被调用，"NullReferenceException"将被抛出。
    public static int Store(object obj)
    {
        lock (objects)
        {
            // Pop a handle off the stack
            int handle = handles[nextHandleIndex];
            nextHandleIndex--;

            // Store the object
            objects[handle] = obj;

            // Return the handle
            return handle;
        }
    }

    /// <summary>
    /// Get the object for a given handle
    /// </summary>
    /// 
    /// <param name="handle">
    /// Handle of the object to get. If this is less than zero
    /// or greater than the maximum number of objects passed to
    /// <see cref="InitObjects"/>, this function will throw an
    /// <see cref="ArrayIndexOutOfBoundsException"/>. If this
    /// is zero, not a handle returned by <see cref="Store"/>,
    /// a handle returned by a call to <see cref="Store"/> with
    /// a null parameter, or a handle passed to
    /// <see cref="Remove"/> and not subsequently returned by
    /// <see cref="Store"/>, this function will return null. If
    /// <see cref="InitObjects"/> has not yet been called, a
    /// <see cref="NullReferenceException"/> will be thrown.
    /// </param>
    public static object Get(int handle)
    {
        return objects[handle];
    }

    /// <summary>
    /// Remove a stored object
    /// </summary>
    /// 
    /// <param name="handle">
    /// Handle of the object to Remove. If this is less than
    /// zero or greater than the maximum number of objects
    /// passed to <see cref="InitObjects"/>, this function will throw
    /// an <see cref="ArrayIndexOutOfBoundsException"/>. The
    /// handle may be be reused. If <see cref="InitObjects"/> has not
    /// yet been called, a <see cref="NullReferenceException"/>
    /// will be thrown.
    /// </param>
    public static void Remove(int handle)
    {
        lock (objects)
        {
            // Forget the object
            objects[handle] = null;

            // Push the handle onto the stack
            nextHandleIndex++;
            handles[nextHandleIndex] = handle;
        }
    }
}