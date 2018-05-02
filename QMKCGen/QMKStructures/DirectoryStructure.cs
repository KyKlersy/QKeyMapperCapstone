using HandlebarsDotNet;
using QKeyCommon.Keyboard_items;
using QMKCGen.helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.QMKStructures
{
    class QMKStructure
    {
        public List<List<string>> directories { get; set; }
        public List<FileTemplate> files { get; set; }
        string qmk_keyboard_dir;
        public QMKStructure()
        {
            directories = new List<List<string>>{};
            files = new List<FileTemplate>{};
        }
        public void testTemplates(Assembly asm, Keyboard keeb)
        {
            foreach (var template_file in files)
            {
                template_file.resolve_hbs(keeb);
                string hbs_raw = Self.getFileContents(asm, template_file.template_path);
                Handlebars.Compile(hbs_raw)(keeb);
            }
        }

        public void generateDirectories(Assembly asm, Keyboard keeb)
        {
            qmk_keyboard_dir = new DirectoryInfo(System.IO.Path.GetDirectoryName(asm.Location)).FullName +
                                   System.IO.Path.DirectorySeparatorChar + "qmk_firmware" +
                                   System.IO.Path.DirectorySeparatorChar + "keyboards";
            foreach (var pathArray in directories)
            {
                string newDirectory = "";
                foreach (var str in pathArray)
                {
                    newDirectory += str + System.IO.Path.DirectorySeparatorChar;
                }
                var f = new FileInfo(System.IO.Path.Combine(
                    qmk_keyboard_dir,
                    keeb.desc.product_name,
                    newDirectory
                ));
                if (!f.Directory.Exists)
                {
                    System.IO.Directory.CreateDirectory(f.DirectoryName);
                }
            }
        }
        public void generateFiles(Assembly asm, Keyboard keeb)
        {
            foreach (var template_file in files)
            {
                template_file.resolve_hbs(keeb);
                using (System.IO.StreamWriter file =
                     new System.IO.StreamWriter(System.IO.Path.Combine(
                        qmk_keyboard_dir,
                        keeb.desc.product_name,
                        template_file.relative_path)))
                {
                    string hbs_raw = Self.getFileContents(asm, template_file.template_path);
                    var hbs_template = Handlebars.Compile(hbs_raw);
                    file.Write(hbs_template(keeb));
                }
            }
        }
    }
}
