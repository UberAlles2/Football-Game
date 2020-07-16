namespace FootballGame
{
  partial class PlayOptionsForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayOptionsForm));
      this.picReceiverTop = new System.Windows.Forms.PictureBox();
      this.picPostPattern = new System.Windows.Forms.PictureBox();
      this.picFlyPattern = new System.Windows.Forms.PictureBox();
      this.picButtonHookPattern = new System.Windows.Forms.PictureBox();
      this.btnStartPlay = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.picReceiverTop)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPostPattern)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFlyPattern)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picButtonHookPattern)).BeginInit();
      this.SuspendLayout();
      // 
      // picReceiverTop
      // 
      this.picReceiverTop.Location = new System.Drawing.Point(55, 47);
      this.picReceiverTop.Name = "picReceiverTop";
      this.picReceiverTop.Size = new System.Drawing.Size(158, 157);
      this.picReceiverTop.TabIndex = 10;
      this.picReceiverTop.TabStop = false;
      // 
      // picPostPattern
      // 
      this.picPostPattern.Image = ((System.Drawing.Image)(resources.GetObject("picPostPattern.Image")));
      this.picPostPattern.Location = new System.Drawing.Point(316, 238);
      this.picPostPattern.Name = "picPostPattern";
      this.picPostPattern.Size = new System.Drawing.Size(100, 100);
      this.picPostPattern.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picPostPattern.TabIndex = 9;
      this.picPostPattern.TabStop = false;
      // 
      // picFlyPattern
      // 
      this.picFlyPattern.Image = ((System.Drawing.Image)(resources.GetObject("picFlyPattern.Image")));
      this.picFlyPattern.Location = new System.Drawing.Point(317, 132);
      this.picFlyPattern.Name = "picFlyPattern";
      this.picFlyPattern.Size = new System.Drawing.Size(100, 100);
      this.picFlyPattern.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picFlyPattern.TabIndex = 8;
      this.picFlyPattern.TabStop = false;
      // 
      // picButtonHookPattern
      // 
      this.picButtonHookPattern.Image = ((System.Drawing.Image)(resources.GetObject("picButtonHookPattern.Image")));
      this.picButtonHookPattern.Location = new System.Drawing.Point(316, 27);
      this.picButtonHookPattern.Name = "picButtonHookPattern";
      this.picButtonHookPattern.Size = new System.Drawing.Size(100, 100);
      this.picButtonHookPattern.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picButtonHookPattern.TabIndex = 7;
      this.picButtonHookPattern.TabStop = false;
      // 
      // btnStartPlay
      // 
      this.btnStartPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnStartPlay.Location = new System.Drawing.Point(683, 384);
      this.btnStartPlay.Name = "btnStartPlay";
      this.btnStartPlay.Size = new System.Drawing.Size(95, 54);
      this.btnStartPlay.TabIndex = 11;
      this.btnStartPlay.Text = "Start Play";
      this.btnStartPlay.UseVisualStyleBackColor = true;
      this.btnStartPlay.Click += new System.EventHandler(this.btnStartPlay_Click);
      // 
      // PlayOptionsForm
      // 
      this.AcceptButton = this.btnStartPlay;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.btnStartPlay);
      this.Controls.Add(this.picReceiverTop);
      this.Controls.Add(this.picPostPattern);
      this.Controls.Add(this.picFlyPattern);
      this.Controls.Add(this.picButtonHookPattern);
      this.Name = "PlayOptionsForm";
      this.Text = "Play Options";
      ((System.ComponentModel.ISupportInitialize)(this.picReceiverTop)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPostPattern)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFlyPattern)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picButtonHookPattern)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox picReceiverTop;
    public System.Windows.Forms.PictureBox picPostPattern;
    public System.Windows.Forms.PictureBox picFlyPattern;
    public System.Windows.Forms.PictureBox picButtonHookPattern;
    private System.Windows.Forms.Button btnStartPlay;
  }
}