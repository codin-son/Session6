using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Session6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);

            
        }

        private void tabControl1_DrawItem(Object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            // code untuk kasi title tab tabpanel horizontal
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabControl1.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Red);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Arial", 10.0f, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ArrayList areaList = new ArrayList();
            areaList = DBCon.GetArea();
            cbArea.DataSource = areaList;

            ArrayList hostList = new ArrayList();
            hostList = DBCon.getHost();
            cbHost.DataSource = hostList;

            ArrayList guestList = new ArrayList();
            guestList = DBCon.getGuest();
            cbGuest.DataSource = guestList;

            secureVal.Text = DBCon.getTotalSecuredBook();
            upcombookVal.Text = DBCon.getUpcomingBook();
            mostBookVal.Text = DBCon.getMostDayBook();
            inactiveListVal.Text = DBCon.getTotalNotActiveListing();
            cancelReservVal.Text = DBCon.getTotalCancelBook();
            mostUsedCouponVal.Text = DBCon.getMostUsedCoupon();
            vacanRatVal.Text = DBCon.getVacanRatio();
            avgScoreVal.Text = DBCon.getAvgScoreItem();
            listHightScoreVal.Text = DBCon.getMostItemScore();
            topowneravgval.Text = DBCon.getTopOwnerScore();
            leascleanownerval.Text = DBCon.getLeastCleanOwner();

            DataTable monthVacanRatioTable = DBCon.GetMonthVacanRatio();

            chart1.Series.Clear();

            //tambah bar chart
            chart1.Series.Add("Vacant");
            chart1.Series.Add("Reserved");
            
            //set char style ke stacked column
            chart1.Series["Reserved"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            chart1.Series["Vacant"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;

            //set data source
            chart1.Series["Vacant"].XValueMember = "Month";
            chart1.Series["Vacant"].YValueMembers = "Vacant";
            chart1.Series["Vacant"].Points.DataBind(monthVacanRatioTable.AsEnumerable(), "Month", "VacantProperties", "");

            chart1.Series["Reserved"].XValueMember = "Month";
            chart1.Series["Reserved"].YValueMembers = "Reserved";
            chart1.Series["Reserved"].Points.DataBind(monthVacanRatioTable.AsEnumerable(), "Month", "ReservedProperties", "");

            // Set the chart title and axis labels as needed
            chart1.Titles.Add("Vacancy Ratio for the Last 3 Months");
            chart1.ChartAreas[0].AxisX.Title = "Month";
            chart1.ChartAreas[0].AxisY.Title = "Number of Properties";

            avgnetownerval.Text = DBCon.getTotalNetRev();
            highnetrevanownerval.Text = DBCon.getHighestnetOwner();
            totalrevcancelval.Text = DBCon.getTotalNetCancel();
            totalDiscVal.Text = DBCon.totalDisc();

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
