// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");
string serverAddress = "example.com";
int serverPort = 443;

try
{
    using (TcpClient client = new TcpClient(serverAddress, serverPort))
    using (SslStream sslStream = new SslStream(client.GetStream()))
    {
        sslStream.AuthenticateAsClient(serverAddress);

        string httpRequest = "GET / HTTP/1.1\r\nHost: " + serverAddress + "\r\nConnection: close\r\n\r\n";
        byte[] requestBytes = Encoding.ASCII.GetBytes(httpRequest);
        sslStream.Write(requestBytes, 0, requestBytes.Length);

        // Sunucudan gelen veriyi tamponlamak için bir StringWriter oluştur
        StringWriter sw = new StringWriter();
        byte[] buffer = new byte[4096];
        int bytesRead;
        do
        {
            bytesRead = sslStream.Read(buffer, 0, buffer.Length);
            sw.Write(Encoding.ASCII.GetString(buffer, 0, bytesRead));
        } while (bytesRead > 0);

        // Tamponlanmış veriyi al
        string response = sw.ToString();
        Console.WriteLine("Sunucudan gelen cevap:\n" + response);
    }
}
catch (Exception e)
{
    Console.WriteLine("Hata: " + e.Message);
}