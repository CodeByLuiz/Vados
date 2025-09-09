using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vados
{
    public partial class UserControlManual : UserControl
    {
        private Button selectedButton = null;
        private Panel panelNav;

        public UserControlManual()
        {
            InitializeComponent();
            SetupNavBar();
        }

        private void SetupNavBar()
        {
            // Panel de navegação
            panelNav = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(48, 61, 99),
                AutoScroll = true
            };
            this.Controls.Add(panelNav);

            // Título
            Label lblTitle = new Label
            {
                Text = "Comandos",
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelNav.Controls.Add(lblTitle);




            // Categorias e botões
            AddSection(panelNav, "Pastas ", new[]
            {
            "Criar uma pasta",
            "Abrir uma pasta",
            "Abrir pasta padrão",
            "Renomear uma pasta",
            "Excluir uma pasta",
            "Mover uma pasta",
            "Duplicar uma pasta"
        });

            AddSection(panelNav, "Arquivos ", new[]
            {
            "Criar um arquivo",
            "Abrir um arquivo",
            "Renomear um arquivo",
            "Excluir um arquivo",
            "Mover um arquivo",
            "Duplicar um arquivo",
            "Abrir múltiplos arquivos"
        });

            AddSection(panelNav, "Sistema ", new[]
            {
            "Abrir software",
            "Alterar volume",
            "Alterar horário",
            "Alterar brilho da tela",
            "Trocar idioma"
        });
        }


        private void AddSection(Panel panel, string sectionTitle, string[] commands)
        {
            Label lblSection = new Label
            {
                Text = sectionTitle,
                ForeColor = Color.FromArgb(200, 219, 236),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Height = 25,
                Dock = DockStyle.Top,
                Padding = new Padding(10, 5, 0, 0)
            };

            panel.Controls.Add(lblSection);
            panel.Controls.SetChildIndex(lblSection, 0);

            foreach (var cmd in commands)
            {
                Button btn = new Button
                {
                    Text = cmd,
                    Height = 35,
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(48, 61, 99),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9),
                    Padding = new Padding(15, 0, 0, 0),
                    UseVisualStyleBackColor = false
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += NavButton_Click;
                panel.Controls.Add(btn);
                panel.Controls.SetChildIndex(btn, 0);
            }
        }



        private void NavButton_Click(object sender, EventArgs e)
        {

            foreach (var btn in panelNav.Controls.OfType<Button>())
            {
                btn.Font = new Font(btn.Font, FontStyle.Regular);
            }// tira a merda do negrito dos outros botoes pra colocar depois apenas no selecionado

            if (selectedButton != null)

                selectedButton.BackColor = Color.FromArgb(48, 61, 99);

            selectedButton = sender as Button;
            selectedButton.BackColor = Color.FromArgb(82, 99, 152);
            selectedButton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

    }
}
