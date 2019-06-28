namespace CoinDetection
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.infoNotCam = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkChange = new System.Windows.Forms.CheckBox();
            this.comboBoxCams = new System.Windows.Forms.ComboBox();
            this.infoCam = new System.Windows.Forms.Label();
            this.comboBoxRes = new System.Windows.Forms.ComboBox();
            this.infoRes = new System.Windows.Forms.Label();
            this.labelPila = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // infoNotCam
            // 
            this.infoNotCam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoNotCam.AutoSize = true;
            this.infoNotCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoNotCam.Location = new System.Drawing.Point(216, 268);
            this.infoNotCam.Name = "infoNotCam";
            this.infoNotCam.Size = new System.Drawing.Size(387, 31);
            this.infoNotCam.TabIndex = 1;
            this.infoNotCam.Text = "Nenhuma Câmera disponível";
            this.infoNotCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.infoNotCam.Visible = false;
            this.infoNotCam.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Uniformizar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
            // 
            // checkChange
            // 
            this.checkChange.AutoSize = true;
            this.checkChange.Location = new System.Drawing.Point(13, 23);
            this.checkChange.Name = "checkChange";
            this.checkChange.Size = new System.Drawing.Size(59, 17);
            this.checkChange.TabIndex = 3;
            this.checkChange.Text = "Bordas";
            this.checkChange.UseVisualStyleBackColor = true;
            this.checkChange.CheckedChanged += new System.EventHandler(this.checkChange_CheckedChanged);
            // 
            // comboBoxCams
            // 
            this.comboBoxCams.FormattingEnabled = true;
            this.comboBoxCams.Location = new System.Drawing.Point(163, 0);
            this.comboBoxCams.Name = "comboBoxCams";
            this.comboBoxCams.Size = new System.Drawing.Size(415, 21);
            this.comboBoxCams.TabIndex = 5;
            this.comboBoxCams.SelectedIndexChanged += new System.EventHandler(this.comboBoxCams_SelectedIndexChanged);
            // 
            // infoCam
            // 
            this.infoCam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoCam.AutoSize = true;
            this.infoCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoCam.Location = new System.Drawing.Point(129, 26);
            this.infoCam.Name = "infoCam";
            this.infoCam.Size = new System.Drawing.Size(513, 31);
            this.infoCam.TabIndex = 6;
            this.infoCam.Text = "1- SELECIONE UMA CÂMERA ACIMA";
            this.infoCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.infoCam.Visible = false;
            // 
            // comboBoxRes
            // 
            this.comboBoxRes.FormattingEnabled = true;
            this.comboBoxRes.Location = new System.Drawing.Point(163, 60);
            this.comboBoxRes.Name = "comboBoxRes";
            this.comboBoxRes.Size = new System.Drawing.Size(415, 21);
            this.comboBoxRes.TabIndex = 7;
            this.comboBoxRes.SelectedIndexChanged += new System.EventHandler(this.comboBoxRes_SelectedIndexChanged);
            // 
            // infoRes
            // 
            this.infoRes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoRes.AutoSize = true;
            this.infoRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoRes.Location = new System.Drawing.Point(129, 94);
            this.infoRes.Name = "infoRes";
            this.infoRes.Size = new System.Drawing.Size(571, 155);
            this.infoRes.TabIndex = 8;
            this.infoRes.Text = "2- SELECIONE UMA RESOLUÇÃO ACIMA\r\nRecomenda-se uma baixa resolução\r\npara diminuir" +
    " o número de cálculos.\r\nCaso a imagem esteja distorcida utilize\r\numa outra resol" +
    "ução.";
            this.infoRes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPila
            // 
            this.labelPila.AutoSize = true;
            this.labelPila.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPila.Location = new System.Drawing.Point(81, 0);
            this.labelPila.Name = "labelPila";
            this.labelPila.Size = new System.Drawing.Size(75, 28);
            this.labelPila.TabIndex = 9;
            this.labelPila.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 611);
            this.Controls.Add(this.labelPila);
            this.Controls.Add(this.infoRes);
            this.Controls.Add(this.comboBoxRes);
            this.Controls.Add(this.infoCam);
            this.Controls.Add(this.comboBoxCams);
            this.Controls.Add(this.checkChange);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.infoNotCam);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed_1);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label infoNotCam;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkChange;
        private System.Windows.Forms.ComboBox comboBoxCams;
        private System.Windows.Forms.Label infoCam;
        private System.Windows.Forms.ComboBox comboBoxRes;
        private System.Windows.Forms.Label infoRes;
        private System.Windows.Forms.Label labelPila;
    }
}

