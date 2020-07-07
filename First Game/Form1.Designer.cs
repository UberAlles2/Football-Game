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
      ((System.ComponentModel.ISupportInitialize)(this.Player2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Player1)).BeginInit();
      this.SuspendLayout();
      // 
      // Player2
      // 
      this.Player2.Image = ((System.Drawing.Image)(resources.GetObject("Player2.Image")));
      this.Player2.Location = new System.Drawing.Point(212, 276);
      this.Player2.Name = "Player2";
      this.Player2.Size = new System.Drawing.Size(32, 32);
      this.Player2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.Player2.TabIndex = 1;
      this.Player2.TabStop = false;
      // 
      // Player1
      // 
      this.Player1.Image = ((System.Drawing.Image)(resources.GetObject("Player1.Image")));
      this.Player1.Location = new System.Drawing.Point(172, 129);
      this.Player1.Name = "Player1";
      this.Player1.Size = new System.Drawing.Size(32, 32);
      this.Player1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.Player1.TabIndex = 0;
      this.Player1.TabStop = false;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Green;
      this.ClientSize = new System.Drawing.Size(1112, 636);
      this.Controls.Add(this.Player2);
      this.Controls.Add(this.Player1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.Shown += new System.EventHandler(this.Form1_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.Player2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Player1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.PictureBox Player1;
    public System.Windows.Forms.PictureBox Player2;
  }
}

