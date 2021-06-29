using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhyNotWin11
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false; //Very bad idea but... I'm lazy af. It was just for "Fun" !
            InitializeComponent();
            MessageBox.Show(String.Format("Welcome {0}", Environment.UserName), Settings.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/rcmaehl/WhyNotWin11");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Activated += CheckNow;
        }
        private void CheckNow(object sender, EventArgs e)
        {
            Program.Form1.Shown += (x, y) => {
                Thread z = new Thread(() => {
                    MessageBox.Show("TPM Can be enabled in your UEFI(not called BIOS anymore) | NOT EVERYONE HAVE A TPM!!", Settings.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show("You will know your result only after every informations showed :')", Settings.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Be patient the windows will be unusable :')", Settings.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ArchR.Text = Stuff.getArch_CPUandOS();
                    BMR.Text = Stuff.getBootMethod();
                    CCR.Text = Stuff.getCPU_Infos("Name");
                    CCCR.Text = String.Format("{0} Cores and {1} Threads", Stuff.getCPU_Infos("NumberOfCores"), Stuff.getCPU_Infos("NumberOfLogicalProcessors"));
                    CFR.Text = String.Format("{0} MHz", Stuff.getCPU_Infos("MaxClockSpeed"));
                    Stuff.getDirectXandWDDM_Version();
                    DAWDDMR.Text = String.Format("DirectX {0} and {1}", Stuff.DxV, Stuff.WDDM);
                    DPTR.Text = String.Format("{0} | Determined Using Partition 0", Stuff.getDriveType(0));
                    RIR.Text = String.Format("{0} GB", Stuff.getRamMemoryNumber());
                    SBR.Text = Stuff.getSecureBootSupport();
                    SAR.Text = String.Format("{0} GB on C:\\", Stuff.getDriveTotalSpace("C").ToString());
                    TPMR.Text = Stuff.getTPM_Version_NeedToFindSomeThing();
                    SRR.Text = Stuff.getResolution();
                    Verifications();
                });
                z.Start();
                z.Join();
            };
        }
        private void Verifications()
        {
            /*                                      */
            if (ArchR.Text.Contains("64 Bits CPU"))
                ArchC.Image = Properties.Resources.Yes;
            else
                ArchC.Image = Properties.Resources.No;
            /*                                      */
            if (!CCR.Text.Contains("@ 1"))
            {
                CCC.Image = Properties.Resources.Yes;
                CFC.Image = Properties.Resources.Yes;
            } else
            {
                CCC.Image = Properties.Resources.No;
                CFC.Image = Properties.Resources.No;
            }
            /*                                      */
            if (int.Parse(Stuff.getCPU_Infos("NumberOfCores")) >= 2)
            {
                CCCC.Image = Properties.Resources.Yes;
            } else
            {
                CCCC.Image = Properties.Resources.No;
                CCCR.Text += " (2 Cores Minimum!)";
            }
            /*                                      */
            if (BMR.Text.Contains("UEFI"))
                BMC.Image = Properties.Resources.Yes;
            else if (BMR.Text.Contains("LEGACY"))
                BMC.Image = Properties.Resources.No;
            else
                BMC.Image = Properties.Resources.NotSure;
            /*                                      */

            if (DAWDDMR.Text.Contains("DirectX 12") && DAWDDMR.Text.Contains("WDDM 2"))
                DAWDDMC.Image = Properties.Resources.Yes;
            else
                DAWDDMC.Image = Properties.Resources.No;
            /*                                      */
            if (DPTR.Text.Contains("GPT"))
                DPTC.Image = Properties.Resources.Yes;
            else
                DPTC.Image = Properties.Resources.No;
            /*                                      */
            if (int.Parse(Stuff.getRamMemoryNumber()) >= 4)
                RIC.Image = Properties.Resources.Yes;
            else
                RIC.Image = Properties.Resources.No;
            /*                                      */
            if (!SBR.Text.Contains("Not Supported"))
                SBC.Image = Properties.Resources.Yes;
            else
                SBC.Image = Properties.Resources.No;
            /*                                      */
            if (int.Parse(Stuff.getDriveTotalSpace("C")) > 64)
                SAC.Image = Properties.Resources.Yes;
            else
                SAC.Image = Properties.Resources.No;
            /*                                      */
            if (!TPMR.Text.Contains("TPM Missing / Disabled"))
                TPMC.Image = Properties.Resources.Yes;
            else
                TPMC.Image = Properties.Resources.No;
            /*                                      */
            if (int.Parse(SRR.Text.Split('x')[1]) > 720)
                SRC.Image = Properties.Resources.Yes;
            else
                SRC.Image = Properties.Resources.No;
            /*                                      */
            MessageBox.Show("Finished!(Yeah not really fast sorry :))");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://docs.microsoft.com/en-us/windows/whats-new/windows-11-requirements#hardware-requirements");
        }
    }
}
