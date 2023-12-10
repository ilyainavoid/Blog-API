namespace BlogApi.Models.DTO;

public class PageInfoModel
{
    public int Size { get; set; }
    
    public int Count { get; set; }
    
    public int Current { get; set; }

    public PageInfoModel(int size, int count, int current)
    {
        Size = size;
        Count = count;
        Current = current;
    }
}