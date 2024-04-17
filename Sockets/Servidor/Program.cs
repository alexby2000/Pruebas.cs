﻿using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Servidor;

class Servidor
{
    private string nickname;
    private Thread hiloRecib;
    private Socket handler;


    public string Nickname { get => nickname; set => nickname = value; }
    public Socket Hanlder { get => handler; set => handler = value; }



    public Servidor()
    {
        this.hiloRecib = new Thread(Receive);
        hiloRecib.Start();
    }

    private void Send(string mensaje)
    {
        byte[] msj = Encoding.ASCII.GetBytes(mensaje + "<EOL>");
        int byteSent = this.handler.Send(msj);
        Console.WriteLine("Enviado: ", mensaje);
    }


    private void Receive()
    {
        while (true)
        {
            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int byteRec = this.handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, byteRec);

                if (data.IndexOf("<EOL>") > -1)
                    break;
            }

            data = data.Replace("<EOL>", "");
            Console.WriteLine(this.nickname + ": " + data);
        }
    }


}









// ---------------------------------------------------------------------------------------------------------

class Program
{
    // El diccionario permite guardar la referencia de cada hilo creado, almacenado en memoria.
    static Dictionary <string, Servidor> dic = new();


    static void Main(string[] args)
    {
        // Se define el protocolo de comunicación.

        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress iPAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new(iPAddress, 8000);

        try
        {
            // El servidor entra en modo de escucha y espera a que algún cliente establezca conexión.

            Socket listener = new(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);
            Console.WriteLine(">> Esperando conexión con un Cliente...");

            while (true)
            {
                Socket handler = listener.Accept();
                string data = null;
                byte [] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int byteRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, byteRec);

                    if (data.IndexOf("<EOL>") > -1)
                        break;
                }

                data = data.Replace("<EOL>", "");

                // Aquí cada cliente que se comunica por primera vez con el servidor, se le asigna
                // un hilo en el cual pueda interactuar de manera aislada.

                Servidor cliente = new();
                cliente.Nickname = data;
                cliente.Hanlder = handler;
                dic.Add(data, cliente);

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

    }
}
