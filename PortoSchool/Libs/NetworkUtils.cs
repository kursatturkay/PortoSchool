using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace PortoSchool.Libs
{
    static class NetworkUtils
    {
        private static string LocalIp_;
        public static string LocalIp
        {

            get
            {
                LocalIp_ = LocalIp_?? GetLocalIp();

                return LocalIp_;
            }

            set
            {
                LocalIp_ = value;
            }
        }

        static NetworkUtils()
        {

        }

        private static string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return "127.0.0.1";
            var hostname =
                NetworkInformation.GetHostNames()
                    .SingleOrDefault(
                        hn =>
                            hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                            == icp.NetworkAdapter.NetworkAdapterId);

            // the ip address
            var a= hostname.CanonicalName??"127.0.0.1";
            return a;
        }

        public static string GetDeviceName()
        {

            EasClientDeviceInformation CurrentDeviceInfor = new EasClientDeviceInformation();

            //DeviceID.Text = CurrentDeviceInfor.Id.ToString();
            //OperatingSystem.Text = CurrentDeviceInfor.OperatingSystem;
            //FriendlyName.Text = CurrentDeviceInfor.FriendlyName;
            //SystemManufacturer.Text = CurrentDeviceInfor.SystemManufacturer;
            //SystemProductName.Text = CurrentDeviceInfor.SystemProductName;
            //SystemSku.Text = CurrentDeviceInfor.SystemSku;

            /*
            List<string> IpAddress = new List<string>();
            var Hosts = Windows.Networking.Connectivity.NetworkInformation.GetHostNames().ToList();
            foreach (var Host in Hosts)
            {
                string deviceName = Host.DisplayName;
                listboxNetwork.Items.Add(deviceName);
                //return deviceName;
            }
            */


            return CurrentDeviceInfor.FriendlyName;
        }
    }
}
