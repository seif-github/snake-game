using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private const int BoardWidth = 20;
        private const int BoardHeight = 20;
        private const int CellSize = 20;
        private Rectangle GameCanvas
        {
            get
            {
                return new Rectangle(0, 0, BoardWidth * CellSize, BoardHeight * CellSize);
            }
        }

        private Snake snake;
        private Food food;
        private Obstacle obstacle;
        private int score;
        private int highScore;
        private int level;
        private Timer gameTimer;

        private SoundPlayer eatSound;
        private SoundPlayer gameOverSound;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            highScore = LoadHighScore();

            // Initialize game components
            snake = new Snake();
            food = new Food();
            obstacle = new Obstacle();
            food.GenerateNewPosition(snake, BoardWidth, BoardHeight);
            score = 0;
            level = 1;

            // Load sound files
            eatSound = new SoundPlayer("sfx/eat.wav"); // Add a sound file for eating
            gameOverSound = new SoundPlayer("sfx/gameover.wav"); // Add a sound file for game over

            // Initialize and start the game timer
            gameTimer = new Timer();
            gameTimer.Interval = 200; // Initial speed
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            // Handle key presses
            this.KeyDown += HandleKeyPress;

            // Set form size and style
            this.ClientSize = new Size(BoardWidth * CellSize + 200, BoardHeight * CellSize); // Extra space for score
            this.DoubleBuffered = true; // Reduce flickering
            this.Paint += Form1_Paint;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            snake.Move();

            // Wrap snake around the board
            var head = snake.Body[0];
            if (head.x < 0) head.x = BoardWidth - 1; // if left, appear from right
            if (head.x >= BoardWidth) head.x = 0; // if right, appear from left
            if (head.y < 0) head.y = BoardHeight - 1; // if top, appear from bottom
            if (head.y >= BoardHeight) head.y = 0; // if bottom, appear form top
            snake.Body[0] = head;

            // Check for collision with itself or touch wall
            if (snake.CheckSelfCollision() || obstacle.wall.Contains(snake.Body[0]))
            {
                gameTimer.Stop();
                gameOverSound.Play();
                MessageBox.Show("Game Over! Your score: " + score);
                UpdateHighScore();
                SaveHighScore(highScore);
                InitializeGame();
                return;
            }

            // Check if snake eats food
            if (head == food.Position)
            {
                snake.Grow();
                food.GenerateNewPosition(snake, BoardWidth, BoardHeight);
                score++;
                eatSound.Play();

                // Increase level every 5 points
                if (score % 5 == 0)
                {
                    level++;
                    gameTimer.Interval = Math.Max(50, gameTimer.Interval - 20); // Increase speed
                }
            }

            // Redraw the form
            this.Invalidate();
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (snake.CurrentDirection != Direction.Down)
                        snake.CurrentDirection = Direction.Up;
                    break;
                case Keys.Down:
                    if (snake.CurrentDirection != Direction.Up)
                        snake.CurrentDirection = Direction.Down;
                    break;
                case Keys.Left:
                    if (snake.CurrentDirection != Direction.Right)
                        snake.CurrentDirection = Direction.Left;
                    break;
                case Keys.Right:
                    if (snake.CurrentDirection != Direction.Left)
                        snake.CurrentDirection = Direction.Right;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(Brushes.LightBlue, GameCanvas);

            // Draw the game board border
            g.DrawRectangle(Pens.Black, 0, 0, BoardWidth * CellSize, BoardHeight * CellSize);

            // Draw the snake
            foreach (var segment in snake.Body)
            {
                g.FillRectangle(Brushes.Green, segment.x * CellSize, segment.y * CellSize, CellSize, CellSize);
            }

            // Draw the food
            g.FillRectangle(Brushes.Red, food.Position.x * CellSize, food.Position.y * CellSize, CellSize, CellSize);

            // Draw Obstacles
            foreach (var block in obstacle.wall)
            {
                g.FillRectangle(Brushes.Gray, block.x * CellSize, block.y * CellSize, CellSize, CellSize);
            }

            // Draw score and high score
            var scoreText = $"Score: {score}\nHigh Score: {highScore}\nLevel: {level}";
            g.DrawString(scoreText, new Font("Arial", 12), Brushes.Black, new PointF(BoardWidth * CellSize + 10, 10));
        }

        private void UpdateHighScore()
        {
            if (score > highScore)
            {
                highScore = score;
            }
        }

        private int LoadHighScore()
        {
            string filePath = "highscore.txt"; // File to read the high score from
            if (File.Exists(filePath))
            {
                string highScoreText = File.ReadAllText(filePath);
                if (int.TryParse(highScoreText, out int highScore))
                {
                    return highScore;
                }
            }
            return 0; // Default high score if file doesn't exist or is invalid
        }

        private void SaveHighScore(int highScore)
        {
            string filePath = "highscore.txt"; // File to store the high score
            File.WriteAllText(filePath, highScore.ToString()); // Save the high score

        }
    }
}   