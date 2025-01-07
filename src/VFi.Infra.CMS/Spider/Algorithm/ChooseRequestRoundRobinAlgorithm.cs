namespace VFi.Infra.CMS.Spider.Algorithm
{
    /// <summary>
    /// Choose round from 0 to maximum item
    /// </summary>
    public class ChooseRequestRoundRobinAlgorithm : ChooseRequestAlgorithm
    {
        private readonly object lockObject = new object();
        private int currentIndex = 0;

        public ChooseRequestRoundRobinAlgorithm()
        {
        }

        public override TEntity Get<TEntity>(IReadOnlyCollection<TEntity> items)
        {
            lock (lockObject)
            {
                var length = items.Count;

                if (currentIndex >= length)
                {
                    currentIndex = 0;
                }

                return items.ElementAt(currentIndex++);
            }
        }
    }
}