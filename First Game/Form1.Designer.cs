namespace FootballGame
{
  partial class Form1
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.Player2 = new System.Windows.Forms.PictureBox();
      this.Player1 = new System.Windows.Forms.PictureBox();
      this.picFootball = new System.Windows.Forms.PictureBox();
      this.picBearsBall = new System.Windows.Forms.PictureBox();
      this.pnlPlayOptions = new System.Windows.Forms.Panel();
      this.btnOK = new System.Windows.Forms.Button();
      this.pnlTopReceiver = new System.Windows.Forms.Panel();
      this.radioButton3 = new System.Windows.Forms.RadioButton();
      this.radioButton2 = new System.Windows.Forms.RadioButton();
      this.radioButton1 = new System.Windows.Forms.RadioButton();
      this.label1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.Player2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Player1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFootball)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picBearsBall)).BeginInit();
      this.pnlPlayOptions.SuspendLayout();
      this.pnlTopReceiver.SuspendLayout();
      this.SuspendLayout();
      // 
      // Player2
      // 
      this.Player2.Image = ((System.Drawing.Image)(resources.GetObject("Player2.Image")));
      this.Player2.Location = new System.Drawing.Point(-8, 314);
      this.Player2.Name = "Player2";
      this.Player2.Size = new System.Drawing.Size(32, 32);
      this.Player2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.Player2.TabIndex = 1;
      this.Player2.TabStop = false;
      this.Player2.Visible = false;
      // 
      // Player1
      // 
      this.Player1.Image = ((System.Drawing.Image)(resources.GetObject("Player1.Image")));
      this.Player1.Location = new System.Drawing.Point(0, 208);
      this.Player1.Name = "Player1";
      this.Player1.Size = new System.Drawing.Size(32, 32);
      this.Player1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.Player1.TabIndex = 0;
      this.Player1.TabStop = false;
      this.Player1.Visible = false;
      // 
      // picFootball
      // 
      this.picFootball.Image = ((System.Drawing.Image)(resources.GetObject("picFootball.Image")));
      this.picFootball.Location = new System.Drawing.Point(0, 284);
      this.picFootball.Name = "picFootball";
      this.picFootball.Size = new System.Drawing.Size(24, 24);
      this.picFootball.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picFootball.TabIndex = 2;
      this.picFootball.TabStop = false;
      // 
      // picBearsBall
      // 
      this.picBearsBall.Image = ((System.Drawing.Image)(resources.GetObject("picBearsBall.Image")));
      this.picBearsBall.Location = new System.Drawing.Point(-8, 246);
      this.picBearsBall.Name = "picBearsBall";
      this.picBearsBall.Size = new System.Drawing.Size(32, 32);
      this.picBearsBall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picBearsBall.TabIndex = 3;
      this.picBearsBall.TabStop = false;
      this.picBearsBall.Visible = false;
      // 
      // pnlPlayOptions
      // 
      this.pnlPlayOptions.BackColor = System.Drawing.Color.Gainsboro;
      this.pnlPlayOptions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pnlPlayOptions.Controls.Add(this.btnOK);
      this.pnlPlayOptions.Controls.Add(this.pnlTopReceiver);
      this.pnlPlayOptions.Location = new System.Drawing.Point(252, 26);
      this.pnlPlayOptions.Name = "pnlPlayOptions";
      this.pnlPlayOptions.Size = new System.Drawing.Size(742, 434);
      this.pnlPlayOptions.TabIndex = 4;
      // 
      // btnOK
      // 
      this.btnOK.BackColor = System.Drawing.Color.Silver;
      this.btnOK.Location = new System.Drawing.Point(631, 373);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(86, 44);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = false;
      // 
      // pnlTopReceiver
      // 
      this.pnlTopReceiver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pnlTopReceiver.Controls.Add(this.radioButton3);
      this.pnlTopReceiver.Controls.Add(this.radioButton2);
      this.pnlTopReceiver.Controls.Add(this.radioButton1);
      this.pnlTopReceiver.Controls.Add(this.label1);
      this.pnlTopReceiver.Location = new System.Drawing.Point(25, 159);
      this.pnlTopReceiver.Name = "pnlTopReceiver";
      this.pnlTopReceiver.Size = new System.Drawing.Size(200, 121);
      this.pnlTopReceiver.TabIndex = 0;
      // 
      // radioButton3
      // 
      this.radioButton3.AutoSize = true;
      this.radioButton3.Location = new System.Drawing.Point(7, 51);
      this.radioButton3.Name = "radioButton3";
      this.radioButton3.Size = new System.Drawing.Size(112, 17);
      this.radioButton3.TabIndex = 3;
      this.radioButton3.TabStop = true;
      this.radioButton3.Text = "Long Button Hook";
      this.radioButton3.UseVisualStyleBackColor = true;
      // 
      // radioButton2
      // 
      this.radioButton2.AutoSize = true;
      this.radioButton2.Location = new System.Drawing.Point(7, 30);
      this.radioButton2.Name = "radioButton2";
      this.radioButton2.Size = new System.Drawing.Size(113, 17);
      this.radioButton2.TabIndex = 2;
      this.radioButton2.TabStop = true;
      this.radioButton2.Text = "Short Button Hook";
      this.radioButton2.UseVisualStyleBackColor = true;
      // 
      // radioButton1
      // 
      this.radioButton1.AutoSize = true;
      this.radioButton1.Location = new System.Drawing.Point(7, 72);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new System.Drawing.Size(38, 17);
      this.radioButton1.TabIndex = 1;
      this.radioButton1.TabStop = true;
      this.radioButton1.Text = "Fly";
      this.radioButton1.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(4, 4);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(137, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Top Wide Receiver Pattern";
      // 
      // Form1
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Green;
      this.ClientSize = new System.Drawing.Size(1242, 636);
      this.Controls.Add(this.pnlPlayOptions);
      this.Controls.Add(this.picBearsBall);
      this.Controls.Add(this.picFootball);
      this.Controls.Add(this.Player2);
      this.Controls.Add(this.Player1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.Shown += new System.EventHandler(this.Form1_Shown);
      this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
      ((System.ComponentModel.ISupportInitialize)(this.Player2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Player1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFootball)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picBearsBall)).EndInit();
      this.pnlPlayOptions.ResumeLayout(false);
      this.pnlTopReceiver.ResumeLayout(false);
      this.pnlTopReceiver.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.PictureBox Player1;
    public System.Windows.Forms.PictureBox Player2;
    public System.Windows.Forms.PictureBox picFootball;
    public System.Windows.Forms.PictureBox picBearsBall;
    private System.Windows.Forms.Panel pnlTopReceiver;
    private System.Windows.Forms.RadioButton radioButton1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.RadioButton radioButton3;
    private System.Windows.Forms.RadioButton radioButton2;
    public System.Windows.Forms.Panel pnlPlayOptions;
  }
}

