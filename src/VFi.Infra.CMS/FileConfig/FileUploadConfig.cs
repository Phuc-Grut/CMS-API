using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.CMS.FileConfig;

public class FileUploadConfig
{
    public string RootFolder { get; set; }

    public string ImageAllowExtension { get; set; }

    public string[] ImageAllowExtensions
    {
        get
        {
            return SplitExtensions(ImageAllowExtension);
        }
    }

    public string FileAllowExtension { get; set; }

    public string[] FileAllowExtensions
    {
        get
        {
            return SplitExtensions(FileAllowExtension);
        }
    }

    private string[] SplitExtensions(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new string[0];
        }

        return input.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }
}
