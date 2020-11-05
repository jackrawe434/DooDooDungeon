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
        Boolean doodooTopCollision = false;
        Boolean doodooBottomCollision = false;
        Boolean doodooRightCollision = false;
        Boolean doodooLeftCollision = false;
        Boolean rollTopCollision = false;
        Boolean rollBottomCollision = false;
        Boolean rollRightCollision = false;
        Boolean rollLeftCollision = false;

        int rollStartX = 20;
        int rollStartY = 20;

        int doodooStartX = 470;
        int doodooStartY = 447;

        int rollSize = 40;
        int doodooSize = 40;

        int moveCounter = 0;

        int x;
        int y;
        int Width = 0;
        int Height = 0;

        int grateX;
        int grateY;
        int grateWidth;
        int grateHeight;

        int levelNumber = 3;

        List<Wall> wallList = new List<Wall>();
        List<Rectangle> wallRecList = new List<Rectangle>();

        //used to draw walls on screen
        SolidBrush wallBrush = new SolidBrush(Color.Black);
        SolidBrush redBrush = new SolidBrush(Color.Red);

        Rectangle rightDooDooRec;
        Rectangle leftDooDooRec;
        Rectangle bottomDooDooRec;
        Rectangle topDooDooRec;
        Rectangle rightRollRec;
        Rectangle leftRollRec;
        Rectangle bottomRollRec;
        Rectangle topRollRec;

        int tickCounter = 0;

        Roll roll;
        DooDoo doodoo;
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            x = roll.x;
            y = roll.y;
            HitBoxCreation();
            DooDooWallHitBox();
            RollWallHitBox();

            if (turnCounter && moveCounter == 0)
            {
                if (wKeyDown && roll.y - 62 >= 0 && rollTopCollision == false)
                {
                    roll.direction = "Up";
                    roll.Move();
                    moveCounter++;
                    turnCounter = false;
                }
                else if (aKeyDown && roll.x - 75 >= 0 && rollLeftCollision == false)
                {
                    roll.direction = "Left";
                    roll.Move();
                    moveCounter++;
                    turnCounter = false;
                }
                else if (sKeyDown && roll.y + 102 <= 500 && rollBottomCollision == false)
                {
                    roll.direction = "Down";
                    roll.Move();
                    moveCounter++;
                    turnCounter = false;
                }
                else if (dKeyDown && roll.x + 122 <= 600 && rollRightCollision == false)
                {
                    roll.direction = "Right";
                    roll.Move();
                    moveCounter++;
                    turnCounter = false;
                }
            }
            if (turnCounter == false && moveCounter > 0 && tickCounter > 10)
            {
                if (upKeyDown && doodoo.y - 62 >= 0 && doodooTopCollision == false)
                {
                    doodoo.direction = "Up";
                    SwitchMove();
                }
                else if (leftKeyDown && doodoo.x - 75 >= 0 && doodooLeftCollision == false)
                {
                    doodoo.direction = "Left";
                    SwitchMove();
                }
                else if (downKeyDown && doodoo.y + 102 <= 500 && doodooBottomCollision == false)
                {
                    doodoo.direction = "Down";
                    SwitchMove();

                }
                else if (rightKeyDown && doodoo.x + 122 <= 600 && doodooRightCollision == false)
                {
                    doodoo.direction = "Right";
                    SwitchMove();
                }
                tickCounter = 0;
            }


            if (turnCounter)
            {
                turnLabel.Text = "Roll's Turn";
            }
            else if (turnCounter == false)
            {
                turnLabel.Text = "Waste Warrior's Turn";
            }
            CollisionCheck();
            tickCounter++;
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
            e.Graphics.DrawImage(Properties.Resources.escapeGrate, grateX, grateY, grateWidth, grateHeight);
             
            e.Graphics.DrawImage(Properties.Resources.New_Piskel__2_, roll.x, roll.y, roll.size, roll.size);
            e.Graphics.DrawImage(Properties.Resources.Waste_Warroir, doodoo.x, doodoo.y, doodoo.size, doodoo.size);

            e.Graphics.FillRectangle(redBrush, rightDooDooRec);
            e.Graphics.FillRectangle(redBrush, leftDooDooRec);
            e.Graphics.FillRectangle(redBrush, bottomDooDooRec);
            e.Graphics.FillRectangle(redBrush, topDooDooRec);

            e.Graphics.FillRectangle(redBrush, rightRollRec);
            e.Graphics.FillRectangle(redBrush, leftRollRec);
            e.Graphics.FillRectangle(redBrush, bottomRollRec);
            e.Graphics.FillRectangle(redBrush, topRollRec);

            //draw walls to screen
            foreach (Wall w in wallList)
            {
                e.Graphics.FillRectangle(wallBrush, w.x, w.y, w.Width, w.Height);
            }
        }

        public void LevelReading()
        {
            if (levelNumber < 4)
            {
                XmlReader reader = XmlReader.Create("Resources/Level" + Convert.ToString(levelNumber) + ".xml", null);

                reader.ReadToFollowing("x");
                roll.x = Convert.ToInt32(reader.Read());
                reader.ReadToFollowing("y");
                roll.y = Convert.ToInt32(reader.Read());

                while (reader.Read())
                {
                    reader.ReadToFollowing("walls");                  

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
            }

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
        
        public void DooDooWallHitBox()
        {
            doodooRightCollision = doodooLeftCollision = doodooBottomCollision = doodooTopCollision = false;

             rightDooDooRec = new Rectangle(doodoo.x + 53, doodoo.y + 18, 10, 10);
             leftDooDooRec = new Rectangle(doodoo.x - 24, doodoo.y + 18, 10, 10);
             bottomDooDooRec = new Rectangle(doodoo.x + 15, doodoo.y + 48, 10, 10);
             topDooDooRec = new Rectangle(doodoo.x + 15, doodoo.y - 14, 10, 10);

            foreach (Wall w in wallList)
            {
                Rectangle wallRec = new Rectangle(w.x, w.y, w.Width, w.Height);
                
                if (rightDooDooRec.IntersectsWith(wallRec) && doodooRightCollision == false)
                {
                    doodooRightCollision = true;
                }
                if (leftDooDooRec.IntersectsWith(wallRec) && doodooLeftCollision == false)
                {
                    doodooLeftCollision = true;
                }
                if (topDooDooRec.IntersectsWith(wallRec) && doodooTopCollision == false)
                {
                    doodooTopCollision = true;
                }
                if (bottomDooDooRec.IntersectsWith(wallRec) && doodooBottomCollision == false)
                {
                    doodooBottomCollision = true;
                }
            }
        }

        public void RollWallHitBox()
        {
            rollTopCollision = rollBottomCollision = rollLeftCollision = rollRightCollision = false;

            rightRollRec = new Rectangle(roll.x + 50, roll.y + 8, 10, 10);
            leftRollRec = new Rectangle(roll.x - 24, roll.y + 8, 10, 10);
            bottomRollRec = new Rectangle(roll.x + 11, roll.y + 40, 10, 10);
            topRollRec = new Rectangle(roll.x + 11, roll.y - 24, 10, 10);

            foreach (Wall w in wallList)
            {
                Rectangle wallRec = new Rectangle(w.x, w.y, w.Width, w.Height);

                if (rightRollRec.IntersectsWith(wallRec) && rollRightCollision == false)
                {
                    rollRightCollision = true;
                }
                if (leftRollRec.IntersectsWith(wallRec) && rollLeftCollision == false)
                {
                    rollLeftCollision = true;
                }
                if (topRollRec.IntersectsWith(wallRec) && rollTopCollision == false)
                {
                    rollTopCollision = true;
                }
                if (bottomRollRec.IntersectsWith(wallRec) && rollBottomCollision == false)
                {
                    rollBottomCollision = true;
                }
            }
        }
        public void HitBoxCreation()
        {
            Rectangle grateRec = new Rectangle(grateX, grateY, grateWidth, grateHeight);

            Rectangle rollRec = new Rectangle(roll.x, roll.y, roll.size, roll.size);

            if (rollRec.IntersectsWith(grateRec))
            {
                wallList.Clear();
                levelNumber++;

                if (levelNumber == 4)
                {
                    //Change to Win Screen
                    Form f = this.FindForm();
                    WinScreen ws = new WinScreen();

                    f.Controls.Remove(this);
                    f.Controls.Add(ws);

                    ws.Focus();
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
