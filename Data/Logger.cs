using System.Collections.Concurrent;
using System.Text.Json;
using System.Numerics;
using System.Threading.Tasks;
using System.IO;
using System;

namespace Data
{
    internal class Logger
    {
        private class LoggerSerialization
        {
            public LoggerSerialization(Vector2 position, string date, int id)
            {
                X = position.X;
                Y = position.Y;
                Date = date;
                Id = id;
            }

            public float X { get; }
            public float Y { get; }
            public string Date { get; }
            public int Id { get; }
        }

        ConcurrentQueue<LoggerSerialization> queue;
        public Logger()
        {
            queue = new ConcurrentQueue<LoggerSerialization>();
            Write();
        }

        private void Write()
        {
            Task.Run(async () =>
            {
                using StreamWriter streamWriter = new StreamWriter("logger.json");
                while (true)
                {
                    while (queue.TryDequeue(out LoggerSerialization item))
                    {
                        string jsonString = JsonSerializer.Serialize(item);
                        streamWriter.WriteLine(jsonString);
                    }
                    await streamWriter.FlushAsync();
                }
            });
        }

        public void Add(IBall obj, DateTime date)
        {
            queue.Enqueue(new LoggerSerialization(obj.Position, date.ToString("HH:mm:ss.fff"), obj.Id));
        }
    }
}
