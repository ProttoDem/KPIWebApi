﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests.Fixtures
{
    public abstract class ApiFixture : IClassFixture<ApiApplicationFactory>
    {
        private readonly ApiApplicationFactory _apiAppFactory;

        protected ApiFixture(ApiApplicationFactory ApiApplicationFactory)
        {
            _apiAppFactory = ApiApplicationFactory;
            ApiHttpClient = _apiAppFactory.CreateClient();
            string validToken =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicHJvdHRvX2RlbSIsImp0aSI6Ijk0NGMwZDM5LWFjYTMtNGI5Yy04MjlmLTg5NzhjM2MwZmYyNyIsImV4cCI6MTcwMzQ0ODcwMywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0MjAwIn0._-pu0skjc_emDj6fVu88y0A2W8VYuyhnIQ-drDaKxjU";
            ApiHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", validToken);
        }

        protected HttpClient ApiHttpClient { get; }
    }
}
