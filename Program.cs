
using System.Text;
using HtmlAgilityPack;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Text.Json;
using System.Dynamic;

Console.OutputEncoding = Encoding.UTF8;

using HttpClient client = new HttpClient();
{
List<string> areaNameList = new List<string>();
await NewMethod(client, areaNameList);
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
        

        await GrabEveryJson(client, areaList);
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
                        candidates.Add(new Candidate(result[i + 4].Substring(18, result[i + 4].Length - 20), result[i].Substring(17, result[i].Length - 19)));
                        if (maxSize == 0)
                            maxSize = 1;
                    }
                }

            }

            //Add class to csv method
            ClassToCsv(maxSize, nameNumber, candidates);
            nameNumber++;
        }
    }
    catch (HttpRequestException e)
  {
    Console.WriteLine($"Request error: {e.Message}");
  }
}

static async Task NewMethod(HttpClient client, List<string> areaNameList)
{
   var htmlWeb = new HtmlWeb();
    var httpText = htmlWeb.Load("""https://www.cik.bg/bg/epns2024/candidates/ns?rik=0&party=0""");
          string[] resultNames = httpText.Text.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    int testNumber = 0;
    for (int i = 0; i < resultNames.Length; i++)
    {      
     // System.Console.WriteLine(resultNames[i]);
        if (resultNames[i].Contains("""<select class="rik" style="width: 100%">"""))
        {
            {
              System.Console.WriteLine(resultNames[i]);
                i++;
                bool startFound = false;
                for (int int2 = 0; int2 < resultNames[i].Length-1; int2++)
                {
                  if (resultNames[i][int2]==('\"'))
                  {
                      if (!startFound)
                      {
                          startFound=true;
                      }
                      else
                      {
                        int charEnd = 0;
                        for (int int3 = int2; int3 < resultNames[i].Length; int3++,charEnd++)
                        {
                          if(resultNames[i][int3]==('<'))
                          {
                            break;
                          }
                        }
                          areaNameList.Add(resultNames[i].Substring(int2,charEnd));
                          startFound = false;
                      }
                      
                      //System.Console.WriteLine(areaNameList[testNumber]);
                      testNumber++;
                      i++;
                  }
                }
                break;
            }
        }

    }
}

static async Task GrabEveryJson(HttpClient client, List<string> areaList)
{
  //i<32
    for (int i = 1; i < 32; i++)
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

static void ClassToCsv(int maxSize, int nameNumberV, List<Candidate> candidates)
{
  using (var writer = new StreamWriter($".\\CsvFolder\\{nameNumberV}.csv"))
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

public class Candidate
{
  public Candidate(string groupName, string personNames)
  {
    GroupName = groupName;
    PersonNames = new List<string>();
    PersonNames.Add(personNames);
  }

  public string GroupName { get; set; } = string.Empty;
  public List<String> PersonNames { get; set; } = new List<string>();


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