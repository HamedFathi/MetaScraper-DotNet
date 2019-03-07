using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace scraperDotNet.Models
{
    public class ImageInfoModel
    {

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String ImgUrl { get; set; }

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>The width of the image.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? ImgWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>The height of the image.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? ImgHeight { get; set; }

        private ImageInfoModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:scraperDotNet.Models.ImageInfoModel"/> class.
        /// Note: The imgUrl parameter is require and should has value.
        /// </summary>
        /// <param name="imgUrl">Image URL.</param>
        public ImageInfoModel(String imgUrl)
        {
            Contract.Requires(imgUrl != null);
            ImgUrl = imgUrl;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:scraperDotNet.Models.ImageInfoModel"/> class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public ImageInfoModel(float width, float height)
        {
            ImgWidth = width;
            ImgHeight = height;
        }
    }
}
