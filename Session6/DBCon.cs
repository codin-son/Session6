using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;


namespace Session6
{
    class DBCon
    {
        public static MySqlConnection GetConnection()
        {
            string provider = "server=localhost;user id=root;database=session6";
            MySqlConnection con = new MySqlConnection(provider);

            try
            {
                con.Open();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show("connection failed. error: " + ex);
            }
            return con;
        }

        public static ArrayList GetArea()
        {
            ArrayList areaList = new ArrayList();
            string sql = "SELECT Name FROM areas";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    areaList.Add(reader.GetString("Name"));
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return areaList;

        }

        public static ArrayList getHost()
        {
            ArrayList hostList = new ArrayList();
            string sql = "SELECT ID, FullName " +
                         "FROM users " +
                         "WHERE ID " +
                         "IN ( " +
                         "SELECT DISTINCT UserID " +
                         "FROM items " +
                         "WHERE UserID " +
                         "IS NOT NULL ) " +
                         "ORDER BY FullName ASC";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hostList.Add(reader.GetString("FullName"));
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return hostList;
        }

        public static ArrayList getGuest()
        {

            ArrayList guestList = new ArrayList();
            string sql = "SELECT FullName FROM users WHERE ID IN ( SELECT UserID FROM bookings WHERE UserID IS NOT NULL ) ORDER BY ( SELECT COUNT(*) FROM bookings b WHERE b.UserID = users.ID ) DESC";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    guestList.Add(reader.GetString("FullName"));
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return guestList;
        }

        public static string getTotalSecuredBook()
        {

            string totalSecuredBook = "";
            string sql = "SELECT COUNT(ID) AS TotalIDs FROM bookingdetails WHERE isRefund = 0;";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    totalSecuredBook = reader.GetString("TotalIDs");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return totalSecuredBook;        
        }
        public static string getUpcomingBook()
        {
            string upcomingBook = "";
            string sql = "SELECT COUNT(b.ID) AS TotalBookings FROM bookings b JOIN bookingdetails bd ON b.ID = bd.BookingID WHERE b.BookingDate > CURDATE() AND bd.isRefund = 0; ";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    upcomingBook = reader.GetString("TotalBookings");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return upcomingBook;
        }
        public static string getMostDayBook()
        {
            string mostDayBook = "";
            string sql = "SELECT dd.DayName, COUNT(*) AS TotalBookings FROM bookings b JOIN bookingdetails bd ON b.ID = bd.BookingID JOIN dimdates dd ON DATE(b.BookingDate) = dd.Date WHERE bd.isRefund = 0 AND b.BookingDate < CURDATE() GROUP BY dd.DayName ORDER BY TotalBookings DESC LIMIT 1";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mostDayBook = reader.GetString("DayName");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return mostDayBook;
        }

        public static string getTotalNotActiveListing()
        {

            string totalNotActiveListing = "";
            string sql = "SELECT COUNT(DISTINCT i.ID) AS TotalIDs FROM items i LEFT JOIN itemprices ip ON i.ID = ip.ItemID WHERE ip.ItemID IS NULL; ";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    totalNotActiveListing = reader.GetString("TotalIDs");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return totalNotActiveListing;
        }

        public static string getTotalCancelBook()
        {
            string totalCancelBook = "";
            string sql = "SELECT COUNT(DISTINCT b.ID) AS TotalCancelledBookings FROM bookings b JOIN bookingdetails bd ON b.ID = bd.BookingID WHERE bd.isRefund = 1;";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    totalCancelBook = reader.GetString("TotalCancelledBookings");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return totalCancelBook;

        }

        public static string getMostUsedCoupon()
        {
            string couponName = "";
            string sql = "SELECT c.CouponCode FROM Coupons c JOIN bookings b ON c.ID = b.CouponID JOIN bookingdetails bd ON b.ID = bd.BookingID WHERE bd.isRefund = 0 GROUP BY c.CouponCode ORDER BY COUNT(*) DESC LIMIT 1;";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    couponName = reader.GetString("CouponCode");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return couponName;
        }

        public static string getVacanRatio()
        {
            string vacanRatio = "";
            string sql = "SELECT IFNULL(SUM(available_nights), 0) / IFNULL(SUM(total_nights), 1) AS VacancyRatio FROM ( SELECT ip.ItemID, COUNT(DISTINCT DATE(ip.Date)) AS total_nights, COUNT(DISTINCT CASE WHEN bd.isRefund = 0 THEN DATE(ip.Date) END) AS available_nights FROM ItemPrices ip LEFT JOIN BookingDetails bd ON ip.ID = bd.ItemPriceID GROUP BY ip.ItemID ) AS availability_summary";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vacanRatio = reader.GetString("VacancyRatio");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return vacanRatio;
        }

        public static string getAvgScoreItem()
        {
            string avgScoreItem = "";
            string sql = "SELECT ROUND(AVG(Value), 2) AS AverageScore FROM ItemScores;";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    avgScoreItem = reader.GetString("AverageScore");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return avgScoreItem;
        }

        public static string getMostItemScore()
        {
            string mostItemScore = "";
            string sql = "SELECT i.Title FROM Items i JOIN ItemScores iscore ON i.ID = iscore.ItemID GROUP BY i.ID, i.Title ORDER BY AVG(iscore.Value) DESC LIMIT 1; ";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mostItemScore = reader.GetString("Title");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return mostItemScore;
        }

        public static string getTopOwnerScore()
        {
            string topOwnerScore = "";
            string sql = "SELECT u.FullName FROM Users u JOIN Items i ON u.ID = i.UserID JOIN ItemScores iscore ON i.ID = iscore.ItemID GROUP BY u.ID, u.FullName ORDER BY AVG(iscore.Value) DESC";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    topOwnerScore = reader.GetString("FullName");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return topOwnerScore;
        }
        public static string getLeastCleanOwner()
        {
            string leastCleanOwner = "";
            string sql = "SELECT u.FullName FROM Users u JOIN Items i ON u.ID = i.UserID JOIN ItemScores iscore ON i.ID = iscore.ItemID WHERE iscore.ScoreID = 2 GROUP BY u.ID, u.FullName ORDER BY AVG(iscore.Value) ASC LIMIT 1; ";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    leastCleanOwner = reader.GetString("FullName");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return leastCleanOwner;
        }

        public static DataTable GetMonthVacanRatio()
        {
            DataTable dataTable = new DataTable();
            string sql = "SELECT\r\n    DATE_FORMAT(b.BookingDate, '%m') AS Month,\r\n    COUNT(DISTINCT CASE WHEN bd.isRefund = 0 THEN i.ID END) AS ReservedProperties,\r\n    COUNT(DISTINCT CASE WHEN bd.isRefund = 1 THEN i.ID END) AS VacantProperties\r\nFROM\r\n    Items i\r\nLEFT JOIN BookingDetails bd ON i.ID = bd.ItemPriceID\r\nLEFT JOIN Bookings b ON bd.BookingID = b.ID\r\nWHERE\r\n    b.BookingDate >= DATE_SUB(NOW(), INTERVAL 3 MONTH)\r\n    AND b.BookingDate <= NOW() -- Include only dates within the last 3 months\r\nGROUP BY\r\n    Month;\r\n";

            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                dataAdapter.Fill(dataTable);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return dataTable;
        }

        public static string getTotalNetRev()
        {

            string totalNetRev = "";
            string sql = "SELECT AVG(net_revenue) AS average_net_revenue\r\nFROM (\r\n    SELECT\r\n        SUM(b.AmountPaid - COALESCE(cr.AmountRefunded, 0) - COALESCE(cf.PenaltyAmount, 0)) AS net_revenue\r\n    FROM Bookings b\r\n    INNER JOIN BookingDetails bd ON b.ID = bd.BookingID\r\n    INNER JOIN ItemPrices ip ON bd.ItemPriceID = ip.ID\r\n    LEFT JOIN (\r\n        SELECT\r\n            BookingID,\r\n            SUM(CASE WHEN isRefund = 1 THEN ip.Price ELSE 0 END) AS AmountRefunded\r\n        FROM BookingDetails bd\r\n        INNER JOIN ItemPrices ip ON bd.ItemPriceID = ip.ID\r\n        WHERE RefundDate IS NOT NULL\r\n        GROUP BY BookingID\r\n    ) cr ON b.ID = cr.BookingID\r\n    LEFT JOIN (\r\n        SELECT\r\n            BookingID,\r\n            SUM(cf.PenaltyPercentage * ip.Price / 100) AS PenaltyAmount\r\n        FROM BookingDetails bd\r\n        INNER JOIN ItemPrices ip ON bd.ItemPriceID = ip.ID\r\n        INNER JOIN CancellationRefundFees cf ON ip.CancellationPolicyID = cf.CancellationPolicyID\r\n        WHERE DATEDIFF(bd.RefundDate, ip.Date) >= cf.DaysLeft\r\n        GROUP BY BookingID\r\n    ) cf ON b.ID = cf.BookingID\r\n    INNER JOIN Users u ON b.UserID = u.ID\r\n    WHERE u.UserTypeID = 2 -- UserTypeID 2 corresponds to the \"owner\" user type\r\n    GROUP BY u.ID\r\n) owner_revenues;\r\n";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    totalNetRev = reader.GetString("average_net_revenue");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return totalNetRev;
        }

        public static string getHighestnetOwner()
        {
            string highestnetOwner = "";
            string sql = "SELECT u.FullName, SUM(b.AmountPaid - COALESCE(cr.AmountRefunded, 0) - COALESCE(cf.PenaltyAmount, 0)) AS net_revenue\r\nFROM Bookings b\r\nINNER JOIN BookingDetails bd ON b.ID = bd.BookingID\r\nINNER JOIN ItemPrices ip ON bd.ItemPriceID = ip.ID\r\nLEFT JOIN (\r\n    SELECT BookingID, SUM(CASE WHEN isRefund = 1 THEN ip.Price ELSE 0 END) AS AmountRefunded\r\n    FROM BookingDetails bd\r\n    INNER JOIN ItemPrices ip ON bd.ItemPriceID = ip.ID\r\n    WHERE RefundDate IS NOT NULL\r\n    GROUP BY BookingID\r\n) cr ON b.ID = cr.BookingID\r\nLEFT JOIN (\r\n    SELECT BookingID, SUM(cf.PenaltyPercentage * ip.Price / 100) AS PenaltyAmount\r\n    FROM BookingDetails bd\r\n    INNER JOIN ItemPrices ip ON bd.ItemPriceID = ip.ID\r\n    INNER JOIN CancellationRefundFees cf ON ip.CancellationPolicyID = cf.CancellationPolicyID\r\n    WHERE DATEDIFF(bd.RefundDate, ip.Date) >= cf.DaysLeft\r\n    GROUP BY BookingID\r\n) cf ON b.ID = cf.BookingID\r\nINNER JOIN Users u ON b.UserID = u.ID\r\nWHERE u.UserTypeID = 2 -- UserTypeID 2 corresponds to the \"owner\" user type\r\nGROUP BY u.ID, u.FullName\r\nORDER BY net_revenue DESC\r\nLIMIT 1;\r\n";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    highestnetOwner = reader.GetString("FullName");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return highestnetOwner;
        }

        public static string getTotalNetCancel()
        {
            string totalNetCancel = "";
            string sql = "SELECT b.AmountPaid, bd.RefundCancellationPoliciyID,ip.CancellationPolicyID ,cp.ID,crf.PenaltyPercentage FROM bookings b JOIN bookingdetails bd ON b.ID = bd.BookingID JOIN itemprices ip ON bd.ItemPriceID = ip.ID JOIN cancellationpolicies cp ON cp.ID = ip.CancellationPolicyID JOIN cancellationrefundfees crf ON crf.ID = bd.RefundCancellationPoliciyID WHERE bd.isRefund = 1";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                int amountPaid = 0;
                int penaltyPercentage = 0;
                int currentTotalNetCancel = 0;
                while (reader.Read())
                {
                    amountPaid = reader.GetInt32("AmountPaid");
                    penaltyPercentage = reader.GetInt32("PenaltyPercentage");
                    currentTotalNetCancel += amountPaid * penaltyPercentage / 100;
                    totalNetCancel = currentTotalNetCancel.ToString();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return totalNetCancel;

        }

        public static string totalDisc()
        {
            string totalDisc = "";
            string sql = "SELECT ip.Price,c.DiscountPercent FROM bookings b JOIN bookingdetails bd ON b.ID = bd.BookingID JOIN itemprices ip ON bd.ItemPriceID = ip.ID JOIN coupons c ON b.CouponID = c.ID WHERE b.CouponID IS NOT NULL";
            MySqlConnection con = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sql, con);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                int price = 0;
                int discountPercent = 0;
                int currentTotalDisc = 0;
                while (reader.Read())
                {
                    price = reader.GetInt32("Price");
                    discountPercent = reader.GetInt32("DiscountPercent");
                    currentTotalDisc += price * discountPercent / 100;
                    totalDisc = currentTotalDisc.ToString();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                con.Close();
            }
            return totalDisc;

        }




    }
}
