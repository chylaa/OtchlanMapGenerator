﻿
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
            this.components = new System.ComponentModel.Container();
            this.correctButton = new System.Windows.Forms.Button();
            this.segmentPanel = new System.Windows.Forms.GroupBox();
            this.routeTextBox = new System.Windows.Forms.TextBox();
            this.languageGroupBox = new System.Windows.Forms.GroupBox();
            this.PLradioButton1 = new System.Windows.Forms.RadioButton();
            this.ENradioButton = new System.Windows.Forms.RadioButton();
            this.infoLabel = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.exitsLabel = new System.Windows.Forms.Label();
            this.button_set_w = new System.Windows.Forms.Button();
            this.button_set_s = new System.Windows.Forms.Button();
            this.button_set_e = new System.Windows.Forms.Button();
            this.button_set_n = new System.Windows.Forms.Button();
            this.textboxName = new System.Windows.Forms.TextBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.keyInputCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.segmentPanel.SuspendLayout();
            this.languageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // correctButton
            // 
            this.correctButton.Location = new System.Drawing.Point(39, 250);
            this.correctButton.Name = "correctButton";
            this.correctButton.Size = new System.Drawing.Size(105, 52);
            this.correctButton.TabIndex = 0;
            this.correctButton.Text = "Correct";
            this.correctButton.UseVisualStyleBackColor = true;
            this.correctButton.Click += new System.EventHandler(this.correctButton_Click);
            // 
            // segmentPanel
            // 
            this.segmentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.segmentPanel.BackColor = System.Drawing.Color.RosyBrown;
            this.segmentPanel.Controls.Add(this.routeTextBox);
            this.segmentPanel.Controls.Add(this.languageGroupBox);
            this.segmentPanel.Controls.Add(this.infoLabel);
            this.segmentPanel.Controls.Add(this.deleteButton);
            this.segmentPanel.Controls.Add(this.exitsLabel);
            this.segmentPanel.Controls.Add(this.button_set_w);
            this.segmentPanel.Controls.Add(this.button_set_s);
            this.segmentPanel.Controls.Add(this.button_set_e);
            this.segmentPanel.Controls.Add(this.button_set_n);
            this.segmentPanel.Controls.Add(this.textboxName);
            this.segmentPanel.Controls.Add(this.correctButton);
            this.segmentPanel.Location = new System.Drawing.Point(519, 1);
            this.segmentPanel.Name = "segmentPanel";
            this.segmentPanel.Size = new System.Drawing.Size(201, 491);
            this.segmentPanel.TabIndex = 1;
            this.segmentPanel.TabStop = false;
            this.segmentPanel.Text = "Segment Panel";
            // 
            // routeTextBox
            // 
            this.routeTextBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.routeTextBox.Location = new System.Drawing.Point(12, 385);
            this.routeTextBox.Name = "routeTextBox";
            this.routeTextBox.Size = new System.Drawing.Size(166, 20);
            this.routeTextBox.TabIndex = 10;
            this.routeTextBox.Visible = false;
            // 
            // languageGroupBox
            // 
            this.languageGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.languageGroupBox.Controls.Add(this.PLradioButton1);
            this.languageGroupBox.Controls.Add(this.ENradioButton);
            this.languageGroupBox.Location = new System.Drawing.Point(3, 442);
            this.languageGroupBox.Name = "languageGroupBox";
            this.languageGroupBox.Size = new System.Drawing.Size(184, 49);
            this.languageGroupBox.TabIndex = 9;
            this.languageGroupBox.TabStop = false;
            this.languageGroupBox.Text = "Select language";
            // 
            // PLradioButton1
            // 
            this.PLradioButton1.AutoSize = true;
            this.PLradioButton1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PLradioButton1.Location = new System.Drawing.Point(102, 20);
            this.PLradioButton1.Name = "PLradioButton1";
            this.PLradioButton1.Size = new System.Drawing.Size(38, 17);
            this.PLradioButton1.TabIndex = 1;
            this.PLradioButton1.TabStop = true;
            this.PLradioButton1.Text = "PL";
            this.PLradioButton1.UseVisualStyleBackColor = true;
            this.PLradioButton1.CheckedChanged += new System.EventHandler(this.PLradioButton1_CheckedChanged);
            // 
            // ENradioButton
            // 
            this.ENradioButton.AutoSize = true;
            this.ENradioButton.Checked = true;
            this.ENradioButton.Location = new System.Drawing.Point(36, 20);
            this.ENradioButton.Name = "ENradioButton";
            this.ENradioButton.Size = new System.Drawing.Size(40, 17);
            this.ENradioButton.TabIndex = 0;
            this.ENradioButton.TabStop = true;
            this.ENradioButton.Text = "EN";
            this.ENradioButton.UseVisualStyleBackColor = true;
            this.ENradioButton.CheckedChanged += new System.EventHandler(this.ENradioButton_CheckedChanged);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(6, 224);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(70, 13);
            this.infoLabel.TabIndex = 8;
            this.infoLabel.Text = "Segment Info";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(38, 316);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(105, 54);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.TabStop = false;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // exitsLabel
            // 
            this.exitsLabel.AutoSize = true;
            this.exitsLabel.Location = new System.Drawing.Point(72, 138);
            this.exitsLabel.Name = "exitsLabel";
            this.exitsLabel.Size = new System.Drawing.Size(32, 13);
            this.exitsLabel.TabIndex = 6;
            this.exitsLabel.Text = " Exits";
            // 
            // button_set_w
            // 
            this.button_set_w.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_set_w.Location = new System.Drawing.Point(26, 124);
            this.button_set_w.Name = "button_set_w";
            this.button_set_w.Size = new System.Drawing.Size(40, 40);
            this.button_set_w.TabIndex = 5;
            this.button_set_w.Text = "w";
            this.button_set_w.UseVisualStyleBackColor = true;
            // 
            // button_set_s
            // 
            this.button_set_s.Location = new System.Drawing.Point(72, 172);
            this.button_set_s.Name = "button_set_s";
            this.button_set_s.Size = new System.Drawing.Size(40, 40);
            this.button_set_s.TabIndex = 4;
            this.button_set_s.Text = "s";
            this.button_set_s.UseVisualStyleBackColor = true;
            // 
            // button_set_e
            // 
            this.button_set_e.Location = new System.Drawing.Point(117, 124);
            this.button_set_e.Name = "button_set_e";
            this.button_set_e.Size = new System.Drawing.Size(40, 40);
            this.button_set_e.TabIndex = 3;
            this.button_set_e.Text = "e";
            this.button_set_e.UseVisualStyleBackColor = true;
            // 
            // button_set_n
            // 
            this.button_set_n.Location = new System.Drawing.Point(72, 79);
            this.button_set_n.Name = "button_set_n";
            this.button_set_n.Size = new System.Drawing.Size(40, 40);
            this.button_set_n.TabIndex = 2;
            this.button_set_n.Text = "n";
            this.button_set_n.UseVisualStyleBackColor = true;
            // 
            // textboxName
            // 
            this.textboxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.3F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textboxName.Location = new System.Drawing.Point(12, 29);
            this.textboxName.Name = "textboxName";
            this.textboxName.ReadOnly = true;
            this.textboxName.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textboxName.Size = new System.Drawing.Size(166, 20);
            this.textboxName.TabIndex = 1;
            this.textboxName.Text = "Location name";
            this.textboxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textboxName.TextChanged += new System.EventHandler(this.textboxName_TextChanged);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.LargeChange = 25;
            this.vScrollBar1.Location = new System.Drawing.Point(703, 1);
            this.vScrollBar1.Maximum = 1000;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 482);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.LargeChange = 25;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 487);
            this.hScrollBar1.Maximum = 1000;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(717, 17);
            this.hScrollBar1.TabIndex = 3;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 30;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // keyInputCheckBox
            // 
            this.keyInputCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.keyInputCheckBox.AutoSize = true;
            this.keyInputCheckBox.Checked = true;
            this.keyInputCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keyInputCheckBox.Location = new System.Drawing.Point(12, 455);
            this.keyInputCheckBox.Name = "keyInputCheckBox";
            this.keyInputCheckBox.Size = new System.Drawing.Size(107, 17);
            this.keyInputCheckBox.TabIndex = 4;
            this.keyInputCheckBox.Text = "Disable key input";
            this.keyInputCheckBox.UseVisualStyleBackColor = true;
            this.keyInputCheckBox.CheckedChanged += new System.EventHandler(this.keyInputCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 221);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.Brown;
            this.ClientSize = new System.Drawing.Size(717, 504);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.keyInputCheckBox);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.segmentPanel);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Otchłań Map Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDoubleClick);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseWheel);
            this.segmentPanel.ResumeLayout(false);
            this.segmentPanel.PerformLayout();
            this.languageGroupBox.ResumeLayout(false);
            this.languageGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Button correctButton;
        private System.Windows.Forms.GroupBox segmentPanel;
        private System.Windows.Forms.TextBox textboxName;
        private System.Windows.Forms.Button button_set_w;
        private System.Windows.Forms.Button button_set_s;
        private System.Windows.Forms.Button button_set_e;
        private System.Windows.Forms.Button button_set_n;
        private System.Windows.Forms.Label exitsLabel;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.GroupBox languageGroupBox;
        private System.Windows.Forms.RadioButton PLradioButton1;
        private System.Windows.Forms.RadioButton ENradioButton;
        private System.Windows.Forms.TextBox routeTextBox;
        private System.Windows.Forms.CheckBox keyInputCheckBox;
        private System.Windows.Forms.Label label1;
    }
}

