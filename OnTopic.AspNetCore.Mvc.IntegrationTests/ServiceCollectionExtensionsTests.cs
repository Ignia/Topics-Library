﻿/*==============================================================================================================================
| Author        Ignia, LLC
| Client        Ignia, LLC
| Project       Topics Library
\=============================================================================================================================*/
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnTopic.AspNetCore.Mvc.IntegrationTests.Host;

namespace OnTopic.AspNetCore.Mvc.IntegrationTests {

  /*============================================================================================================================
  | TEST: SERVICE COLLECTION EXTENSIONS
  \---------------------------------------------------------------------------------------------------------------------------*/
  /// <summary>
  ///   The <see cref="ServiceCollectionExtensions"/> are responsible, primarily, for establishing routes based on the <see cref
  ///   ="IEndpointRouteBuilder"/> interface. These integration tests validate that those routes are operating as expected.
  /// </summary>
  [TestClass]
  public class ServiceCollectionExtensionsTests {

    /*==========================================================================================================================
    | PRIVATE VARIABLES
    \-------------------------------------------------------------------------------------------------------------------------*/
    private readonly            WebApplicationFactory<Startup>  _factory;

    /*==========================================================================================================================
    | CONSTRUCTOR
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Initializes a new instance of the <see cref="TopicViewLocationExpanderTest"/>.
    /// </summary>
    public ServiceCollectionExtensionsTests() {
      _factory = new WebApplicationFactory<Startup>();
    }

    /*==========================================================================================================================
    | TEST: MAP TOPIC SITEMAP: RESPONDS TO REQUEST
    \-------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    ///   Evaluates a route associated with <see cref="ServiceCollectionExtensions.MapTopicSitemap(IEndpointRouteBuilder)"/> and
    ///   confirms that it responds appropriately.
    /// </summary>
    [TestMethod]
    public async Task MapTopicSitemap_RespondsToRequest() {

      var client                = _factory.CreateClient();
      var uri                   = new Uri($"/Sitemap/", UriKind.Relative);
      var response              = await client.GetAsync(uri).ConfigureAwait(false);
      var content               = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      response.EnsureSuccessStatusCode();

      Assert.AreEqual<string?>("text/xml", response.Content.Headers.ContentType?.ToString());
      Assert.IsTrue(content.Contains("/Web/ContentList/</loc>", StringComparison.OrdinalIgnoreCase));

    }

  }
}