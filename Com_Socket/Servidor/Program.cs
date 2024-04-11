using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Servidor;
class Servidor
{
    public static void Main()
    {
        // Se define la conexión inicial (IP + Puerto)

        IPAddress direccionIP = IPAddress.Parse("127.0.0.1");
        TcpListener servidorSocket = new TcpListener(direccionIP, 8888);
        TcpClient clienteSocket = default(TcpClient);


        // Se inicializa el Servidor y se establece la conexión con el Cliente

        servidorSocket.Start();
        Console.WriteLine(" >> Servidor Iniciado");
        Console.WriteLine(" >> Esperando la vinculación con el Cliente...");
        clienteSocket = servidorSocket.AcceptTcpClient();
        Console.WriteLine(" >> Cliente conectado !!!");
        NetworkStream networkStream = clienteSocket.GetStream();


        // Se administra el buffer con los datos enviados por el Cliente y se traducen los bits recibidos
        // en un mensaje legible almacenado en un string.

        byte[] bytesFrom = new byte[10025];
        networkStream.Read(bytesFrom, 0, bytesFrom.Length);
        string dataFromClient = Encoding.ASCII.GetString(bytesFrom);
        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));


        // Envía al Cliente un aviso del último mensaje recibido codificado en un array de Byte

        Console.WriteLine(" >> Datos del Cliente - " + dataFromClient);
        string serverResponse = "Ultimo mesaje del Cliente RECIBIDO. Fin de la Conexion !";
        Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
        networkStream.Write(sendBytes, 0, sendBytes.Length);
        networkStream.Flush();


        // Cierra la comunicaci+on con el Cliente y los procesos activos asociados

        clienteSocket.Close();
        servidorSocket.Stop();
        Console.WriteLine(" >> Salida de aplicación...");
        Console.ReadLine();

    }
}
