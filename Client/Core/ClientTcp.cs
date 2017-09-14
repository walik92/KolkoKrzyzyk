using System.IO;
using System.Net.Sockets;
using Common.Interfaces;

namespace Client.Core
{
    public class ClientTcp : ITcpConnectable
    {
        private TcpClient _client;
        private BinaryReader _reader;
        private BinaryWriter _writer;

        public void Connect(string addressIp, int port)
        {
            _client = new TcpClient();
            _client.Connect(addressIp, port);
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