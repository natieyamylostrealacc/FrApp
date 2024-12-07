using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the FrApp!");
        Console.WriteLine("Provide a username you wish to use.");
        string username = Console.ReadLine();

        Console.WriteLine("Hello, " + username + "!");
        Console.WriteLine("1. Start Server");
        Console.WriteLine("2. Start Client");
        Console.Write("Choose an option (1 or 2): ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            // Start the server
            Console.Write("Enter port for the server: ");
            int port = int.Parse(Console.ReadLine());
            ChatServer.StartServer(port);  // Start the server
        }
        else if (choice == "2")
        {
            // Start the client
            Console.Write("Enter server IP address: ");
            string serverIp = Console.ReadLine();
            Console.Write("Enter server port: ");
            int port = int.Parse(Console.ReadLine());
            ChatClient.StartClient(serverIp, port);  // Start the client
        }
        else
        {
            Console.WriteLine("Invalid choice!");
        }
    }
}