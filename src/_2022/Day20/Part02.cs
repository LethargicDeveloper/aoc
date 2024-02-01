using AocLib;

namespace _2022.Day20;

public partial class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var list = input
            .SplitLines()
            .Select((v, i) => (Order: i, Value: long.Parse(v) * 811589153))
            .ToList();

        var crypto = new CircularList(list.ToList());

        for (int i = 0; i < 10; ++i)
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
            var zero = FindIndex(0, _ => _.Value == 0);
            var n1000 = this[(zero + 1000) % Count].Value;
            var n2000 = this[(zero + 2000) % Count].Value;
            var n3000 = this[(zero + 3000) % Count].Value;

            return n1000 + n2000 + n3000;
        }

        public void Move(long order)
        {
            var index = FindIndex(_ => _.Order == order);
            var item = this[index];
            var value = this[index].Value;

            if (value == 0) return;

            if (value > 0)
            {
                int newIndex = (int)((index + value) % (Count - 1));
                RemoveAt(index);
                Insert(newIndex, item);
            }
            else
            {
                int newIndex = (int)((index + value) % (Count - 1));
                newIndex = newIndex < 0 ? newIndex + (Count - 1) : newIndex;
                RemoveAt(index);
                Insert(newIndex, item);
            }
        }

        public IEnumerable<long> GetNumbers()
        {
            while (true)
            {
                yield return this[index].Value;
                index++;
                index = index > Count - 1 ? 0 : index;
            }
        }
    }
}
