// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace SampleSinglePageApp
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    /*  NOTE (Cameron): This sample demonstrates the code required to secure a Single Page Application (SPA).  */

    public class Program
    {
        public static void Main(string[] args) => WebHost.CreateDefaultBuilder(args).UseUrls("http://+:5013").UseStartup<Startup>().Build().Run();
    }
}
