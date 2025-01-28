using FirstCrud.Business;
using FirstCrud.Entities;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FirstCrud.UI
{
    public partial class Form1 : Form
    {
        private readonly CorService _service;

        public Form1()
        {
            InitializeComponent();
            _service = new CorService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void AtualizarGrid()
        {
            dataGridView1.DataSource = null;

            // Obtém os dados e atribui ao DataGridView
            var cores = _service.ObterTodasCores();
            dataGridView1.DataSource = cores;

            // Adiciona a coluna de checkbox, se não existir
            if (!dataGridView1.Columns.Contains("Selecionar"))
            {
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Selecionar",
                    Name = "Selecionar",
                    Width = 50
                };
                dataGridView1.Columns.Insert(0, checkBoxColumn); // Insere a coluna como a primeira
            }

            // Permite edição no DataGridView
            dataGridView1.Columns["Id_Cor"].ReadOnly = true; // O ID não pode ser editado
            dataGridView1.Columns["Desc_Cor"].ReadOnly = false; // A descrição pode ser editada
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var isChecked = Convert.ToBoolean(row.Cells["Selecionar"].Value);
                if (isChecked)
                {
                    var idCorCell = row.Cells["Id_Cor"].Value;
                    var descCorCell = row.Cells["Desc_Cor"].Value;

                    if (idCorCell != null && int.TryParse(idCorCell.ToString(), out int idCor))
                    {
                        // Atualiza a cor existente
                        var corAtualizada = new Cor
                        {
                            Id_Cor = idCor,
                            Desc_Cor = descCorCell?.ToString() ?? string.Empty
                        };
                        _service.AtualizarCor(corAtualizada);
                    }
                }
            }

            MessageBox.Show("Dados atualizados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarGrid();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            // Obtém a nova cor do TextBox
            var novaCorDescricao = txtNewColor.Text.Trim();

            if (!string.IsNullOrWhiteSpace(novaCorDescricao))
            {
                var novaCor = new Cor { Desc_Cor = novaCorDescricao };
                _service.AdicionarCor(novaCor);

                MessageBox.Show("Nova cor adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpa o TextBox e atualiza o grid
                txtNewColor.Clear();
                AtualizarGrid();
            }
            else
            {
                MessageBox.Show("A descrição da cor não pode estar vazia.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var isChecked = Convert.ToBoolean(row.Cells["Selecionar"].Value);
                if (isChecked)
                {
                    var idCorCell = row.Cells["Id_Cor"].Value;

                    if (idCorCell != null && int.TryParse(idCorCell.ToString(), out int idCor))
                    {
                        _service.ExcluirCor(idCor);
                    }
                }
            }

            MessageBox.Show("Dados excluídos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarGrid();
        }
    }
}
