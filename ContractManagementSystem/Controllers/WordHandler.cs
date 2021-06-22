using DocumentFormat.OpenXml.Packaging;
using NLog;
using OpenXmlPowerTools;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace ContractManagementSystem.Controllers
{
    public class WordHandler
    {

        static Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static string ConvertToHtml(string file, string outputDirectory)
        {

            Logger.Info("Called word upload function. | file " + file + "outputDirectory : " + outputDirectory);

            try
            {
                var fi = new FileInfo(file);
                Console.WriteLine(fi.Name);
                byte[] byteArray = File.ReadAllBytes(fi.FullName);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(byteArray, 0, byteArray.Length);
                    using (WordprocessingDocument wDoc = WordprocessingDocument.Open(memoryStream, true))
                    {
                        var destFileName = new FileInfo(fi.Name.Replace(".docx", ".html"));
                        if (outputDirectory != null && outputDirectory != string.Empty)
                        {
                            DirectoryInfo di = new DirectoryInfo(outputDirectory);
                            if (!di.Exists)
                            {
                                throw new OpenXmlPowerToolsException("Output directory does not exist");
                            }
                            destFileName = new FileInfo(Path.Combine(di.FullName, destFileName.Name));
                        }
                        var imageDirectoryName = destFileName.FullName.Substring(0, destFileName.FullName.Length - 5) + "_files";
                        int imageCounter = 0;

                        var pageTitle = fi.FullName;
                        var part = wDoc.CoreFilePropertiesPart;
                        if (part != null)
                        {
                            pageTitle = (string)part.GetXDocument().Descendants(DC.title).FirstOrDefault() ?? fi.FullName;
                        }

                        // TODO: Determine max-width from size of content area.
                        WmlToHtmlConverterSettings settings = new WmlToHtmlConverterSettings()
                        {
                            AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                            PageTitle = pageTitle,
                            FabricateCssClasses = true,
                            CssClassPrefix = "pt-",
                            RestrictToSupportedLanguages = false,
                            RestrictToSupportedNumberingFormats = false,
                            ImageHandler = imageInfo =>
                            {
                                ++imageCounter;
                                string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                                ImageFormat imageFormat = null;
                                if (extension == "png")
                                    imageFormat = ImageFormat.Png;
                                else if (extension == "gif")
                                    imageFormat = ImageFormat.Gif;
                                else if (extension == "bmp")
                                    imageFormat = ImageFormat.Bmp;
                                else if (extension == "jpeg")
                                    imageFormat = ImageFormat.Jpeg;
                                else if (extension == "tiff")
                                {
                                // Convert tiff to gif.
                                extension = "gif";
                                    imageFormat = ImageFormat.Gif;
                                }
                                else if (extension == "x-wmf")
                                {
                                    extension = "wmf";
                                    imageFormat = ImageFormat.Wmf;
                                }

                            // If the image format isn't one that we expect, ignore it,
                            // and don't return markup for the link.
                            if (imageFormat == null)
                                    return null;

                                string base64 = null;
                                try
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        imageInfo.Bitmap.Save(ms, imageFormat);
                                        var ba = ms.ToArray();
                                        base64 = System.Convert.ToBase64String(ba);
                                    }
                                }
                                catch (System.Runtime.InteropServices.ExternalException)
                                {
                                    return null;
                                }

                                ImageFormat format = imageInfo.Bitmap.RawFormat;
                                ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == format.Guid);
                                string mimeType = codec.MimeType;

                                string imageSource = string.Format("data:{0};base64,{1}", mimeType, base64);

                                XElement img = new XElement(Xhtml.img,
                                    new XAttribute(NoNamespace.src, imageSource),
                                    imageInfo.ImgStyleAttribute,
                                    imageInfo.AltText != null ?
                                        new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                                return img;
                            }
                        };
                        XElement htmlElement = WmlToHtmlConverter.ConvertToHtml(wDoc, settings);

                        // Produce HTML document with <!DOCTYPE html > declaration to tell the browser
                        // we are using HTML5.
                        var html = new XDocument(
                            new XDocumentType("html", null, null, null),
                            htmlElement);

                        var htmlString = html.ToString(SaveOptions.DisableFormatting);

                        Logger.Info("Extracted word data. | file " + file + "outputDirectory : " + outputDirectory);

                        File.WriteAllText(destFileName.FullName, htmlString, Encoding.UTF8);
                        return htmlString;

                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Error("Ex.Message: " + Ex.Message + "StackTrace : " + Ex.StackTrace);
                return null;
            }
        }
    }

}