namespace Zhihu.SharedKernel.Repository;

public interface IHttpRepository
{
    string AppId { get; set; }
    
    string BaseRouter { get; set; }
}