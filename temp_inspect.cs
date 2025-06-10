using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var assembly = Assembly.LoadFrom("/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB/bin/Debug/netstandard2.1/CwLibNet.dll");
        
        Console.WriteLine("Assembly: " + assembly.GetName().Name);
        Console.WriteLine("Available types:");
        
        foreach (var type in assembly.GetTypes().Take(20))
        {
            Console.WriteLine($"  {type.FullName}");
        }
    }
}
