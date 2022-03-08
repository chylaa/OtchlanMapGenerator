//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;



//namespace OtchlanMapGenerator
//{
//    class ProccesReader
//    {
//        const int PROCESS_WM_READ = 0x0010;

//        [DllImport("kernel32.dll")]
//        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

//        [DllImport("kernel32.dll")]
//        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

//        public string output = "";

//        void eraseOutputString()
//        {
//            this.output = "";
//        }
//        void startProcess()
//        {
//            var proc = new Process
//            {
//                StartInfo = new ProcessStartInfo
//                {
//                    FileName = @"C:\Users\Dell\Desktop\Otchlan\otchlan.exe",
//                    FileName = "cmd.exe",
//                    Arguments = "",
//                    UseShellExecute = false,
//                    RedirectStandardOutput = true,
//                    CreateNoWindow = false
//                }
//            };

//            if (proc.Start())
//            {
//                this.output += proc.StandardOutput.ReadToEnd();
//                proc.WaitForExit();
//            }

//            proc.StandardInput.WriteLine("n,n,n,n");

//            while (!proc.StandardOutput.EndOfStream)
//            {
//                this.output += proc.StandardOutput.ReadLine();
//                // do something with line
//                //if (this.output.Length > 10000) this.output = "";
//            }

//        }

//        public void StartProcess()
//        {
//            Process process = Process.GetProcessesByName("otchlan.exe")[0];
//            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);

//            IntPtr bytesRead;
//            byte[] buffer = new byte[64]; //To read a 64 byte unicode string

//            ReadProcessMemory(processHandle,, buffer, buffer.Length, out bytesRead);

//            Console.WriteLine(Encoding.Unicode.GetString(buffer) +
//                  " (" + bytesRead.ToString() + "bytes)");
//            Console.ReadLine();

//        }

//    }
//}

//=================MEMORY HELPER========================
