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
                HttpResponseMessage hoge = await new HttpClient{Timeout = TimeSpan.FromSeconds(30)}.GetAsync("https://xgf.nu/"+ link);
                hoge.EnsureSuccessStatusCode();
                if(hoge.RequestMessage is not null && hoge.RequestMessage.RequestUri is not null) //注意がだるいからここで確認入れとく
                if(hoge.RequestMessage.RequestUri.ToString() != "https://gigafile.nu/")
                {
                    //Console.WriteLine(hoge.RequestMessage.RequestUri.ToString());
                    return true;
                }
                return false;
            }
            catch(Exception ex) when(ex is HttpRequestException || ex is TaskCanceledException) // || ex is NullReferenceException)
            {
                return false;
            }
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
        System.Net.ServicePointManager.DefaultConnectionLimit = int.MaxValue;
        var lis = new List<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        int le = lis.Count;
        var ran = new Random();
        var gomi = 62*62*62*62*62;
        gomi = (gomi-gomi%256)/256+1;
        //全探索はあきらめててきとーに探索する!?
        //同じサイズで
        Parallel.For(0, gomi, new ParallelOptions(){MaxDegreeOfParallelism = 8}, (i) =>
        {
            var random = new Random();
            var tasks = new List<Task<string>>();
            for(int j = 0; j < 256; j++) tasks.Add(RetLinkAsync(string.Concat(lis[random.Next(le)], lis[random.Next(le)], lis[random.Next(le)], lis[random.Next(le)], lis[random.Next(le)])));
            Task.WhenAll(tasks).Wait();
            foreach(var task in tasks) if(task.Result != "") Console.WriteLine("https://xgf.nu/"+task.Result);
            tasks.Clear();
        });
        /*
        var all = new List<string>();
        foreach(char i in lis.OrderBy(x => ran.Next()))
        {
            var random = new Random();
            var tasks = new List<Task<string>>();
            foreach(char j in lis.OrderBy(x => random.Next()))
            {
                foreach(char k in lis.OrderBy(x => random.Next()))
                {
                    foreach(char l in lis.OrderBy(x => random.Next()))
                    {
                        foreach(char m in lis.OrderBy(x => random.Next())) all.Add(string.Concat(i, j, k, l, m));
                    }
                }
            }
        }
        */
        /*
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
                        foreach(char m in lis.OrderBy(x => random.Next())) tasks.Add(RetLinkAsync(string.Concat(m, l, k, j, i))); //いろんなところを探索させるために逆順にする
                        Task.WhenAll(tasks).Wait();
                        foreach(var task in tasks) if(task.Result != "") Console.WriteLine("https://xgf.nu/"+task.Result);
                        tasks.Clear();
                    }
                }
            }
        });
        */
    }
    }
}