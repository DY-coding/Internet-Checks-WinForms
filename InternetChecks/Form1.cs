    using System;
    using System.Net.Http;
    using System.Threading.Tasks;


    namespace InternetChecks
    {

    public partial class Form1 : Form
    {
        public void ExternStopTimer3()
        {
            this.timer3_Fading.Stop();
        }

        private const int RESTORE_INTERVAL_IN_SECS = 1;
        private const int PING_INTERVAL_IN_SECS = 15;
        private int CheckInterval = PING_INTERVAL_IN_SECS;
        //private const int CHECK_TIMEOUT_IN_SECS = 3;
        private DateTime lastTick;
        private bool? alive;

        private AlertForm? _alertFormInstance = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void testToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            Color moneyGreen = Color.FromArgb(192, 220, 192);
            if (_alertFormInstance == null || _alertFormInstance.IsDisposed)
            {
                _alertFormInstance = new AlertForm();
                _alertFormInstance.Owner = this;
                _alertFormInstance.Show();
            }
            else
            {
                _alertFormInstance.BringToFront();
            }
            _alertFormInstance.SetCase("TEST", moneyGreen, Color.ForestGreen, moneyGreen, Color.ForestGreen, moneyGreen);
            this.timer4_DelayFading.Start();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;
            notifyIcon1.Icon = InternetChecks.Resource1.grayNet;
            notifyIcon1.Visible = true;
            this.infoToolStripMenuItem.Text = "Период обновления " + PING_INTERVAL_IN_SECS + " секунд";

            lastTick = DateTime.Now;
            await UpdateNetStatus();

            //this.timer1.Interval = 1000 * this.CheckInterval;
            this.timer1_CheckNetStatus.Start();



        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            lastTick = DateTime.Now;
            this.timer1_CheckNetStatus.Stop();
            await UpdateNetStatus();
            this.timer1_CheckNetStatus.Start();
        }

        private async Task UpdateNetStatus()
        {
            Color moneyGreen = Color.FromArgb(192, 220, 192);
            notifyIcon1.Icon = InternetChecks.Resource1.grayNet;
            bool status = await NetCheck.IsNetAlive();

            if (status)
            {
                notifyIcon1.Icon = InternetChecks.Resource1.greenNet;
                if (alive == null || alive == false)
                {
                    //if(alive != null) MessageBox.Show("Сеть есть");

                    if (alive != null)
                    {
                        if (_alertFormInstance == null || _alertFormInstance.IsDisposed)
                        {
                            _alertFormInstance = new AlertForm();
                            _alertFormInstance.Owner = this;
                            _alertFormInstance.Show();
                        }
                        else
                        {
                            _alertFormInstance.BringToFront();
                        }
                        _alertFormInstance.SetCase("CЕТЬ ВОССТАНОВЛЕНА", moneyGreen, Color.ForestGreen, moneyGreen, Color.ForestGreen, moneyGreen);
                        this.timer4_DelayFading.Start();
                    }


                    CheckInterval = PING_INTERVAL_IN_SECS;
                    this.timer1_CheckNetStatus.Interval = CheckInterval * 1000;
                }
                alive = true;
            }
            else
            {
                notifyIcon1.Icon = InternetChecks.Resource1.redNet;
                if (alive == null || alive == true)
                {

                    // if(alive!= null) MessageBox.Show("Сеть пропала!");
                    if (alive != null)
                    {
                        if (_alertFormInstance == null || _alertFormInstance.IsDisposed)
                        {
                            _alertFormInstance = new AlertForm();
                            _alertFormInstance.Owner = this;
                            _alertFormInstance.Show();
                        }
                        else
                        {
                            _alertFormInstance.BringToFront();
                        }
                        _alertFormInstance.SetCase("ПОТЕРЯ СЕТИ", Color.Pink, Color.DarkRed, Color.Pink, Color.DarkRed, Color.LightPink);
                        this.timer4_DelayFading.Start();
                    }

                    CheckInterval = RESTORE_INTERVAL_IN_SECS;
                    this.timer1_CheckNetStatus.Interval = CheckInterval * 1000;
                }
                alive = false;
            }

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int seconds = CheckInterval - (DateTime.Now - lastTick).Seconds;
            this.infoToolStripMenuItem.Text = (alive == true ? "✔" : "❌") + "Проверка сети через " + (seconds > 0 ? seconds.ToString() : "0") + " сек.";
            this.timer2_ContextMenuUpdate.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int seconds = CheckInterval - (DateTime.Now - lastTick).Seconds;
            this.infoToolStripMenuItem.Text = (alive == true ? "✔" : "❌") + "Проверка сети через " + (seconds > 0 ? seconds.ToString() : "0") + " сек.";
        }


        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("control.exe", "ncpa.cpl");
        }

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.timer2_ContextMenuUpdate.Stop();

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (_alertFormInstance != null)
            {

                if (_alertFormInstance.Opacity < 0.25)
                {
                    _alertFormInstance.Close();
                    timer3_Fading.Stop();
                }
                _alertFormInstance.Opacity -= 0.002;
            }
        }

        private void timer4_DelayFading_Tick(object sender, EventArgs e)
        {
            timer4_DelayFading.Stop();
            timer3_Fading.Start();
        }
    }


    public class NetCheck
        {
            private const string Url = "https://www.google.com/generate_204";
            private static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
            public static async Task<bool> IsNetAlive()
            {
                try
                {
                
                    var response = await client.GetAsync(Url, HttpCompletionOption.ResponseHeadersRead);
                    return response.IsSuccessStatusCode;
                }
                catch (HttpRequestException)
                {
                    // ошибка сети
                    return false;
                }
                catch (TaskCanceledException)
                {
                    // сработал таймаут
                    return false;
                }
                catch (Exception)
                {
                    // всё остальное
                    return false;
                }
            }
        }
    }
