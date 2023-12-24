using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    public abstract class ApiFixture : IClassFixture<ApiApplicationFactory>
    {
        private readonly ApiApplicationFactory _apiAppFactory;

        protected ApiFixture(ApiApplicationFactory ApiApplicationFactory)
        {
            _apiAppFactory = ApiApplicationFactory;
            ApiHttpClient = _apiAppFactory.CreateClient();
            string validToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicHJvdHRvX2RlbSIsImp0aSI6ImJlYzkzZGRjLTY2MjAtNGVmNS1hM2NhLTM0NjcwYmUzY2ZjNCIsImV4cCI6MTcwMzM5NDgyMywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0MjAwIn0.dRpjs7ViErFBv_fGTnUfbifFHHE_cbYXQtJ5UxDmBYQ"; // Replace with actual method to retrieve or store a valid token

            ApiHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", validToken);
        }

        protected HttpClient ApiHttpClient { get; }
    }
}
