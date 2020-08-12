using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballGame
{
  public partial class frmIntroduction : Form
  {
    public frmIntroduction()
    {
      InitializeComponent();
    }

    private void frmIntroduction_Load(object sender, EventArgs e)
    {
      richTextBox1.Text = "It is the beginning of the 4th quarter. You are losing by 3 points." + Environment.NewLine;
      richTextBox1.Text += "There are 15 minutes left in the game." + Environment.NewLine;
      richTextBox1.Text += "" + Environment.NewLine;
      richTextBox1.Text += "To run, use the arrow keys." + Environment.NewLine;
      richTextBox1.Text += "To pass, click the mouse where you want the ball to go." + Environment.NewLine;
      richTextBox1.Text += "To attempt field goal, click the mouse past the goal posts in the center." + Environment.NewLine;
    }

    private void btnStartGame_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
