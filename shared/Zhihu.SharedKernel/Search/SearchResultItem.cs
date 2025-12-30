namespace Zhihu.SharedKernel.Search;

public class SearchResultItem<TDoc> where TDoc : class
{
    public string Index { get; set; } = null!;

    public double? Score { get; set; }

    public TDoc? Source { get; set; }

    public IReadOnlyDictionary<string, IReadOnlyCollection<string>>? Highlight { get; set; }
}