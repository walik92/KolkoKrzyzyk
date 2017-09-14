using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Common.Core;
using Common.Interfaces;

namespace Common.ViewModels
{
    public abstract class MainViewModel : BaseViewModel
    {
        private readonly List<char> _chars = new List<char> {'X', 'O'};
        private readonly KolkoKrzyzykCore _kolkoKrzyzykCore;
        private readonly int _port = 8000;
        private readonly ITcpConnectable _tcpCaller;
        private readonly BackgroundWorker _workerConnect;
        private readonly BackgroundWorker _workerReceiveData;
        private readonly BackgroundWorker _workerSendData;
        private readonly char _znakPrzeciwnika;
        private bool _isConnected;
        private bool _mode;
        private bool _remis;
        private bool? _result;
        private bool _showProgress;
        private bool _showWaitingForPlayer;

        protected MainViewModel(ITcpConnectable tcpCaller, char znak)
        {
            _tcpCaller = tcpCaller;
            Znak = znak;
            _kolkoKrzyzykCore = new KolkoKrzyzykCore();

            if (!_chars.Contains(znak))
            {
                throw new Exception("Nieprawidłowy znak");
            }
            _chars.Remove(znak);
            _znakPrzeciwnika = _chars[0];

            Plansza = new char[9];

            _workerConnect = new BackgroundWorker();
            _workerConnect.DoWork += WorkerConnectDoWork;
            _workerConnect.RunWorkerCompleted += WorkerConnectRunWorkerCompleted;

            _workerSendData = new BackgroundWorker();
            _workerSendData.DoWork += WorkerSendDataDoWork;
            _workerSendData.RunWorkerCompleted += WorkerSendDataRunWorkerCompleted;

            _workerReceiveData = new BackgroundWorker();
            _workerReceiveData.DoWork += WorkerReceiveDataDoWork;
            _workerReceiveData.RunWorkerCompleted += WorkerReceiveDataRunWorkerCompleted;
        }

        public char Znak { get; set; }

        public bool Remis
        {
            get { return _remis; }
            set
            {
                _remis = value;
                OnPropertyChanged();
            }
        }

        public string AddressIp { get; set; } = "127.0.0.1";

        public bool? Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public bool ShowProgress
        {
            get { return _showProgress; }
            set
            {
                _showProgress = value;
                OnPropertyChanged();
            }
        }

        public bool Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                OnPropertyChanged();
            }
        }

        public char[] Plansza { get; set; }

        public bool ShowWaitingForPlayer
        {
            get { return _showWaitingForPlayer; }
            set
            {
                _showWaitingForPlayer = value;
                OnPropertyChanged();
            }
        }

        public void Connect()
        {
            if (!ValidateIp(AddressIp))
            {
                MessageBox.Show("Nieprawidłowy adres IP");
                return;
            }

            ShowWaitingForPlayer = true;
            if (!_workerConnect.IsBusy)
            {
                _workerConnect.RunWorkerAsync();
                ShowProgress = true;
            }
        }

        private void WorkerConnectDoWork(object sender, DoWorkEventArgs e)
        {
            _tcpCaller.Connect(AddressIp, _port);
        }

        private void ReceiveData()
        {
            if (!_workerReceiveData.IsBusy)
            {
                ShowProgress = true;
                _workerReceiveData.RunWorkerAsync();
            }
        }

        private void WorkerReceiveDataDoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = _tcpCaller.Receive();
        }

        private void SendData(int numerPola)
        {
            if (!_workerSendData.IsBusy)
            {
                ShowProgress = true;
                _workerSendData.RunWorkerAsync(numerPola);
            }
        }

        private void WorkerSendDataDoWork(object sender, DoWorkEventArgs e)
        {
            var pole = int.Parse(e.Argument.ToString());
            _tcpCaller.Send(pole);
        }


        /// <summary>
        ///     Metoda uruchamiana po probie nawiazania polaczenia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerConnectRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                IsConnected = true;
                RunAfterConnected();
                ReceiveData();
            }
            ShowProgress = false;
        }

        public abstract void RunAfterConnected();

        /// <summary>
        ///     Metoda uruchamiana po odebraniu danych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerReceiveDataRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                var pole = int.Parse(e.Result.ToString());

                Plansza[pole] = _znakPrzeciwnika;
                OnPropertyChanged(nameof(Plansza));

                Mode = true;
                ReceiveData();
                if (_kolkoKrzyzykCore.SpradzCzyWygral(_znakPrzeciwnika, Plansza))
                {
                    Result = false;
                    Mode = false;
                }
                else if (_kolkoKrzyzykCore.SprawdzCzyRemis(Plansza))
                {
                    Remis = true;
                    Mode = false;
                }
            }

            ShowProgress = false;
        }

        /// <summary>
        ///     Metoda uruchamiana po wysłaniu danych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkerSendDataRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                Mode = false;
            }
            ShowProgress = false;
        }

        public void Select(int pole)
        {
            if (pole < 0 && pole > 8)
                throw new Exception("Nieprawidłowy numer pola");

            if (Plansza[pole] == '\0')
            {
                Plansza[pole] = Znak;
                OnPropertyChanged(nameof(Plansza));
                SendData(pole);
                if (_kolkoKrzyzykCore.SpradzCzyWygral(Znak, Plansza))
                    Result = true;
                else if (_kolkoKrzyzykCore.SprawdzCzyRemis(Plansza))
                    Remis = true;
            }
        }

        public bool ValidateIp(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            var splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
    }
}