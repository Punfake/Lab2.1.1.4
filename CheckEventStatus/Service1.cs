using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CheckEventStatus
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SessionChangeDescription sschange = new SessionChangeDescription();
            OnSessionChange(sschange);
        }

        protected override void OnStop()
        {
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    popUp("logon");
                    WriteToFile("logon");
                    break;
                case SessionChangeReason.SessionLogoff:
                    popUp("logoff");
                    WriteToFile("logoff");
                    break;
                case SessionChangeReason.SessionLock:
                    popUp("lock");
                    WriteToFile("lock");
                    break;
                case SessionChangeReason.SessionUnlock:
                    {
                        popUp("unlock");
                        WriteToFile("unlock");
                        break;
                    }  
            }

            base.OnSessionChange(changeDescription);
        }

        void WriteToFile(string mess)
        {
            string filepath = "C:/Users/nhaquynh/source/repos/CheckEventStatus/CheckEventStatus/bin/Debug/Log.txt";
            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine(DateTime.Now + " " + mess);
            }
        }

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
                                           IntPtr hServer,
                                           [MarshalAs(UnmanagedType.I4)] int SessionId,
                                           String pTitle,
                                           [MarshalAs(UnmanagedType.U4)] int TitleLength,
                                           String pMessage,
                                           [MarshalAs(UnmanagedType.U4)] int MessageLength,
                                           [MarshalAs(UnmanagedType.U4)] int Style,
                                           [MarshalAs(UnmanagedType.U4)] int Timeout,
                                           [MarshalAs(UnmanagedType.U4)] out int pResponse,
                                           bool bWait);

        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        static public void popUp(string mess)
        {
            bool result = false;
            string title = "Hello";
            int tlen = title.Length;
            string msg = "StudentID: 18520084!";
            int mlen = msg.Length;
            int resp = 0;

            result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, 2, title, tlen, msg, mlen, 0, 0, out resp, true);
        }
    }
}
