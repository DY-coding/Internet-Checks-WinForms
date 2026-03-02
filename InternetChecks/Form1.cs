using System.Numerics;
using System.Reflection;

namespace InternetChecks
{
    public enum NetMode
    {
        Google = 0,
        CloudFlare = 1,
        MS = 2
    }

    public partial class Form1 : Form
    {
        

        public void ExternStopTimer3()
        {
            this.timer3_Fading.Stop();
        }

        // периодичность пинга при отсутствии сети
        private const int RESTORE_INTERVAL = 1;
        // периодичносить пинга при наличии сети
        private const int PING_INTERVAL = 15;
        // время восстановления, меньше которого показ только фейдинга окна оповещения
        private int FALSE_ALARM_INTERVAL = 10;
        // время показа оповещения, не включая фейдинг
        private int ALARM_SHOW_INTERVAL = 3; 

        private DateTime lastTick;
        private bool? alive;
        private int CheckInterval = PING_INTERVAL;

        private AlertForm? _alertFormInstance = null;

        private int disconnectCount = 0;

        public Form1()
        {
            InitializeComponent();

            string longestText = "✔ Проверка сети через 00 сек.";
            using (Graphics g = this.CreateGraphics())
            {
                SizeF sizr = g.MeasureString(longestText, infoToolStripMenuItem.Font);

                contextMenuStrip1.AutoSize = false;
                contextMenuStrip1.Width = 330;
            }


        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;
            notifyIcon1.Icon = InternetChecks.Resource1.grayNet;
            notifyIcon1.Visible = true;
            this.infoToolStripMenuItem.Text = "Период обновления " + PING_INTERVAL + " секунд";

            timer4_DelayFading.Interval = ALARM_SHOW_INTERVAL * 1000;

            lastTick = DateTime.Now;
            await UpdateNetStatus();

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


            bool status = false;
            // учет ложноположительного отказа
            int retries = 3;
            for(int i=0; i< retries; i++)
            {
                status = await NetCheck.IsNetAlive();
                if (status) break;

                await Task.Delay(1000);
            }

            

            if (status)
            {
                notifyIcon1.Icon = InternetChecks.Resource1.greenNet;
                if (alive == null || alive == false)
                {
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
                        _alertFormInstance.Opacity = 1.0;
                        _alertFormInstance.SetCase("CЕТЬ ВОССТАНОВЛЕНА", moneyGreen, Color.ForestGreen, moneyGreen, Color.ForestGreen, moneyGreen);

                         this.timer4_DelayFading.Start();

                    }


                    CheckInterval = PING_INTERVAL;
                    this.timer1_CheckNetStatus.Interval = CheckInterval * 1000;
                }
                alive = true;
            }
            else
            {
                notifyIcon1.Icon = InternetChecks.Resource1.redNet;
                if (alive == null || alive == true)
                {
                    if (alive != null)
                    {
                        disconnectCount++;
                        notifyIcon1.Text = "Net Check Alive [Disconnected " + disconnectCount + " times]";

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
                        _alertFormInstance.Opacity = 1.0;
                        _alertFormInstance.SetCase("ПОТЕРЯ СЕТИ", Color.Pink, Color.DarkRed, Color.Pink, Color.DarkRed, Color.LightPink);
                        this.timer4_DelayFading.Start();

                    }

                    CheckInterval = RESTORE_INTERVAL;
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

        private void TrayIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi?.Invoke(notifyIcon1, null);
            }
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
        
        
        private static readonly string[] Urls = new[]
        {
            "https://www.google.com/generate_204",
            "http://cp.cloudflare.com/",
            "http://www.msftconnecttest.com/connecttest.txt"
        };

        private static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
        public static async Task<bool> IsNetAlive(NetMode mode = NetMode.Google)
        {
            if (!Enum.IsDefined(typeof(NetMode), mode)){
                return false;
            }

            try
            {
                var response = await client.GetAsync(Urls[(int)mode], HttpCompletionOption.ResponseHeadersRead);
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
