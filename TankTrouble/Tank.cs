﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TankTrouble
{
    public class Tank
    {
        readonly int FIELD_WIDTH = 900;
        readonly int FIELD_HEIGHT = 600;
        readonly int block_WIDTH = 10;
        readonly int block_HEIGHT = 10;
        public int X;
        public int Y;
        public Image tankImage;
        public Direction tankDirection;
        Rectangle bounds, tankRectangle;
        public List<Bullet> bullets;
           public bool shouldDraw;
            Timer timer_explosion;
            TankColor color;
           public bool isDead;
            public Tank otherTank;
            public Rectangle[][] rectangleMatrix;
        
        public Tank(TankColor c, Direction d, Rectangle r, int x, int y)
        {
            this.color = c;
            tankDirection = d;
            bullets = new List<Bullet>();
            bounds = r;
            if (color == TankColor.Green)
                tankImage = global::TankTrouble.Properties.Resources.greenTank_right;
            else
                tankImage = global::TankTrouble.Properties.Resources.redTank_Left;
            X = x+tankImage.Width/2;

            Y = y+tankImage.Height/2;
            shouldDraw=true;
            isDead = false;
            timer_explosion = new Timer();
            timer_explosion.Interval = 1000;
            timer_explosion.Tick += new EventHandler(timer_explosion_tick);
           


        }

        public void addOtherTank(Tank t)
        {
            otherTank = t;
        }
        public void addMatrix(Rectangle[][] rectangleMatrix)
        {
            this.rectangleMatrix = rectangleMatrix;
        }
        void timer_explosion_tick(object sender, EventArgs e)
        {
            otherTank.shouldDraw = false;
            
            if (!shouldDraw)
            {
             timer_explosion.Stop();
             
            }
        }
        public void Draw(Graphics g)
        {
            if (shouldDraw)
            {
                tankRectangle = new Rectangle(X, Y, tankImage.Width, tankImage.Height);
                g.DrawImageUnscaledAndClipped(tankImage, tankRectangle);
                foreach (Bullet b in bullets)
                {
                    b.Draw(g);
                }
            }

            
        }
        public bool canMove(bool [][]blockMatrix, Rectangle [][]rectangleMatrix)
        {
           

            for (int i = 0; i < FIELD_HEIGHT / block_HEIGHT; i++)
            {
                for (int j = 0; j < FIELD_WIDTH / block_WIDTH; j++)
                {
                    
                   if (blockMatrix[i][j])
                    {
                        

                        if (tankDirection == Direction.Up)
                        {
                           Rectangle tnkRectangle = new Rectangle(X, Y - 10, tankImage.Width, tankImage.Height );
                            if (rectangleMatrix[i][j].IntersectsWith(tnkRectangle))
                            {
                                return false;
                            }
                            if (otherTank.tankRectangle.IntersectsWith(tankRectangle))

                            {
                                return false;
                            }
                           

                        }
                        else if (tankDirection == Direction.Down)
                        {
                            Rectangle tnkRectangle = new Rectangle(X, Y + 10, tankImage.Width, tankImage.Height);
                            if (rectangleMatrix[i][j].IntersectsWith(tnkRectangle))
                            {
                                return false;
                            }

                            if (otherTank.tankRectangle.IntersectsWith(tankRectangle))
                            {
                                return false;
                            }
                           

                        }
                        else if (tankDirection == Direction.Left)
                        {
                            Rectangle tnkRectangle = new Rectangle(X - 10, Y, tankImage.Width, tankImage.Height);
                            if (rectangleMatrix[i][j].IntersectsWith(tnkRectangle))
                            {
                                return false;
                            }
                            if (otherTank.tankRectangle.IntersectsWith(tankRectangle))
                            {
                                return false;
                            }
                           
                            
                        }
                        else if (tankDirection == Direction.Right)
                        {
                           Rectangle tnkRectangle = new Rectangle(X + 10, Y, tankImage.Width, tankImage.Height);
                            if (rectangleMatrix[i][j].IntersectsWith(tnkRectangle))
                            {
                                return false;
                            }
                            
                            if (otherTank.tankRectangle.IntersectsWith(tankRectangle))
                            {
                                return false;
                            }
                            

                        }
                   
                    }
                     
                }
            }

       
         
           
                return true;
        }

       

        public void Move(Rectangle bounds, Direction direction)
        {
            if (!isDead)
            {
                

                if (direction.Equals(Direction.Up))
                {

                  

                    if (color == TankColor.Green)
                        tankImage = global::TankTrouble.Properties.Resources.greenTank_up;
                    else
                        tankImage = global::TankTrouble.Properties.Resources.redTank_Up;
                    if (Y > bounds.Top)
                    {
                        Y -= 3;

                    }

                }
                else if (direction.Equals(Direction.Down))
                {
                   
                    if (color == TankColor.Green)
                        tankImage = global::TankTrouble.Properties.Resources.greenTank_down;
                    else
                        tankImage = global::TankTrouble.Properties.Resources.redTank_Down;

                    if (Y + tankImage.Height < bounds.Bottom)
                        Y += 3;
                }
                else if (direction.Equals(Direction.Left))
                {

                    if (color == TankColor.Green)
                        tankImage = global::TankTrouble.Properties.Resources.greenTank_left;
                    else
                        tankImage = global::TankTrouble.Properties.Resources.redTank_Left;
                    if (X > bounds.Left)
                        X -= 3;

                }
                else if (direction.Equals(Direction.Right))
                {
                    if (color == TankColor.Green)
                        tankImage = global::TankTrouble.Properties.Resources.greenTank_right;
                    else
                        tankImage = global::TankTrouble.Properties.Resources.redTank_right;
                    if (X + tankImage.Width < bounds.Right)
                        X += 3;
                }


                tankDirection = direction;
            }
            
        }
        public void Fire(bool[][] blockMatrix, Rectangle[][] rectangleMatrix)
        {
            foreach (Bullet b in bullets)
                b.Move(blockMatrix, rectangleMatrix);
        }
        public void Destroy()
        {

            foreach (Bullet b in bullets)
            {
                if (b.shouldDraw)
                {
                    if (b.X > otherTank.X && b.X < otherTank.X + otherTank.tankImage.Width && b.Y > otherTank.Y && b.Y < otherTank.Y + otherTank.tankImage.Height)
                    {
                        b.shouldDraw = false;
                        otherTank.tankImage = global::TankTrouble.Properties.Resources.kaboom1;
                        
                        otherTank.isDead = true;
                        timer_explosion.Start();
                    }
                        
                }
            }
            

            
        }
    }
}