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

namespace Vados
{
    public partial class FormMessage : Form
    {
        CommandCriteria criteria;
        Form parentForm;


        private static string GetConfirmationMessage(CommandCriteria criteria)
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

            string finalMessage = "Você deseja";


            //Comando
            string commandConnector = "";

            if (commandType != "")
            {
                finalMessage += " " + commandType;

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
                finalMessage += commandConnector + objectType;
            }


            //Formato
            if (format != "")
            {
                finalMessage += " de " + format;
            }


            //Nome
            if (name != "")
            {
                string connector = " chamado";
                if (objectType == "pasta") connector = " chamada";
                if (amount != "") connector += "s";

                finalMessage += connector + " \"" + name + "\"";
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

                finalMessage += modifier + size + " " + sizeUnit;
            }


            //Pasta de origem
            if (origin != "")
            {
                string insideIndicator = " presente na pasta ";
                if (amount != "") insideIndicator = " presentes na pasta ";

                finalMessage += insideIndicator + '\"' + origin + "\"";
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


                finalMessage += destinationIndicator + '\"' + destination + "\"";
            }


            //Novo nome
            if (newName != "")
            {
                finalMessage += " para \"" + newName + "\"";
            }


            finalMessage += "?";
            return finalMessage;
        }


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
            txtMessage.Text = GetConfirmationMessage(this.criteria);
            Global.TextBoxFitContent(txtMessage);

            //Ajustar tamanho do form para caber a mensagem
            int messageMarginBottom = 20;
            int minDistance = txtMessage.Top - lblTitle.Bottom + messageMarginBottom;
            int actualDistance = btnConfirm.Top - txtMessage.Bottom;

            MessageBox.Show("min: " + minDistance + " actual: " + actualDistance);

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
            Form1 form = (Form1)parentForm;
            form.ToggleOverlay(false);
            this.Hide();
        }
    }
}
