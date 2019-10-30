using System;
using System.Runtime.InteropServices;
using System.Text;
namespace ch25
{
    class Program
    {
        [DllImport("libc")]
        private static extern string getcwd(StringBuilder buf, int size);

        [DllImport("libc")]
        private static extern int mkdir (string filename, int mode);
        // Ensure GTK is installed:
        // sudo apt install libgtk-3-dev
        [DllImport("libgtk-x11-2.0.so.0")]
        private static extern void gtk_init(ref int argc, ref IntPtr argv);
        [DllImport("libgtk-x11-2.0.so.0")]
        static extern IntPtr gtk_message_dialog_new(IntPtr parent_window, int flags, int type, int bt, string msg, IntPtr args);
        //static extern IntPtr gtk_message_dialog_new(IntPtr parent_window, DialogFlags flags, MessageType type, ButtonsType bt, string msg, IntPtr args);
        [DllImport("libgtk-x11-2.0.so.0")]
        static extern int gtk_dialog_run(IntPtr raw);
        [DllImport("libgtk-x11-2.0.so.0")]
        static extern void gtk_widget_destroy(IntPtr widget);
        [Flags]
        public enum DialogFlags
        {
            Modal = 1,
            DestroyWithParent = 2,
        }

        public enum MessageType
        {
            Info,
            Warning,
            Question,
            Error,
            Other,
        }

        public enum ButtonsType
        {
            None,
            Ok,
            Close,
            Cancel,
            YesNo,
            OkCancel,
        }

        private delegate int DirClbk(string fName, StatClass stat, int typeFlag);

        //[DllImport("libc.so.6")]
        [DllImport("libc")]
        private static extern int ftw(string dirpath, DirClbk cl, int descriptors);

        private static int DirInfoCallback(string fName, StatClass stat, int typeFlag)
        {
            Console.WriteLine($"{fName} {stat.Blocks} blocks {stat.Size} bytes");
            return 0;
        }

    [StructLayout(LayoutKind.Sequential)]
    public class StatClass
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

        public static void Main(string[] args)
        {
            ftw("/tmp", DirInfoCallback, 10);
        }        

        static void Mainx(string[] args)
        {
            // 384 decimal = 600 octal, or rw- --- --- permissions
            //Console.WriteLine($"Exit code: {MkDir("/tmp/nutMade", 384)}");
            Console.WriteLine(Cwd());
        }

        static string Cwd()
        {
            StringBuilder sb = new StringBuilder(256);
            return getcwd(sb, sb.Capacity);
        }

        static int MkDir(string filename, int mode)
        {
            return mkdir (filename, mode);
        }
        static void DialogBox()
        {
            var argc = 0;
            var argv = IntPtr.Zero;
            gtk_init(ref argc, ref argv);
            var diag =
                gtk_message_dialog_new(IntPtr.Zero,
                    //DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
                    1, 0, 1,
                    "Hello from .NET Core", IntPtr.Zero);
            var res = gtk_dialog_run(diag);
            gtk_widget_destroy(diag);
        }
    }
}
