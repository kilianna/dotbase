using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace DotBase.Test
{
    class TestTmp : TestBase
    {

        public void run()
        {
            var menuForm = getForm<MenuGlowneForm>(true);
            press(field<Button>(menuForm, "button1"));
            wait(() =>
            {
                var biuroForm = getForm<MenuBiuroForm>(true);
                press(field<Button>(biuroForm, "button1"));
                wait(() =>
                {
                    var zlecenieForm = getForm<ZlecenieForm>(true);
                    wait(() =>
                    {
                        var id = field<NumericUpDown>(zlecenieForm, "numericUpDown1");
                        id.Select(0, id.Text.Length);
                        send(id, "13000{TAB}");
                        wait(() =>
                        {
                            var podgl = field<ToolStripMenuItem>(zlecenieForm, "podglądToolStripMenuItem");
                            method(zlecenieForm, "podglądToolStripMenuItem_Click", zlecenieForm, new EventArgs());
                        });
                    });
                });
            });
        }


    }
}
