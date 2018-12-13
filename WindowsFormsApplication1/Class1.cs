using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Data;
using System.Data.SqlClient;


namespace WindowsFormsApplication1
{
    class Class1
    {
        public IPEndPoint SeverIP;
        public int port = 6666;
        public void MainRun()
        {
            Socket_Start();
        }
        // 监听端口
        // 监听最大连接数
        private static int listenCount = 10;
        //负责监听客户端连接请求的线程
        private static Thread threadWatch = null;
        //用来保存客户端连接套接字
        private static Socket socketWatch = null;
        private void Socket_Start()
        {
            //string IP0 = GetAddressIP();

            SeverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            //SeverIP = new IPEndPoint(IPAddress.Parse(IP0), port);

            if (socketWatch == null)
            {
                socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            else
            {
                return;
            }

            socketWatch.Bind(SeverIP);
            socketWatch.Listen(listenCount);

            threadWatch = new Thread(WatchConnection);
            threadWatch.IsBackground = true;//设置为后台
            threadWatch.Start();// 启动线程

        }


        //监听客户端连接
        private void WatchConnection()
        {
            while (true)
            {
                if (socketWatch != null)
                {
                    //创建监听的连接套接字
                    Socket sockConnection = socketWatch.Accept();

                    //为客户端套接字连接开启新的线程用于接收客户端发送的数据
                    Thread t = new Thread(Receive);
                    //设置为后台线程
                    t.IsBackground = true;
                    //为客户端连接开启线程
                    t.Start(sockConnection);

                    Console.WriteLine("Receive Start");
                }
            }
        }
        private void Receive(object obj)
        {
            Socket client = obj as Socket;
            byte[] buffer = new byte[10000];
            byte[] sendbuffer = new byte[10000];
            bool keepaliave = true;
            int receivenum = 0;
            int UnitNo = 0;

            IPAddress remote_ip = ((System.Net.IPEndPoint)client.RemoteEndPoint).Address;
            byte[] ip4 = remote_ip.GetAddressBytes();
            byte ip = ip4[3];

            while (keepaliave)
            {
                try
                {
                    receivenum = client.Receive(buffer);

                    if (receivenum > 0)//正常联接
                    {
                        string recvStr = "";
                        byte[] recvBytes = new byte[10000];
                        int bytes;
                        //bytes = client.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
                        recvStr += Encoding.ASCII.GetString(buffer, 0, receivenum);
                        Console.WriteLine(recvStr);
                        sendbuffer = Encoding.ASCII.GetBytes(recvStr);
                        client.Send(sendbuffer);
                    }
                    else//断开联接
                    {

                        keepaliave = false;
                        client.Close();
                        client.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    keepaliave = false;
                    client.Close();
                }

            }

        }
        string[] Unitaddr = new string[10];
        string[] bagcode = new string[255];
        bool[,] divStateChange = new bool[10, 255];
        bool[,] divstate = new bool[10, 255];

    } 
}
