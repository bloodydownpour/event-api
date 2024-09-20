 namespace EventList.Application.Pagination;

public class Pager
{
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

    public int TotalPages { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }


    public Pager() { }
    public Pager(int TotalItems, int Page, int PageSize = 5)
    {
        this.TotalItems = TotalItems;
        CurrentPage = Page;
        this.PageSize = PageSize;
        TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalItems) / Convert.ToDecimal(PageSize)));

        int StartPage = CurrentPage - 2;
        int EndPage = CurrentPage + 2;
        if (StartPage <= 0)
        {
            EndPage = EndPage - (StartPage - 1);
            StartPage = 1;
        }
        if (EndPage > TotalPages)
        {
            EndPage = TotalPages;
            if (EndPage > 5)
            {
                StartPage = EndPage - 4;
            }
        }
        
        this.StartPage = StartPage;
        this.EndPage = EndPage;

    }
}
