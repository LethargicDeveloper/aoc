using AocLib;

namespace AdventOfCode._2022.Day20;

public partial class Part01 : PuzzleSolver<long>
{
    public override long Solve()
    {
        var list = this.input
            .SplitLines()
            .Select((v, i) => (Order: i, Value: long.Parse(v)))
            .ToList();

        var crypto = new CircularList(list.ToList());

        foreach (var num in list)
        {
            crypto.Move(num.Order);
        }

        return crypto.GroveCoordinate();
    }

    class CircularList : List<(int Order, long Value)>
    {
        int index = 0;

        public CircularList(List<(int, long)> list)
            : base(list) { }

        public long GroveCoordinate()
        {
            var zero = this.FindIndex(0, _ => _.Value == 0);
            var n1000 = this[(zero + 1000) % (this.Count)].Value;
            var n2000 = this[(zero + 2000) % (this.Count)].Value;
            var n3000 = this[(zero + 3000) % (this.Count)].Value;

            return n1000 + n2000 + n3000;
        }

        public void Move(long order)
        {
            var index = this.FindIndex(_ => _.Order == order);
            var item = this[index];
            var value = this[index].Value;

            if (value == 0) return;

            if (value > 0)
            {
                int newIndex = (int)((index + value) % (this.Count - 1));
                this.RemoveAt(index);
                this.Insert(newIndex, item);
            }
            else
            {
                int newIndex = (int)((index + value) % (this.Count - 1));
                newIndex = newIndex < 0 ? newIndex + (this.Count - 1) : newIndex;
                this.RemoveAt(index);
                this.Insert(newIndex, item);
            }
        }

        public IEnumerable<long> GetNumbers()
        {
            while (true)
            {
                yield return this[index].Value;
                index++;
                index = index > this.Count - 1 ? 0 : index;
            }
        }
    }
}
