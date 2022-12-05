using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITMovies
{
    public partial class GestionAchats : Form
    {
        public GestionAchats()
        {
            InitializeComponent();
            fillData();
        }
        private void fillData()
        {
            //Fill the data grid view with the films in the database
            //Use the connection object in the Database class
            string sql = "select a.id, f.titre Film, c.nom Client, a.dateAchat Date, f.prix Prix, f.stock Stock, a.etat Etat from achats a inner join films f on a.idFilm = f.id inner join clients c on a.idClient = c.id";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, Database.connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            //set all the columns readonly
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }
            // set the Film column fill
            dataGridView1.Columns["Film"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // set the Client column fill
            dataGridView1.Columns["Client"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void validerBtn_Click(object sender, EventArgs e)
        {
            int idAchat = int.Parse(idField.Text);
            Achat achat = new Achat(idAchat);
            if (achat.validerAchat())
            {
                MessageBox.Show("Achat validé avec succès");
            }
            else
            {
                MessageBox.Show("Erreur lors de la validation");
            }
            dataGridView1.DataSource = null;
            fillData();
        }

        private void refuserBtn_Click(object sender, EventArgs e)
        {
            int idAchat = int.Parse(idField.Text);
            Achat achat = new Achat(idAchat);
            if (achat.refuserAchat())
            {
                MessageBox.Show("Achat refusé avec succès");
            }
            else
            {
                MessageBox.Show("Erreur lors du refus");
            }
        }

        private void GestionAchats_Load(object sender, EventArgs e)
        {
            dataGridView1.MultiSelect = false;
            idField.Enabled = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // get the row of the selected cell
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            // get the id of the selected row
            idField.Text = row.Cells[0].Value.ToString();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // get the id of the selected row
                idField.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void searchField_TextChanged(object sender, EventArgs e)
        {
            // search by all the columns of the data grid view
            string sql = $"select a.id, f.titre Film, c.nom Client, a.dateAchat Date, f.prix Prix, f.stock Stock, a.etat Etat from achats a inner join films f on a.idFilm = f.id inner join clients c on a.idClient = c.id where a.id like '%{searchField.Text}%' or f.titre like '%{searchField.Text}%' or c.nom like '%{searchField.Text}%' or a.dateAchat like '%{searchField.Text}%' or f.prix like '%{searchField.Text}%' or f.stock like '%{searchField.Text}%' or a.etat like '%{searchField.Text}%'";
            if (Database.connection.State == ConnectionState.Closed)
            {
                Database.connection.Open();
            }
            SqlDataAdapter adapter = new SqlDataAdapter(sql, Database.connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            //set all the columns readonly
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ReadOnly = true;
            }
            // set the Film column fill
            dataGridView1.Columns["Film"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // set the Client column fill
            dataGridView1.Columns["Client"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (Database.connection.State == ConnectionState.Open)
            {
                Database.connection.Close();
            }


        }
    }
}
