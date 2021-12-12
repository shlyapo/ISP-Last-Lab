using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
namespace Auto
{
    public class StreamService
    {
        private object locker = new object();
        public Task WriteToStream(Stream stream)
        {
            return Task.Run(() =>
            {
                var Rand = new Random();
                lock (locker)
                {
                    Console.WriteLine($"Writing to stream [{Thread.CurrentThread.ManagedThreadId}]");
                    var streamWriter = new StreamWriter(stream) { AutoFlush = true };
                    for (byte i = 0; i < 100; i++)
                        streamWriter.WriteLine(i + "\nCar Model #{" + i + "}\n" + Rand.Next(0, 1));
                    Console.WriteLine($"Writing to stream end [{Thread.CurrentThread.ManagedThreadId}]");

                }

            }
            );
        }

        public Task CopyFromStream(Stream stream, string fileName)
        {

            return Task.Run(() =>
            {
                lock (locker)
                {
                    Console.WriteLine($"Copying from stream... [{Thread.CurrentThread.ManagedThreadId}]");
                    stream.Seek(0, SeekOrigin.Begin);
                    if (File.Exists(fileName)) File.Delete(fileName);
                    using var fileStream = File.Open(fileName, FileMode.Create);
                    stream.CopyTo(fileStream);
                    Console.WriteLine($"Copying from stream end [{Thread.CurrentThread.ManagedThreadId}]");
                }

            });
        }

        public async Task<int> GetStatisticsAsync(string filename, Func<Autopark, bool> filter) =>
            await Task.Run(() => GetStatistics(filename, filter));

        public async Task<int> GetStatistics(string fileName, Func<Autopark, bool> filter)
        {
            int count = 0;
            Console.WriteLine($"Calculating stats... [{Thread.CurrentThread.ManagedThreadId}]");
            var streamReader = new StreamReader(File.Open(fileName, FileMode.Open));
            List<Autopark> autos = new List<Autopark>();
            for (var i = 0; i < 100; i++)
            {

                autos.Add(new Autopark(Convert.ToByte(await streamReader.ReadLineAsync()), await streamReader.ReadLineAsync(), Convert.ToInt32(await streamReader.ReadLineAsync())));
                if (filter(autos[i])) count++;
                Console.WriteLine(autos[i].Name);

            }
            streamReader.Dispose();
            Console.WriteLine($"Calculating stats end [{Thread.CurrentThread.ManagedThreadId}]");
            return count;

        }
        public StreamService()
        {
        }
    }
}