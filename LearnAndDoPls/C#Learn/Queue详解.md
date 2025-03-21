# Queue（队列）详解

Queue<T>是C#中的泛型队列集合，实现了先进先出（FIFO: First-In-First-Out）的数据结构。队列的特点是从一端添加元素，从另一端移除元素，就像排队一样，最先排队的人最先办理业务。

## Queue<T>的主要特性

- **先进先出(FIFO)**: 最先添加的元素最先被移除
- **动态大小**: 队列会根据需要自动调整大小
- **类型安全**: 通过泛型保证队列中所有元素都是同一类型
- **高效操作**: 队列的主要操作（入队和出队）的时间复杂度为O(1)

## 命名空间和类定义

```csharp
namespace System.Collections.Generic
{
    public class Queue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        public Queue();
        public Queue(IEnumerable<T> collection);
        public Queue(int capacity);

        public int Count { get; }

        public void Clear();
        public bool Contains(T item);
        public void CopyTo(T[] array, int arrayIndex);
        public T Dequeue();
        public void Enqueue(T item);
        public Enumerator GetEnumerator();
        public T Peek();
        public T[] ToArray();
        public void TrimExcess();
        public bool TryDequeue(out T result);
        public bool TryPeek(out T result);

        public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            public T Current { get; }

            public void Dispose();
            public bool MoveNext();
        }
    }
}
```

## 构造函数详解

1. **Queue()**
   - 创建一个空队列，初始容量为默认值
   - 例：`Queue<int> queue = new Queue<int>();`

2. **Queue(IEnumerable<T> collection)**
   - 创建一个队列，并将指定集合中的元素复制到新队列中
   - 例：`Queue<int> queue = new Queue<int>(new int[] {1, 2, 3});`

3. **Queue(int capacity)**
   - 创建一个具有指定初始容量的空队列
   - 可以提前设置容量以减少动态调整大小的次数，提高性能
   - 例：`Queue<string> queue = new Queue<string>(100);`

## 属性详解

**Count**
- 获取队列中包含的元素数量
- 只读属性，不能直接修改
- 例：`int count = queue.Count;`

## 方法详解

1. **Clear()**
   - 清除队列中的所有元素
   - 队列的容量保持不变，但元素个数变为0
   - 例：`queue.Clear();`

2. **Contains(T item)**
   - 判断指定元素是否在队列中
   - 返回布尔值：true表示元素在队列中，false表示不在
   - 使用EqualityComparer<T>.Default.Equals进行比较
   - 例：`bool hasItem = queue.Contains(42);`

3. **CopyTo(T[] array, int arrayIndex)**
   - 将队列的元素复制到一个数组中，从指定的索引位置开始
   - array: 目标数组
   - arrayIndex: 目标数组的起始索引
   - 例：`int[] arr = new int[10]; queue.CopyTo(arr, 0);`

4. **Dequeue()**
   - 移除并返回队列开头的元素
   - 如果队列为空，抛出InvalidOperationException异常
   - 例：`int value = queue.Dequeue();`

5. **Enqueue(T item)**
   - 在队列末尾添加一个元素
   - 如果需要，队列会自动扩容
   - 例：`queue.Enqueue(100);`

6. **GetEnumerator()**
   - 返回一个遍历队列的枚举器
   - 可用于foreach循环
   - 例：`foreach(var item in queue) { ... }`

7. **Peek()**
   - 返回队列开头的元素，但不将其移除
   - 如果队列为空，抛出InvalidOperationException异常
   - 例：`int value = queue.Peek();`

8. **ToArray()**
   - 将队列中的元素复制到一个新数组中
   - 返回的数组中的元素按照出队顺序排列
   - 例：`int[] array = queue.ToArray();`

9. **TrimExcess()**
   - 设置容量为队列中元素的实际数量
   - 可以释放未使用的内存
   - 一般在队列大小稳定后调用
   - 例：`queue.TrimExcess();`

10. **TryDequeue(out T result)**
    - 尝试移除并返回队列开头的元素
    - 如果成功，返回true并将元素值赋给result参数
    - 如果队列为空，返回false，result为默认值
    - 例：`if(queue.TryDequeue(out int value)) { ... }`

11. **TryPeek(out T result)**
    - 尝试返回队列开头的元素，但不将其移除
    - 如果成功，返回true并将元素值赋给result参数
    - 如果队列为空，返回false，result为默认值
    - 例：`if(queue.TryPeek(out string value)) { ... }`

## 枚举器 (Enumerator) 结构

队列的Enumerator结构允许遍历队列中的元素：

- **Current**: 获取枚举数当前位置的元素
- **Dispose()**: 释放枚举器使用的所有资源
- **MoveNext()**: 将枚举数推进到集合的下一个元素

## 使用示例

```csharp
// 创建队列
Queue<string> queue = new Queue<string>();

// 添加元素
queue.Enqueue("第一个");
queue.Enqueue("第二个");
queue.Enqueue("第三个");

// 查看队列长度
Console.WriteLine($"队列长度: {queue.Count}");  // 输出: 队列长度: 3

// 查看队首元素但不移除
string peekResult = queue.Peek();
Console.WriteLine($"队首元素: {peekResult}");  // 输出: 队首元素: 第一个

// 移除并获取队首元素
string item = queue.Dequeue();
Console.WriteLine($"出队元素: {item}");  // 输出: 出队元素: 第一个
Console.WriteLine($"出队后队列长度: {queue.Count}");  // 输出: 出队后队列长度: 2

// 遍历队列
Console.WriteLine("队列中的所有元素:");
foreach(string s in queue)
{
    Console.WriteLine(s);  // 输出: 第二个 和 第三个
}
```

## 在Unity中的应用

在Unity游戏开发中，Queue<T>常被用于：

1. **对象池系统**: 管理可重用游戏对象
2. **AI行为队列**: 存储NPC要执行的一系列动作
3. **事件系统**: 按顺序处理游戏事件
4. **关卡加载**: 管理需要加载的关卡或资源
5. **网络消息队列**: 处理网络通信中的消息

## 性能考虑

- Queue<T>的Enqueue和Dequeue操作的时间复杂度为O(1)
- Contains方法需要遍历整个队列，时间复杂度为O(n)
- 当频繁使用队列且大小可预测时，使用有容量参数的构造函数可以提高性能

## 形象示例：队列在游戏中的应用

### 1. 餐厅排队系统
想象一个餐厅经营游戏，顾客按照先来后到的顺序排队等待座位：

```csharp
public class Restaurant : MonoBehaviour
{
    private Queue<Customer> waitingLine = new Queue<Customer>();
    private int maxSeats = 5;
    private int occupiedSeats = 0;

    // 新顾客到来
    public void CustomerArrived(Customer customer)
    {
        if (occupiedSeats < maxSeats)
        {
            ServeCustomer(customer);
        }
        else
        {
            // 没有空位，加入等待队列
            waitingLine.Enqueue(customer);
            Debug.Log($"请耐心等待，前面还有{waitingLine.Count}位顾客");
        }
    }

    // 当有顾客离开时
    public void CustomerLeft()
    {
        occupiedSeats--;
        
        // 检查等待队列是否有人
        if (waitingLine.Count > 0)
        {
            Customer nextCustomer = waitingLine.Dequeue();
            ServeCustomer(nextCustomer);
        }
    }

    private void ServeCustomer(Customer customer)
    {
        occupiedSeats++;
        Debug.Log($"请{customer.Name}就座");
    }
}
```

### 2. 技能连招系统
在格斗游戏中，玩家输入的按键组合需要按顺序依次处理：

```csharp
public class ComboSystem : MonoBehaviour
{
    private Queue<KeyCode> inputBuffer = new Queue<KeyCode>();
    private float bufferTime = 2f; // 输入缓存时间
    private float lastInputTime;

    void Update()
    {
        // 检查并清理过期的输入
        if (Time.time - lastInputTime > bufferTime)
        {
            inputBuffer.Clear();
        }

        // 记录新的按键输入
        if (Input.GetKeyDown(KeyCode.J)) // 拳击
        {
            inputBuffer.Enqueue(KeyCode.J);
            lastInputTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.K)) // 踢腿
        {
            inputBuffer.Enqueue(KeyCode.K);
            lastInputTime = Time.time;
        }

        // 检查是否触发特殊连招
        CheckCombo();
    }

    private void CheckCombo()
    {
        // 比如：拳-拳-踢 组合技
        if (inputBuffer.Count >= 3)
        {
            var inputs = inputBuffer.ToArray();
            if (inputs[0] == KeyCode.J && 
                inputs[1] == KeyCode.J && 
                inputs[2] == KeyCode.K)
            {
                Debug.Log("触发连招：双拳回旋踢！");
                inputBuffer.Clear();
            }
        }
    }
}
```

### 3. 音乐播放队列
音乐播放器中的播放列表实现：

```csharp
public class MusicPlayer : MonoBehaviour
{
    private Queue<AudioClip> playlist = new Queue<AudioClip>();
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 添加歌曲到播放列表
    public void AddToPlaylist(AudioClip song)
    {
        playlist.Enqueue(song);
        Debug.Log($"已添加歌曲到播放列表，当前列表还有{playlist.Count}首歌");
    }

    void Update()
    {
        // 当前歌曲播放完毕后，自动播放下一首
        if (!audioSource.isPlaying && playlist.Count > 0)
        {
            PlayNextSong();
        }
    }

    private void PlayNextSong()
    {
        if (playlist.Count > 0)
        {
            AudioClip nextSong = playlist.Dequeue();
            audioSource.clip = nextSong;
            audioSource.Play();
            Debug.Log($"正在播放：{nextSong.name}");
        }
    }
}
```

### 4. 任务系统
RPG游戏中的任务系统，确保任务按正确顺序完成：

```csharp
public class QuestManager : MonoBehaviour
{
    private Queue<Quest> questLine = new Queue<Quest>();

    void Start()
    {
        // 初始化主线任务队列
        InitializeMainQuests();
    }

    private void InitializeMainQuests()
    {
        // 按剧情顺序添加主线任务
        questLine.Enqueue(new Quest("拜访村长", "去村长家了解情况"));
        questLine.Enqueue(new Quest("寻找药材", "在森林中收集三种药材"));
        questLine.Enqueue(new Quest("制作药剂", "将收集的药材制作成药剂"));
        questLine.Enqueue(new Quest("治疗村民", "将药剂交给生病的村民"));

        Debug.Log($"已载入{questLine.Count}个主线任务");
        
        // 显示当前任务
        if (questLine.Count > 0)
        {
            Quest currentQuest = questLine.Peek();
            Debug.Log($"当前任务：{currentQuest.Name}");
        }
    }

    public void CompleteCurrentQuest()
    {
        if (questLine.Count > 0)
        {
            Quest completedQuest = questLine.Dequeue();
            Debug.Log($"完成任务：{completedQuest.Name}");

            // 显示下一个任务
            if (questLine.Count > 0)
            {
                Quest nextQuest = questLine.Peek();
                Debug.Log($"下一个任务：{nextQuest.Name}");
            }
            else
            {
                Debug.Log("恭喜！所有主线任务已完成！");
            }
        }
    }
}

public class Quest
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Quest(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
```

这些例子展示了Queue在游戏开发中的实际应用场景：

1. **餐厅排队系统**：完美展示了先进先出的队列特性，就像现实生活中的排队一样。
2. **技能连招系统**：展示了队列在处理有序输入序列时的应用。
3. **音乐播放队列**：展示了队列在管理播放列表时的便利性。
4. **任务系统**：展示了队列在管理需要按特定顺序完成的游戏内容时的应用。

每个示例都包含了完整的代码实现，并且都是游戏开发中常见的实际应用场景。通过这些例子，我们可以看到Queue不仅是一个简单的数据结构，更是解决许多实际问题的有力工具。