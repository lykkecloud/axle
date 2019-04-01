﻿// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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
