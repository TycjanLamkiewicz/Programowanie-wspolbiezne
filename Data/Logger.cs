using System.Collections.Concurrent;
using System.Text.Json;
using System.Numerics;
using System.Threading.Tasks;
using System.IO;

namespace Data
{
    // Need to check
    // poprawienie czasu stopwatch w ballu
    // sprawdzic czy jest ten bufor, ale wedlug mnie za to odpowiada concurrentqueue
    // sekcja krytyczna w loggerze, ale chyba te nasze propertisy w loggerze sa immutable wiec chyba nie trzeba?
    // sprawdzic, czy jeżeli nie da się zapisać do bufora to mają być te dane utracone 
    
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

        public void Add(IBall obj, string date)
        {
            queue.Enqueue(new LoggerSerialization(obj.Position, date, obj.Id));
        }
    }
}
