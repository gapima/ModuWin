using FirstCrud.Business;
using FirstCrud.Entities;
using System;
using System.Collections.Generic;
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
            // Reseta o DataSource
            dataGridView1.DataSource = null;

            // Obtém os dados
            var cores = _service.ObterTodasCores();
            dataGridView1.DataSource = cores;

            // Adiciona a coluna de checkbox se não existir
            if (!dataGridView1.Columns.Contains("Selecionar"))
            {
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Selecionar",
                    Name = "Selecionar",
                    Width = 50
                };
                dataGridView1.Columns.Insert(0, checkBoxColumn); // Insere como a primeira coluna
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Verifica se o campo ID está vazio (nova inserção ou atualização)
            if (string.IsNullOrWhiteSpace(txtIdCor.Text))
            {
                // Adiciona uma nova cor
                var novaCor = new Cor { Desc_Cor = txtDescCor.Text };
                _service.AdicionarCor(novaCor);
                MessageBox.Show("Cor adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Atualiza a cor existente
                if (int.TryParse(txtIdCor.Text, out int idCor))
                {
                    var corAtualizada = new Cor
                    {
                        Id_Cor = idCor,
                        Desc_Cor = txtDescCor.Text
                    };
                    _service.AtualizarCor(corAtualizada);
                    MessageBox.Show("Cor atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ID inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Atualiza o grid e limpa os campos
            AtualizarGrid();
            LimparCampos();
        }

        private void LimparCampos()
        {
            txtIdCor.Text = string.Empty;
            txtDescCor.Text = string.Empty;
            txtIdCor.Enabled = false;
            txtDescCor.Enabled = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se a célula clicada pertence à coluna de checkbox
            if (e.ColumnIndex == dataGridView1.Columns["Selecionar"].Index && e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                bool isChecked = Convert.ToBoolean(row.Cells["Selecionar"].Value ?? false);
                row.Cells["Selecionar"].Value = !isChecked;

                if (!isChecked)
                {
                    // Carrega os valores da linha selecionada nos campos de texto
                    txtIdCor.Text = row.Cells["Id_Cor"].Value.ToString();
                    txtDescCor.Text = row.Cells["Desc_Cor"].Value.ToString();

                    txtIdCor.Enabled = false; // Desabilita edição do ID
                    txtDescCor.Enabled = true;

                    // Desmarca os checkboxes das outras linhas
                    foreach (DataGridViewRow otherRow in dataGridView1.Rows)
                    {
                        if (otherRow != row)
                        {
                            otherRow.Cells["Selecionar"].Value = false;
                        }
                    }
                }
                else
                {
                    // Limpa os campos caso a linha seja desmarcada
                    LimparCampos();
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Itera pelas linhas e verifica os checkboxes
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var isChecked = Convert.ToBoolean(row.Cells["Selecionar"].Value);
                if (isChecked)
                {
                    var cor = (Cor)row.DataBoundItem;
                    _service.ExcluirCor(cor.Id_Cor);
                }
            }
            AtualizarGrid();
        }
    }
}
