namespace Iterator
{
    public class CustomCollection : IEnumerable<int>
    {
        public int this[int index]
        {
            get => index;
            //You could even define a set accessor...
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return i;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}