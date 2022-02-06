using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace ClientOs
{
    public partial class Form1 : Form
    {
        public static int port = 8005;
        public static int mynumber = 1;
        public static bool inloop = false;
        public static string lastmessage = " ";


        public static string address = "127.0.0.1";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)//connect 1 ser
        {
            conserver1();
        }

        public void conserver1()
        {
            address = "127.0.0.1";
            port = 8005;
            try
            {
                IPEndPoint ipPoint1 = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint1);
                string message = "getfreeport|" + mynumber;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                port = int.Parse(builder.ToString());
                // закрываем сокет
                richTextBox1.Text += "Подключение на порту " + builder.ToString() + " открыто" + "\n";
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.Message + "\n";
            }

        }

        public void breakserver1()
        {
            address = "127.0.0.1";
            port = 8005;
            try
            {
                IPEndPoint ipPoint1 = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint1);

                string message = "breakport|" + mynumber;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                richTextBox1.Text += "Подключение на порту "+ builder.ToString() + " успешно завершено"+ "\n";
                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.Message + "\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)//connect 2 ser
        {
            conserver2();
        }

        public void conserver2()
        {
            address = "127.0.0.2";
            port = 8005;
            try
            {
                IPEndPoint ipPoint1 = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint1);
                string message = "getfreeport|" + mynumber;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                port = int.Parse(builder.ToString());
                // закрываем сокет
                richTextBox1.Text += "Подключение на порту " + builder.ToString() + " открыто" + "\n";
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.Message + "\n";
            }

        }

        public void breakserver2()
        {
            address = "127.0.0.2";
            port = 8005;
            try
            {
                IPEndPoint ipPoint1 = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint1);

                string message = "breakport|" + mynumber;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                richTextBox1.Text += "Подключение на порту " + builder.ToString() + " успешно завершено" + "\n";
                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.Message + "\n";
            }
        }



        private void button3_Click(object sender, EventArgs e)//send 1
        {
            try
            {
                server1(address, port);
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.Message + "\n";
            }
        }

        public void server1(string address1, int port1)
        {
            IPEndPoint ipPoint1 = new IPEndPoint(IPAddress.Parse(address1), port1);
            Socket socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string message = "dfgh";
            byte[] data;
            int bytes;
            StringBuilder builder = new StringBuilder();

            // подключаемся к удаленному хосту
            socket1.Connect(ipPoint1);
            data = Encoding.Unicode.GetBytes(message);
            socket1.Send(data);
            data = new byte[256]; // буфер для ответа
            bytes = 0; // количество полученных байт
            do
            {
                bytes = socket1.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket1.Available > 0);
            richTextBox1.Text += "ответ сервера: " + builder.ToString() + ";" + "\n";
            richTextBox2.Text = "";
            // закрываем сокет
            socket1.Shutdown(SocketShutdown.Both);
            socket1.Close();
        }

        private void button4_Click(object sender, EventArgs e)//send 2
        {
            try
            {
                server2(address, port);
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.Message + "\n";
            }
        }

        private void server2(string address, int port)
        {
            IPEndPoint ipPoint1 = new IPEndPoint(IPAddress.Parse(address), port);
            Socket socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string message = "dfgh";
            byte[] data;
            int bytes;
            StringBuilder builder = new StringBuilder();

            // подключаемся к удаленному хосту
            socket1.Connect(ipPoint1);
            data = Encoding.Unicode.GetBytes(message);
            socket1.Send(data);
            data = new byte[256]; // буфер для ответа
            bytes = 0; // количество полученных байт
            do
            {
                bytes = socket1.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket1.Available > 0);
            richTextBox1.Text += "ответ сервера: " + builder.ToString() + ";" + "\n";
            richTextBox2.Text = "";
            // закрываем сокет
            socket1.Shutdown(SocketShutdown.Both);
            socket1.Close();
        }

        private void button5_Click(object sender, EventArgs e)//stop1
        {
            breakserver1();
        }

        private void button6_Click(object sender, EventArgs e)//stop2
        {
            breakserver2();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                Random rnd = new Random();
                mynumber = rnd.Next(0, 100000); 
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (!richTextBox2.Text.Equals(lastmessage))
            {
                lastmessage = richTextBox2.Text;
                if (address.Equals("127.0.0.2"))
                {
                    try
                    {
                        server2(address, port);
                    }
                    catch (Exception ex)
                    {
                        richTextBox1.Text = ex.Message + "\n";
                    }
                }
                else
                {
                    try
                    {
                        server1(address, port);
                    }
                    catch (Exception ex)
                    {
                        richTextBox1.Text = ex.Message + "\n";
                    }
                }
            }
            else
            {
                richTextBox1.Text += "not changed\n";
            }
            
        }
    }
}
