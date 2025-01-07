namespace VFi.Infra.CMS.Spider.Algorithm
{
    /// <summary
    /// Define choose RequestAlgorithm logic
    /// </summary>
    public abstract class ChooseRequestAlgorithm
    {
        /// <summary>
        /// <br></br>
        /// Get RequestAlgorithm
        /// </summary>
        /// 
        /// <remarks>
        /// Get RequestAlgorithm by logic in Dependencies Injection(DI), can be configured in Startup.cs
        /// </remarks>
        public abstract TEntity Get<TEntity>(IReadOnlyCollection<TEntity> items);
    }
}