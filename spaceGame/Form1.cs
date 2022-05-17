using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace spaceGame
{
    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle(200, 365, 15, 35);
        Rectangle player2 = new Rectangle(400, 365, 15, 35);
        int playerSpeed = 3;

        Rectangle timer = new Rectangle(300, 0, 20, 400);
        int timerCounter = 0;
        int timerCounter2 = 1;

        Random randGen = new Random();

        List<Rectangle> leftAsteroids = new List<Rectangle>();
        List<Rectangle> rightAsteroids = new List<Rectangle>();
        int asteroidSpeed = 6;
        int asteroidXSize = 8;
        int asteroidYSize = 3;

        int player1Score = 0;
        int player2Score = 0;

        int leftAsteroidCounter;
        int rightAsteroidCounter;
        

        bool wDown = false;
        bool upDown = false;

        SolidBrush whiteBrush = new SolidBrush(Color.White);

        string gameState = "waiting";

        //sounds
        SoundPlayer music = new SoundPlayer (Properties.Resources.gameMusic);
        SoundPlayer moving = new SoundPlayer(Properties.Resources.movingSound);
        SoundPlayer death = new SoundPlayer(Properties.Resources.goodDeathSound);
        SoundPlayer gameOver = new SoundPlayer(Properties.Resources.gameOverSound);

        Image spaceshipImage = Properties.Resources.spaceship;

        public Form1()
        {
            // game needs...
            //2 players/key events
            //only up or down
            //point given to player when they touch the top of the screen
            //asteroids that cross the screen, alternating each row
            //if player intersects with asteroid return to start
            //scoring system
            //cooldown if player dies for respawn
            //title screen and game over
            //max 3 points
            //sounds
            //if time add rocket model

            InitializeComponent();
        }

        public void GameInitialize()
        {
            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameEngine.Enabled = true;
            gameState = "running";
            leftAsteroids.Clear();
            rightAsteroids.Clear();
            p1ScoreLabel.Visible = true;
            p2ScoreLabel.Visible = true;

            player1.X = 200;
            player1.Y = 365;

            player2.X = 400;
            player2.Y = 365;

            timer.Y = 0;

            p1ScoreLabel.Text = "0";
            p2ScoreLabel.Text = "0";

            player1Score = 0;
            player2Score = 0;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    moving.Play();
                    break;
                case Keys.Up:
                    upDown = true;
                    moving.Play();
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over" || gameState == "noWin")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over" || gameState == "noWin")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.Up:
                    upDown = false;
                    break;
            }
        }

        private void gameEngine_Tick(object sender, EventArgs e)
        {
            timerCounter ++;
            
            if (timerCounter == 7)
            {
                timer.Y += timerCounter2;
                timerCounter = 0;
            }
            
            if (timer.Y == 400)
            {
                gameState = "noWin";
                gameOver.Play();
            }


            //move players
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }
            if (upDown == true && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
            }

            //move left asteroids
            for (int i = 0; i < leftAsteroids.Count(); i++)
            {
                int lX = leftAsteroids[i].X + asteroidSpeed;

                leftAsteroids[i] = new Rectangle(lX, leftAsteroids[i].Y, asteroidXSize, asteroidYSize);
            }

            //create left Asteroids
            leftAsteroidCounter++;

            if (leftAsteroidCounter == 10)
            {
                leftAsteroids.Add(new Rectangle(-20, randGen.Next(0, 301), asteroidXSize, asteroidYSize));
                leftAsteroidCounter = 0;
            }

            //move right asteroids
            for (int i = 0; i < rightAsteroids.Count(); i++)
            {
                int rX = rightAsteroids[i].X - asteroidSpeed;

                rightAsteroids[i] = new Rectangle(rX, rightAsteroids[i].Y, asteroidXSize, asteroidYSize);
            }

            //create right Asteroids
            rightAsteroidCounter++;

            if (rightAsteroidCounter == 10)
            {
                rightAsteroids.Add(new Rectangle(620, randGen.Next(0, 301), asteroidXSize, asteroidYSize));
                rightAsteroidCounter = 0;
            }

            //Player intersects with top of form
            
            if (player1.Y < 0)
            {
                player1.X = 200;
                player1.Y = 365;
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";
            }

            if (player2.Y < 0)
            {
                player2.X = 400;
                player2.Y = 365;
                player2Score++;
                p2ScoreLabel.Text = $"{player2Score}";
            }
            //cooldownTimer--;
            //if (cooldownTimer == 0)
            //{
            //    wDown = false;

            //}

            //Player intersects with orbs
            for (int i = 0; i < leftAsteroids.Count(); i++)
            {
                if(player1.IntersectsWith(leftAsteroids[i]))
                {
                    player1.X = 200;
                    player1.Y = 365;
                    death.Play();
                }

                if (player1.IntersectsWith(rightAsteroids[i]))
                {
                    player1.X = 200;
                    player1.Y = 365;
                    death.Play();
                }
            }

            for (int i = 0; i < leftAsteroids.Count(); i++)
            {
                if (player2.IntersectsWith(leftAsteroids[i]))
                {
                    player2.X = 400;
                    player2.Y = 365;
                }

                if (player2.IntersectsWith(rightAsteroids[i]))
                {
                    player2.X = 400;
                    player2.Y = 365;
                }
            }

            //if player score is 3 end the game
            if(player1Score == 3)
            {
                gameState = "over";
            }
            else if (player2Score == 3)
            {
                gameState = "over";
            }



            //timer counting down
            
            
            

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")

            {

                titleLabel.Text = "SPACE GAME 2022";

                subtitleLabel.Text = "Press Space Bar to Start or Escape to Exit";

            }

            else if (gameState == "running")

            {
                //draw players
                
                e.Graphics.DrawImage(spaceshipImage, player1);
                e.Graphics.DrawImage(spaceshipImage, player2);

                //draw timer
                e.Graphics.FillRectangle(whiteBrush, timer);

                //draw left asteroids
                for (int i = 0; i < leftAsteroids.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, leftAsteroids[i]);
                }
                //draw right asteroids
                for (int i = 0; i < rightAsteroids.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, rightAsteroids[i]);
                }
                music.Play();
            }
            else if (gameState == "over")

            {
                if(player1Score == 3)
                {
                    titleLabel.Text = $"Player 1 wins!";
                    subtitleLabel.Text = $"Press Space Bar to Start or Escape to Exit";
                    p1ScoreLabel.Visible = false;
                    p2ScoreLabel.Visible = false;
                    music.Stop();
                }
                else if (player2Score == 3)
                {
                    titleLabel.Text = $"Player 2 wins!";
                    subtitleLabel.Text = $"Press Space Bar to Start or Escape to Exit";
                    p1ScoreLabel.Visible = false;
                    p2ScoreLabel.Visible = false;
                    music.Stop();
                }
                

            }
            else if(gameState == "noWin")
            {
                titleLabel.Text = $"Nobody won!";
                subtitleLabel.Text = $"Press Space Bar to Start or Escape to Exit";
                p1ScoreLabel.Visible = false;
                p2ScoreLabel.Visible = false;
                music.Stop();
            }
        }
    }
}
