using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ch25
{
    public class Dir
    {
        [DllImport("libc")]
        private static extern string getcwd(StringBuilder buf, int size);

        [DllImport("libc")]
        private static extern int mkdir (string filename, int mode);

        //[DllImport("libc.so.6")]
        [DllImport("libc")]
        private static extern int ftw(string dirpath, DirClbk cl, int maxFileDescriptorsToUse);

        [StructLayout(LayoutKind.Sequential)]
        public class StatClass_ORIG
        {
            public uint DeviceID;
            public uint InodeNumber;
            public uint Mode;
            public uint HardLinks;
            public uint UserID;
            public uint GroupID;
            public uint SpecialDeviceID;
            public ulong Size;
            public ulong BlockSize;
            public uint Blocks;
            public long TimeLastAccess;
            public long TimeLastModification;
            public long TimeLastStatusChange;
        }        
        [StructLayout(LayoutKind.Sequential)]
        public class StatClass // Shortened for book formatting
        {
            public uint DeviceID, InodeNumber, Mode, HardLinks, UserID, GroupID, SpecialDeviceID;
            public ulong Size, BlockSize, Blocks;
            public long TimeLastAccess, TimeLastModification, TimeLastStatusChange;
        }        


        private delegate int DirClbk(string fName, StatClass stat, int typeFlag);

        private static int DirEntryCallback(string fName, StatClass stat, int typeFlag)
        {
            Console.WriteLine($"{fName} {stat.Blocks} blocks {stat.Size} bytes");
            return 0;
        }

        static public void List()
        {
            var maxFileDescriptorsToUse = 0;
            ftw("/tmp", DirEntryCallback, maxFileDescriptorsToUse);
        }
        static public string Cwd()
        {
            // Usage: ftw("/tmp", DirInfoCallback, 10);
            StringBuilder sb = new StringBuilder(256);
            return getcwd(sb, sb.Capacity);
        }

        static public void MkDirInTmp()
        {
            // 384 decimal = 600 octal, or rw- --- --- permissions
            Console.WriteLine($"Exit code: {MkDir("/tmp/nutMade", 384)}");
        }
        static int MkDir(string filename, int mode)
        {
            return mkdir (filename, mode);
        }

    }
}