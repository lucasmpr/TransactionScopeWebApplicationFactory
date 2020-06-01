using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Api.Test
{
    public class TestA : IDisposable
    {
        private HttpClient client;
        private ApiFactory _factory;
        protected readonly ITestOutputHelper _output;
        public TestA(ITestOutputHelper output)
        {
            _factory = new ApiFactory();
            client = _factory.CreateClient();
            _output = output;
        }
        [Fact]
        public async void TestHomeDoStuff()
        {
            var response = await client.PostAsync("/home/dostuff", null);

            var a = await response.Content.ReadAsStringAsync();

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(a, ex);
            }

            _output.WriteLine(a);
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
    public class TestB : IDisposable
    {
        private HttpClient client;
        private ApiFactory _factory;
        protected readonly ITestOutputHelper _output;

        public TestB(ITestOutputHelper output)
        {
            _factory = new ApiFactory();
            client = _factory.CreateClient();
            _output = output;
        }
        [Fact]
        public async void TestNotHomeDoStuff()
        {
            var response = await client.PostAsync("/nothome/dostuff", null);
            var b = await response.Content.ReadAsStringAsync();
            
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(b, ex);
            }
            _output.WriteLine(b);
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
    
}