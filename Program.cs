using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace gifi
{
    internal class Program
    {
    public static async Task<bool> IsVaildAsync(string link)
    {
            try
            {
                var hoge = await new HttpClient{Timeout = TimeSpan.FromSeconds(30)}.GetAsync("https://xgf.nu/"+ link);
                hoge.EnsureSuccessStatusCode();
                if(hoge.RequestMessage.RequestUri.ToString() != "") //NullReferenceExceptionをcatchしてるのに注意される 無視
                {
                    Console.WriteLine(hoge.RequestMessage.RequestUri.ToString());
                    return true;
                }
            }
            catch(Exception ex) when(ex is HttpRequestException || ex is TaskCanceledException || ex is NullReferenceException)
            {
                return false;
            }
            return false;
    }
public static async Task<string> RetLinkAsync(string link) 
{
  if(await Task.Run(() => IsVaildAsync(link)))
  {
    return link; 
  }
  return "";
}
    public static void Main(string[] args)
    {
        var lis = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        var ran = new Random();
        Parallel.ForEach(lis.OrderBy(x => ran.Next()), new ParallelOptions(){MaxDegreeOfParallelism = 8}, i =>
        {
            var random = new Random();
            var tasks = new List<Task<string>>();
            foreach(char j in lis.OrderBy(x => random.Next()))
            {
                foreach(char k in lis.OrderBy(x => random.Next()))
                {
                    foreach(char l in lis.OrderBy(x => random.Next()))
                    {
                        foreach(char m in lis.OrderBy(x => random.Next())) tasks.Add(RetLinkAsync(string.Concat(i, j, k, l, m)));
                        Task.WhenAll(tasks).Wait();
                        //foreach(var task in tasks) if(task.Result != "") //Console.WriteLine("https://xgf.nu/"+task.Result);
                        tasks.Clear();

                    }
                }
            }
        });
    }
    }
}