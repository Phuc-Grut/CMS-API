namespace VFi.Infra.CMS.Spider.Interface
{

    public interface IRequestManager
    {
        
        Task LoadProxy(); 
        Task Downtime(string? proxyKey); 
        SpiderLeg GetSpiderLeg();
        List<SpiderLeg> GetSpiderLegs();
        MercariToken MercariToken { get;}
    }
}