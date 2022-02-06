using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace Server2
{
    class Program
    {
        static Dictionary<int, int> busyports = new Dictionary<int, int>();

        static int port = 8006; // порт для приема входящих запросов
        static int maxconnections = 5;
        static void Main(string[] args)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length < 2)
            {
                Thread myThread1 = new Thread(osnova);
                myThread1.Start();

                for (int i = 0; i < maxconnections; i++)
                {
                    Thread myThread = new Thread(new ParameterizedThreadStart(podkl));
                    myThread.Start(port + i);
                }
            }
        }


        static void osnova()
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.2"), 8005);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);
                //DateTime.Now.ToShortTimeString()

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);
                    string message = "";

                    if (builder.ToString().Split('|')[0].Equals("getfreeport"))
                    {
                        for (int i = 0; i < maxconnections; i++)
                        {
                            if (!busyports.ContainsValue(8006 + i))
                            {
                                if (busyports.ContainsKey(int.Parse(builder.ToString().Split('|')[1])))
                                {
                                    int value = 0;
                                    busyports.TryGetValue(int.Parse(builder.ToString().Split('|')[1]), out value);
                                    message = value.ToString();
                                    data = Encoding.Unicode.GetBytes(message);
                                    handler.Send(data);
                                    handler.Shutdown(SocketShutdown.Both);
                                    handler.Close();
                                    break;
                                }
                                busyports.Add(int.Parse(builder.ToString().Split('|')[1]), 8006 + i);
                                message = (8006 + i).ToString();
                                data = Encoding.Unicode.GetBytes(message);
                                handler.Send(data);
                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                                break;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            int value = 0;
                            busyports.TryGetValue(int.Parse(builder.ToString().Split('|')[1]), out value);
                            message = value.ToString();
                            busyports.Remove(int.Parse(builder.ToString().Split('|')[1]));
                            data = Encoding.Unicode.GetBytes(message);
                            handler.Send(data);
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                        catch
                        {
                            message = "error";
                            data = Encoding.Unicode.GetBytes(message);
                            handler.Send(data);
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void podkl(object x)
        {
            int port1 = (int)x;
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.2"), port1);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);
                //DateTime.Now.ToShortTimeString()
                Console.WriteLine($"Сервер {port1 - 8005} запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    string message = "";

                    var process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)[0];
                    int b = 0;

                    foreach (ProcessThread i in process.Threads)
                    {
                        if (i.UserProcessorTime != TimeSpan.Zero)
                        {
                            b++;
                        }
                    }
                    int a = process.Modules.Count;

                    Console.WriteLine(a + " " + b);

                    message = "Количество потоков на сервере: " + b+ "Количество модулей на сервере:" +a + " | " + DateTime.Now.ToString();
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
