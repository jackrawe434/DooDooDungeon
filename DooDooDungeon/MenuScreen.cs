﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DooDooDungeon
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void playButton_Enter(object sender, EventArgs e)
        {
            playButton.BackColor = Color.Brown;
            instructionsButton.BackColor = Color.LightGray;
            exitButton.BackColor = Color.LightGray;
        }

        private void instructionsButton_Enter(object sender, EventArgs e)
        {
            playButton.BackColor = Color.LightGray;
            instructionsButton.BackColor = Color.Brown;
            exitButton.BackColor = Color.LightGray;
        }

        private void exitButton_Enter(object sender, EventArgs e)
        {
            playButton.BackColor = Color.LightGray;
            instructionsButton.BackColor = Color.LightGray;
            exitButton.BackColor = Color.Brown;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            GameScreen gs = new GameScreen();

            f.Controls.Remove(this);
            f.Controls.Add(gs);
        }

        private void instructionsButton_Click(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            InstructionsScreen ins = new InstructionsScreen();

            f.Controls.Remove(this);
            f.Controls.Add(ins);

            ins.Focus();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}