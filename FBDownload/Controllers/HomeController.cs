using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FBDownload.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public string GetFBVideo(string info)
        {
            string urls = GetFBURL(info);

            return urls;
        }


        string GetFBURL(string info)
        {
            string jsonURLS = string.Empty;

            if (info.IndexOf("sd_src:") > 0)
            {
                string sdVideoInfo = info.Substring(info.IndexOf("sd_src:"));
                string hdVideoInfo = info.Substring(info.IndexOf("hd_src:"));

                string urlSD = sdVideoInfo.Substring(0, sdVideoInfo.IndexOf(","));
                string urlHD = hdVideoInfo.Substring(0, hdVideoInfo.IndexOf(","));

                string sd = urlSD.Substring(urlSD.IndexOf(":") + 1).Replace("\"", "");
                string hd = urlHD.Substring(urlHD.IndexOf(":") + 1).Replace("\"", "");

                jsonURLS = new JavaScriptSerializer().Serialize(new
                {
                    sd,
                    hd
                });
            }
            else if (info.IndexOf("property=\"og:video:url\" content=\"") > 0)
            {
                string sdVideoInfo = info.Substring(info.IndexOf("property=\"og:video:url\" content=\""));
                sdVideoInfo = sdVideoInfo.Substring(sdVideoInfo.IndexOf("https"));
                string urlSD = sdVideoInfo = sdVideoInfo.Substring(0, sdVideoInfo.IndexOf("\""));

                urlSD = Server.HtmlDecode(urlSD.Replace("video.fmex12-1.fna.fbcdn.net", "video.xx.fbcdn.net"));

                jsonURLS = new JavaScriptSerializer().Serialize(new
                {
                    sd = urlSD,
                    hd = "null"
                });
            }
            else
                throw new Exception();

            return jsonURLS;
        }

    }
}