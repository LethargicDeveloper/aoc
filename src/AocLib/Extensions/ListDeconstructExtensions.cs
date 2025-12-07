namespace AocLib;

public static class ListDeconstructExtensions
{
    extension<T>(IReadOnlyList<T> list)
    {
        public void Deconstruct(out T? t0)
        {
            t0 = list.Count > 0 ? list[0] : default;
        }

        public void Deconstruct(out T? t0, out T? t1)
        {
            t0 = list.Count > 0 ? list[0] : default;
            t1 = list.Count > 0 ? list[1] : default;
        }

        public void Deconstruct(out T? t0, out T? t1, out T? t2)
        {
            t0 = list.Count > 0 ? list[0] : default;
            t1 = list.Count > 0 ? list[1] : default;
            t2 = list.Count > 0 ? list[2] : default;
        }

        public void Deconstruct(out T? t0, out T? t1, out T? t2, out T? t3)
        {
            t0 = list.Count > 0 ? list[0] : default;
            t1 = list.Count > 0 ? list[1] : default;
            t2 = list.Count > 0 ? list[2] : default;
            t3 = list.Count > 0 ? list[3] : default;
        }

        public void Deconstruct(out T? t0, out T? t1, out T? t2, out T? t3, out T? t4)
        {
            t0 = list.Count > 0 ? list[0] : default;
            t1 = list.Count > 0 ? list[1] : default;
            t2 = list.Count > 0 ? list[2] : default;
            t3 = list.Count > 0 ? list[3] : default;
            t4 = list.Count > 0 ? list[4] : default;
        }
    }
}