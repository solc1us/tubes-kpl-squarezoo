namespace tubes_kpl_squarezoo.Tests
{
    internal class Evidences<T>
    {
        public object Type { get; set; }
        public object Content { get; set; }

        internal IAsyncEnumerable<char>? GetSummary()
        {
            throw new NotImplementedException();
        }

        internal bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}