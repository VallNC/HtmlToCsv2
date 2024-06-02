
using System.Text;
using HtmlAgilityPack;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Text.Json;
using System.Dynamic;
using HtmlToCsv2;

Console.OutputEncoding = Encoding.UTF8;

using HttpClient client = new HttpClient();
{
List<Area> areaNameList = new List<Area>();
areaNameList = await GrabAreaNames(client);
  client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
  client.DefaultRequestHeaders.Add("Referer", "https://example.com");
  client.DefaultRequestHeaders.Add("Origin", "https://example.com");
  client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
  client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");


  client.DefaultRequestHeaders.Add("Cookie", "sessionId=xyz123; otherCookie=abc456");
  List<string> areaList = new List<string>();

  try
    {
        
int areaInt = 0;
        await GrabEveryJson(client, areaList,areaNameList.Count);
        int nameNumber = 0;
        foreach (var item in areaList)
        {
          
            int maxSize = 0;
            List<Candidate> candidates = new List<Candidate>();
            var result = item.Split(new[] { '\r', '\n' });
            for (int i = 0; i < result.Length; i++)
            {

                if (result[i].Contains("\"name\""))
                {

                    bool found = false;
                    foreach (var item2 in candidates)
                    {
                        if (item2.GroupName == result[i + 4].Substring(18, result[i + 4].Length - 20))
                        {
                            item2.PersonNames.Add(result[i].Substring(17, result[i].Length - 19));
                            found = true;
                            if (item2.PersonNames.Count > maxSize)
                                maxSize = item2.PersonNames.Count;
                        }
                    }
                    if (!found)
                    {
                        candidates.Add(new Candidate(result[i + 4].Substring(18, result[i + 4].Length - 20), result[i].Substring(17, result[i].Length - 19),areaNameList[candidates.Count]));
                        if (maxSize == 0)
                            maxSize = 1;
                    }
                }

            }

            //Add class to csv method
            ClassToCsv(maxSize, nameNumber, candidates, areaInt);
            using (var db = new DataToSql()){
            db.candidates.Add(candidates[areaInt]);
            db.SaveChanges();
           }
            areaInt++;
            nameNumber++;
            
        }
        
    }
    catch (HttpRequestException e)
  {
    Console.WriteLine($"Request error: {e.Message}");
  }
}

static async Task<List<Area>> GrabAreaNames(HttpClient client)
{
  List<string> areaNameList = new List<string>();
   var htmlWeb = new HtmlWeb();
   List<Area> areas = new List<Area>();
    var httpText = htmlWeb.Load("""https://www.cik.bg/bg/epns2024/candidates/ns?rik=0&party=0""");
          string[] resultNames = httpText.Text.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    System.Console.WriteLine(resultNames[1]);
    System.Console.WriteLine(resultNames[2]);
    for (int i = 0; i < resultNames.Length; i++)
    {      
     
        if (resultNames[i].Contains("""<select class="rik"""))
        {
            
              areaNameList=ExtractPartyNames(resultNames[i]);
                break;
            
        }

    }
    foreach(var item in areaNameList)
    {
      System.Console.WriteLine(item);
      areas.Add(new Area(item));
      using (var db = new DataToSql()){
        db.areas.Add(new Area(item));
        }
    }
    return areas;
}

static async Task GrabEveryJson(HttpClient client, List<string> areaList,int listSize)
{
  //i<32
    for (int i = 1; i < listSize; i++)
    {
       // if (i == 2)
            //2ри район не връща json
            //continue;
        string link = @"https://www.cik.bg/bg/epns2024/candidates/ns?rik=" + i + @"&party=0";
        //System.Console.WriteLine(link);
        HttpResponseMessage response = await client.GetAsync(link);
        response.EnsureSuccessStatusCode();


        string responseBody = await response.Content.ReadAsStringAsync();
        string decodedResponse = JObject.Parse(responseBody).ToString();
        areaList.Add(decodedResponse);
    }
}

static void ClassToCsv(int maxSize, int nameNumberV, List<Candidate> candidates,int areaInt)
{
  string areaName;
 // if(areaList.Count>areaInt){
    areaName = candidates[areaInt]._Area.Name;
 // }
  // else{
//areaName = areaInt.ToString();
  // }
  using (var writer = new StreamWriter($".\\CsvFolder\\{areaName}.csv"))
  {
    string names = "";
    foreach (var itemList in candidates)
    {

      names = names + itemList.GroupName.Replace(',', '.') + ",";                      
    }
    writer.WriteLine(names);
    for (int i = 0; i < maxSize; i++)
    {
      string combinedString = string.Join(", ", candidates
      .Select(list => list.PersonNames.Count > i ? list.PersonNames[i] : string.Empty));
      writer.WriteLine(combinedString);
    }
    
  }
}

 static List<string> ExtractPartyNames(string html)
    {
        var partyNames = new List<string>();

        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(html);

        var partySelect = document.DocumentNode.SelectSingleNode("//select[@class='rik']");
        if (partySelect != null)
        {
            foreach (var option in partySelect.SelectNodes(".//option"))
            {
                string name = option.InnerText;
                if (!name.Equals("Всички"))
                {
                    partyNames.Add(name);
                }
            }
        }

        return partyNames;
    }
public class Candidate
{
    public Candidate(string groupName, string personNames, Area area)
    {
        GroupName = groupName;
        PersonNames = new List<string>();
        PersonNames.Add(personNames);
        _Area = area;
    }
    public int Id {get;set;}
    public Area _Area {get;set;}
  public string GroupName { get; set; } = string.Empty;
  public List<String> PersonNames { get; set; } = new List<string>();


}

public class Area{
    public Area(string name)
    {
        Name = name;
    }

    public int Id {get;set;}
  public string Name{get;set;} = string.Empty;
}





/*

If I want to implement using city names instead of numbers

HttpResponseMessage responseName = await client.GetAsync("""www.cik.bg/bg/epns2024/candidates/ns?rik=0&party=0""");
       string responseBodyName = await responseName.Content.ReadAsStringAsync();
       string[] resultNames = responseBodyName.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
       List <String> areaNameList = new List<string>();
for (int i = 0; i < resultNames.Length; i++)
          {
            if(resultNames[i].Contains("""<select class="rik" style=""width: 100%>""")){
           {
            i++;
            while (resultNames[i].Contains("option value"))
            {
              if(areaNameList.Count>10){
                 areaNameList.Add(resultNames[i].Substring(10));
              }
              else{
 areaNameList.Add(resultNames[i].Substring(10));
              }
            i++;  
            }
            break;
           }
            }  
            
          } */