using System.IO;
using System.Net;
using System.Net.Sockets;
using Common.Interfaces;

namespace Server.Core
{
    public class ServerTcp : ITcpConnectable
    {
        private TcpClient _client;
        private BinaryReader _reader;
        private BinaryWriter _writer;

        public void Connect(string addressIp, int port)
        {
            var listener = new TcpListener(IPAddress.Parse(addressIp), port);

            listener.Start();

            _client = listener.AcceptTcpClient();
            _writer = new BinaryWriter(_client.GetStream());
            _reader = new BinaryReader(_client.GetStream());
        }

        public void Send(int numerPola)
        {
            _writer.Write(numerPola);
        }

        public int Receive()
        {
            return _reader.ReadInt32();
        }
    }
}