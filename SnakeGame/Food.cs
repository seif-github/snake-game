using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Food
    {
        public (int x, int y) Position { get; private set; }
        Obstacle obstacle = new Obstacle();

        public void GenerateNewPosition(Snake snake, int boardWidth, int boardHeight)
        {
            Random random = new Random();
            do
            {
                do
                {
                    Position = (random.Next(boardWidth), random.Next(boardHeight));
                }
                while (obstacle.wall.Contains(Position)); // don't generate food into wall

            } while (snake.Body.Contains(Position));
        }
    }
}
