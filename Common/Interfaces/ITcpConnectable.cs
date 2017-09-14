namespace Common.Interfaces
{
    public interface ITcpConnectable
    {
        void Send(int numerPola);
        int Receive();
        void Connect(string adressIp, int port);
    }
}