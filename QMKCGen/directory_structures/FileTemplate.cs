using HandlebarsDotNet;
using QKeyCommon.Keyboard_items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.directory_structures
{
    class FileTemplate
    {
        //I think I can get these to auto render their hbs
        public string relative_path { get; set; }
        public string template_path { get; set; }
        public void resolve_hbs(Keyboard context)
        {
            var hbs_template = Handlebars.Compile(relative_path);
            relative_path = hbs_template(context);
        }
    }
}
