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
        public UserControl userControl;
        private string ComandoBdTxt;

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
            Form1 form = (Form1)Owner;
            form.Focus();
            form.ToggleOverlay(false);
            Hide();
        }


        private void FocusCommand(bool clear = false)
        {
            UserControlHome page = (UserControlHome)userControl;
            page.FocusCommand(clear);
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
                Global.AppendFormattedText(textBox, " " + commandType, Colors.blueHighlight, FontStyle.Bold);

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
                Global.AppendPlainText(textBox, commandConnector + objectType);
            }


            //Formato
            if (format != "")
            {
                Global.AppendPlainText(textBox, " de " + format);
            }


            //Nome
            if (name != "")
            {
                string connector = " chamado";
                if (objectType == "pasta") connector = " chamada";
                if (amount != "") connector += "s";

                Global.AppendPlainText(textBox, connector + " ");
                Global.AppendFormattedText(textBox, name, Colors.greenHighlight, FontStyle.Bold);
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

                Global.AppendPlainText(textBox, modifier + size + " " + sizeUnit);
            }


            //Pasta de origem
            if (origin != "")
            {
                string insideIndicator = " presente na pasta ";
                if (amount != "") insideIndicator = " presentes na pasta ";

                Global.AppendPlainText(textBox, insideIndicator);
                Global.AppendFormattedText(textBox, origin, Colors.greenHighlight, FontStyle.Bold);
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


                Global.AppendPlainText(textBox, destinationIndicator);
                Global.AppendFormattedText(textBox, destination, Colors.greenHighlight, FontStyle.Bold);
            }


            //Novo nome
            if (newName != "")
            {
                Global.AppendPlainText(textBox, " para ");
                Global.AppendFormattedText(textBox, newName, Colors.greenHighlight, FontStyle.Bold);
            }


            Global.AppendPlainText(textBox, "?");
        }


        private static void SetErrorMessage(RichTextBox textBox, CommandCriteria criteria)
        {
            //Comando não identificado
            if (string.IsNullOrEmpty(criteria.Action))
            {
                textBox.Text = "Comando não identificado.";
                return;
            }


            //Objeto não identificado
            if (string.IsNullOrEmpty(criteria.ObjectType))
            {
                string objects = "(arquivo / pasta)";
                if (criteria.Action == "abrir") objects = "(arquivo / aplicativo / atalho)";
                textBox.Text = $"Especifique o que você quer {criteria.Action} {objects}.";
                return;
            }


            //Formato não identificado
            if (!string.IsNullOrEmpty(criteria.ObjectFormat))
            {
                if (!Comandos.allExtensionsWords.Contains(criteria.ObjectFormat))
                    textBox.Text = "Formato de arquivo não identificado.";
            }


            //Erro em um dos critérios
            string criteriaMessage = "";

            switch(criteria.Action)
            {
                case "criar":
                    //Nome não especificado
                    if (string.IsNullOrEmpty(criteria.ObjectName))
                    {
                        string objToBeCreated = "do arquivo a ser criado";
                        if (criteria.ObjectType == "pasta") objToBeCreated = "da pasta a ser criada";
                        criteriaMessage = "Especifique o nome " + objToBeCreated + ".";
                    }

                    break;


                case "mover":
                    //Pasta de destino não identificada
                    if (string.IsNullOrEmpty(criteria.Destination))
                    {
                        string objToBeCreated = "do arquivo a ser movido";
                        if (criteria.ObjectType == "pasta") objToBeCreated = "da pasta a ser movida";
                        criteriaMessage = "Especifique a pasta de destino " + objToBeCreated + ".";
                    }

                    break;


                //Mensagem padrão para o erro de comando
                default:
                    criteriaMessage = "Especifique os critérios necessários para o comando.";
                    break;
            }

            textBox.Text = criteriaMessage;
        }


        #endregion

        

        public FormMessage(CommandCriteria criteria_, bool isErrorMessage_, string messageRtf = "", string txtbd="")
        {
            InitializeComponent();
            criteria = criteria_;
            isErrorMessage = isErrorMessage_;

            
            ComandoBdTxt = txtbd;
            //Definir mensagem
            var messageFont = new System.Drawing.Font("Segoe UI", 11f);
            txtMessage.Rtf = Global.RtfChangeFont(messageRtf, messageFont);

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

            //Mensagem de erro
            if (isErrorMessage)
            {
                //Definir título
                lblTitle.Text = "Erro de comando";
                lblTitle.ForeColor = Colors.redErrorDark;
                Global.LabelFitWidth(lblTitle);

                //Definir ícone
                imgTitleIcon.Image = System.Drawing.Image.FromFile(Path.Combine(System.Windows.Forms.Application.StartupPath, @"Images\Icons\warningIcon.png"));

                //Definir botões
                btnConfirm.Text = "Editar";
                btnConfirm.BackColor = Colors.redErrorLight;

                btnCancel.Text = "Descartar";
                btnCancel.FlatAppearance.BorderColor = Colors.redErrorLight;

                //Mensagem
                if (txtMessage.Text == "")
                {
                    SetErrorMessage(txtMessage, criteria);
                }
            }
            //Mensagem de confirmação
            else
            {
                if (txtMessage.Text == "")
                {
                    SetConfirmationMessage(txtMessage, criteria);
                }
            }


            //Ajustar tamanho do form para caber a mensagem
            Global.TextBoxFitHeight(txtMessage);
            int messageMarginBottom = 20;
            int minDistance = txtMessage.Top - lblTitle.Bottom + messageMarginBottom;
            int actualDistance = btnConfirm.Top - txtMessage.Bottom;

            this.Height += minDistance - actualDistance;
            this.StartPosition = FormStartPosition.CenterScreen;

            //Corrigir posição do ícone ao lado do título
            Global.LabelFitWidth(lblTitle);
            int iconDist = imgTitleIcon.Left - lblTitle.Right;
            imgTitleIcon.Left = lblTitle.Right + iconDist;


            //Definir variáveis dos botões
            btnConfirm.BehindColor = BackColor;
            btnConfirm.HoverLightenFactor = 0.2f;
            btnConfirm.PressDarkenFactor = -0.1f;

            btnCancel.BehindColor = BackColor;
            btnCancel.HoverLightenFactor = 0.7f;
            btnCancel.PressDarkenFactor = -0.1f;


            //Corrigir posição da mensagem na tela
            parentForm.CorrectMessageForm();
        }


        private void txtMessage_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            ExitMessage();

            //Limpar textbox (descartar)
            if (isErrorMessage == false)
            {
                FocusCommand(true);
            }
        }


        private async void btnConfirm_Click(object sender, EventArgs e)
        {

            //Limpar textbox
            if (isErrorMessage == true)
            {
                FocusCommand();
            }
            //Executar comando
            else
            {
                Form1 form = (Form1)Owner;
                CommandCriteria criteriaCopy = criteria;

                _ = Task.Run(async () =>    //Iniciar uma task, que realiza o código separadamente quando terminar
                {
                    try
                    {
                        //Realizar comando
                        var errorMessage = await Comandos.ExecuteCommand(criteria);
                       
                        //Mostrar mensagem de erro
                        if (errorMessage != "")
                        {
                            form.BeginInvoke((MethodInvoker)(() =>
                            {
                                form.ShowPopupMessage(true, form, userControl, criteriaCopy, errorMessage);
                            }));
                        }
                        else if (!String.IsNullOrEmpty(ComandoBdTxt))
                        {
                            BancoDeDados.AdicionarEntrada(
                             comando: ComandoBdTxt,
                             titulo: $"{criteria.Action} {criteria.ObjectType} "
                             
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao executar comando: {ex.Message}");
                    }
                });
            }

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
