using System.Collections.Concurrent;
using System.Text.Json;
using System.Numerics;
using System.Threading.Tasks;
using System.IO;

namespace Data
{
    // Uncomment these lines in Data.cs
    // From 17 to 30
    // 34
    // Maybe add an ID to the balls
    // Need to check
    //   - [ ] protect balls velocity against any influence from other balls and the environmental behavior
    //   - [ ] prove that the protection of data (balls position on the screen) integration is implemented

    internal class Logger
    {
        private class LoggerSerialization
        {
            public LoggerSerialization(Vector2 position, string date)
            {
                X = position.X;
                Y = position.Y;
                Date = date;
            }

            public float X { get; }
            public float Y { get; }
            public string Date { get; }
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

        public void Add(IBall obj, string date)
        {
            queue.Enqueue(new LoggerSerialization(obj.Position, date));
        }
    }
}
