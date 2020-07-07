using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

namespace nbxcOCM
{
    public partial class PoolEntry : UserControl
    {
        private string address, api, miningAddress, lowPort, midPort, highPort;
        private int ping = -1, hashrate = -1;
        private double fee = -1, payout = -1;
        public bool autoUpdate = false;
        private long pingStart;

        public PoolEntry(string pAddress, string pApi, string pMiningAddress, string plowPort, string pmidPort, string phighPort)
        {
            InitializeComponent();
            address = pAddress;
            api = pApi;
            miningAddress = pMiningAddress;
            lowPort = plowPort;
            midPort = pmidPort;
            highPort = phighPort;
        }

        private void PoolEntry_Load(object sender, EventArgs e)
        {

            addressLbl.Text = address;
            this.addressLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.addressLbl_LinkClicked);
            updateStatsTimer.Start();
        }


        private void addressLbl_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            this.addressLbl.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start(this.address);
        }

        public async void UpdateStats()
        {
            WebClient webCl = new WebClient();
            webCl.DownloadStringCompleted += new DownloadStringCompletedEventHandler(setStats);
            if (api.Contains("https"))
            { 
                webCl.DownloadStringAsync(new Uri(api + "stats"));
            } else
            {
                payoutLbl.Text = "?";
                hashLbl.Text = "?";
                feeLbl.Text = "?";
            }

            pingLbl.Text = "pinging...";
            pingStart = DateTime.Now.Ticks;
            try
            {
                PingReply res = await new Ping().SendPingAsync(miningAddress, 10000);
                if (res.Status == IPStatus.Success)
                {
                    ping = (int)((DateTime.Now.Ticks - pingStart) / 10000);
                    pingLbl.Text = ping.ToString() + " ms";
                }
                else if (res.Status == IPStatus.TimedOut)
                {
                    ping = 999;
                    pingLbl.Text = ping.ToString() + " POOL NOT IN USE";

                }
                else
                {
                    ping = -1;
                    pingLbl.Text = "?";

                    try
                    {
                        this.Parent.Controls.Remove(this);
                    }
                    catch { }
                }
            }
            catch (Exception e)
            {
                ping = -1;
                pingLbl.Text = "?";

                try
                {
                    this.Parent.Controls.Remove(this);
                }
                catch { }
            }
        }
        
        private void setStats(object sender, DownloadStringCompletedEventArgs e)
        {
             try
                {
                    payout = double.Parse(getFromJSON(@"""minPaymentThreshold""", e.Result)) / 100.0;
                    hashrate = int.Parse(getFromJSON(@"""hashrate""", e.Result));
                    fee = double.Parse(getFromJSON(@"""fee""", e.Result).Replace('.', ','));

                    payoutLbl.Text = payout.ToString() + " NBXC";
                    float f_hashrate = hashrate;
                    f_hashrate = f_hashrate / 1000;
                    hashLbl.Text = f_hashrate.ToString("0.00")  + " KH/s";
                    feeLbl.Text = fee.ToString() + "%";
                }
                catch
                {
                    payout = -1;
                hashrate = -1;
                fee = -1;

                    payoutLbl.Text = "?";
                    hashLbl.Text = "?";
                    feeLbl.Text = "?";

                try
                {
                //    this.Parent.Controls.Remove(this);
                }
                catch { }
            }
        }

        private string getFromJSON(string lookup, string json)
        {
            try
            {
                string result = (json.Substring(json.IndexOf(lookup)).Split(':')[1]).Split(',')[0];
                return result;
            } catch
            {
                return null;
            }
        }

        private void updateStatsTimer_Tick(object sender, EventArgs e)
        {
            UpdateStats();

            if (!autoUpdate) updateStatsTimer.Stop();
        }

        
        public void setAutoUpdate(bool how)
        {
            if (how)
            {
                updateStatsTimer.Enabled = true;
                autoUpdate = true;
            }
            else
            {
                updateStatsTimer.Enabled = false;
                autoUpdate = false;
            }
        }

        public void ToggleAutoSelect()
        {
            if (selectedCb.Enabled)
            {
                selectedCb.Enabled = false;
            }
            else
            {
                selectedCb.Enabled = true;
            }
        }
        
        public bool getSelected()
        {
            return selectedCb.Checked;
        }

        public void setSelected(bool how)
        {
            selectedCb.Checked = how;
        }

        public string getAddress()
        {
            return address;
        }

        public string getLowMiningPort()
        {
            return lowPort;
        }

        public string getMidMiningPort()
        {
            return midPort;
        }

        public string getHighMiningPort()
        {
            return highPort;
        }

        public string getMiningAddress()
        {
            return miningAddress;
        }

        public int getPing()
        {
            return ping;
        }

        public double getMinPayout()
        {
            return payout;
        }

        public int getHashrate()
        {
            return hashrate;
        }

        public double getFee()
        {
            return fee;
        }

        private void label13_Click(object sender, EventArgs e)
        {
        //    this.Parent.Controls.Remove(this);
        }

        private void selectedCb_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedCb.Checked && !nbxcOCM.Properties.Settings.Default.savedPools.Contains(miningAddress))
                nbxcOCM.Properties.Settings.Default.savedPools += miningAddress;
            else if (!selectedCb.Checked)
                nbxcOCM.Properties.Settings.Default.savedPools = nbxcOCM.Properties.Settings.Default.savedPools.Replace(miningAddress, "");
        }
    }
}

