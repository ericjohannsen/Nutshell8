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
        private static extern int mkdir(string filename, int mode);

        //[DllImport("libc.so.6")]
        [DllImport("libc")]
        private static extern int ftw(string dirpath, DirClbk cl, int maxFileDescriptorsToUse);

        [StructLayout(LayoutKind.Sequential)]
        struct Timespec
        {
            long tv_sec;                 /* seconds */
            long tv_nsec;                /* nanoseconds */
        }

        [StructLayout(LayoutKind.Sequential)]
        unsafe struct Stat
        {
            public ulong st_dev;             /* Device.  */
            public ulong st_ino;             /* File serial number.  */
            public ulong st_nlink;         /* Link count.  */
            public uint st_mode;           /* File mode.  */
            public uint st_uid;            /* User ID of the file's owner. */
            public uint st_gid;            /* Group ID of the file's group.*/
            int __pad0;
            public ulong st_rdev;           /* Device number, if device.  */
            public uint st_size;            /* Size of file, in bytes. (might be __off64_t) */
            public ulong st_blksize;     /* Optimal block size for I/O.  */
            public ulong st_blocks;      /* Number 512-byte blocks allocated. (might be __blkcnt64_t) */
            public Timespec st_atim;            /* Time of last access.  */
            public Timespec st_mtim;            /* Time of last modification.  */
            public Timespec st_ctim;            /* Time of last status change.  */
            fixed ulong __glibc_reserved[3];
        }

        private delegate int DirClbk(string fName, ref Stat stat, int typeFlag);

        private static int DirEntryCallback(string fName, ref Stat stat, int typeFlag)
        {
            Console.WriteLine($"{fName} - {stat.st_size} bytes");
            return 0;
        }

        static public void List()
        {
            var maxFileDescriptorsToUse = 0;
            ftw("/tmp", DirEntryCallback, maxFileDescriptorsToUse);
            Console.WriteLine($"Struct size expected 144, have: {Marshal.SizeOf(typeof(Stat))}");
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
            return mkdir(filename, mode);
        }

    }
}