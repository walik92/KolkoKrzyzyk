using Common.Interfaces;
using Common.ViewModels;

namespace Client.ViewModels
{
    public class ClientViewModel : MainViewModel
    {
        public ClientViewModel(ITcpConnectable tcpCaller, char znak) : base(tcpCaller, znak)
        {
        }

        public override void RunAfterConnected()
        {
            Mode = false;
        }
    }
}