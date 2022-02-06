using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Server1
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
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("User32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);



        static void osnova()
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);

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
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port1);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);
                //DateTime.Now.ToShortTimeString()
                Console.WriteLine($"Сервер {port1-8005} запущен. Ожидание подключений...");

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

                    int height = (int)SystemParameters.PrimaryScreenHeight;
                    int width = (int)SystemParameters.PrimaryScreenWidth;//ekran
                    int maxw = Console.LargestWindowWidth;
                    int maxh = Console.LargestWindowHeight;

                    int prew = Console.WindowWidth;
                    int preh = Console.WindowHeight;

                    Console.Title = "WinAPI113";
                    var hWnd = FindWindow(null, Console.Title);
                    var wndRect = new RECT();
                    GetWindowRect(hWnd, out wndRect);
                    string s = wndRect.Right + "- правая граница";
                    s += wndRect.Left + "- левая граница";
                    s += wndRect.Bottom + "- нижняя граница";
                    s += wndRect.Top + "- верхняя граница";

                    Console.WriteLine(s);//okno
                    message = "Координатые окна сервера: " + s + "; Разрешение экрана: " + width + "x" + height + " | " + DateTime.Now.ToString();
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
