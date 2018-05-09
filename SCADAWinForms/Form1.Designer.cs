namespace SCADAWinForms
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lb_mensajes = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pb_analogica1 = new System.Windows.Forms.ProgressBar();
            this.pb_analogica2 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bt_pulsado1 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bt_pulsador2 = new System.Windows.Forms.Button();
            this.tb_con = new System.Windows.Forms.TextBox();
            this.tb_proto = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_analogica1 = new System.Windows.Forms.TextBox();
            this.tb_analogica2 = new System.Windows.Forms.TextBox();
            this.bt_encender2 = new System.Windows.Forms.Button();
            this.bt_encender1 = new System.Windows.Forms.Button();
            this.bt_ActualizarBD = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_mensajes
            // 
            this.lb_mensajes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_mensajes.FormattingEnabled = true;
            this.lb_mensajes.Location = new System.Drawing.Point(9, 275);
            this.lb_mensajes.Name = "lb_mensajes";
            this.lb_mensajes.Size = new System.Drawing.Size(373, 69);
            this.lb_mensajes.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(215, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "Led 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(215, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 26);
            this.label2.TabIndex = 6;
            this.label2.Text = "Led 2:";
            // 
            // pb_analogica1
            // 
            this.pb_analogica1.Location = new System.Drawing.Point(155, 190);
            this.pb_analogica1.Name = "pb_analogica1";
            this.pb_analogica1.Size = new System.Drawing.Size(150, 18);
            this.pb_analogica1.TabIndex = 7;
            // 
            // pb_analogica2
            // 
            this.pb_analogica2.Location = new System.Drawing.Point(155, 232);
            this.pb_analogica2.Name = "pb_analogica2";
            this.pb_analogica2.Size = new System.Drawing.Size(150, 19);
            this.pb_analogica2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 26);
            this.label3.TabIndex = 9;
            this.label3.Text = "Analógica 1:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 228);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 26);
            this.label4.TabIndex = 10;
            this.label4.Text = "Analógica 2:";
            // 
            // bt_pulsado1
            // 
            this.bt_pulsado1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_pulsado1.ImageIndex = 0;
            this.bt_pulsado1.ImageList = this.imageList1;
            this.bt_pulsado1.Location = new System.Drawing.Point(301, 94);
            this.bt_pulsado1.Name = "bt_pulsado1";
            this.bt_pulsado1.Size = new System.Drawing.Size(83, 82);
            this.bt_pulsado1.TabIndex = 4;
            this.bt_pulsado1.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "botonAzul.png");
            this.imageList1.Images.SetKeyName(1, "botonGris.png");
            this.imageList1.Images.SetKeyName(2, "");
            // 
            // bt_pulsador2
            // 
            this.bt_pulsador2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_pulsador2.ImageIndex = 0;
            this.bt_pulsador2.ImageList = this.imageList1;
            this.bt_pulsador2.Location = new System.Drawing.Point(301, 4);
            this.bt_pulsador2.Name = "bt_pulsador2";
            this.bt_pulsador2.Size = new System.Drawing.Size(83, 82);
            this.bt_pulsador2.TabIndex = 3;
            this.bt_pulsador2.UseVisualStyleBackColor = true;
            // 
            // tb_con
            // 
            this.tb_con.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_con.Location = new System.Drawing.Point(9, 354);
            this.tb_con.Name = "tb_con";
            this.tb_con.Size = new System.Drawing.Size(112, 20);
            this.tb_con.TabIndex = 12;
            // 
            // tb_proto
            // 
            this.tb_proto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_proto.Location = new System.Drawing.Point(9, 380);
            this.tb_proto.Name = "tb_proto";
            this.tb_proto.Size = new System.Drawing.Size(112, 20);
            this.tb_proto.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(127, 354);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 46);
            this.button1.TabIndex = 14;
            this.button1.Text = "Limpiar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_analogica1
            // 
            this.tb_analogica1.Location = new System.Drawing.Point(311, 188);
            this.tb_analogica1.Name = "tb_analogica1";
            this.tb_analogica1.Size = new System.Drawing.Size(73, 20);
            this.tb_analogica1.TabIndex = 15;
            // 
            // tb_analogica2
            // 
            this.tb_analogica2.Location = new System.Drawing.Point(311, 231);
            this.tb_analogica2.Name = "tb_analogica2";
            this.tb_analogica2.Size = new System.Drawing.Size(73, 20);
            this.tb_analogica2.TabIndex = 16;
            // 
            // bt_encender2
            // 
            this.bt_encender2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_encender2.ImageIndex = 2;
            this.bt_encender2.ImageList = this.imageList1;
            this.bt_encender2.Location = new System.Drawing.Point(110, 4);
            this.bt_encender2.Name = "bt_encender2";
            this.bt_encender2.Size = new System.Drawing.Size(83, 82);
            this.bt_encender2.TabIndex = 17;
            this.bt_encender2.UseVisualStyleBackColor = true;
            this.bt_encender2.Click += new System.EventHandler(this.bt_encender1_Click);
            // 
            // bt_encender1
            // 
            this.bt_encender1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bt_encender1.ImageIndex = 2;
            this.bt_encender1.ImageList = this.imageList1;
            this.bt_encender1.Location = new System.Drawing.Point(110, 94);
            this.bt_encender1.Name = "bt_encender1";
            this.bt_encender1.Size = new System.Drawing.Size(83, 82);
            this.bt_encender1.TabIndex = 18;
            this.bt_encender1.UseVisualStyleBackColor = true;
            this.bt_encender1.Click += new System.EventHandler(this.bt_encender1_Click);
            // 
            // bt_ActualizarBD
            // 
            this.bt_ActualizarBD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_ActualizarBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_ActualizarBD.Location = new System.Drawing.Point(249, 351);
            this.bt_ActualizarBD.Name = "bt_ActualizarBD";
            this.bt_ActualizarBD.Size = new System.Drawing.Size(133, 23);
            this.bt_ActualizarBD.TabIndex = 19;
            this.bt_ActualizarBD.Text = "Actualizar BD";
            this.bt_ActualizarBD.UseVisualStyleBackColor = true;
            this.bt_ActualizarBD.Click += new System.EventHandler(this.bt_ActualizarBD_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(249, 380);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 24);
            this.button2.TabIndex = 20;
            this.button2.Text = "Informe";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 26);
            this.label5.TabIndex = 22;
            this.label5.Text = "Botón 2:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 26);
            this.label6.TabIndex = 21;
            this.label6.Text = "Botón 1:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(158, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label8.Location = new System.Drawing.Point(263, 211);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "4.096";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label9.Location = new System.Drawing.Point(270, 254);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "4.088";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label10.Location = new System.Drawing.Point(224, 254);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label11.Location = new System.Drawing.Point(154, 254);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "-4.088";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 410);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bt_ActualizarBD);
            this.Controls.Add(this.bt_encender1);
            this.Controls.Add(this.bt_encender2);
            this.Controls.Add(this.tb_analogica2);
            this.Controls.Add(this.tb_analogica1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_proto);
            this.Controls.Add(this.tb_con);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pb_analogica2);
            this.Controls.Add(this.pb_analogica1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_pulsado1);
            this.Controls.Add(this.bt_pulsador2);
            this.Controls.Add(this.lb_mensajes);
            this.Name = "Form1";
            this.Text = "SCADA SHIM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lb_mensajes;
        private System.Windows.Forms.Button bt_pulsador2;
        private System.Windows.Forms.Button bt_pulsado1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pb_analogica1;
        private System.Windows.Forms.ProgressBar pb_analogica2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        //private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        //private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private System.Windows.Forms.TextBox tb_con;
        private System.Windows.Forms.TextBox tb_proto;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox tb_analogica1;
        private System.Windows.Forms.TextBox tb_analogica2;
        private System.Windows.Forms.Button bt_encender2;
        private System.Windows.Forms.Button bt_encender1;
        private System.Windows.Forms.Button bt_ActualizarBD;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
    }
}

