using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alipay.Class;
using SL.Web.Service;
using System.IO;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Drawing;
using SL.Util;
using System.Collections.Specialized;

namespace SL.Web.Controllers
{
    public class CoreController : Controller
    {
        #region 页面路由
        public ActionResult Index(string catalog, string handle)
        {
            this.ViewBag.RouteData = this.RouteData.Values;

            return View("~/Views/" + catalog + (string.IsNullOrEmpty(handle) ? "" : "/" + handle) + ".cshtml");
        }

        public ActionResult JsonAction(string catalog, string handle)
        {
            return View("~/Views/Json/" + catalog + (string.IsNullOrEmpty(handle) ? "" : "/" + handle) + ".cshtml");
        }

        public ActionResult Manage(string catalog, string handle = null)
        {
            string admin = SessionUtil.Get<string>("Admin");
            if (string.IsNullOrEmpty(admin) && !"login".Equals(catalog, StringComparison.OrdinalIgnoreCase))
            {
                if (Request.AcceptTypes.Contains("application/json"))
                {
                    return Json(new { success = false, msg = "请先登录" });
                }
                else
                {
                    return Redirect(Url.Content("~/Manage/Login"));
                }
            }
            return View("~/Views/Manage/" + catalog + (string.IsNullOrEmpty(handle) ? "" : ("/" + handle)) + ".cshtml");
        }
        #endregion

        #region 图片上传
        public ActionResult Upload(string dir = null)
        {
            HttpPostedFileBase imgFile = Request.Files["imgFile"];
            if (imgFile == null)
            {
                return showError("请选择文件。");
            }

            int maxSize = 5000000;

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return showError("上传文件大小超过限制。");
            }

            //定义允许上传的文件扩展名
            Dictionary<string, string> extTable = new Dictionary<string, string>();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");
            String dirName = dir;
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return showError("目录名不正确。");
            }

            string dirDay = DateTime.Today.ToString("yy-MM-dd");
            String dirPath = Server.MapPath("~/upload") + "\\" + dirName + "\\" + dirDay;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }

            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = Path.Combine(dirPath, newFileName);

            imgFile.SaveAs(filePath);

            String fileUrl = "http://" + Request.Url.Authority + "/upload/" + dirName + "/" + dirDay + "/" + newFileName;

            return Content(System.Web.Helpers.Json.Encode(new { error = 0, url = fileUrl }));
        }

        private ActionResult showError(string msg)
        {
            return Content(System.Web.Helpers.Json.Encode(new { error = 1, message = msg }));
        }
        #endregion

        #region 图片预览
        [HttpPost]
        public ActionResult ImagePreview()
        {
            RequestUtil req = new RequestUtil();

            string callback = req.String("callback");
            int width = req.Int("width", defaultValue: 640);
            int height = req.Int("height", defaultValue: 1024);

            HttpPostedFileBase pic = Request.Files.Count == 0 ? null : Request.Files[0];
            if (pic != null && pic.ContentLength != 0)
            {
                byte[] imageBuffer = ImageUtil.Compress(pic.InputStream, 40, width, height);

                string guid = System.Guid.NewGuid().ToString("N");

                CacheUtil.CreateCache("preview-" + guid, 0.1, imageBuffer);

                return Content(HtmlUtil.Result(callback, new { success = true, guid = guid, name = Request.Files.Keys[0] }));
            }
            else
            {
                return Content(HtmlUtil.Result(callback, new { success = false, msg = "您还未选择图片" }));
            }
        }

        public ActionResult ImagePreview(string guid)
        {
            guid = "preview-" + guid;

            if (CacheUtil.ExistCache(guid))
            {
                byte[] imageBuffer = CacheUtil.Get<byte[]>(guid);
                return File(imageBuffer, "image/Jpeg");
            }
            else
                return Content("图片不存在！" + guid);

        }
        #endregion

        #region 图片压缩
        public ActionResult CompressImage(string image)
        {
            string path = Server.MapPath("~/" + image);

            RequestUtil req = new RequestUtil();
            byte[] buffer = ImageUtil.Compress(path, req.Int("w", defaultValue: 174));

            return File(buffer, "image/jpeg");
        }

        public ActionResult Base64Image(string image)
        {
            string path = Server.MapPath("~/" + image);
            byte[] buffer;
            using (Stream sm = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
            {
                buffer = ImageUtil.GetThumbNailImageBytes(sm, 640, 0);
            }

            string callback = Request.QueryString["callback"];
            string dataUrl = "data:image/" + image.Substring(image.LastIndexOf(".") + 1) + ";base64," + Convert.ToBase64String(buffer);
            if (!string.IsNullOrEmpty(callback))
            {
                return Content(callback + "(\"" + dataUrl + "\");");
            }
            else
            {
                return Content(dataUrl);
            }
        }
        #endregion

        #region 验证码
        public ActionResult CheckCode()
        {
            string checkCode;
            byte[] imageBuffer = ImageUtil.CreateImage(out checkCode);
            Session["CheckCode"] = checkCode;

            return File(imageBuffer, "image/Jpeg");
        }
        #endregion

    }
}
