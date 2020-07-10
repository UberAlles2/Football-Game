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
      ((System.ComponentModel.ISupportInitialize)(this.Player2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Player1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFootball)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picBearsBall)).BeginInit();
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
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Green;
      this.ClientSize = new System.Drawing.Size(1242, 636);
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
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.PictureBox Player1;
    public System.Windows.Forms.PictureBox Player2;
    public System.Windows.Forms.PictureBox picFootball;
    public System.Windows.Forms.PictureBox picBearsBall;
  }
}

