using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class Snake
    {
        private int cellSize, offset;
        private LinkedList<Point> points = new LinkedList<Point>();
        private Point direction = new Point(0, 1);
        private Point newDirection = new Point(0, 1);

        public Snake(int cellSize, int row, int column)
        {
            this.cellSize = cellSize;
            var head = new Point(column, row);
            points.AddFirst(head);
            points.AddLast(head);
        }

        public void Update()
        {
            offset += 1;
            if(offset >= cellSize)
            {
                
                offset = 0;
                points.RemoveLast();
                var head = points.First.Value;
                head.Offset(direction.X, direction.Y);
                points.First.Value = head;
                points.AddFirst(head);

                if (direction.X != -newDirection.X && direction.Y != newDirection.Y)
                {
                    direction = newDirection;
                }
            }
        }

        public int GetLength()
        {
            return points.Count - 1;
        }

        public void OnKeyPress(Keys key)
        {
            switch(key)
            {
                case Keys.Up:
                    newDirection = new Point(0, -1);
                    break;
                case Keys.Down:
                    newDirection = new Point(0, 1);
                    break;
                case Keys.Left:
                    newDirection = new Point(-1, 0);
                    break;
                case Keys.Right:
                    newDirection = new Point(1, 0);
                    break;
            }
        }

        public Point GetHead()
        {
            var head = points.First.Value;
            head.Offset(direction.X, direction.Y);
            return head;
        }

        public bool Intersects(int x, int y)
        {
            foreach(var point in points)
            {
                if(point.X == x && point.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public void Grow()
        {
            var head = points.First.Value;
            head.Offset(direction.X, direction.Y);
            points.First.Value = head;
            points.AddFirst(head);
        }

        public void Draw(Graphics g)
        {
            var point = points.First;

            DrawpointWithOffset(g, point.Value, direction);

            for(point = point.Next; point != points.Last; point = point.Next)
            {
                var dot = point.Value;
                g.FillRectangle(Brushes.Coral, dot.X * cellSize, dot.Y * cellSize, cellSize, cellSize);
            }

            DrawpointWithOffset(g, point.Value, GetTailDirection());
           
        }

        private void DrawpointWithOffset(Graphics g, Point point, Point direction)
        {
            var position = new Point(point.X * cellSize, point.Y * cellSize);
            var pointOffset = Multiply(direction, offset);
            position.Offset(pointOffset.X, pointOffset.Y);
            g.FillRectangle(Brushes.Coral, position.X, position.Y, cellSize, cellSize);
        }

        private Point PointToScreen(Point point)
        {
            return Multiply(point, cellSize);
        }

        private Point Multiply(Point point, float c)
        {
            return new Point((int)(point.X * c), (int)(point.Y * c));
        }

        private Point GetTailDirection()
        {
            if(points.Count < 3)
            {
                return direction;
            }

            var tail = points.Last;
            var next = tail.Previous.Value;

            return new Point(next.X - tail.Value.X, next.Y - tail.Value.Y);
        }

    }
}
