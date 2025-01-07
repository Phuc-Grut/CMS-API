namespace VFi.Infra.CMS.Spider.Algorithm
{
    /// <summary>
    /// Choose random from 0 to maximum item
    /// </summary>
    public class ChooseRequestRandomAlgorithm : ChooseRequestAlgorithm
    {
        public override TEntity Get<TEntity>(IReadOnlyCollection<TEntity> items)
        {
            var random = new Random();
            var length = items.Count;
            var index = random.Next(0, length);

            return items.ElementAt(index);
        }
    }
}