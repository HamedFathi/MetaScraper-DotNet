using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            if (string.IsNullOrEmpty(url))
            {
                return NotFound(new ApiErrorModel(500, "Url parameter should has value."));
            }

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
                metaData.Title = GetTitleFrom(document);
                metaData.ImageInfo = GetImageInfoFrom(document);

                foreach (var tag in metaTags)
                {
                    var tagName = tag.Attributes["name"];
                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];
                    var tagItemprop = tag.Attributes["itemprop"];

                    if (tagName != null && tagContent != null)
                    {
                        switch (tagName.Value.ToLower())
                        {
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

                    if (tagProperty != null)
                    {
                        switch (tagProperty.Value.ToLower())
                        {
                            case "twitter:description":
                            case "og:descriptionn":
                                metaData.Descriptions = string.IsNullOrEmpty(metaData.Descriptions) ? tagContent.Value : metaData.Descriptions;
                                break;

                            case "og:locale":
                                metaData.Locale = tagContent.Value;
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


            return Ok(metaData);
        }

        /// <summary>
        /// Gets the title from html content.
        /// </summary>
        /// <returns>The title of website.</returns>
        /// <param name="document">Document is content of website</param>
        private string GetTitleFrom(HtmlDocument document)
        {
            var metaTags = document.DocumentNode.SelectNodes("//meta");

            var mTitle = metaTags.FirstOrDefault((tag) =>
            {
                return tag?.Attributes["name"]?.Value == "title";
            });

            if (mTitle == null)
            {
                mTitle = metaTags.FirstOrDefault((tag) =>
                {
                    return tag?.Attributes["proprty"]?.Value == "twitter:title" ||
                                tag?.Attributes["proprty"]?.Value == "og:title";
                });
            }

            if (mTitle != null)
            {
                return mTitle.Attributes["content"].Value;
            }

            // If there isn't any meta tag for description, try to get content of website page.
            var otherTags = document.DocumentNode.SelectNodes("//title");

            if (otherTags != null)
            {
                var title = otherTags[0];

                if (title != null)
                {
                    return title.InnerHtml;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the image info from website.
        /// </summary>
        /// <returns>The image info of website.</returns>
        /// <param name="document">Document is content of website</param>
        private ImageInfoModel GetImageInfoFrom(HtmlDocument document)
        {
            ImageInfoModel imageInfo = null;
            var metaTags = document.DocumentNode.SelectNodes("//meta");

            var mImage = metaTags.FirstOrDefault((tag) =>
            {
                return tag?.Attributes["itemprop"]?.Value == "image";
            });

            if (mImage == null)
            {
                mImage = metaTags.FirstOrDefault((tag) =>
                {
                    return tag?.Attributes["proprty"]?.Value == "twitter:image" ||
                                tag?.Attributes["proprty"]?.Value == "og:image";
                });
            }

            if (mImage != null && !string.IsNullOrEmpty(mImage.Attributes["content"].Value))
            {
                imageInfo = new ImageInfoModel(mImage.Attributes["content"].Value);
            }

            // If there isn't any meta tag for image, try to get fav icon of website page.
            var otherTags = document.DocumentNode.SelectNodes("//link");
            if (otherTags != null)
            {
                var favIcon = otherTags.FirstOrDefault((tag) => tag?.Attributes["rel"]?.Value?.ToLower() == "icon");

                if (favIcon != null)
                {
                    var tagProperty = favIcon?.Attributes["href"];

                    if (imageInfo == null)
                    {
                        imageInfo = new Models.ImageInfoModel(tagProperty?.Value);
                    }
                    else
                    {
                        imageInfo.ImgUrl = tagProperty.Value;
                    }
                }
            }

            return imageInfo;
        }

        /// <summary>
        /// Ises the valid URL.
        /// </summary>
        /// <returns><c>true</c>, if valid URL was ised, <c>false</c> otherwise.</returns>
        /// <param name="url">URL.</param>
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