using System;
using System.Collections.Generic;
using System.Text;

namespace ElMaitre.Service
{
    public class Utils
    {
        public static string GetVideo(string EmbedCode)
        {
            var EmbedCodeNew = "";
            if (EmbedCode == null)
                return "";
            if (EmbedCode.Contains("youtube.com"))
            {
                if (EmbedCode.Contains("/v/"))
                {
                    EmbedCodeNew = EmbedCode;
                }
                else if (EmbedCode.Contains("/watch?v"))
                {
                    EmbedCodeNew = EmbedCode.Replace("/watch?v=", "/v/");
                }
                else if (EmbedCode.Contains("/watch?feature=player_embedded&v="))
                {
                    EmbedCodeNew = EmbedCode.Replace("/watch?feature=player_embedded&v=", "/v/");
                }
            }

            string NeededEmbed = "<object width=\"500\" height=\"334\">";
            NeededEmbed += "<param name=\"movie\" value=\"" + EmbedCodeNew + "\"></param>";
            NeededEmbed += "<param name=\"allowFullScreen\" value=\"true\"></param>";
            NeededEmbed += "<param name=\"allowscriptaccess\" value=\"always\"></param>";
            NeededEmbed += "<param value=\"transparent\" name=\"wmode\"/>";
            NeededEmbed += "<embed src=\"" + EmbedCodeNew + "\" type=\"application/x-shockwave-flash\" width=\"500\" height=\"334\" allowscriptaccess=\"always\" allowfullscreen=\"true\" wmode=\"transparent\"></embed>";
            NeededEmbed += "</object>";
            return NeededEmbed;
        }
    }
}
