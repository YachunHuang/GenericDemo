using System;
using System.Collections.Generic;

namespace GenericDemo
{
    /// <summary>
    /// 定義 IMessage
    /// </summary>
    public interface IMessage
    {
        string TaskDescription { get; }
    }

    /// <summary>
    /// 定義裝修任務
    /// </summary>
    public class RenovationTask : IMessage
    {
        public string TaskDescription { get; }

        public RenovationTask(string taskDescription)
        {
            TaskDescription = taskDescription;
        }
    }

    /// <summary>
    /// 定義 IMessageQueue
    /// </summary>
    public interface IMessageQueue
    {
        void Receive<T>(Action<T> action) where T : IMessage;
    }

    /// <summary>
    /// 定義訊息佇列
    /// </summary>
    public class MessageQueue : IMessageQueue
    {
        private Queue<IMessage> taskQueue = new Queue<IMessage>(); // 任務佇列

        /// <summary>
        /// 用來接收任務並執行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public void Receive<T>(Action<T> action) where T : IMessage
        {
            while (taskQueue.Count > 0)
            {
                T task = (T)taskQueue.Dequeue();
                action(task); // 執行任務
            }
        }

        /// <summary>
        /// 新增任務到佇列
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(IMessage task)
        {
            taskQueue.Enqueue(task);
        }
    }

    // 定義工人類別
    public class Worker
    {
        public void HandleTask(RenovationTask task)
        {
            Console.WriteLine($"工人正在處理: {task.TaskDescription}");
        }
    }

    class Program
    {
        static void Main()
        {
            // 工人
            Worker worker = new Worker();

            // 訊息佇列
            IMessageQueue messageQueue = new MessageQueue();

            // 給兩個任務
            RenovationTask task1 = new RenovationTask("粉刷牆壁，顏色：白色");
            RenovationTask task2 = new RenovationTask("訂製客廳沙發和茶几");

            // 把任務加入佇列
            ((MessageQueue)messageQueue).AddTask(task1);
            ((MessageQueue)messageQueue).AddTask(task2);

            // 使用 IMessageQueue 接收並處理任務
            messageQueue.Receive<RenovationTask>(task => worker.HandleTask(task));

            Console.ReadLine();
        }
    }
}
