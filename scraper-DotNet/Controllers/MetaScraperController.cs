using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using scraperDotNet.Models;

namespace scraperDotNet.Controllers
{
    [Route("api/[controller]")]
    public class MetaScraperController: ControllerBase
    {
        public MetaScraperController()
        { }

        /// <summary>
        /// Gets the URL meta data.
        /// </summary>
        /// <returns>The URL meta data.</returns>
        /// <param name="url">URL.</param>
        [HttpGet("{url}")]
        public async Task<IActionResult> GetUrlMetaData(String url)
        {
            MetaDataModel metaData;
            var httpClient = new HttpClient();
            UriBuilder uriBuild = new UriBuilder(url);
            HttpResponseMessage responseData = new HttpResponseMessage();

            try
            {
                responseData = await httpClient.GetAsync(uriBuild.Uri, HttpCompletionOption.ResponseContentRead);
            } catch (Exception exc)
            {
                return NotFound(new ApiErrorModel(500, exc.InnerException?.Message));
            }

            var siteString = await responseData.Content.ReadAsStringAsync();
            
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(siteString);
            var metaTags = document.DocumentNode.SelectNodes("//meta");

            metaData = new MetaDataModel(uriBuild.Host);
            metaData.PropertyChanged += (s,e) => metaData.HasData = true;

            if (metaTags != null)
            {
                foreach(var tag in metaTags)
                {
                    var tagName = tag.Attributes["name"];
                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];
                    var tagItemprop = tag.Attributes["itemprop"];

                    if (tagName != null && tagContent != null)
                    {
                        switch (tagName.Value.ToLower())
                        {
                            case "title":
                                metaData.Title = tagContent.Value;
                                break;

                            case "description":
                                if (!String.IsNullOrEmpty(tagContent.Value))
                                {
                                    metaData.Descriptions = tagContent.Value;
                                }
                                break;

                            case "keywords":
                                metaData.Keywords = tagContent.Value.Split(',').ToList();
                                break;

                            case "another":
                                metaData.Another = tagContent.Value;
                                break;
                        }
                    }

                    if(tagItemprop != null)
                    {
                       if (tagItemprop.Value.Contains("image"))
                        {
                            metaData.ImageInfo = new Models.ImageInfoModel(tagContent.Value);
                        }
                    }

                    if (tagProperty != null)
                    {
                        switch (tagProperty.Value.ToLower())
                        {
                            case "twitter:title":
                            case "og:title":
                                metaData.Title = string.IsNullOrEmpty(metaData.Title) ? tagContent.Value : metaData.Title;
                                break;

                            case "twitter:description":
                            case "og:descriptionn":
                                metaData.Descriptions = string.IsNullOrEmpty(metaData.Descriptions) ? tagContent.Value : metaData.Descriptions;
                                break;

                            case "og:locale":
                                metaData.Locale = tagContent.Value;
                                break;

                            case "twitter:image":
                            case "og:image":
                                if (metaData.ImageInfo == null)
                                {
                                    metaData.ImageInfo = new Models.ImageInfoModel(tagContent.Value);
                                }
                                else
                                {
                                    metaData.ImageInfo.ImgUrl = tagContent.Value;
                                }
                                break;


                            case "og:image:width":
                                if (metaData.ImageInfo == null)
                                {
                                    metaData.ImageInfo = new Models.ImageInfoModel(int.Parse(tagContent.Value),0);
                                }
                                else
                                {
                                    metaData.ImageInfo.ImgWidth = int.Parse(tagContent.Value);
                                }
                                break;

                            case "og:image:height":
                                if (metaData.ImageInfo == null)
                                {
                                    metaData.ImageInfo = new Models.ImageInfoModel(0, int.Parse(tagContent.Value));
                                }
                                else
                                {
                                    metaData.ImageInfo.ImgHeight = int.Parse(tagContent.Value);
                                }
                                break;

                            case "twitter:site":
                            case "og:site_name":
                                metaData.SiteName = tagContent.Value;
                                break;

                        }
                    }
                }
            }

            // If there isn't any meta tag for image, try to get fav icon of website page.
            if ((metaData.ImageInfo == null || String.IsNullOrEmpty(metaData.ImageInfo.ImgUrl)))
            {
                var otherTags = document.DocumentNode.SelectNodes("//link");

                if (otherTags != null)
                {
                    var favIcon = otherTags.First((tag) => tag.Attributes["rel"].Value.ToLower() == "icon");

                    if (favIcon != null)
                    {
                        var tagProperty = favIcon.Attributes["href"];

                        if (metaData.ImageInfo == null)
                        {
                            metaData.ImageInfo = new Models.ImageInfoModel(tagProperty.Value);
                        }
                        else
                        {
                            metaData.ImageInfo.ImgUrl = tagProperty.Value;
                        }
                    }
                }

            }

            // If there isn't any meta tag for description, try to get content of website page.
            if (metaData.Title == null)
            {
                var otherTags = document.DocumentNode.SelectNodes("//title");

                if (otherTags != null)
                {
                    var title = otherTags[0];

                    if (title != null)
                    {
                        metaData.Title = title.InnerHtml;
                    }
                }

            }

            return Ok(metaData);
        }

        private static Boolean isValidUrl(String url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            try
            {
                var uriBuilder = new UriBuilder(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}