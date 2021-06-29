using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WhyNotWin11
{
    public static class Stuff
    {
        private static string x64 = "64 Bits", x86 = "32 Bits";
        public static string DxV, WDDM;


        private static bool isX86()
        {
            if((Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%").Length == 0))
                return true;
            return false;
        }
        public static string getArch_CPUandOS()
        {
            string x = string.Empty;
            switch (isX86())
            {
                case true:
                    x = x86;
                    break;
                case false:
                    x = x64;
                    break;
            }
            //Oof
            return String.Format("{0} CPU and {1} OS", x, Environment.Is64BitOperatingSystem ? x64 : x86);
        }
        public static string getBootMethod()
        {
            
            return cmdOutput("powershell.exe", "$(Get-ComputerInfo).BiosFirmwareType").ToUpper();
        }
        public static string getCPU_Infos(string x)
        {
            var cpu = new ManagementObjectSearcher("select * from Win32_Processor").Get().Cast<ManagementObject>().First();
            return (string)cpu[x].ToString();
        }

        //public static int getDirectX_Version()
        public static void getDirectXandWDDM_Version()
        {
            Process.Start("dxdiag", "/x HelloWorld.xml");
            while (!File.Exists("HelloWorld.xml"))
                Thread.Sleep(100);
            XmlDocument doc = new XmlDocument();
            doc.Load("HelloWorld.xml");
            XmlNode dxd = doc.SelectSingleNode("//DxDiag");
            XmlNode dxv = dxd.SelectSingleNode("//DirectXVersion");
            DxV = dxv.InnerText.Split(' ')[1];
            XmlNode wddm = dxd.SelectSingleNode("//DriverModel");
            WDDM = wddm.InnerText;
            File.Delete("HelloWorld.xml");
        }
        public static string getDriveType(int x)
        {
            if(cmdOutput("powershell.exe", $"(Get-Disk -Number {x}).PartitionStyle -eq 'GPT'").Contains("True"))
            {
                return "GPT";
            }
            return "MBR";
        }

        public static string getRamMemoryNumber()
        {
            long HelloToYourRAM;
            NativeStuff.GetPhysicallyInstalledSystemMemory(out HelloToYourRAM);
            return (HelloToYourRAM / 1024 / 1024).ToString();
        }

        public static string getSecureBootSupport()
        {
            string output = cmdOutput("powershell.exe", "powershell -Command Confirm-SecureBootUEFI | Out-String");
            if (output.Contains("True") || output.Contains("False"))
            {
                return "Supported";
            }
            return "Not Supported";
        }
        public static string getDriveTotalSpace(string x)
        {
            DriveInfo cDrive = new DriveInfo($"{x}");
            if (cDrive.IsReady)
            {
                return (cDrive.TotalSize / 1024 / 1024 / 1024).ToString();
            }
            return "Error !";
        }

        public static string getTPM_Version_NeedToFindSomeThing()
        {

            string output = cmdOutput("powershell.exe", @"Get-TPM");
            if (output.Contains("TpmPresent                : False"))
            {
                return "TPM Missing / Disabled";
            }
            //MessageBox.Show(output);
            return "Found!";
        }

        public static string getResolution()
        {
            return $"{Screen.PrimaryScreen.Bounds.Width.ToString()}x{Screen.PrimaryScreen.Bounds.Height.ToString()}";
        }

        private static string cmdOutput(string x, string y)
        {
            Process p = new Process();
            p.StartInfo.Arguments = String.Format("/C {0}", y);
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = x;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();
            return p.StandardOutput.ReadToEnd();
        }
    }
}