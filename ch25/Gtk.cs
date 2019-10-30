using System;
using System.Runtime.InteropServices;

namespace ch25
{
    public class Gtk
    {
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

        static public void DialogBox()
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