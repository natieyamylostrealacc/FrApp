using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;

class ChatServer
{
    private static List<(NetworkStream, string)> clientStreams = new List<(NetworkStream, string)>();

    // This method starts the server and listens for incoming clients
    public static void StartServer(int port)
    {
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine("Server started on port " + port);

        while (true)
        {
            // Accept new clients
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("New client connected.");

            // Start a new thread to handle the client
            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    // Handle client communication
    private static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;
        string username = string.Empty;

        try
        {
            // First, receive the username from the client
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                username = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine("Username: " + username);
            }

            clientStreams.Add((stream, username));

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"{username} says: {message}");
                BroadcastMessage($"{username}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            clientStreams.RemoveAll(c => c.Item1 == stream);
            stream.Close();
            client.Close();
        }
    }

    // Broadcast a message to all clients
    private static void BroadcastMessage(string message)
    {
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        foreach (var (stream, _) in clientStreams)
        {
            stream.Write(messageBytes, 0, messageBytes.Length);
        }
    }
}