using System;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using System.Text;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    // /*FUNC: 69*/ void *(*IDE_CreatePopupItem)(int ID, int Index, char *Name, char *ObjectType);
    delegate void IdeCreatePopupItem(int id, int index, string name, string objectType);
    // /*FUNC: 74*/ int (*IDE_GetPopupObject)(char **ObjectType, char **ObjectOwner, char **ObjectName, char **SubObject);
    delegate int IdeGetPopupObject(out string objectType, out string objectOwner, out string objectName, out string subObject);

    public class PlsqlDeveloperUtPlsqlPlugin
    {
        private const string PLUGIN_NAME = "utPLSQL Plugin";
        private const int PLUGIN_POPUP_INDEX = 1;

        private static PlsqlDeveloperUtPlsqlPlugin plugin;

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
            if (index == 69)
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
                string type = "";
                string owner = "";
                string name = "";
                string subType = "";
                getPopupObject(out type, out owner, out name, out subType);

                About about = new About();
                about.Show(plugin, "Pop up " + type + " " + owner + " " + name + " " + subType);
            }
        }
        #endregion
    }
}
