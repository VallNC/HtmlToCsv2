namespace HtmlToCsv2.Dtos
{
    public class CandidateDto
    {
          public int Id {get;set;}
    public Area _Area {get;set;}
  public string GroupName { get; set; } = string.Empty;
  public List<String> PersonNames { get; set; } = new List<string>();
    }
}