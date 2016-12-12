using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THC
{
    public partial class HashsumCalculatorForm : Form
    {
        public HashsumCalculatorForm()
        {
            InitializeComponent();
        }

        private void btn_AddTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Xml File|*.xml";
            ofd.RestoreDirectory = true;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {

                    lb_Templates.Items.Add(new Template(file));
                }
            }
        }

        private void btn_DeleteTemplate_Click(object sender, EventArgs e)
        {
            foreach (var selected in lb_Templates.SelectedItems)
            {
                lb_Templates.Items.Remove(selected);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            lb_Templates.Items.Clear();
        }

        private void btn_Calculate_Click(object sender, EventArgs e)
        {
            foreach (Template template in lb_Templates.SelectedItems)
            {
                template.Calculate();
            }
            lb_Templates.ClearSelected();
        }

        private void lb_Templates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_Templates.SelectedIndex != -1 && lb_Templates.SelectedIndices.Count == 1)
            {
                tb_Hashsum.Text = (lb_Templates.SelectedItem as Template).Checksum.ToString();
            } else
            {
                tb_Hashsum.Text = "";
            }
        }
    }
}
