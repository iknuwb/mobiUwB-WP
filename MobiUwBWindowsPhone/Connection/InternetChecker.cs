#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Xna.Framework.Audio;
using MobiUwB.Resources;
using MobiUwB.Utilities;

#endregion

namespace MobiUwB.Connection
{
    /// <summary>
    /// Reprezentuje akcję uzyskania połaczenia z internetem.
    /// </summary>
    /// <param name="internetConnectionType">Typ uzyskanego połączenia</param>
    public delegate void ConnectionReceived(InternetConnectionType internetConnectionType);

    /// <summary>
    /// Reprezentuje akcję utraty połączenia z internetem.
    /// </summary>
    public delegate void ConnectionLost();

    /// <summary>
    /// Sprawdza stan połączenia z interetem.
    /// </summary>
    public class InternetChecker : BackgroundWorker
    {
        /// <summary>
        /// Występuje gdy nawiązano połączenie z internetem lub zmienił się typ połączenia.
        /// </summary>
        public event ConnectionReceived ConnectionReceived;

        /// <summary>
        /// Występuje gdy utracono połączenie z internetem.
        /// </summary>
        public event ConnectionLost ConnectionLost;

        /// <summary>
        /// Instancja tej klasy.
        /// </summary>
        private static InternetChecker _instance;

        /// <summary>
        /// Obrazek połączenia 3G.
        /// </summary>
        private readonly BitmapImage _3GBitmapImage;

        /// <summary>
        /// Obrazek połączenia 4G.
        /// </summary>
        private readonly BitmapImage _4GBitmapImage;

        /// <summary>
        /// Obrazek połączenia WiFi.
        /// </summary>
        private readonly BitmapImage _wiFiBitmapImage;

        /// <summary>
        /// Obrazek braku połączenia.
        /// </summary>
        private readonly BitmapImage _noConnectionBitmapImage;

        /// <summary>
        /// Obrazek połączenia przez kabel.
        /// </summary>
        private readonly BitmapImage _pcBitmapImage;

        /// <summary>
        /// Obrazek nieznanego połączenia.
        /// </summary>
        private readonly BitmapImage _unknownBitmapImage;

        /// <summary>
        /// Ścieżka do obrazka 3G.
        /// </summary>
        private readonly Uri _3GImageUri;

        /// <summary>
        /// Ścieżka do obrazka 4G.
        /// </summary>
        private readonly Uri _4GImageUri;

        /// <summary>
        /// Ścieżka do obrazka WiFi.
        /// </summary>
        private readonly Uri _wiFiImageUri;

        /// <summary>
        /// Ścieżka do obrazka brak połączenia.
        /// </summary>
        private readonly Uri _noConnectionImageUri;

        /// <summary>
        /// Ścieżka do obrazka połączenia przez kabel.
        /// </summary>
        private readonly Uri _pcImageUri;

        /// <summary>
        /// Ścieżka do obrazka nieznanego połączenia.
        /// </summary>
        private readonly Uri _unknownImageUri;

        /// <summary>
        /// Czy InternetChecker powinien skończyć działanie.
        /// </summary>
        private Boolean _shouldStop;

        /// <summary>
        /// Typ obecnego połączenia z internetem.
        /// </summary>
        private InternetConnectionType? _internetConnectionType;

        /// <summary>
        /// Wypełnia zmienne.
        /// </summary>
        public InternetChecker()
        {
            _3GImageUri = 
                new Uri(
                    "Assets\\ToastIcons\\3g.png", 
                    UriKind.Relative);

            _4GImageUri =
                new Uri(
                    "Assets\\ToastIcons\\4g.png",
                    UriKind.Relative);

            _wiFiImageUri =
                new Uri(
                    "Assets\\ToastIcons\\wifi.png",
                    UriKind.Relative);

            _noConnectionImageUri =
                new Uri(
                    "Assets\\ToastIcons\\no_connection.png",
                    UriKind.Relative);

            _pcImageUri =
                new Uri(
                    "Assets\\ToastIcons\\pc.png",
                    UriKind.Relative);

            _unknownImageUri =
                new Uri(
                    "Assets\\ToastIcons\\unknown.png",
                    UriKind.Relative);

            _3GBitmapImage = new BitmapImage(_3GImageUri);
            _4GBitmapImage = new BitmapImage(_4GImageUri);
            _wiFiBitmapImage = new BitmapImage(_wiFiImageUri);
            _noConnectionBitmapImage = new BitmapImage(_noConnectionImageUri);
            _pcBitmapImage = new BitmapImage(_pcImageUri);
            _unknownBitmapImage = new BitmapImage(_unknownImageUri);

            WorkerReportsProgress = true;
            DoWork += InternetChecker_DoWork;
            ProgressChanged += InternetChecker_ProgressChanged;
        }

        /// <summary>
        /// Zwraca instancję tej klasy.
        /// </summary>
        /// <returns>Instancja tej klasy</returns>
        public static InternetChecker GetInstance()
        {
            if (_instance == null)
            {
                _instance = new InternetChecker();
            }
            return _instance;
        }

        /// <summary>
        /// Rozpoczyna działanie sprawdzania połączenia z internetem.
        /// </summary>
        public void StartChecker()
        {
            RunWorkerAsync();
            _shouldStop = false;
        }

        /// <summary>
        /// Przerywa działanie sprawdzania połączenia z internetem.
        /// </summary>
        public void StopChecker()
        {
            _shouldStop = false;
        }

        /// <summary>
        /// Informuje o zmianie stanu połączenia
        /// </summary>
        /// <param name="sender">Obiekt wywołujący</param>
        /// <param name="e">Zawiera informacje o zmianie połączenia</param>
        void InternetChecker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            InternetConnectionType connectionType = (InternetConnectionType) e.ProgressPercentage;

            UseResponse(connectionType);
            InvokeEvents(connectionType);
        }

        /// <summary>
        /// Wywołuje odpowiedni event.
        /// </summary>
        /// <param name="connectionType">Typ połączenia</param>
        private void InvokeEvents(InternetConnectionType connectionType)
        {
            if (connectionType == InternetConnectionType.NoConnection)
            {
                if (ConnectionLost != null)
                {
                    ConnectionLost();
                }
            }
            else
            {
                if (ConnectionReceived != null)
                {
                    ConnectionReceived(connectionType);
                }
            }
        }

        /// <summary>
        /// Cyklicznie sprawdza stan połączenia
        /// </summary>
        /// <param name="sender">Obiekt wywołujący</param>
        /// <param name="e">Zawiera informacje o parametrach wejściowych</param>
        void InternetChecker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<InternetConnectionType> currentConnectionTypes = new List<InternetConnectionType>();
            while (!_shouldStop)
            {
                var currentList = new NetworkInterfaceList().
                    Where(i => i.InterfaceState == ConnectState.Connected).
                    Select(i => i.InterfaceSubtype);

                foreach (NetworkInterfaceSubType interfaceSubType in currentList)
                {
                    switch (interfaceSubType)
                    {
                        case NetworkInterfaceSubType.Cellular_EVDO:
                        case NetworkInterfaceSubType.Cellular_3G:
                        case NetworkInterfaceSubType.Cellular_HSPA:
                        case NetworkInterfaceSubType.Cellular_EVDV:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.Unknown);
                            break;
                        }
                        case NetworkInterfaceSubType.Cellular_GPRS:
                        case NetworkInterfaceSubType.Cellular_1XRTT:
                        case NetworkInterfaceSubType.Cellular_EDGE:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.G3);
                            break;
                        }
                        case NetworkInterfaceSubType.WiFi:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.WiFi);
                            break;
                        }
                        case NetworkInterfaceSubType.Desktop_PassThru:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.Cable);
                            break;
                        }
                        case NetworkInterfaceSubType.Cellular_EHRPD:
                        case NetworkInterfaceSubType.Cellular_LTE:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.G4);
                            break;
                        }
                        case NetworkInterfaceSubType.Unknown:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.Unknown);
                            break;
                        }
                        default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                InternetConnectionType connectionType = 
                    AnalizeConnectionTypes(
                        currentConnectionTypes);

                DetermineReportProgress(connectionType);

                currentConnectionTypes.Clear();
            }
        }

        /// <summary>
        /// Określa czy zmienił się stan połączenia.
        /// </summary>
        /// <param name="currConnectionType"></param>
        private void DetermineReportProgress(InternetConnectionType currConnectionType)
        {
            if (_internetConnectionType != null && _internetConnectionType != currConnectionType)
            {
                ReportProgress((Int32)currConnectionType);
            }
            _internetConnectionType = currConnectionType;
        }

        /// <summary>
        /// Analizuje listę połączeń wybierając właściwe
        /// </summary>
        /// <param name="connectionTypes">Lista wszystkich połączeń</param>
        /// <returns>Właściwy typ połączenia</returns>
        private InternetConnectionType AnalizeConnectionTypes(List<InternetConnectionType> connectionTypes)
        {
            if (connectionTypes.Contains(InternetConnectionType.WiFi))
            {
                return InternetConnectionType.WiFi;
            }
            if (connectionTypes.Contains(InternetConnectionType.Cable))
            {
                return InternetConnectionType.Cable;
            }
            if (connectionTypes.Contains(InternetConnectionType.G4))
            {
                return InternetConnectionType.G4;
            }
            if (connectionTypes.Contains(InternetConnectionType.G3))
            {
                return InternetConnectionType.G3;
            }
            if (connectionTypes.Contains(InternetConnectionType.G2))
            {
                return InternetConnectionType.G2;
            }
            return InternetConnectionType.NoConnection;
        }

        /// <summary>
        /// Tworzy i pokazuje komunikat o zmianie połączenia.
        /// </summary>
        /// <param name="connectionType">Typ połączenia</param>
        public void UseResponse(InternetConnectionType connectionType)
        {
            String title = AppResources.InternetCheckerToastTitle;
            String message;
            BitmapImage bitmapImage;

            switch (connectionType)
            {
                case InternetConnectionType.Cable:
                {
                    message = AppResources.InternetCheckerToastPc;
                    bitmapImage = _pcBitmapImage;
                    break;
                }
                case InternetConnectionType.WiFi:
                {
                    message = AppResources.InternetCheckerToastWiFi;
                    bitmapImage = _wiFiBitmapImage;
                    break;
                }
                case InternetConnectionType.G2:
                {
                    message = AppResources.InternetCheckerToast2g;
                    bitmapImage = _unknownBitmapImage;
                    break;
                }
                case InternetConnectionType.G3:
                {
                    message = AppResources.InternetCheckerToastMobile3G;
                    bitmapImage = _3GBitmapImage;
                    break;
                }
                case InternetConnectionType.G4:
                {
                    message = AppResources.InternetCheckerToastMobile4G;
                    bitmapImage = _4GBitmapImage;
                    break;
                }
                case InternetConnectionType.Unknown:
                {
                    message = AppResources.InternetCheckerToastUnknown;
                    bitmapImage = _unknownBitmapImage;
                    break;
                }
                case InternetConnectionType.NoConnection:
                {
                    message = AppResources.InternetCheckerToastNoConnection;
                    bitmapImage = _noConnectionBitmapImage;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException("connectionType", connectionType, null);
                }
            }

            Toaster.Make(title, message, bitmapImage);
        }
    }
}
