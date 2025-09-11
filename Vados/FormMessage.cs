using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Vados
{
    public partial class FormMessage : Form
    {
        CommandCriteria criteria;
        bool isErrorMessage = false;

        //Bordas arredondadas
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );


        private void ExitMessage()
        {
            Form1 form = (Form1)this.Owner;
            form.Focus();
            form.ToggleOverlay(false);
            this.Hide();
        }


        #region DEFINIR MENSAGEM

        private static void SetConfirmationMessage(RichTextBox textBox, CommandCriteria criteria)
        {
            string commandType = criteria.Action;
            string objectType = criteria.ObjectType;
            string name = criteria.ObjectName;
            string newName = criteria.ObjectNewName;
            string format = criteria.ObjectFormat;
            string origin = criteria.Origin;
            string destination = criteria.Destination;
            string amount = criteria.ObjectAmount;
            string size = criteria.SizeAmount;
            string sizeUnit = criteria.SizeUnit;
            string sizeModifier = criteria.SizeModifier;

            textBox.Text = "Você deseja";


            //Comando
            string commandConnector = "";

            if (commandType != "")
            {
                AppendFormattedText(textBox, " " + commandType, Colors.blueHighlight, FontStyle.Bold);

                //Conector após comando
                switch (commandType)
                {
                    case "criar":
                        commandConnector = " um ";
                        if (objectType == "pasta") commandConnector = " uma ";
                        break;
                    default:
                        commandConnector = " o ";
                        if (objectType == "pasta") commandConnector = " a ";
                        break;
                }
            }


            //Quantidade
            switch (amount)
            {
                case "":
                    break;

                case "todos":
                    commandConnector = " todos os ";
                    if (objectType == "pasta") commandConnector = " todas as ";
                    break;

                case "metade":
                    commandConnector = " metade dos ";
                    if (objectType == "pasta") commandConnector = " metade das ";
                    break;
            }


            //Objeto
            if (objectType != "")
            {
                string objectStr = objectType;
                if (amount != "") objectStr += "s";
                AppendPlainText(textBox, commandConnector + objectType);
            }


            //Formato
            if (format != "")
            {
                AppendPlainText(textBox, " de " + format);
            }


            //Nome
            if (name != "")
            {
                string connector = " chamado";
                if (objectType == "pasta") connector = " chamada";
                if (amount != "") connector += "s";

                AppendPlainText(textBox, connector + " ");
                AppendFormattedText(textBox, name, Colors.greenHighlight, FontStyle.Bold);
            }


            //Tamanho
            if (size != "")
            {
                string modifier = "";

                switch (sizeModifier)
                {
                    case "igual":
                        modifier = " de ";
                        break;

                    case "maior":
                        modifier = " maior que ";
                        if (amount != "") modifier = " maiores que ";
                        break;

                    case "menor":
                        modifier = " menor que ";
                        if (amount != "") modifier = " menores que ";
                        break;
                }

                AppendPlainText(textBox, modifier + size + " " + sizeUnit);
            }


            //Pasta de origem
            if (origin != "")
            {
                string insideIndicator = " presente na pasta ";
                if (amount != "") insideIndicator = " presentes na pasta ";

                AppendPlainText(textBox, insideIndicator);
                AppendFormattedText(textBox, origin, Colors.greenHighlight, FontStyle.Bold);
            }


            //Pasta de destino
            if (destination != "")
            {
                //Obter indicador da pasta
                string destinationIndicator = "";

                switch (commandType)
                {
                    case "criar":
                        destinationIndicator = " na pasta ";
                        break;
                    case "mover":
                        destinationIndicator = " para dentro da pasta ";
                        break;
                }


                AppendPlainText(textBox, destinationIndicator);
                AppendFormattedText(textBox, destination, Colors.greenHighlight, FontStyle.Bold);
            }


            //Novo nome
            if (newName != "")
            {
                AppendPlainText(textBox, " para ");
                AppendFormattedText(textBox, newName, Colors.greenHighlight, FontStyle.Bold);
            }


            AppendPlainText(textBox, "?");
        }


        public static void AppendPlainText(RichTextBox textBox, string text)
        {
            //Iniciar seleção no fim da string
            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;

            //Resetar formatação
            textBox.SelectionColor = textBox.ForeColor;
            textBox.SelectionFont = textBox.Font;

            //Adicionar texto
            textBox.AppendText(text);
        }

        public static void AppendFormattedText(RichTextBox textBox, string text, Color color, FontStyle fontStyle)
        {
            //Iniciar seleção no fim da string
            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;

            //Formatar texto
            textBox.SelectionColor = color;
            textBox.SelectionFont = new System.Drawing.Font(textBox.Font, fontStyle);

            //Adicionar texto
            textBox.AppendText(text);
        }

        #endregion


        public FormMessage(CommandCriteria criteria_, bool isErrorMessage_)
        {
            InitializeComponent();
            criteria = criteria_;
            isErrorMessage = isErrorMessage_;

            //Otimizar pintura
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
        }


        //Configurar mensagem
        private void FormMessage_Load(object sender, EventArgs e)
        {
            Form1 parentForm = (Form1)Owner;
            parentForm.CorrectMessageForm();
            Global.TextBoxFitHeight(txtMessage);

            //Ajustar tamanho do form para caber a mensagem
            int messageMarginBottom = 20;
            int minDistance = txtMessage.Top - lblTitle.Bottom + messageMarginBottom;
            int actualDistance = btnConfirm.Top - txtMessage.Bottom;

            this.Height += minDistance - actualDistance;
            this.StartPosition = FormStartPosition.CenterScreen;


            Global.LabelFitWidth(lblTitle);
            int iconDist = imgTitleIcon.Left - lblTitle.Right;

            //Mensagem de erro
            if (isErrorMessage)
            {
                //Definir título
                lblTitle.Text = "Erro de comando";
                lblTitle.ForeColor = Colors.redErrorDark;
                Global.LabelFitWidth(lblTitle);

                //Definir ícone
                imgTitleIcon.Left = lblTitle.Right + iconDist;
                imgTitleIcon.Image = System.Drawing.Image.FromFile(Path.Combine(System.Windows.Forms.Application.StartupPath, @"Images\Icons\warningIcon.png"));

                //Definir botões
                btnConfirm.Text = "Editar";
                btnConfirm.BackColor = Colors.redErrorLight;

                btnCancel.Text = "Descartar";
                btnCancel.FlatAppearance.BorderColor = Colors.redErrorLight;

                //Mensagem
                txtMessage.Text = "Comando não identficado";
            }
            //Mensagem de confirmação
            else
            {
                SetConfirmationMessage(txtMessage, this.criteria);
            }


            //Definir variáveis dos botões
            btnConfirm.BehindColor = BackColor;
            btnConfirm.HoverLightenFactor = 0.2f;
            btnConfirm.PressDarkenFactor = -0.1f;

            btnCancel.BehindColor = BackColor;
            btnCancel.HoverLightenFactor = 0.7f;
            btnCancel.PressDarkenFactor = -0.1f;
        }


        private void txtMessage_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            ExitMessage();
        }


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Comandos.ExecuteCommand(criteria);
            ExitMessage();
        }


        private void FormMessage_Resize(object sender, EventArgs e)
        {
            //Definir arredondamento da janela
            int roundValue = (int)(0.2 * Height);
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, roundValue, roundValue));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ExitMessage();
        }
    }
}
