using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace CmdExecutor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(1251);

            string output, error;

            Console.WriteLine("wait...");

            //Вводимо любу команду
            Tuple<string, string> outputTuple = await CmdExecutorClass.CommandExecute("ipconfig");

            output = outputTuple.Item1;
            error = outputTuple.Item2;

            Console.Clear();
            // Виводимо результати
            Console.WriteLine("Output:");
            Console.WriteLine(output);

            Console.WriteLine("Error:");
            if (string.IsNullOrEmpty(error))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("There are no errors");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);

            Console.ReadLine();
        }
    }

    public static class CmdExecutorClass
    {
        public static async Task<Tuple<string, string>> CommandExecute(string command)
        {
            // Створюємо новий процес
            Process process = new Process();

            // Налаштовуємо параметри для нового процесу
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe", // Командний рядок
                Arguments = string.Format("/C chcp 65001 & {0} & exit", command), // Параметр /C вказує на виконання команди
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false // Щоб не створювалася вікно командного рядка
            };

            process.StartInfo = startInfo;

            // Запускаємо процес
            process.Start();

            // Очікуємо завершення процесу
            await Task.Run(() => process.WaitForExit());

            // Отримуємо вивід командного рядка
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            return Tuple.Create(output, error);
        }


    }
}
