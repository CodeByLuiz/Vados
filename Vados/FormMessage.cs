using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        Form parentForm;


        private void ExitMessage()
        {
            Form1 form = (Form1)parentForm;
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

            MessageBox.Show("old width: " + textBox.Width.ToString());
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


        public FormMessage(CommandCriteria criteria_, Form parentForm_)
        {
            InitializeComponent();
            criteria = criteria_;
            parentForm = parentForm_;

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
            SetConfirmationMessage(txtMessage, this.criteria);
            Global.TextBoxFitHeight(txtMessage);
            MessageBox.Show("new width: " + txtMessage.Width.ToString());

            //Ajustar tamanho do form para caber a mensagem
            int messageMarginBottom = 20;
            int minDistance = txtMessage.Top - lblTitle.Bottom + messageMarginBottom;
            int actualDistance = btnConfirm.Top - txtMessage.Bottom;

            this.Height += minDistance - actualDistance;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMessage_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ExitMessage();
        }
    }
}
