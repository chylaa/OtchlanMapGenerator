
namespace OtchlanMapGenerator
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.confirmButton = new System.Windows.Forms.Button();
            this.addPanel = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_set_w = new System.Windows.Forms.Button();
            this.button_set_s = new System.Windows.Forms.Button();
            this.button_set_e = new System.Windows.Forms.Button();
            this.button_set_n = new System.Windows.Forms.Button();
            this.textboxName = new System.Windows.Forms.TextBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.addPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(32, 254);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(105, 52);
            this.confirmButton.TabIndex = 0;
            this.confirmButton.Text = "confirm";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // addPanel
            // 
            this.addPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addPanel.Controls.Add(this.label1);
            this.addPanel.Controls.Add(this.button_set_w);
            this.addPanel.Controls.Add(this.button_set_s);
            this.addPanel.Controls.Add(this.button_set_e);
            this.addPanel.Controls.Add(this.button_set_n);
            this.addPanel.Controls.Add(this.textboxName);
            this.addPanel.Controls.Add(this.confirmButton);
            this.addPanel.Location = new System.Drawing.Point(359, 1);
            this.addPanel.Name = "addPanel";
            this.addPanel.Size = new System.Drawing.Size(184, 366);
            this.addPanel.TabIndex = 1;
            this.addPanel.TabStop = false;
            this.addPanel.Text = "Add Panel";
            this.addPanel.Enter += new System.EventHandler(this.addPanel_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = " Exits";
            // 
            // button_set_w
            // 
            this.button_set_w.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_set_w.Location = new System.Drawing.Point(19, 120);
            this.button_set_w.Name = "button_set_w";
            this.button_set_w.Size = new System.Drawing.Size(40, 40);
            this.button_set_w.TabIndex = 5;
            this.button_set_w.Text = "w";
            this.button_set_w.UseVisualStyleBackColor = true;
            // 
            // button_set_s
            // 
            this.button_set_s.Location = new System.Drawing.Point(65, 168);
            this.button_set_s.Name = "button_set_s";
            this.button_set_s.Size = new System.Drawing.Size(40, 40);
            this.button_set_s.TabIndex = 4;
            this.button_set_s.Text = "s";
            this.button_set_s.UseVisualStyleBackColor = true;
            // 
            // button_set_e
            // 
            this.button_set_e.Location = new System.Drawing.Point(110, 120);
            this.button_set_e.Name = "button_set_e";
            this.button_set_e.Size = new System.Drawing.Size(40, 40);
            this.button_set_e.TabIndex = 3;
            this.button_set_e.Text = "e";
            this.button_set_e.UseVisualStyleBackColor = true;
            // 
            // button_set_n
            // 
            this.button_set_n.Location = new System.Drawing.Point(65, 75);
            this.button_set_n.Name = "button_set_n";
            this.button_set_n.Size = new System.Drawing.Size(40, 40);
            this.button_set_n.TabIndex = 2;
            this.button_set_n.Text = "n";
            this.button_set_n.UseVisualStyleBackColor = true;
            // 
            // textboxName
            // 
            this.textboxName.Location = new System.Drawing.Point(6, 30);
            this.textboxName.Name = "textboxName";
            this.textboxName.Size = new System.Drawing.Size(166, 20);
            this.textboxName.TabIndex = 1;
            this.textboxName.Text = "Enter location name";
            this.textboxName.TextChanged += new System.EventHandler(this.textboxName_TextChanged);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.Location = new System.Drawing.Point(543, 1);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 366);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(0, 371);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(544, 17);
            this.hScrollBar1.TabIndex = 3;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(557, 388);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.addPanel);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
            this.addPanel.ResumeLayout(false);
            this.addPanel.PerformLayout();
            this.ResumeLayout(false);



        }


        #endregion

        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.GroupBox addPanel;
        private System.Windows.Forms.TextBox textboxName;
        private System.Windows.Forms.Button button_set_w;
        private System.Windows.Forms.Button button_set_s;
        private System.Windows.Forms.Button button_set_e;
        private System.Windows.Forms.Button button_set_n;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
    }
}

