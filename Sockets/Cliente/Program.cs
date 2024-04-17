using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Cliente;

class Cliente
{
    private Thread recibir;
    private Thread enviar;
    private Socket handler;


    public Socket Handler { get => handler; set => handler = value; }


   public Cliente()
    {
        this.recibir = new (Receive);
        this.enviar = new (Send);
        this.recibir.Start();
        this.enviar.Start();
    }


    public void Receive()
    {
        while (true)
        {
            string data = null;
            byte[] bytes = null;

            while (true)
            {
                try
                {
                    bytes = new byte[1024];
                    int byteRec = this.handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, byteRec);

                    if (data.IndexOf("<EOL>") > -1)
                        break;                  
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                data = data.Replace("<EOL>", "");
                Console.WriteLine(data);
            }
        }
    }


    public void Send()
    {
        string mensaje = "";
        Console.WriteLine(">> Ingrese uno o varios mensajes para enviar");

        while (mensaje != "exit")
        {
            mensaje = Console.ReadLine();

            if (mensaje != "exit")
            {
                byte[] msj = Encoding.ASCII.GetBytes(mensaje + "<EOL>");
                int byteSent = this.handler.Send(msj);
            }
        }

        this.handler.Shutdown(SocketShutdown.Both);
        this.handler.Close();
        Environment.Exit(0);
    }
}








// ----------------------------------------------------------------------------------

class Program
{
    static void Main(string[] args)
    {
        // Se establecen las variables que se van a utilizar para definir un protocolo de comunicación.
        
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress iPAddress = host.AddressList[0];
        IPEndPoint remoteEP = new (iPAddress, 8000);


        try
        {
            // Se establece conexión con el Servidor. Luego, se le pide al Cliente ingresar un nickname
            // que se envía codificado al Servidor.

            Socket sender = new (iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);

            Console.Write(">> Ingrese nickname: ");
            string comando = Console.ReadLine();

            byte[] msj = Encoding.ASCII.GetBytes(comando + "<EOL>");
            sender.Send(msj);

            // Cada cliente que quiera establecer conexión con el Servidor, debe utilizar un hilo difrente
            // detallado en la clase 'Cliente'.

            Cliente conect = new ();
            conect.Handler = sender;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
