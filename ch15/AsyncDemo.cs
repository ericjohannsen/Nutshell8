using System;
using System.IO;

namespace ch15
{
    public class AsyncDemo
    {
        async static public void Run()
        {
            using (Stream s = new FileStream ("test.txt", FileMode.Create))
            {
                byte[] block = { 1, 2, 3, 4, 5 };
                await s.WriteAsync (block, 0, block.Length);    // Write asychronously

                s.Position = 0;                       // Move back to the start

                // Read from the stream back into the block array:
                Console.WriteLine (await s.ReadAsync (block, 0, block.Length));   // 5
            }

            // Clean up
            File.Delete("test.txt");
        }

    }
}