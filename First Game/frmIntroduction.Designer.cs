namespace FootballGame
{
  partial class frmIntroduction
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
      this.btnStartGame = new System.Windows.Forms.Button();
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // btnStartGame
      // 
      this.btnStartGame.Location = new System.Drawing.Point(151, 391);
      this.btnStartGame.Name = "btnStartGame";
      this.btnStartGame.Size = new System.Drawing.Size(112, 34);
      this.btnStartGame.TabIndex = 0;
      this.btnStartGame.Text = "Start Game";
      this.btnStartGame.UseVisualStyleBackColor = true;
      this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
      // 
      // richTextBox1
      // 
      this.richTextBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
      this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.richTextBox1.Location = new System.Drawing.Point(23, 23);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.Size = new System.Drawing.Size(389, 133);
      this.richTextBox1.TabIndex = 1;
      this.richTextBox1.Text = "intro text";
      // 
      // frmIntroduction
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(424, 450);
      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.btnStartGame);
      this.Name = "frmIntroduction";
      this.Text = "Introduction";
      this.Load += new System.EventHandler(this.frmIntroduction_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnStartGame;
    private System.Windows.Forms.RichTextBox richTextBox1;
  }
}