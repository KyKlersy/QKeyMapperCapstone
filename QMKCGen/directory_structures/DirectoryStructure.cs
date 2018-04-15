using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.directory_structures
{
    class DirectoryStructure
    {
        public List<List<string>> directories { get; set; }
        public List<FileTemplate> files { get; set; }
        DirectoryStructure()
        {
            directories = new List<List<string>>{};
            files = new List<FileTemplate>{};
        }

    }
}
