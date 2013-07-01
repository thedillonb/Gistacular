using System;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace MonoTouch
{
    public static class Utilities
    {
        /// <summary>
        ///   A shortcut to the main application
        /// </summary>
        public static UIApplication MainApp = UIApplication.SharedApplication;

        public readonly static string BaseDir = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "..");

        //
        // Since we are a multithreaded application and we could have many
        // different outgoing network connections (api.twitter, images,
        // searches) we need a centralized API to keep the network visibility
        // indicator state
        //
        static readonly object NetworkLock = new object ();
        static int _active;

        public static void PushNetworkActive ()
        {
            lock (NetworkLock){
                _active++;
                MainApp.NetworkActivityIndicatorVisible = true;
            }
        }

        public static void PopNetworkActive ()
        {
            lock (NetworkLock){
                if (_active == 0)
                    return;

                _active--;
                if (_active == 0)
                    MainApp.NetworkActivityIndicatorVisible = false;
            }
        }

        public static NSUserDefaults Defaults = NSUserDefaults.StandardUserDefaults;

        static long _lastTime;
        [Conditional ("TRACE")]
        public static void ReportTime (string s)
        {
            long now = DateTime.UtcNow.Ticks;

            Debug.WriteLine (string.Format ("[{0}] ticks since last invoke: {1}", s, now-_lastTime));
            _lastTime = now;
        }
        
        [Conditional ("TRACE")]
        public static void Log (string format, params object [] args)
        {
            Debug.WriteLine (String.Format (format, args));
        }

        public static void LogException (string text, Exception e)
        {
            using (var s = File.AppendText (BaseDir + "/Documents/crash.log")){
                var msg = String.Format ("On {0}, message: {1}\nException:\n{2}", DateTime.Now, text, e);
                s.WriteLine (msg);
                Console.WriteLine (msg);
            }
        }

        static UIActionSheet _sheet;
        public static UIActionSheet GetSheet (string title)
        {
            _sheet = new UIActionSheet (title);
            return _sheet;
        }

        static CultureInfo _americanCulture;
        public static CultureInfo AmericanCulture {
            get { return _americanCulture ?? (_americanCulture = new CultureInfo("en-US")); }
        }


        public static void ShowAlert(string title, string message)
        {
            var alert = new UIAlertView {Title = title, Message = message};
            alert.DismissWithClickedButtonIndex(alert.AddButton("Ok"), true);
            alert.Show();
        }

        public static bool IsTall
        {
            get 
            { 
                return UIDevice.CurrentDevice.UserInterfaceIdiom 
                    == UIUserInterfaceIdiom.Phone 
                        && UIScreen.MainScreen.Bounds.Height 
                        * UIScreen.MainScreen.Scale >= 1136;
            }     
        }

        public static Tuple<int, int> iOSVersion
        {
            get
            {
                try
                {
                    var version = UIDevice.CurrentDevice.SystemVersion.Split('.');
                    var major = Int32.Parse(version[0]);
                    var minor = Int32.Parse(version[1]);
                    return new Tuple<int, int>(major, minor);
                }
                catch (Exception e)
                {
                    Console.WriteLine("When attempting to get version: " + e.Message);
                    return new Tuple<int, int>(5, 0);
                }
            }
        }
    }
}

