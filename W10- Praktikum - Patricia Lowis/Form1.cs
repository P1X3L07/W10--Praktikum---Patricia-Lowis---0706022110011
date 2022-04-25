using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace W10__Praktikum___Patricia_Lowis
{
    public partial class Hasil : Form
    {
        public Hasil()
        {
            InitializeComponent();
        }
        public static string sqlConnection = "server=localhost;uid=root;pwd=;database=premier_league";
        public MySqlConnection sqlConnect = new MySqlConnection(sqlConnection);
        public MySqlCommand sqlCommand;
        public MySqlDataAdapter sqlAdapter;
        public string sqlQuery;

        private void Hasil_Load(object sender, EventArgs e)
        {
            sqlConnect.Open();
            DataTable dtTeamL = new DataTable();
            DataTable dtTeamR = new DataTable();
            sqlQuery = "SELECT t.team_name as `Team Name`, m.manager_name as `Manager`, t.team_id as 'ID Team', t.home_stadium as 'Home Stadium', t.capacity as 'Capacity', p.player_name FROM team t, manager m, player p where t.manager_id = m.manager_id and p.player_id = t.captain_id";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeamL);
            sqlAdapter.Fill(dtTeamR);
            cBoxLeft.DataSource = dtTeamL;
            cBoxRight.DataSource = dtTeamR;
            cBoxLeft.DisplayMember = "Team Name";
            cBoxRight.DisplayMember = "Team Name";
            cBoxLeft.ValueMember = "ID Team";
            cBoxRight.ValueMember = "ID Team";
        }

        private void cBoxLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            sqlQuery = "SELECT t.team_name, t.team_id, t.home_stadium, t.capacity , m.manager_name, p.player_name FROM team t, manager m, player p where t.manager_id = m.manager_id and p.player_id = t.captain_id";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dt1);

            lblManagerL.Text = dt1.Rows[cBoxLeft.SelectedIndex][4].ToString();
            lblCaptainL.Text = dt1.Rows[cBoxLeft.SelectedIndex][5].ToString();
            lblStadium.Text = dt1.Rows[cBoxLeft.SelectedIndex][2].ToString();
            lblCapacity.Text = dt1.Rows[cBoxLeft.SelectedIndex][3].ToString();
        }

        private void cBoxRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt2 = new DataTable();
            sqlQuery = "SELECT t.team_name, t.team_id, t.home_stadium, t.capacity , m.manager_name, p.player_name FROM team t, manager m, player p where t.manager_id = m.manager_id and p.player_id = t.captain_id";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dt2);

            lblManagerR.Text = dt2.Rows[cBoxRight.SelectedIndex][4].ToString();
            lblCaptainR.Text = dt2.Rows[cBoxRight.SelectedIndex][5].ToString();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            DataTable dt3 = new DataTable();
            sqlQuery = "SELECT DATE_FORMAT(match_date, '%d %M %Y') as 'tanggal' FROM `match` WHERE team_home = '" + cBoxLeft.SelectedValue.ToString() + "' AND team_away = '" + cBoxRight.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dt3);

            lblTanggal.Text = dt3.Rows[0]["tanggal"].ToString();

            DataTable dt4 = new DataTable();
            sqlQuery = "SELECT CONCAT(goal_home,' - ' , goal_away) FROM `match` WHERE goal_home = '" + cBoxLeft.SelectedValue.ToString() + "' AND goal_away = '" + cBoxRight.SelectedValue.ToString() + "'";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dt4);

            lblSkor.Text = dt4.Rows[0][0].ToString();

            sqlQuery = "select d.minute as Minute, if(p.team_id != m.team_home, '', p.player_name) as 'Player Name 1', if(p.team_id != m.team_home, '', if(d.type = 'CY', 'Yellow Card', if(d.type = 'CR', 'Red Card', if(d.type = 'GO', 'Goal', if(d.type = 'GP', 'Goal Penalty', if(d.type = 'GW', 'Own Goal', if(d.type = 'PM', 'Penalty Miss', ''))))))) as 'Tipe 1', if(p.team_id != m.team_away, '', p.player_name) as 'Player Name 2', if(p.team_id != m.team_away, '', if(d.type = 'CY', 'Yellow Card', if(d.type = 'CR', 'Red Card', if(d.type = 'GO', 'Goal', if(d.type = 'GP', 'Goal Penalty', if(d.type = 'GW', 'Own Goal', if(d.type = 'PM', 'Penalty Miss', ''))))))) as 'Tipe 2'  from dmatch d, player p, `match` m where d.match_id = m.match_id and p.player_id = d.player_id and m.team_home = '" + cBoxLeft.SelectedValue.ToString() + "' and m.team_away = '" + cBoxRight.SelectedValue.ToString() + "' order by 1";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            DataTable dgv = new DataTable();
            sqlAdapter.Fill(dgv);
            dgvResult.DataSource = dgv;
        }
    }
}
