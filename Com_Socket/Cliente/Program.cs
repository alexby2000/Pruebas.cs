using System.Net.Sockets;
using System.Text;

namespace Cliente;
class Cliente
{
    public static void Main()
    {
        TcpClient clientSocket = new TcpClient();
        clientSocket.Connect("127.0.0.1", 8888);
        Console.WriteLine(" >> Conexión Establecida !!!");
        NetworkStream serverStream = clientSocket.GetStream();

        string mensaje = Console.ReadLine();

        byte[] outStream = Encoding.ASCII.GetBytes(mensaje + "$");
        serverStream.Write(outStream, 0, outStream.Length);
        serverStream.Flush();

        byte[] inStream = new byte[10025];
        serverStream.Read(inStream, 0, inStream.Length);
        string returnData = Encoding.ASCII.GetString(inStream);
        Console.WriteLine(" >> " + returnData);

        clientSocket.Close();

    }
}
