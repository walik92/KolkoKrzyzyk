using Common.Interfaces;
using Common.ViewModels;

namespace Server.ViewModels
{
    public class ServerViewModel : MainViewModel
    {
        public ServerViewModel(ITcpConnectable tcpCaller, char znak) : base(tcpCaller, znak)
        {
        }


        public override void RunAfterConnected()
        {
            Mode = true;
            ShowWaitingForPlayer = false;
        }
    }
}