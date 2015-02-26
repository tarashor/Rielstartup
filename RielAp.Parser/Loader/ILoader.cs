using RielAp.Parser.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Parser.Loader
{
    public interface ILoader
    {
        string GetUrl(Rubrics rubric, int page);
        string GetUrl(Rubrics rubric, Subrubrics subrubric, int page);
        Stream LoadPage(string url);
        string LoadPageHtml(string url);
    }
}
