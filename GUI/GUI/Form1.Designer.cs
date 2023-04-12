namespace GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelBackground = new Panel();
            checkBox1 = new CheckBox();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label1 = new Label();
            timeLbl = new Label();
            memLabel = new Label();
            cpuLabel = new Label();
            errorLbl = new Label();
            progressBar1 = new ProgressBar();
            dragDropBox = new ListBox();
            passwdBox = new TextBox();
            panelBackground.SuspendLayout();
            SuspendLayout();
            // 
            // panelBackground
            // 
            panelBackground.BackgroundImage = Properties.Resources.a;
            panelBackground.Controls.Add(checkBox1);
            panelBackground.Controls.Add(radioButton2);
            panelBackground.Controls.Add(radioButton1);
            panelBackground.Controls.Add(label1);
            panelBackground.Controls.Add(timeLbl);
            panelBackground.Controls.Add(memLabel);
            panelBackground.Controls.Add(cpuLabel);
            panelBackground.Controls.Add(errorLbl);
            panelBackground.Controls.Add(progressBar1);
            panelBackground.Controls.Add(dragDropBox);
            panelBackground.Controls.Add(passwdBox);
            panelBackground.Dock = DockStyle.Fill;
            panelBackground.Location = new Point(0, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(657, 471);
            panelBackground.TabIndex = 0;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Segoe Print", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox1.Location = new Point(457, 430);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(160, 27);
            checkBox1.TabIndex = 11;
            checkBox1.Text = "Auto delete original";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Checked = true;
            radioButton2.Font = new Font("Segoe Print", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            radioButton2.Location = new Point(549, 399);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(86, 27);
            radioButton2.TabIndex = 10;
            radioButton2.TabStop = true;
            radioButton2.Text = "256 BIT";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Font = new Font("Segoe Print", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            radioButton1.Location = new Point(457, 399);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(86, 27);
            radioButton1.TabIndex = 9;
            radioButton1.Text = "128 BIT";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("SimSun", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(21, 26);
            label1.Name = "label1";
            label1.Size = new Size(118, 24);
            label1.TabIndex = 8;
            label1.Text = "Password:";
            // 
            // timeLbl
            // 
            timeLbl.AutoSize = true;
            timeLbl.Font = new Font("Segoe Print", 12F, FontStyle.Regular, GraphicsUnit.Point);
            timeLbl.Location = new Point(417, 299);
            timeLbl.Name = "timeLbl";
            timeLbl.Size = new Size(121, 28);
            timeLbl.TabIndex = 7;
            timeLbl.Text = "Time elapsed:";
            // 
            // memLabel
            // 
            memLabel.AutoSize = true;
            memLabel.Font = new Font("Segoe Print", 12F, FontStyle.Regular, GraphicsUnit.Point);
            memLabel.Location = new Point(417, 261);
            memLabel.Name = "memLabel";
            memLabel.Size = new Size(105, 28);
            memLabel.TabIndex = 6;
            memLabel.Text = "RAM usage:";
            // 
            // cpuLabel
            // 
            cpuLabel.AutoSize = true;
            cpuLabel.Font = new Font("Segoe Print", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cpuLabel.Location = new Point(417, 221);
            cpuLabel.Name = "cpuLabel";
            cpuLabel.Size = new Size(100, 28);
            cpuLabel.TabIndex = 5;
            cpuLabel.Text = "CPU usage:";
            // 
            // errorLbl
            // 
            errorLbl.AutoSize = true;
            errorLbl.BackColor = Color.Transparent;
            errorLbl.Font = new Font("Times New Roman", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            errorLbl.Location = new Point(21, 436);
            errorLbl.Name = "errorLbl";
            errorLbl.Size = new Size(165, 21);
            errorLbl.TabIndex = 4;
            errorLbl.Text = "                               ";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(21, 185);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(614, 20);
            progressBar1.TabIndex = 3;
            // 
            // dragDropBox
            // 
            dragDropBox.AllowDrop = true;
            dragDropBox.FormattingEnabled = true;
            dragDropBox.ItemHeight = 15;
            dragDropBox.Location = new Point(21, 55);
            dragDropBox.Name = "dragDropBox";
            dragDropBox.Size = new Size(614, 124);
            dragDropBox.TabIndex = 2;
            dragDropBox.DragDrop += dragDropBox_DragDrop_1;
            dragDropBox.DragEnter += dragDropBox_DragEnter_1;
            // 
            // passwdBox
            // 
            passwdBox.Location = new Point(145, 26);
            passwdBox.Name = "passwdBox";
            passwdBox.Size = new Size(490, 23);
            passwdBox.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(657, 471);
            Controls.Add(panelBackground);
            Name = "Form1";
            Text = "DND 2.2 Extended";
            panelBackground.ResumeLayout(false);
            panelBackground.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelBackground;
        private TextBox passwdBox;
        private ListBox dragDropBox;
        private ProgressBar progressBar1;
        private Label errorLbl;
        private Label timeLbl;
        private Label memLabel;
        private Label cpuLabel;
        private Label label1;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private CheckBox checkBox1;
    }
}