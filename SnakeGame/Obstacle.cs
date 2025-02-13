using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Obstacle
    {
        public List<(int x, int y)> wall { get; set; }

        public Obstacle()
        {
            wall = new List<(int x, int y)>{
                (3,1), (3,2), (3,3), (3,4), (4,4), (5,4),
                (10,7), (10,8), (10,11), (10,12)
            };
            
        }
    }
}
