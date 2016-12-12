namespace THC
{
    partial class HashsumCalculatorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_AddTemplate = new System.Windows.Forms.Button();
            this.btn_DeleteTemplate = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.gb_Templates = new System.Windows.Forms.GroupBox();
            this.gb_Hashsum = new System.Windows.Forms.GroupBox();
            this.lb_Templates = new System.Windows.Forms.ListBox();
            this.tb_Hashsum = new System.Windows.Forms.TextBox();
            this.btn_Calculate = new System.Windows.Forms.Button();
            this.gb_Templates.SuspendLayout();
            this.gb_Hashsum.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_AddTemplate
            // 
            this.btn_AddTemplate.Location = new System.Drawing.Point(208, 19);
            this.btn_AddTemplate.Name = "btn_AddTemplate";
            this.btn_AddTemplate.Size = new System.Drawing.Size(75, 23);
            this.btn_AddTemplate.TabIndex = 1;
            this.btn_AddTemplate.Text = "Add";
            this.btn_AddTemplate.UseVisualStyleBackColor = true;
            this.btn_AddTemplate.Click += new System.EventHandler(this.btn_AddTemplate_Click);
            // 
            // btn_DeleteTemplate
            // 
            this.btn_DeleteTemplate.Location = new System.Drawing.Point(208, 48);
            this.btn_DeleteTemplate.Name = "btn_DeleteTemplate";
            this.btn_DeleteTemplate.Size = new System.Drawing.Size(75, 23);
            this.btn_DeleteTemplate.TabIndex = 2;
            this.btn_DeleteTemplate.Text = "Delete";
            this.btn_DeleteTemplate.UseVisualStyleBackColor = true;
            this.btn_DeleteTemplate.Click += new System.EventHandler(this.btn_DeleteTemplate_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(208, 77);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(75, 23);
            this.btn_Clear.TabIndex = 3;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // gb_Templates
            // 
            this.gb_Templates.Controls.Add(this.lb_Templates);
            this.gb_Templates.Controls.Add(this.btn_AddTemplate);
            this.gb_Templates.Controls.Add(this.btn_Clear);
            this.gb_Templates.Controls.Add(this.btn_DeleteTemplate);
            this.gb_Templates.Location = new System.Drawing.Point(12, 12);
            this.gb_Templates.Name = "gb_Templates";
            this.gb_Templates.Size = new System.Drawing.Size(289, 174);
            this.gb_Templates.TabIndex = 4;
            this.gb_Templates.TabStop = false;
            this.gb_Templates.Text = "Templates";
            // 
            // gb_Hashsum
            // 
            this.gb_Hashsum.Controls.Add(this.tb_Hashsum);
            this.gb_Hashsum.Location = new System.Drawing.Point(12, 192);
            this.gb_Hashsum.Name = "gb_Hashsum";
            this.gb_Hashsum.Size = new System.Drawing.Size(208, 47);
            this.gb_Hashsum.TabIndex = 5;
            this.gb_Hashsum.TabStop = false;
            this.gb_Hashsum.Text = "Hashsum";
            // 
            // lb_Templates
            // 
            this.lb_Templates.FormattingEnabled = true;
            this.lb_Templates.Location = new System.Drawing.Point(6, 19);
            this.lb_Templates.Name = "lb_Templates";
            this.lb_Templates.Size = new System.Drawing.Size(196, 147);
            this.lb_Templates.TabIndex = 0;
            this.lb_Templates.SelectedIndexChanged += new System.EventHandler(this.lb_Templates_SelectedIndexChanged);
            // 
            // tb_Hashsum
            // 
            this.tb_Hashsum.Location = new System.Drawing.Point(6, 19);
            this.tb_Hashsum.Name = "tb_Hashsum";
            this.tb_Hashsum.ReadOnly = true;
            this.tb_Hashsum.Size = new System.Drawing.Size(196, 20);
            this.tb_Hashsum.TabIndex = 0;
            // 
            // btn_Calculate
            // 
            this.btn_Calculate.Location = new System.Drawing.Point(226, 209);
            this.btn_Calculate.Name = "btn_Calculate";
            this.btn_Calculate.Size = new System.Drawing.Size(75, 23);
            this.btn_Calculate.TabIndex = 6;
            this.btn_Calculate.Text = "Calculate";
            this.btn_Calculate.UseVisualStyleBackColor = true;
            this.btn_Calculate.Click += new System.EventHandler(this.btn_Calculate_Click);
            // 
            // HashsumCalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 249);
            this.Controls.Add(this.btn_Calculate);
            this.Controls.Add(this.gb_Hashsum);
            this.Controls.Add(this.gb_Templates);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "HashsumCalculatorForm";
            this.Text = "HashsumCalculator";
            this.gb_Templates.ResumeLayout(false);
            this.gb_Hashsum.ResumeLayout(false);
            this.gb_Hashsum.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_AddTemplate;
        private System.Windows.Forms.Button btn_DeleteTemplate;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.GroupBox gb_Templates;
        private System.Windows.Forms.ListBox lb_Templates;
        private System.Windows.Forms.GroupBox gb_Hashsum;
        private System.Windows.Forms.TextBox tb_Hashsum;
        private System.Windows.Forms.Button btn_Calculate;
    }
}