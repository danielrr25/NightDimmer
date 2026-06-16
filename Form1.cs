using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NightDimmer
{
    // The dark, click-through overlay that covers all screens.
    public class Form1 : Form
    {
        private int opacity = 120; // 0 (clear) -> 255 (black)

        private NotifyIcon tray;
        private SliderForm slider;

        private const int GWL_EXSTYLE       = -20;
        private const int WS_EX_LAYERED     = 0x00080000;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int WS_EX_TOOLWINDOW  = 0x00000080;

        public Form1()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition   = FormStartPosition.Manual;
            Bounds          = SystemInformation.VirtualScreen;
            TopMost         = true;
            BackColor       = Color.Black;
            ShowInTaskbar   = false;
            Opacity         = opacity / 255.0;

            // Slider window (hidden until opened from the tray)
            slider = new SliderForm(opacity);
            slider.OpacityChanged += value =>
            {
                opacity = value;
                Opacity = opacity / 255.0;
            };

            // Tray icon
            tray = new NotifyIcon();
            tray.Icon = SystemIcons.Application;
            tray.Text = "Night Dimmer";
            tray.Visible = true;

            var menu = new ContextMenuStrip();
            menu.Items.Add("Adjust brightness", null, (s, e) => ShowSlider());
            menu.Items.Add("Exit", null, (s, e) => Application.Exit());
            tray.ContextMenuStrip = menu;

            tray.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left) ShowSlider();
            };
        }

        private void ShowSlider()
        {
            slider.Show();
            slider.Activate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int exStyle = GetWindowLong(Handle, GWL_EXSTYLE);
            SetWindowLong(Handle, GWL_EXSTYLE,
                exStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW);

            // Show the slider once on startup so it's easy to find
            ShowSlider();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            tray.Visible = false;
            tray.Dispose();
            base.OnFormClosed(e);
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }

    // Small window with a slider to control darkness.
    public class SliderForm : Form
    {
        private TrackBar bar;
        public event Action<int> OpacityChanged;

        public SliderForm(int initial)
        {
            Text = "Night Dimmer";
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(300, 70);
            TopMost = true;
            ShowInTaskbar = false;

            var label = new Label
            {
                Text = "Darkness",
                Location = new Point(12, 10),
                AutoSize = true
            };

            bar = new TrackBar
            {
                Minimum = 0,
                Maximum = 230,   // capped so the screen never goes fully black
                Value = initial,
                TickFrequency = 30,
                Location = new Point(10, 30),
                Width = 280
            };
            bar.Scroll += (s, e) => OpacityChanged?.Invoke(bar.Value);

            Controls.Add(label);
            Controls.Add(bar);
        }

        // Clicking X just hides the slider; the app keeps running
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }
    }
}