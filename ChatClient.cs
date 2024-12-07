using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatClient
{
    private static TcpClient tcpClient;
    private static NetworkStream stream;
    private static byte[] buffer = new byte[1024];
    private static string username;

    // This method starts the client and connects to the server
    public static void StartClient(string serverIp, int port)
    {
        tcpClient = new TcpClient(serverIp, port);
        stream = tcpClient.GetStream();

        Console.WriteLine("Enter your username: ");
        username = Console.ReadLine();

        // Send the username to the server
        SendMessage(username);

        Console.WriteLine("Connected to server.");

        // Start a thread to listen for incoming messages
        Thread listenThread = new Thread(ListenForMessages);
        listenThread.Start();

        // Main thread will handle sending messages
        while (true)
        {
            string message = Console.ReadLine();
            SendMessage(message);
        }
    }

    // Listen for incoming messages from the server
    private static void ListenForMessages()
    {
        while (true)
        {
            int bytesRead;
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(message);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Connection lost.");
                break;
            }
        }
    }

    // Send message to the server
    private static void SendMessage(string message)
    {
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        stream.Write(messageBytes, 0, messageBytes.Length);
    }
}