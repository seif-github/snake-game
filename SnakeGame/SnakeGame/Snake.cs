using SnakeGame;
using System.Collections.Generic;

public class Snake
{
    public List<(int x, int y)> Body { get; private set; }
    public Direction CurrentDirection { get; set; }

    public Snake()
    {
        Body = new List<(int x, int y)> { (0, 0) };
        CurrentDirection = Direction.Right;
    }

    public void Move()
    {
        var head = Body[0];
        var newHead = head;

        switch (CurrentDirection)
        {
            case Direction.Up:
                newHead = (head.x, head.y - 1);
                break;
            case Direction.Down:
                newHead = (head.x, head.y + 1);
                break;
            case Direction.Left:
                newHead = (head.x - 1, head.y);
                break;
            case Direction.Right:
                newHead = (head.x + 1, head.y);
                break;
        }

        Body.Insert(0, newHead);
        Body.RemoveAt(Body.Count - 1);
    }

    public void Grow()
    {
        var tail = Body[Body.Count - 1];
        Body.Add(tail);
    }

    public bool CheckSelfCollision()
    {
        var head = Body[0];
        for (int i = 1; i < Body.Count; i++)
        {
            if (Body[i] == head)
                return true;
        }
        return false;
    }
}