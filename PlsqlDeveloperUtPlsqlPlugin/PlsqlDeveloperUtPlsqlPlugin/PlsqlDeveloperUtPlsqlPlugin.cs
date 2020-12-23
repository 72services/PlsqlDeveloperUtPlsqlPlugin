using System;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    //*FUNC: 40*/ int (*SQL_Execute)(char *SQL);
    delegate int SqlExecute(string sql);
    //*FUNC: 42*/ BOOL (*SQL_Eof)();
    delegate bool SqlEof();
    //*FUNC: 43*/ int (*SQL_Next)();
    delegate int SqlNext();
    //*FUNC: 44*/ char *(*SQL_Field)(int Field);
    delegate IntPtr SqlField(int field);
    //*FUNC: 48*/ char *(*SQL_ErrorMessage)();
    delegate IntPtr SqlErrorMessage();

    //*FUNC: 69*/ void *(*IDE_CreatePopupItem)(int ID, int Index, char *Name, char *ObjectType);
    delegate void IdeCreatePopupItem(int id, int index, string name, string objectType);
    //*FUNC: 74*/ int (*IDE_GetPopupObject)(char **ObjectType, char **ObjectOwner, char **ObjectName, char **SubObject);
    delegate int IdeGetPopupObject(out IntPtr objectType, out IntPtr objectOwner, out IntPtr objectName, out IntPtr subObject);

    public class PlsqlDeveloperUtPlsqlPlugin
    {
        private const string PLUGIN_NAME = "utPLSQL Plugin";
        private const int PLUGIN_POPUP_INDEX = 1;

        private static PlsqlDeveloperUtPlsqlPlugin plugin;

        private static SqlExecute sqlExecute;
        private static SqlEof sqlEof;
        private static SqlNext sqlNext;
        private static SqlField sqlField;
        private static SqlErrorMessage sqlErrorMessage;

        private static IdeCreatePopupItem createPopupItem;
        private static IdeGetPopupObject getPopupObject;

        private static int pluginId;

        #region DLL exported API
        [DllExport("IdentifyPlugIn", CallingConvention = CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int id)
        {
            if (plugin == null)
            {
                plugin = new PlsqlDeveloperUtPlsqlPlugin();
                pluginId = id;
            }
            return PLUGIN_NAME;
        }

        [DllExport("RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            if (index == 40)
            {
                sqlExecute = (SqlExecute)Marshal.GetDelegateForFunctionPointer(function, typeof(SqlExecute));
            }
            else if (index == 42)
            {
                sqlEof = (SqlEof)Marshal.GetDelegateForFunctionPointer(function, typeof(SqlEof));
            }
            else if (index == 43)
            {
                sqlNext = (SqlNext)Marshal.GetDelegateForFunctionPointer(function, typeof(SqlNext));
            }
            else if (index == 44)
            {
                sqlField = (SqlField)Marshal.GetDelegateForFunctionPointer(function, typeof(SqlField));
            }
            else if (index == 48)
            {
                sqlErrorMessage = (SqlErrorMessage)Marshal.GetDelegateForFunctionPointer(function, typeof(SqlErrorMessage));
            }
            else if (index == 69)
            {
                createPopupItem = (IdeCreatePopupItem)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeCreatePopupItem));
            }
            else if (index == 74)
            {
                getPopupObject = (IdeGetPopupObject)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetPopupObject));
            }
        }


        [DllExport("OnActivate", CallingConvention = CallingConvention.Cdecl)]
        public static void OnActivate()
        {
            createPopupItem(pluginId, PLUGIN_POPUP_INDEX, "Run utPLSQL Test", "PACKAGE+");
            createPopupItem(pluginId, PLUGIN_POPUP_INDEX, "Run utPLSQL Test", "PACKAGE BODY+");
        }

        [DllExport("OnMenuClick", CallingConvention = CallingConvention.Cdecl)]
        public static void OnMenuClick(int index)
        {
            if (index == PLUGIN_POPUP_INDEX)
            {
                IntPtr type;
                IntPtr owner;
                IntPtr name;
                IntPtr subType;
                getPopupObject(out type, out owner, out name, out subType);

                TestRunner about = new TestRunner();
                about.Show(plugin, Marshal.PtrToStringAnsi(type), Marshal.PtrToStringAnsi(owner), Marshal.PtrToStringAnsi(name), Marshal.PtrToStringAnsi(subType));
            }
        }
        #endregion

        public void ExecuteSql(string sql) 
        {
            int code = sqlExecute(sql);
            if (code != 0)
            {
                IntPtr message = sqlErrorMessage();
                MessageBox.Show(Marshal.PtrToStringAnsi(message));
            }
        }

        public string GetResult()
        {
            StringBuilder sb = new StringBuilder();
            while (!sqlEof())
            {
                IntPtr value = sqlField(0);
                
                string converteredValue = Marshal.PtrToStringAnsi(value);

                sb.Append(converteredValue).Append("\r\n");
                
                sqlNext();
            }
            return sb.ToString();
        }
    }
}
