using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Demo462
{
    public partial class DataGridViewHeadersScaling : Form
    {
        #region Developer has to calculate DPI valus by P/Invoking windows API
        [DllImport("User32", ExactSpelling = true, EntryPoint = "GetDC", CharSet = CharSet.Auto)]
        private static extern IntPtr GetDC(HandleRef hWnd);

        [DllImport("Gdi32", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        [DllImport("User32", ExactSpelling = true, EntryPoint = "ReleaseDC", CharSet = CharSet.Auto)]
        private static extern int ReleaseDC(HandleRef hWnd, HandleRef hDC);

        private int deviceDpi;
        #endregion

        private void InitializeDpi()
        {
            IntPtr hDC = GetDC(new HandleRef(null, IntPtr.Zero));
            if (hDC != IntPtr.Zero)
            {
                deviceDpi = GetDeviceCaps(new HandleRef(null, hDC), 88);

                ReleaseDC(new HandleRef(null, IntPtr.Zero), new HandleRef(null, hDC));
            }
        }

        public DataGridViewHeadersScaling()
        {
            InitializeComponent();

            // AutoSizeMode for columns is set to ColumnHeader
            dataGridView1.Rows[0].ErrorText = "error text";
            dataGridView1.Rows.Add(new object[] { "aaa", "bbbb" });
            dataGridView1.Rows.Add(new object[] { "baaa", "abbbb" });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.RightToLeft = (dataGridView1.RightToLeft == RightToLeft.Yes ? RightToLeft.No: RightToLeft.Yes);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDpi();
            currentDpiLabel.Text = $"Current scalling = {(int)Math.Round(deviceDpi / 96.0) * 100}%";

            // Resize row header width for DPI.
            dataGridView1.RowHeadersWidth *= (int)Math.Round(deviceDpi / 96.0);
        }

        private void exceptionButton_Click(object sender, EventArgs e)
        {
            (new ThreadExceptionDialog(new Exception("Really long exception description string, because we want to see if it properly wraps around or is truncated."))).Show();
        }
    }
}
