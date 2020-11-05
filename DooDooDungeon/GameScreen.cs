﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DooDooDungeon
{
    public partial class GameScreen : UserControl
    {
        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        Boolean leftKeyDown, rightKeyDown, downKeyDown, upKeyDown, wKeyDown, aKeyDown, sKeyDown, dKeyDown;
        Boolean turnCounter = true;

        int rollStartX = 20;
        int rollStartY = 20;

        int doodooStartX = 540;
        int doodooStartY = 440;

        int rollSize = 40;
        int doodooSize = 40;

        int moveCounter = 0;

        int x = 0;
        int y = 0;
        int Width = 0;
        int Height = 0;

        int grateX;
        int grateY;
        int grateWidth;
        int grateHeight;

        int levelNumber = 1;

        List<Wall> wallList = new List<Wall>();

        //used to draw walls on screen
        SolidBrush wallBrush = new SolidBrush(Color.Black);
        int tickCounter = 0;
        Roll roll;
        DooDoo doodoo;
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            TurnChecking();
            CollisionCheck();
            HitBoxCreation();
            Refresh();
        }

        public void CollisionCheck()
        {
            if (doodoo.Collision(roll))
            {
                gameTimer.Enabled = false;

                Form f = this.FindForm();
                GameOverScreen gos = new GameOverScreen();

                f.Controls.Remove(this);
                f.Controls.Add(gos);

                gos.Focus();
            }
        }

        public void SwitchMove()
        {
            doodoo.Move();
            moveCounter++;
            if (moveCounter == 3)
            {
                moveCounter = 0;
                turnCounter = true;
            }
        }

        public void OnStart()
        {
            roll = new Roll(rollStartX, rollStartY, rollSize, "None");
            doodoo = new DooDoo(doodooStartX, doodooStartY, doodooSize, "None");

            HitBoxCreation();
            LevelReading();            
        }
        #region Key Declaration

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftKeyDown = false;
                    break;
                case Keys.Right:
                    rightKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.D:
                    dKeyDown = false;
                    break;
            }
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftKeyDown = true;
                    break;
                case Keys.Right:
                    rightKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.D:
                    dKeyDown = true;
                    break;
            }
        }
        #endregion

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
             e.Graphics.DrawImage(Properties.Resources.New_Piskel__2_, roll.x, roll.y, roll.size, roll.size);
             e.Graphics.DrawImage(Properties.Resources.Waste_Warroir, doodoo.x, doodoo.y, doodoo.size, doodoo.size);
             e.Graphics.DrawImage(Properties.Resources.escapeGrate, grateX, grateY, grateWidth, grateHeight);

            //draw walls to screen
            foreach (Wall w in wallList)
            {
                e.Graphics.FillRectangle(wallBrush, w.x, w.y, w.Width, w.Height);
            }
        }

        public void LevelReading()
        {
            XmlReader reader = XmlReader.Create("Resources/Level" + Convert.ToString(levelNumber) + ".xml", null);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    int x = Convert.ToInt16(reader.ReadString());

                    reader.ReadToFollowing("y");
                    int y = Convert.ToInt16(reader.ReadString());

                    reader.ReadToFollowing("Width");
                    int Width = Convert.ToInt16(reader.ReadString());

                    reader.ReadToFollowing("Height");
                    int Height = Convert.ToInt16(reader.ReadString());

                    Wall w = new Wall(x, y, Width, Height);

                    wallList.Add(w);
                }
            }
            reader.Close();

            if (levelNumber == 1)
            {
                grateX = 537;
                grateY = 8;
                grateWidth = 52;
                grateHeight = 46;

                roll.x = 162;
                roll.y = 199;

                doodoo.x = 8;
                doodoo.y = 451;
            }
            else if (levelNumber == 2)
            {
                grateX = 237;
                grateY = 450;
                grateWidth = 52;
                grateHeight = 46;

                roll.x = 11;
                roll.y = 455;

                doodoo.x = 87;
                doodoo.y = 134;
            }
            else if (levelNumber == 3)
            {
                grateX = 10;
                grateY = 447;
                grateWidth = 52;
                grateHeight = 46;

                roll.x = 245;
                roll.y = 450;

                doodoo.x = 12;
                doodoo.y = 10;
            }
        }

        public void TurnChecking()
        {
            if (turnCounter && moveCounter == 0)
            {
                if (wKeyDown && roll.y - 62 >= 0)
                {
                    roll.direction = "Up";
                    roll.Move();
                    moveCounter++;
                    turnCounter = false;
                }
                else if (aKeyDown && roll.x - 75 >= 0)
                {
                    roll.direction = "Left";
                    roll.Move();
                    moveCounter++;
                    turnCounter = false;
                }
                else if (sKeyDown && roll.y + 102 <= 500)
                {
                    roll.direction = "Down";
                    roll.Move();
                    moveCounter++;
                    turnCounter = false;
                }
                else if (dKeyDown && roll.x + 122 <= 600)
                {
                    roll.direction = "Right";
                    roll.Move();
                    moveCounter++;
                    turnCounter = false;
                }

                x = roll.x;
                y = roll.y;

            }
            if (turnCounter == false && moveCounter > 0)
            {
                if (upKeyDown && doodoo.y - 62 >= 0)
                {
                    doodoo.direction = "Up";
                    SwitchMove();
                }
                else if (leftKeyDown && doodoo.x - 75 >= 0)
                {
                    doodoo.direction = "Left";
                    SwitchMove();
                }
                else if (downKeyDown && doodoo.y + 122 <= 500)
                {
                    doodoo.direction = "Down";
                    SwitchMove();

                }
                else if (rightKeyDown && doodoo.x + 122 <= 600)
                {
                    doodoo.direction = "Right";
                    SwitchMove();
                }
            }
        }

        public void HitBoxCreation()
        {


            Rectangle grateRec = new Rectangle(grateX, grateY, grateWidth, grateHeight);

            Rectangle rollRec = new Rectangle(roll.x, roll.y, roll.size, roll.size);          

            if(rollRec.IntersectsWith(grateRec))
            {
                wallList.Clear();
                levelNumber++;

                if(levelNumber == 4)
                {
                    //Change to Win Screen
                    Form f = this.FindForm();
                    GameOverScreen gos = new GameOverScreen();
                }
                else 
                {
                 LevelReading();
                }
                
            }

            foreach (Wall w in wallList)
            {
               Rectangle wallRec = new Rectangle(w.x, w.y, w.Width, w.Height);
                if (rollRec.IntersectsWith(wallRec))
                {
                    moveCounter = 0;
                    roll.x = x;
                    roll.y = y;
                }
            }
        }
    }
}





